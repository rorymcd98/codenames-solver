namespace codenames_solver;
public class SimilarityClient
{
    private readonly HttpClient httpClient;

    public SimilarityClient(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }
    public async Task<string> RequestWords(string word)
    {
        var response = await httpClient.PostAsJsonAsync("api/Values", word);
        response.EnsureSuccessStatusCode();
        var orderId = await response.Content.ReadAsStringAsync();
        return orderId;
    }
}