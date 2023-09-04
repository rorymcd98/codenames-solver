using FuzzySharp;
public class FuzzySort
{
    private static Func<string, IEnumerable<string>, int, List<string>> FuzzyListFactory = (input, words, limit) => words
        .Select(word => new { Word = word, Score = Fuzz.PartialRatio(input, word) })
        .OrderByDescending(x => x.Score)
        .Take(limit)
        .Select(x => x.Word)
        .ToList();

    public static List<string> GetFuzzyList(string input, IEnumerable<string> words, int limit = 30)
    {
        return FuzzyListFactory.Invoke(input, words, limit);
    }
}
