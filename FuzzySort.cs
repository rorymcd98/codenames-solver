using FuzzySharp;
public class FuzzySort
{
    private static readonly Func<string, IEnumerable<string>, int, List<string>> s_fuzzyListFactory = (input, words, limit) => words
        .Select(word => new { Word = word, Score = Fuzz.PartialRatio(input, word) })
        .OrderByDescending(x => x.Score)
        .Take(limit)
        .Select(x => x.Word)
        .ToList();

    public static List<string> GetFuzzyList(string input, IEnumerable<string> words, int limit = 30)
    {
        return s_fuzzyListFactory.Invoke(input, words, limit);
    }
}
