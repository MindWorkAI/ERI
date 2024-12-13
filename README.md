# ERI - (E)xternal (R)etrieval (I)nterface

The [ERI](https://github.com/MindWorkAI/ERI) is the External Retrieval Interface which could be used by [AI Studio](https://github.com/MindWorkAI/AI-Studio) and other tools. The ERI acts as a contract between decentralized data sources and LLM tools. The ERI is implemented by the data sources, allowing them to be integrated into, e.g., AI Studio later. This means that the data sources assume the server role and the LLM tool assumes the client role of the API. This approach serves to realize a [Retrieval-Augmented Generation](https://en.wikipedia.org/wiki/Retrieval-augmented_generation) (RAG) process with external data. You can imagine it like this: Hypothetically, when Wikipedia implemented the ERI, it would vectorize all pages using an [embedding method](https://en.wikipedia.org/wiki/Word_embedding). All of Wikipedia's data would remain with Wikipedia, including the [vector database](https://en.wikipedia.org/wiki/Vector_database) (decentralized approach). Then, any AI Studio user could add Wikipedia as a data source to significantly reduce the hallucination of the LLM in knowledge questions.

When you want to integrate your own local data into AI Studio, you don't need an ERI. Instead, AI Studio will offer an RAG process for this in the future. Is your organization interested in integrating internal company data into AI Studio? [Here](https://mindworkai.org/swagger-ui.html) you will find the [interactive documentation](https://mindworkai.org/swagger-ui.html) of the related OpenAPI interface.

Links:
- [Interactive documentation aka Swagger UI](https://mindworkai.org/swagger-ui.html)
- [ERI specification](https://mindworkai.org/eri-specification.json), which you could use with tools like [OpenAPI Generator](https://github.com/OpenAPITools/openapi-generator).