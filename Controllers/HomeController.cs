using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Diagnostics;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        private MongoClient client = new MongoClient("mongodb://127.0.0.1:27017/");

        public IActionResult Submit(IFormCollection data)
        {
            Console.WriteLine("Submit");
            ViewBag.Name = data["Name"];
            ViewBag.Country = data["Country"];
            string name = data["Name"].ToString();
            string country = data["Country"].ToString();
            var database = client.GetDatabase("users");
            var table = database.GetCollection<BsonDocument>("user_country");
            var user_identity = new BsonDocument
            {
                {"Name", name},
                {"Country",country}
            };
            table.InsertOne(user_identity);
            return View("Index");
        }
        public IActionResult Index()
        {
            Console.WriteLine("Index");
            return View();
        }


        public IActionResult Privacy()
        {
            var database = client.GetDatabase("users");
            var table = database.GetCollection<BsonDocument>("user_country");
            var list_of_users = table.Find(new BsonDocument()).ToList();
            //var docs = table.Find(new BS).ToList();
            BsonDocument t = new BsonDocument();
            List<UserData> termsList = new List<UserData>();
            list_of_users.ForEach(doc =>
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