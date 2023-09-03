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


        public SimilarityController(Vocabulary vocabulary, ValidWords validWords)
        {
            _vocabulary = vocabulary;
            _validWords = validWords;
        }

        [HttpPost]
        public SimilarityPostResponseDTO Post([FromBody] SimilarityPostDTO similarityPostBody)
        {
            SimilarityPostResponseDTO similarityResponse;

            var similarityCodewordsGenerator = new SimilarityCodewordsGenerator(similarityPostBody, _vocabulary, _validWords);
            var similarityItems = similarityCodewordsGenerator.GenerateSimilarityItems();
            var similarityPostResponse = new SimilarityPostResponseDTO(similarityItems);
            return similarityPostResponse;
        }
    }
}
