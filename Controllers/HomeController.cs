using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Diagnostics;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class HomeController : Controller
    {
        private readonly UsersService _userService;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, UsersService userService)
        {
            _logger = logger;
            _userService = userService;
        }
        

        public async Task<IActionResult> Submit(IFormCollection data)
        {
            Console.WriteLine("Submit");
            ViewBag.Name = data["Name"];
            ViewBag.Country = data["Country"];
            string name = data["Name"].ToString();
            string country = data["Country"].ToString();
            /*var database = client.GetDatabase("users");
            var table = database.GetCollection<BsonDocument>("user_country");*/
            var userIdentity = new BsonDocument
            {
                {"Name", name},
                {"Country",country}
            };
            await _userService.Create(userIdentity);
            //table.InsertOne(user_identity);
            return View("Index");
        }
        public IActionResult Index()
        {
            Console.WriteLine("Index");
            return View();
        }


        public async Task<IActionResult> Privacy()
        {
            /*var database = client.GetDatabase("users");
            var table = database.GetCollection<BsonDocument>("user_country");*//*

            var list_of_users = table.Find(new BsonDocument()).ToList();
            //var docs = table.Find(new BS).ToList();*/

            var listOfUsers = await _userService.Get(); 
            BsonDocument t = new BsonDocument();
            List<UserData> termsList = new List<UserData>();
            listOfUsers.ForEach(doc =>
            {
                Console.WriteLine(doc);
                t = doc;
                var userData = new UserData()
                {
                    Name = t["Name"].ToString(),
                    Country = t["Country"].ToString(),
                };
                termsList.Add(userData);
            });
            UserData[] terms = termsList.ToArray();
            Console.Write(terms);
            return View(terms);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}