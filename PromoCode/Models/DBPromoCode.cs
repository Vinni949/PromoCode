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
           
            optionsBuilder.UseSqlServer("Server = (localdb)\\mssqllocaldb; Database = BdPromoCode; Trusted_Connection = True;");
        }


    }
}


