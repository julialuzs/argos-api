using Microsoft.AspNetCore.Mvc;

namespace ArgosApi.Features.Projetos
{
    [ApiController]
    [Route("[controller]")]
    public class ProjetosController : ControllerBase
    {
        public ProjetosController(
            
        )
        {
            
        }

        [HttpGet]
        public ActionResult<string> Get()
        {
            return "";
        }

        [HttpPost]
        public ActionResult<string> Post([FromBody] string value)
        {
            return "";
        }

        [HttpPut("{id}")]
        public ActionResult<string> Put(int id, [FromBody] string value)
        {
            return "";
        }

        [HttpDelete("{id}")]
        public ActionResult<string> Delete(int id)
        {
            return "";
        }
    }
}