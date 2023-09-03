using Microsoft.AspNetCore.Mvc;
using Word2vec.Tools;

namespace codenames_solver.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SimilarityController : ControllerBase
    {
        private Vocabulary _vocabulary;
        private ValidWords _validWords;

        public SimilarityController(Vocabulary vocabulary, ValidWords validWords) { 
            _vocabulary = vocabulary;
            _validWords = validWords;
        }

        [HttpPost]
        public string? Post([FromBody] List<CardInfo> CardsInfo)
        {
            string boy = "cat_NOUN";
            foreach (var card in CardsInfo)
            {
                var originalWord = _validWords.GetOriginalWord(card.Text);
                
                Console.WriteLine( originalWord + "here: ");
                
                if (originalWord != null)
                {
                    boy = originalWord;
                }

            }

            //string girl = "dog_NOUN";
            //string hamster = "hamster_NOUN";

            //int count = 50;

            //var additionRepresentation = (vocabulary[boy]).Add(vocabulary[girl]).Add(vocabulary[hamster]);
            //var closestAdditions = vocabulary.Distance(additionRepresentation, count);
            //foreach (var neightboor in closestAdditions)
            //    Console.WriteLine($"{neightboor.Representation.WordOrNull}\t\t{neightboor.DistanceValue}");


            //Console.WriteLine(additionRepresentation.WordOrNull);

            return boy;
        }
    }
}
