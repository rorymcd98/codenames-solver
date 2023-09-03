using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace codenames_solver
{
    public class ValidWords
    {
        private Random random = new Random();
        private JObject jsonWords;
        public List<string> Words { get; set; }

        private JObject GetJsonWords(string path)
        {
            using StreamReader file = File.OpenText(path);
            using JsonTextReader reader = new JsonTextReader(file);
            return (JObject)JToken.ReadFrom(reader);
        }
        private List<string> GetJsonKeys()
        {
            List<string> keys = new List<string>();
            foreach (KeyValuePair<string, JToken> pair in jsonWords)
            {
                keys.Add(pair.Key);
            }
            return keys;
        }
        public string GetOriginalWord(string formattedWord)
        {
            JToken value;
            if (!jsonWords.TryGetValue(formattedWord, out value))
                return null;
            return value.ToString();

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
