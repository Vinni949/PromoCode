using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic.FileIO;
using PromoCode.Models;
using System.Diagnostics;
using System.Security.Claims;


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
        public IActionResult Login()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            List<string> s = new List<string>();
            StreamReader f = new StreamReader("test.txt");
            while (!f.EndOfStream)
            {
                s.Add(f.ReadLine());
            }

            if (s[0] == loginViewModel.Login && s[1] == loginViewModel.Password)
            {
                await AuthAsync(loginViewModel.Login);
                return RedirectToAction(nameof(activatedPromo));
            }
            else
                return View(loginViewModel);
        }
        [Authorize]
        public IActionResult Index(string searchString)
        {
            if (searchString != null)
            {
                index(searchString);
            }
            return View();
        }
        [Authorize]
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
                    return View(code);

                }
                return View();

            }
            else
            return View();
        }
        [Authorize]
        public IActionResult Exeption(string searchString)
        {
           
             ViewData["Message"] =  searchString ;
            return View();
        }
        [Authorize]
        public IActionResult Privacy(int? page)
        {
            int pageSize = 20;
            page = page ?? 0;
            List<Models.PromoCode> promoCodes = new List<Models.PromoCode>();
            promoCodes = dBPromoCode.PromoCode.Where(s=>s.activaton==false).Skip(pageSize * page.Value).Take(pageSize).ToList();
            return View(new PagedList<Models.PromoCode> (page.Value, dBPromoCode.PromoCode.Count(), promoCodes, pageSize));
        }
        [Authorize]
        public void AddPromo(string name)
        {
            if (name != null)
            {
                Models.PromoCode promo = new Models.PromoCode();
                promo.name = name;
                promo.dataTimeCreated = DateTime.Now;
                promo.activaton = false;
                promo.extradition = false;
                dBPromoCode.Add(promo);
                dBPromoCode.SaveChanges();
            }
        }
        [Authorize]
        public IActionResult activatedPromo(int? page,string? message)
        {
            if(message!=null)
            {
               @ViewData["Message"]=message;
            }
            int pageSize = 20;
            page = page ?? 0;
            List<Models.PromoCode> promoCodes = new List<Models.PromoCode>();
            promoCodes = dBPromoCode.PromoCode.Where(s=>s.activaton==true).Skip(pageSize * page.Value).Take(pageSize).ToList();
            return View(new PagedList<Models.PromoCode>(page.Value, dBPromoCode.PromoCode.Count(), promoCodes, pageSize));
        }
        [Authorize]
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
        [Authorize]
        [HttpGet]
        public ActionResult Upload()
        {
            return View();
        }
        [Authorize]
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
        //https://localhost:7232/api/RedirectToPage?promo=QKWZHH7
        [HttpPost]
        public IActionResult ActivatedPromo(string name)
        {
            var code = dBPromoCode.PromoCode.SingleOrDefault(p => p.name == name);
            if (code != null)
            {
                if (code.activaton == false)
                {
                    code.activationDate = DateTime.Now;
                    code.activaton = true;
                    dBPromoCode.SaveChanges();
                    return RedirectToAction("activatedPromo", "Home", new { message = "Промокод: "+ code.name+" активирован!" });
                }
            }
            return View();
        }
    }
}