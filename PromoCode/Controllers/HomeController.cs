using Microsoft.AspNetCore.Mvc;
using PromoCode.Models;
using System;
using System.Diagnostics;

namespace PromoCode.Controllers
{
    public class HomeController : Controller
    {
        public DBPromoCode dBPromoCode=new DBPromoCode();
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        public IActionResult Index(string searchString)
        {
            if (searchString != null)
            {
                index(searchString);
            }
            return View();
        }
        [HttpPost]
        public string index(string searchString)
        {
            var code = dBPromoCode.PromoCode.SingleOrDefault(s => s.name == searchString);
            if (code != null)
            {
                if (code.activaton == false)
                {
                    code.activationDate = DateTime.Now;
                    code.activaton = true;
                    dBPromoCode.SaveChanges();
                    return "Активирован промокод";
                }
                return "Промокод активирова ранее!";
            }
            return "Помокода не существует";
        }

        public IActionResult Privacy(string name)
        {
            if (name != null)
            {
                Models.PromoCode promo = new Models.PromoCode();
                promo.name = name;
                promo.dataTimeCreated = DateTime.Now;
                promo.activaton = false;
                dBPromoCode.Add(promo);
                dBPromoCode.SaveChanges();
                return View(promo);
            }
            return View(new string("asd"));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}