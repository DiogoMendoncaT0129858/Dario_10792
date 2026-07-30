using Microsoft.AspNetCore.Mvc;
using RentAAnimatronicDeluxe.Models;

namespace RentAAnimatronicDeluxe.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class rentAnimatronicController : ControllerBase
    {
        public static List<tronicRequest> listaTronic = new List<tronicRequest>();
        /*{
            new tronicRequest
            {
                id = 1,
                name = "Fredbear",
                ctg = "Relic",
                prc = 5000,
                desc = "The original star of the show! Big yellow and lovely, Fredbear",
                available = true
            },
            new tronicRequest
            {
                id = 2,
                name = "SpringBonnie",
                ctg = "Relic",
                prc = 5000,
                desc = "Fredbear's best friend and a cracker at guittar, Springbonnie!",
                available = true
            }
        };*/

        [HttpPost("/CreatePage")]
        public JsonResult CreatePage(tronicRequest tronic)
        {
            var added = new tronicRequest
            {
                id = listaTronic.Count + 1,
                name = tronic.name,
                ctg = tronic.ctg,
                prc = tronic.prc,
                desc = tronic.desc,
                available = tronic.available
            };
            listaTronic.Add(added);
            return new JsonResult(added);
        }

        [HttpGet("/SearchById/{id}")]
        public JsonResult GetById(int id)
        {
            var specific = listaTronic.FirstOrDefault(x => x.id == id);
            return new JsonResult(specific);
        }

        [HttpPut("/UpdateAvailability/{id}")]
        public JsonResult UpdateAvailability(int id, int preco, bool avail)
        {
            var found = listaTronic.FirstOrDefault(x => x.id == id);

            if (found == null)
            {
                return new JsonResult(NotFound("Id Not Found, Try Again"));
            }

            found.prc = preco;
            found.available = avail;

            return new JsonResult(found);
        }
    }
}
