﻿using Microsoft.AspNetCore.Mvc;
using Word2vec.Tools;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace codenames_solver.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private Vocabulary _vocabulary;
        private ValidWords _validWords;

        public ValuesController(Vocabulary vocabulary, ValidWords validWords) { 
            _vocabulary = vocabulary;
            _validWords = validWords;
        }

        [HttpPost]
        public string? Post([FromBody] string value)
        {
            string boy = "cat_NOUN";
            foreach (var word in  _validWords.Words)
            {
                Console.WriteLine(word);
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
