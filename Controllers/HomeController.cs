using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Net.Http;

namespace quotingdojo.Controllers
{
    public class HomeController : Controller
    {
        private readonly DbConnector _dbConnector;

        public HomeController(DbConnector connect)
        {
            _dbConnector = connect;
        }

        // GET: /Home/
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            List<Dictionary<string, object>> AllQuotes = _dbConnector.Query("SELECT * FROM quotes");
            return View();
        }

        [HttpPost]
        [Route("quotes")]
        public IActionResult SubmitQuote(string quoteField, string nameField)
        {
            try
            {
                string query = $"INSERT INTO quotes (quotetext, created_at, updated_at, user) VALUES (\"{quoteField}\", NOW(), NOW(), \"{nameField}\")";
                _dbConnector.Execute(query);
                return RedirectToAction("Quotes");
            }
            catch
            {
                ViewBag.error = "There was an error! Try again.";
                return View("Index");
            }
            
        }

        [HttpGet]
        [Route("quotes")]
        public IActionResult Quotes()
        {
            List<Dictionary<string, object>> AllQuotes = _dbConnector.Query("SELECT * FROM quotes ORDER BY created_at DESC");
            ViewBag.allQuotes = AllQuotes;
            return View();
        }
    }
}
