using System.Data.Entity;

namespace CMSShoppingCART.Models.Data
{
    public class Db : DbContext
    {
        public DbSet<PageDTO> Pages { get; set; }
    }
}