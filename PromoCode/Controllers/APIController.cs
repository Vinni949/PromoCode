using MessagingToolkit.QRCode.Codec;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;
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
        public System.Drawing.Bitmap QRGeneration(string sringQR)
        {
            QRCodeEncoder encoder = new QRCodeEncoder();
            Bitmap qrcode = encoder.Encode(sringQR);
            return qrcode;
        }

        // /api/RedirectToPage?promo=asdqsd
        // /api/RedirectToPage?promo=QK21509
        [HttpGet("RedirectToPage")]
        public IActionResult Redirect(string promo)
        {
            string exeption = "";
            var dbCode = dBPromoCode.PromoCode.SingleOrDefault(p => p.name == promo);
            if (dbCode != null)
            {
                if (dbCode.activaton == false)
                {
                    return RedirectToAction("Index", "Home", new { searchString = promo });
                }
                else
                { exeption = "Промо код: " + promo + " был активирован ранее!"+"\n" +"Дата активации: " + dbCode.activationDate; }

            }
            else
            { exeption = "Промо код: " + promo + " не существует!"; }
            return RedirectToAction("Exeption", "Home", new {searchString= exeption});
        }

        // PUT api/<APIController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<APIController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
