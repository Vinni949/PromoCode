using MessagingToolkit.QRCode.Codec;
using Microsoft.AspNetCore.Mvc;
using PromoCode.Models;
using System.Drawing;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PromoCode.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class APIController : ControllerBase
    {
        public DBPromoCode dBPromoCode = new DBPromoCode();

        // GET: api/<APIController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // /api/QRGeneration?sringQR=
        [HttpGet("QRGeneration")]
        public byte[] QRGeneration(string str, string sringQR)
        {
            //string key = str;
            string key = '"' + str + '"';
            string[] promo = sringQR.Split('=');
            string[] code = promo[1].Split('&');
            var db = dBPromoCode.PromoCode.SingleOrDefault(p =>p.name == code[0]);
            if (db.keySessions==key)
            {
                QRCodeEncoder encoder = new QRCodeEncoder();
                Bitmap qrcode = encoder.Encode(sringQR);
                ImageConverter _imageConverter = new ImageConverter();
                byte[] xByte = (byte[])_imageConverter.ConvertTo(qrcode, typeof(byte[]));
                db.qrString = xByte.ToString();
                dBPromoCode.SaveChanges();
                return xByte;
            }
            else
            {
                byte[] s = System.Text.Encoding.UTF8.GetBytes("Exeption");
                return s;
            }

        }

        // /api/RedirectToPage?promo=asdqsd
        // /api/RedirectToPage?promo=QK21509
        [HttpGet("RedirectToPage")]
        public IActionResult Redirect(string promo, string? str)
        {
            if (HttpContext.User.Identity.Name == null)
            {
                HttpContext.Session.SetString("url", promo);
                return RedirectToAction("login", "Home");
            }
            else
            {
                string exeption = "";
                var dbCode = dBPromoCode.PromoCode.SingleOrDefault(p => p.name == promo);
                if (dbCode != null)
                {
                    if (dbCode.extradition == true)
                    {
                        if (dbCode.qrString != null)
                        {
                            if (dbCode.activaton == false)
                            {
                                return RedirectToAction("Index", "Home", new { searchString = promo });
                            }
                            else
                            { exeption = "Промо код: " + promo + " был активирован ранее!" + "\n" + "Дата активации: " + dbCode.activationDate; }
                        }
                        else
                        {
                            exeption = "Промо код: " + promo + " не оплачен! " ;
                        }
                    }
                    else
                    { exeption = "Промо код: " + promo + " не выдавался"; }

                }
                else
                { exeption = "Промо код: " + promo + " не существует!"; }
                return RedirectToAction("Exeption", "Home", new { searchString = exeption });

            }


        }

        [HttpGet("Extradition")]
        public string Extradition(string key)
        {
            var keyses = HttpContext.Session;

            if (key != null && key.Length >= 32)
            {
                var keyqr = dBPromoCode.PromoCode.SingleOrDefault(p => p.keySessions == key);
                if (keyqr == null)
                {
                    DateTime dataTime = DateTime.Now;
                    var qr = dBPromoCode.PromoCode.Where(p =>  p.extradition ==false).ToList();
                    qr[0].extradition = true;
                    qr[0].extraditionDate = DateTime.Now;
                    qr[0].keySessions = key;
                    dBPromoCode.SaveChanges();
                    return qr[0].name;
                }
                else
                    return keyqr.name;

            }
            else
                return "exeption";
        }

        /*[HttpGet("dell")]
        public void Dell()
        {
           var promoCodes = dBPromoCode.PromoCode.Where(p => p.extradition == false).ToList();
            foreach(var item in promoCodes)
            {
                dBPromoCode.PromoCode.Remove(item);
            }
            dBPromoCode.SaveChanges();
        }*/

        [HttpGet("DellPromo")]
        public void DellPromo(string name,string key)
        {
            if (key == "522Vin")
            {
                var promoCodes = dBPromoCode.PromoCode.SingleOrDefault(p => p.name == name);
                dBPromoCode.PromoCode.Remove(promoCodes);
                dBPromoCode.SaveChanges();
            }
        }
    }
}
