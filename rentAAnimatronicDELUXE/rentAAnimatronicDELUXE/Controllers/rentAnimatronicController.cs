using Microsoft.AspNetCore.Mvc;
using rentAAnimatronicDELUXE.Models;
using System.Net.Http.Json;
using Polly;
using Polly.Caching;
using Polly.Caching.Memory;

namespace rentAAnimatronicDeluxe.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class rentAnimatronicController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly AsyncCachePolicy _policy;

        public rentAnimatronicController(IHttpClientFactory httpClientFactory, AsyncCachePolicy policy)
        {
            _httpClientFactory = httpClientFactory;
            _policy = policy;
        }

        [HttpGet("/SearchById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var context = new Context("animatronic-" + id +"");

            var verySpecific = await _policy.ExecuteAsync(async (ctx) =>
            {
                var client = _httpClientFactory.CreateClient("InventoryCheck");
                var response = await client.GetAsync("/inventory");

                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                var specific = await response.Content.ReadFromJsonAsync<List<tronicResult>>();
                return specific?.FirstOrDefault(x => x.id == id);
            }, context);

            if (verySpecific == null)
            {
                return NotFound("Animatronic Nao Existe");
            }

            return Ok(verySpecific);
        }

        [HttpGet("/SearchGeneric")]
        public async Task<IActionResult> GetGeneric()
        {
            var context = new Context("inventoryGeneric");
            //Esta linha debaixo diz ao Polly para esperar e ver se tem a memora do GET ja ter sido feito; 
            //Se sim ele ignora o codigo e faz o que vem depois. Se nao ele executa o codigo.
            var listFound = await _policy.ExecuteAsync(async (ctx) =>
            {
                var client = _httpClientFactory.CreateClient("InventoryCheck");
                var response = await client.GetAsync("/inventory");

                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                return await response.Content.ReadFromJsonAsync<List<tronicResult>>();
            }, context);

            if (listFound == null)
            {
                return StatusCode(502, "Resposta invalida");
            }

            return Ok(listFound);
        }

        [HttpPut("/UpdateStatus/{id}")]
        public async Task<IActionResult> UpdateStatus(int id)
        {
            var client = _httpClientFactory.CreateClient("InventoryCheck");

            var response = await client.PutAsync("/inventory/unavailable", null);
            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode, "Não foi possível contactar o imposter.");
            }
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return StatusCode((int)response.StatusCode, "Não foi possível encontrar o animatronico.");
            }
;           var found = await response.Content.ReadFromJsonAsync<List<tronicResult>>();
            if (found == null)
            {
                return NotFound("Animatronic nao existe");
            }

            var foundNUpdated = found.FirstOrDefault(x => x.id == id);

            return Ok(foundNUpdated);
        }
    }
}
