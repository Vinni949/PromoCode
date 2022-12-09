using MessagingToolkit.QRCode.Codec;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;
using PromoCode.Models;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using static System.Net.Mime.MediaTypeNames;

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
        public byte[] QRGeneration(string sringQR)
        {
            QRCodeEncoder encoder = new QRCodeEncoder();
            Bitmap qrcode = encoder.Encode(sringQR);
            ImageConverter _imageConverter = new ImageConverter();
            byte[] xByte = (byte[])_imageConverter.ConvertTo(qrcode, typeof(byte[]));

            /*string returnString = "";
            foreach(var x in xByte)
            {
                returnString += x;
            }*/
            return xByte;
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
                if (dbCode.extradition == true)
                {
                    if (dbCode.activaton == false)
                    {
                        return RedirectToAction("Index", "Home", new { searchString = promo });
                    }
                    else
                    { exeption = "Промо код: " + promo + " был активирован ранее!" + "\n" + "Дата активации: " + dbCode.activationDate; }

                }
                else
                { exeption = "Промо код: " + promo + " не выдавался"; }

            }
            else
            { exeption = "Промо код: " + promo + " не существует!"; }
            return RedirectToAction("Exeption", "Home", new {searchString= exeption});
        }
        [HttpGet("Extradition")]
        //выдача qr
        public string Extradition()
        {
            DateTime dataTime = DateTime.Now;
            var qr = dBPromoCode.PromoCode.Where(p => p.extraditionDate <= dataTime.AddHours(-1)||p.extradition==false).ToList();
            qr[0].extradition = true;
            qr[0].extraditionDate = DateTime.Now;
            dBPromoCode.SaveChanges();
            return qr[0].name;
        }

       /* [HttpGet("AddState")]
        //выдача qr
        public void AddState()
        {
            foreach (var qr in dBPromoCode.PromoCode)
            {
                qr.extradition = false;
            }
            dBPromoCode.SaveChanges();
            
        }
       */
    }
}
