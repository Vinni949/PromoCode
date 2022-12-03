using Microsoft.EntityFrameworkCore;

namespace PromoCode.Models
{
    public class DBPromoCode : DbContext
    {

        public DBPromoCode()
        {
        }

        public DBPromoCode(DbContextOptions<DBPromoCode> options) : base(options)
        {

        }
        public DbSet<PromoCode> PromoCode { get; set; }
        public DbSet<LoginViewModel> loginViewModels { get; set; }
       
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            List<string> s = new List<string>();
            StreamReader f = new StreamReader("test.txt");
            while (!f.EndOfStream)
            {
                s.Add(f.ReadLine());
            }
            optionsBuilder.UseSqlServer(s[2]);
        }


    }
}


