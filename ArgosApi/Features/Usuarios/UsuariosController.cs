using Microsoft.AspNetCore.Mvc;

namespace ArgosApi.Features.Usuarios
{
    [ApiController]
    [Route("[controller]")]
    public class UsuariosController : ControllerBase
    {
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