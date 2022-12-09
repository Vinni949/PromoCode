using System;

namespace PromoCode.Models
{
    public class PromoCode
    {
        public int id { get; set; }
        public string name { get; set; }
        public DateTime dataTimeCreated { get; set; }
        public DateTime? activationDate { get; set; }
        public Boolean activaton { get; set; }
        public Boolean? extradition { get; set; }
    }
}
