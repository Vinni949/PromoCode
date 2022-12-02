using MessagingToolkit.QRCode.Codec;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;
using System.Drawing;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PromoCode.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class APIController : ControllerBase
    {
        // GET: api/<APIController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<APIController>/5
        [HttpGet("QRGeneration")]
        public System.Drawing.Bitmap QRGeneration(string sringQR)
        {
            QRCodeEncoder encoder = new QRCodeEncoder();
            Bitmap qrcode = encoder.Encode(sringQR);
            return qrcode;
        }

        // POST api/<APIController>
        [HttpPost("Redirect")]
        public IActionResult Redirect()
        {
            return RedirectToAction("Index");

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
