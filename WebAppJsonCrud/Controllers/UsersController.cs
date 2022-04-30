using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebAppJsonCrud.Models;

namespace WebAppJsonCrud.Controllers
{
    public class UsersController : Controller
    {
        public async Task<ActionResult> GetAllAsync(string sortOrder)
        {
            using (var client = new HttpClient())
            {
                var json = await client.GetStringAsync(@"http://localhost:5000/api/users");
                var usersList = JsonConvert.DeserializeObject<List<User>>(json);
                var users = from u in usersList select u;
                ViewBag.IDSortParm = String.IsNullOrEmpty(sortOrder) ? "id_desc" : "";
                ViewBag.NameSortParm = sortOrder == "Name" ? "name_desc" : "Name";

                switch (sortOrder)
                {
                    case "id_desc":
                        users = users.OrderByDescending(u => u.Id);
                        break;

                    case "Name":
                        users = users.OrderBy(u => u.Name);
                        break;
                    case "name_desc":
                        users = users.OrderByDescending(u => u.Name);
                        break;
                    default:
                        users = users.OrderBy(u => u.Id);
                        break;
                }
                return View("GetAll", users.ToList());
            }
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
                return RedirectToAction("GetAll");
            }
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
                TempData["SuccessDeleted"] = "Success, you have successfully deleted User";
            return RedirectToAction("GetAll");
        }


        [HttpGet]
        public IActionResult Edit(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            using (var webClient = new WebClient())
            {
                var json = webClient.DownloadString(@"http://localhost:5000/api/users/" + id);
                var user = JsonConvert.DeserializeObject<User>(json);
                return View(user);
            }
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


        public ActionResult SearchForm()
        {
            return View();
        }

   
        public async Task<IActionResult> Search(string searchPhrase)
        {
           
            var webClient = new WebClient();
            var json = webClient.DownloadString(@"http://localhost:5000/api/users/");
      
            List<User>usersList = JsonConvert.DeserializeObject<List<User>>(json);

            var newList = usersList.Where(u => u.Name.ToLower().Contains(searchPhrase.Trim().ToLower()));
            
            if(newList.Count() != 0)
            {
                return View("GetAll", newList);
            }

            TempData["NotFound"] = "User not found";
            return View("GetAll", newList);
        }
    }
}
