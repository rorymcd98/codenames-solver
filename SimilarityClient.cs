namespace codenames_solver;
public class SimilarityClient
{
    private readonly HttpClient httpClient;
    private CardsState cardsState;

    public SimilarityClient(HttpClient httpClient, CardsState CardsState)
    {
        this.httpClient = httpClient;
        this.cardsState = CardsState;
    }
    public async Task<string> RequestSimilarWords()
    {
        var response = await httpClient.PostAsJsonAsync("api/Similarity", cardsState.cards);
        response.EnsureSuccessStatusCode();
        var orderId = await response.Content.ReadAsStringAsync();
        return orderId;
    }
}