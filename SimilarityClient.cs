namespace codenames_solver;
public class SimilarityClient
{
    private readonly HttpClient httpClient;
    private readonly SimilarityGeneratorDTOBuilder dtoBuilder;

    public SimilarityClient(HttpClient httpClient, SimilarityGeneratorDTOBuilder DTOBuilder)
    {
        this.httpClient = httpClient;
        this.dtoBuilder = DTOBuilder;
    }
    public async Task<SimilarityGeneratorPostResponseDTO> RequestSimilarWords()
    {
        SimilarityGeneratorPostRequestDTO SimilarityPostBody = dtoBuilder.BuildSimilarityPostDTO();
        var response = await httpClient.PostAsJsonAsync("api/Similarity", SimilarityPostBody);
        response.EnsureSuccessStatusCode();
        var SimilarityPostResponse = await response.Content.ReadFromJsonAsync<SimilarityGeneratorPostResponseDTO>();
        if (SimilarityPostResponse is null)
        {
            throw new Exception("Null response when attempting to find similarities");
        }
        return SimilarityPostResponse;
    }
}
