using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace codenames_solver.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // POST api/<ValuesController>
        [HttpPost]
        public string Post([FromBody] string value)
        {
            return "value1";
        }
    }
}
