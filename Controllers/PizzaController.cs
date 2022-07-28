using DominosPizzaMvc.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace DominosPizzaMvc.Controllers
{
    public class PizzaController : Controller
    {
        string Baseurl = "https://localhost:7194/";
        public async Task<IActionResult> GetAllPizza()
        {
            List<Pizza> pizzas = new List<Pizza>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await client.GetAsync("api/Pizzas");
                if (Res.IsSuccessStatusCode)
                {
                    var ProdResponse = Res.Content.ReadAsStringAsync().Result;
                    pizzas = JsonConvert.DeserializeObject<List<Pizza>>(ProdResponse);
                }
                return View(pizzas);
            }
        }

        public IActionResult AddPizza()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddPizza(Pizza pizza)
        {
            Pizza pizzaobj = new Pizza();
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(Baseurl);
                StringContent content = new StringContent(JsonConvert.SerializeObject(pizza), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PostAsync("api/Pizzas", content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    pizzaobj = JsonConvert.DeserializeObject<Pizza>(apiResponse);
                }
            }
            return RedirectToAction("GetAllPizza");
        }

        [HttpGet]
        public async Task<IActionResult> Details(int PizzaId)
        {
            TempData["Pizzaid"] = PizzaId;
            Pizza pizzaobj = new Pizza();
            using (var httpClient = new HttpClient())
            {


                using (var response = await httpClient.GetAsync("https://localhost:7194/api/Pizzas/" + PizzaId))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    pizzaobj = JsonConvert.DeserializeObject<Pizza>(apiResponse);
                }
            }
            return View(pizzaobj);
        }



        [HttpGet]
        public async Task<IActionResult> DeletePizza(int PizzaId)
        {
            TempData["Pizzaid"] = PizzaId;
            Pizza pizzaobj = new Pizza();
            using (var httpClient = new HttpClient())
            {


                using (var response = await httpClient.GetAsync("https://localhost:7194/api/Pizzas/" + PizzaId))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    pizzaobj = JsonConvert.DeserializeObject<Pizza>(apiResponse);
                }
            }
            return View(pizzaobj);
        }

        [HttpPost]

        public async Task<IActionResult> DeletePizza(Pizza pizza)
        {
            int pizzaid = Convert.ToInt32(TempData["Pizzaid"]);
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(Baseurl);

                using (var response = await httpClient.DeleteAsync("api/Pizzas/" + pizzaid))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();

                }
            }
            return RedirectToAction("GetAllPizza");
        }



        [HttpGet]
        public async Task<IActionResult> EditPizza(int PizzaId)
        {
            TempData["Pizzaid"] = PizzaId;
            Pizza pizzaobj = new Pizza();
            using (var httpClient = new HttpClient())
            {


                using (var response = await httpClient.GetAsync("https://localhost:7194/api/Pizzas/" + PizzaId))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    pizzaobj = JsonConvert.DeserializeObject<Pizza>(apiResponse);
                }
            }
            return View(pizzaobj);
        }

        [HttpPost]

        public async Task<IActionResult> EditPizza(Pizza pizza)
        {
            Pizza obj = new Pizza();
            using (var httpClient = new HttpClient())
            {
                int pizzaid = pizza.PizzaId;
                StringContent content = new StringContent(JsonConvert.SerializeObject(pizza), Encoding.UTF8, "application/json");


                using (var response = await httpClient.PutAsync("https://localhost:7194/api/Pizzas/" + pizzaid, content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    ViewBag.Result = "sucess";
                    obj = JsonConvert.DeserializeObject<Pizza>(apiResponse);

                }
            }
            return RedirectToAction("GetAllPizza");
        }






    }
}
