using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic.FileIO;
using PromoCode.Models;
using System.Diagnostics;
using System.IO;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Web;

namespace PromoCode.Controllers
{
    public class HomeController : Controller
    {
        public DBPromoCode dBPromoCode=new DBPromoCode();
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger,DBPromoCode dBPromoCode)
        {
            _logger = logger;
            this.dBPromoCode=dBPromoCode;
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
        public IActionResult index(string searchString)
        {
            var code = dBPromoCode.PromoCode.SingleOrDefault(s => s.name == searchString);
            if (code != null)
            {
                if (code.activaton == false)
                {
                    code.activationDate = DateTime.Now;
                    code.activaton = true;
                    //dBPromoCode.SaveChanges();
                    ViewData["Message"] = "Промокод "+searchString + "  активирован!";
                    return View();

                }
                return View();

            }
            else
            return View();
        }
      
        public IActionResult Exeption(string searchString)
        {
           
             ViewData["Message"] =  searchString ;
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {

            var counterParty = dBPromoCode.loginViewModels.SingleOrDefault(s => s.Login == loginViewModel.Login && s.Password == loginViewModel.Password);
            if (counterParty != null)
            {
                await AuthAsync(counterParty.id.ToString());
                return RedirectToAction(nameof(Privacy));
            }
            else
                return View(loginViewModel);
        }
     
        public IActionResult Privacy(int? page)
        {
            int pageSize = 20;
            page = page ?? 0;
            List<Models.PromoCode> promoCodes = new List<Models.PromoCode>();
            promoCodes = dBPromoCode.PromoCode.Where(s=>s.activaton==false).Skip(pageSize * page.Value).Take(pageSize).ToList();
            return View(new PagedList<Models.PromoCode> (page.Value, dBPromoCode.PromoCode.Count(), promoCodes, pageSize));
        }

        public void AddPromo(string name)
        {
            if (name != null)
            {
                Models.PromoCode promo = new Models.PromoCode();
                promo.name = name;
                promo.dataTimeCreated = DateTime.Now;
                promo.activaton = false;
                dBPromoCode.Add(promo);
                dBPromoCode.SaveChanges();
            }
        }

        public IActionResult activatedPromo(int? page)
        {
            int pageSize = 20;
            page = page ?? 0;
            List<Models.PromoCode> promoCodes = new List<Models.PromoCode>();
            promoCodes = dBPromoCode.PromoCode.Where(s=>s.activaton==true).Skip(pageSize * page.Value).Take(pageSize).ToList();
            return View(new PagedList<Models.PromoCode>(page.Value, dBPromoCode.PromoCode.Count(), promoCodes, pageSize));
        }
        

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        private async Task AuthAsync(string userId)
        {
            // создаем один claim
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userId)
            };
            // создаем объект ClaimsIdentity
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            // установка аутентификационных куки
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));

        }
        [HttpGet]
        public ActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddFile(IFormFile uploadedFile)
        {
            
            if (uploadedFile != null)
            {
                using (TextFieldParser parser = new TextFieldParser(uploadedFile.OpenReadStream()))
                {
                    parser.TextFieldType = FieldType.Delimited;
                    parser.SetDelimiters(",");
                    while (!parser.EndOfData)
                    {
                        //Processing row
                        string[] fields = parser.ReadFields();
                        foreach (string field in fields)
                        {
                            var PromoCodeDb = dBPromoCode.PromoCode.SingleOrDefault(s => s.name == field);
                            if(PromoCodeDb == null)
                            {
                                AddPromo(field);
                            }
                        }
                    }
                }

            }

            return RedirectToAction("Index");
        }

    }
}