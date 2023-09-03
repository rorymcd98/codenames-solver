using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace codenames_solver
{
    public class ValidWords
    {
        private Random random = new Random();
        public List<string> Words { get; set; }
        private List<string> GetJsonKeys(string path)
        {
            // Read JSON directly from a file
            using StreamReader file = File.OpenText(path);
            using JsonTextReader reader = new JsonTextReader(file);
            JObject jsonObject = (JObject)JToken.ReadFrom(reader);

            List<string> keys = new List<string>();
            foreach (KeyValuePair<string, JToken> pair in jsonObject)
            {
                keys.Add(pair.Key);
            }
            return keys;
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
            Words = GetJsonKeys(PATH);
        }

    }
}
