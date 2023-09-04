﻿namespace codenames_solver;
public class SimilarityClient
{
    private readonly HttpClient httpClient;
    private readonly DTOBuilder dtoBuilder;

    public SimilarityClient(HttpClient httpClient, DTOBuilder DTOBuilder)
    {
        this.httpClient = httpClient;
        this.dtoBuilder = DTOBuilder;
    }
    public async Task<SimilarityPostResponseDTO> RequestSimilarWords()
    {
        SimilarityPostDTO SimilarityPostBody = dtoBuilder.BuildSimilarityPostDTO();
        var response = await httpClient.PostAsJsonAsync("api/Similarity", SimilarityPostBody);
        response.EnsureSuccessStatusCode();
        var SimilarityPostResponse = await response.Content.ReadFromJsonAsync<SimilarityPostResponseDTO>();
        if (SimilarityPostResponse is null)
        {
            throw new Exception("Null response when attempting to find similarities");
        }
        return SimilarityPostResponse;
    }
}
