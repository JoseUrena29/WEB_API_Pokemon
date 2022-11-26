using m = API_Pokemon.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using API_Pokemon.Models;
using System.IO;

namespace API_Pokemon.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }


        public async Task<IActionResult> Index()
        {
            m.PokemonList results = null;
            string apiResponse = "";

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://pokeapi.co/api/v2/pokemon?limit=151"))
                {
                    apiResponse = await response.Content.ReadAsStringAsync();
                    results = JsonConvert.DeserializeObject<m.PokemonList>(apiResponse);
                }
            }

            foreach (Pokemon pokemon in results.results)
            {

                using (var httpClient2 = new HttpClient())
                {
                    using (var response = await httpClient2.GetAsync(pokemon.url))
                    {
                        apiResponse = await response.Content.ReadAsStringAsync();
                        results = JsonConvert.DeserializeObject<m.PokemonList>(apiResponse);
                    }
                }

            }

            ViewBag.PokemonList = results;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new m.ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}