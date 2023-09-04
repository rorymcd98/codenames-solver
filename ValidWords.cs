using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace codenames_solver
{
    public class ValidWords
    {
        private readonly Random random = new();
        private readonly JObject jsonWords;
        public List<string> Words { get; set; }

        private JObject GetJsonWords(string path)
        {
            using StreamReader file = File.OpenText(path);
            using JsonTextReader reader = new(file);
            return (JObject)JToken.ReadFrom(reader);
        }
        private List<string> GetJsonKeys()
        {
            List<string> keys = new();
            keys.AddRange(from KeyValuePair<string, JToken> pair in jsonWords
                          select pair.Key);
            return keys;
        }
        public string GetOriginalWord(string formattedWord)
        {
            jsonWords.TryGetValue(formattedWord, out JToken? value);
            return value is not null ? value.ToString() : string.Empty;
        }

        public bool IsValidWord(string word)
        {
            return Words.Contains(word);
        }

        public string GetRandomWord()
        {
            int index = random.Next(Words.Count);
            return Words[index];
        }

        const string PATH = @"C:\Users\rorym\OneDrive\Desktop\Word2vec\nlpl\words.json";
        public ValidWords()
        {
            jsonWords = GetJsonWords(PATH);
            Words = GetJsonKeys();
        }

    }
}
