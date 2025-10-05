using System.Collections.Concurrent;
using System.Text.Json;

namespace DemoServer.Handler;

public static class RetrievalV2
{
    private const string TAG = "Retrieval";
    
    private static readonly ConcurrentDictionary<Guid, v20.RetrievalSession> SESSIONS = new();
    private static readonly ConcurrentDictionary<Guid, List<v11.Context>> RESULTS = new();
    
    private static readonly v20.RetrievalInfo[] RETRIEVAL_INFO = 
    [
        new ()
        {
            Id = "DEMO-1",
            Name = "DEMO: Wikipedia Links",
            Description = "Contains some links to Wikipedia articles.",
            Link = null,
            Embeddings = [],
            MustUseERIServerLLMProvider = false,
        },
    ];

    public static void AddRetrievalV2Handlers(this IEndpointRouteBuilder app)
    {
        var router = app.MapGroup("/retrieval")
            .WithTags(TAG)
            .WithApiVersionSet(Versions.SET_V2_0_AND_ABOVE);

        router.MapGet("/info", GetRetrievalInfoV20)
            .WithDescription("Get information about the retrieval processes implemented by this data source.")
            .WithName("GetRetrievalInfoV20");

        router.MapPut("/announce", AnnounceRetrievalV20)
            .WithDescription("Announce a new retrieval session for the specified data source.")
            .WithName("AnnounceRetrievalV20");

        router.MapGet("/state", GetRetrievalStateV20)
            .WithDescription("Get the current state of the specified retrieval session.")
            .WithName("GetRetrievalStateV20");

        router.MapPost("/processStep", QueryProcessStepV20)
            .WithDescription("""
                             Query the next step in the retrieval process for a given session ID. This is used
                             to allow for retrieval processes that require client-side actions, e.g.,
                             use the user's LLM to perform a task.
                             """)
            .WithName("QueryProcessStepV20");

        router.MapGet("/results", GetResultsV20)
            .WithDescription("""
                             Get the retrieval results for the specified session ID. You might need to call this
                             method multiple times to get all results, depending on the number of results and
                             the maximum number of results returned per call.
                             """)
            .WithName("GetResultsV20");
        
        router.MapPost("/chatCompletion", ChatCompletionV20)
            .WithDescription("""
                             Get a chat completion from the ERI server's LLM provider, based on the provided messages.
                             The response is a stream of SSE (Server-Sent Events) messages. This API is compatible
                             with the OpenAI chat completion API. Differences are: (1) Only a list of messages
                             is accepted as input. Other parameters, like model name, temperature, etc., are
                             not supported. (2) The response data structure might be a subset of the OpenAI
                             response data structure.
                             """)
            .WithName("ChatCompletionV20");
    }
    
    /// <summary>
    /// Get information about the retrieval processes implemented by the specified data source.
    /// </summary>
    /// <param name="dataSourceId">The data source ID for which to get the retrieval information.</param>
    /// <returns>Information about the retrieval processes implemented by the specified data source.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if the data source ID is unknown.</exception>
    private static v20.RetrievalInfo[] GetRetrievalInfoV20(int dataSourceId)
    {
        if(dataSourceId != 0)
            throw new ArgumentOutOfRangeException(nameof(dataSourceId), "Invalid data source ID.");
        
        return RETRIEVAL_INFO;
    }

    /// <summary>
    /// Announce a new retrieval session for the specified data source.
    /// </summary>
    /// <param name="dataSourceId">The data source ID for which to announce the retrieval session.</param>
    /// <param name="request">The retrieval request.</param>
    /// <returns>The ID of the created retrieval session.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if the data source ID is unknown.</exception>
    private static Guid AnnounceRetrievalV20(int dataSourceId, v10.RetrievalRequest request)
    {
        if(dataSourceId != 0)
            throw new ArgumentOutOfRangeException(nameof(dataSourceId), "Invalid data source ID.");
        
        // Create a session id:
        var sessionId = Guid.NewGuid();
        
        // Create the semaphore. We need this to ensure that the retrieval task
        // doesn't start before we have added the session to the SESSIONS
        // dictionary:
        var semaphore = new SemaphoreSlim(1, 1);
        
        // Lock the semaphore to block the retrieval task:
        semaphore.Wait();
        
        // Create the retrieval session:
        var session = new v20.RetrievalSession
        {
            Id = sessionId,
            DataSourceId = dataSourceId,
            CreatedAt = DateTime.UtcNow,
            State = RetrievalState.NOT_STARTED,
            
            // We use the number of items as progress type:
            Progress = new(RetrievalProgressType.NUMBER_OF_ITEMS, 0, ExampleData.EXAMPLE_DATA.Length),
            
            // Start the retrieval task asynchronously, without waiting for it:
            RetrievalTask = Retrieve(sessionId, request, semaphore),
            
            // Store the semaphore so that we can dispose it after the task
            // has finished. This is important to avoid a memory leak:
            Semaphore = semaphore,
        };
        
        SESSIONS[session.Id] = session;
        RESULTS[session.Id] = [];
        
        // Release the semaphore to allow the retrieval task to start:
        semaphore.Release();
        return session.Id;
    }

    private static async Task Retrieve(Guid sessionId, v10.RetrievalRequest request, SemaphoreSlim semaphore)
    {
        // Wait until the session has been added to the SESSIONS dictionary:
        await semaphore.WaitAsync();
        semaphore.Release();
        
        // Get the session:
        if (!SESSIONS.TryGetValue(sessionId, out var session))
            return;
        
        if (session.State is RetrievalState.DONE)
            return;
        
        // Update the session state to ONGOING:
        SESSIONS[sessionId] = session with { State = RetrievalState.ONGOING };
        
        // Get the list of results:
        var results = RESULTS[sessionId];
        var processedItems = 0;
        
        // Simulate a retrieval process by iterating over the example data:
        foreach (var article in ExampleData.EXAMPLE_DATA)
        {
            // Find matches:
            if(!request.LatestUserPrompt.Contains(article.Title, StringComparison.InvariantCultureIgnoreCase))
                continue;
            
            // Check if we reached the maximum number of matches:
            if(request.MaxMatches > 0 && results.Count >= request.MaxMatches)
                break;
        
            results.Add(new v11.Context
            {
                Name = article.Title,
                Category = "Wikipedia Article",
                Path = article.Url,
                ConfidenceScore = 1f,
                Type = ContentType.TEXT,
                MatchedContent = article.Url,
                SurroundingContent = [],
                Links = [],
            });
            
            // Update the retrieval progress:
            SESSIONS[sessionId] = session with
            {
                Progress = session.Progress with { Current = ++processedItems }
            };
        }
    }
    
    /// <summary>
    /// Get the current state of the specified retrieval session.
    /// </summary>
    /// <param name="sessionId">The ID of the retrieval session. You get this ID when announcing the session.</param>
    /// <returns>The current state of the retrieval session.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the session ID is unknown.</exception>
    private static v20.RetrievalStatus GetRetrievalStateV20(Guid sessionId)
    {
        if(!SESSIONS.TryGetValue(sessionId, out var session))
            throw new ArgumentOutOfRangeException(nameof(sessionId), "Invalid session ID.");
        
        return new(session.State, session.Progress);
    }

    /// <summary>
    /// Query the next step in the retrieval process for a given session ID.
    /// </summary>
    /// <param name="sessionId">The unique identifier of the retrieval session to query.</param>
    /// <param name="llmResponse">The response from the user's LLM, if applicable.</param>
    /// <returns>The next step in the retrieval process. Depending on the retrieval process,
    /// a client-side action may be required.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if the provided session ID is invalid or does not exist.</exception>
    private static v20.RetrievalProcessStepResponse QueryProcessStepV20(Guid sessionId, v20.LLMResponse? llmResponse)
    {
        if (!SESSIONS.TryGetValue(sessionId, out var session))
            throw new ArgumentOutOfRangeException(nameof(sessionId), "Invalid session ID.");
        
        // If the retrieval process is done, return DONE:
        if(session.State is RetrievalState.DONE)
            return new(
                RetrievalProcessStepType.DONE,
                Guid.Empty,
                null);
        
        // In this demo server, we have a fixed retrieval process that
        // doesn't require any client-side interactions:
        return new(
            RetrievalProcessStepType.NO_ACTION_NEEDED,
            Guid.Empty,
            null);
    }
    
    /// <summary>
    /// Get the retrieval results for the specified session ID.
    /// </summary>
    /// <param name="sessionId">The ID of the retrieval session. You get this ID when announcing the session.</param>
    /// <returns>The retrieval results.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the session ID is unknown.</exception>
    private static v20.RetrievalResult GetResultsV20(Guid sessionId)
    {
        if (!SESSIONS.TryGetValue(sessionId, out var session))
            throw new ArgumentOutOfRangeException(nameof(sessionId), "Invalid session ID.");
        
        // When the retrieval process is not done yet, return PROCESS_STILL_ONGOING:
        if (session.State is not RetrievalState.DONE)
            return new()
            {
                State = RetrievalResultState.PROCESS_STILL_ONGOING,
                Contexts = [],
                ConclusiveAnswer = null,
                MoreResultsAvailable = false,
            };
        
        // We will return just one result per request to demonstrate
        // the pagination capabilities:
        var results = RESULTS[sessionId];
        if (results.Count is 0)
        {
            return new()
            {
                State = RetrievalResultState.NO_RESULTS_FOUND,
                Contexts = [],
                ConclusiveAnswer = null,
                MoreResultsAvailable = false,
            };
        }
        
        // Get the next result and remove it from the list:
        var result = results[0];
        results.RemoveAt(0);
        RESULTS[sessionId] = results;
        
        // Return the result:
        return new()
        {
            State = RetrievalResultState.RESULTS_READY,
            Contexts = [ result ],
            ConclusiveAnswer = null,
            
            // Indicate whether more results are available:
            MoreResultsAvailable = results.Count > 0,
        };
    }
    
    /// <summary>
    /// Get a chat completion from the ERI server's LLM provider.
    /// </summary>
    /// <param name="context">The HTTP context to write the SSE stream to.</param>
    /// <param name="messages">The list of chat messages to base the completion on.</param>
    /// <param name="sessionId">The retrieval session ID to use for context. The retrieval process must be done.</param>
    /// <returns>A stream of SSE (Server-Sent Events) messages containing the chat completion.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if the provided session ID is invalid or does not exist.</exception>
    /// <exception cref="InvalidOperationException">Thrown if the retrieval process is not done yet.</exception>
    private static async IAsyncEnumerable<string> ChatCompletionV20(HttpContext context, IList<v20.ChatMessage> messages, Guid sessionId)
    {
        if (!SESSIONS.TryGetValue(sessionId, out var session))
            throw new ArgumentOutOfRangeException(nameof(sessionId), "Invalid session ID.");
        
        if(session.State is not RetrievalState.DONE)
            throw new InvalidOperationException("Retrieval process is not done yet.");
    
        context.Response.Headers.ContentType = "text/event-stream";
        await Task.CompletedTask;
        var emptyContent = Array.Empty<string>();
        foreach (var text in emptyContent)
        {
            var sse = new v20.ChatCompletionDeltaStreamLine
            {
                Choices =
                [
                    new()
                    {
                        Delta = new()
                        {
                            Content = text,
                        },
                        Index = 0,
                    }
                ],
            };
            
            var json = JsonSerializer.Serialize(sse);
            yield return $"data: {json}\n\n";
        }

        yield return "data: [END]\n\n";
    }
}