using FuzzySharp;
public interface IFuzzySort
{
    List<string> GetFuzzyList(string input, IEnumerable<string> words, int limit = 30);
}

public class FuzzySort : IFuzzySort
{
    public List<string> GetFuzzyList(string input, IEnumerable<string> words, int limit = 30)
    {
        return words.Select(word => new { Word = word, Score = Fuzz.PartialRatio(input, word) })
                    .OrderByDescending(x => x.Score)
                    .Take(limit)
                    .Select(x => x.Word)
                    .ToList();
    }
}