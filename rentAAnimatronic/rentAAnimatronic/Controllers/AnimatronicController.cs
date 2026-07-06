using Microsoft.AspNetCore.Mvc;
using rentAAnimatronic.Models;
using System.Collections.Generic;

namespace rentAAnimatronic.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class AnimatronicController : Controller
    {
        private static List<tronicResult> animatronicList = new List<tronicResult>();

        [HttpPost]
        public JsonResult PostAnimatronicRequest(tronicRequest freddy)
        {
            var animatro = new tronicResult()
            {
                Id = animatronicList.Count + 1,
                price = freddy.price,
                Availability = freddy.Availability,
                AnimatronicName = freddy.AnimatronicName,
            };

            animatronicList.Add(animatro);

            return new JsonResult(freddy);
        }

        [HttpGet]
        public JsonResult GetAnimatronicList()
        {
            return new JsonResult(animatronicList);
        }

        [HttpPut ("update/{Id}")]
        public JsonResult UpdateAnimaTronic(int Id)
        {
            var foundYou = animatronicList.FirstOrDefault(x => x.Id == Id);

            if (foundYou != null)
            {
                if (foundYou.Availability == "Free")
                {
                    foundYou.Availability = "Rented";
                } else
                {
                    foundYou.Availability = "Free";
                }
                    
                return new JsonResult("Found it!");
            } else
            {
                return new JsonResult("Maybe it never existed!");
            }

        }

    }
    
}
