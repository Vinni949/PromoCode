namespace PromoCode.Models
{
    public class LoginViewModel
    {
        public int id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string? qr { get; set; }
    }
}
