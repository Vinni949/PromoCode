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
           
            optionsBuilder.UseSqlServer("workstation id=DbPromoCode.mssql.somee.com;packet size=4096;user id=LLEEVV2020_SQLLogin_1;pwd=682shn3e1p;data source=DbPromoCode.mssql.somee.com;persist security info=False;initial catalog=DbPromoCode");
        }


    }
}


