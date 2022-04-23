using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebAppJsonCrud.Models;

namespace WebAppJsonCrud.Controllers
{
    public class UsersController : Controller
    {
        [HttpGet]
        public ActionResult GetAll()
        {
            var webClient = new WebClient();
            var json = webClient.DownloadString(@"http://localhost:5000/api/users");
            var objJson = JsonConvert.DeserializeObject<List<User>>(json);

            return View("GetAll", objJson);
        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string name, string emailAddress)
        {
            var user = new User
            {
                Name = name,
                EmailAddress = emailAddress
            };
            var objJson = JsonConvert.SerializeObject(user);

            using (var client = new HttpClient())
            {
                HttpResponseMessage result = await client.PostAsync("http://localhost:5000/api/users",
                     new StringContent(objJson, Encoding.UTF8, "application/json"));

            }

            return RedirectToAction("GetAll");
        }


        [HttpGet("{id}")]
        public IActionResult Delete(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            var webClient = new WebClient();
            var json = webClient.DownloadString(@"http://localhost:5000/api/users/" + id);
            var user = JsonConvert.DeserializeObject<User>(json);
            return View(user);
        }


        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.DeleteAsync(
                $"http://localhost:5000/api/users/{id}");
         
            var statusCode = response.StatusCode;
            if (statusCode == HttpStatusCode.OK)
                TempData["Success"] = $"Success, you have successfully deleted User (Id: {id})";
            return RedirectToAction("GetAll");
        }


        [HttpGet]
        public IActionResult Edit(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            var webClient = new WebClient();
            var json = webClient.DownloadString(@"http://localhost:5000/api/users/" + id);
            var user = JsonConvert.DeserializeObject<User>(json);
            return View(user);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(int id, string name, string emailAddress)
        {
            var user = new User
            {
                Name = name,
                EmailAddress = emailAddress ?? "test@test.pl"
            };
            var objJson = JsonConvert.SerializeObject(user);

            using (var client = new HttpClient())
            {
                HttpResponseMessage result = await client.PutAsync("http://localhost:5000/api/users/"+id,
                     new StringContent(objJson, Encoding.UTF8, "application/json"));
                var statusCode = result.StatusCode;
                if (statusCode == HttpStatusCode.OK)
                    TempData["Success"] = $"Success, you have successfully changed User (Id: {id})";
                
            }
           
            return RedirectToAction("Edit");
        }

    }
}
