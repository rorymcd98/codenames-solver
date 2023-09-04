using Microsoft.AspNetCore.Mvc;
using Word2vec.Tools;

namespace codenames_solver.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SimilarityController : ControllerBase
    {
        private readonly Vocabulary _vocabulary;
        private readonly ValidWords _validWords;


        public SimilarityController(Vocabulary vocabulary, ValidWords validWords)
        {
            _vocabulary = vocabulary;
            _validWords = validWords;
        }

        [HttpPost]
        public SimilarityGeneratorPostResponseDTO Post([FromBody] SimilarityGeneratorPostRequestDTO similarityPostBody)
        {
            var similarityCodewordsGenerator = new SimilarityCodewordsGenerator(similarityPostBody, _vocabulary, _validWords);
            var similarityItems = similarityCodewordsGenerator.GenerateSimilarityItems();
            var similarityPostResponse = new SimilarityGeneratorPostResponseDTO { SimilarCodewords = similarityItems };
            return similarityPostResponse;
        }
    }
}
