using Hotspot.Model.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Hotspot.Model
{
    public class HotspotContext : IdentityDbContext
    {
        public HotspotContext(DbContextOptions options) : base(options)
        {
        }

        //Codes DbSets
        public DbSet<Batch> Batch { get; set; }
        public DbSet<Ticket> Ticket { get; set; }
        public DbSet<Courtesy> Coutesy { get; set; }
        public DbSet<SpecialTicket> SpecialTicket { get; set; }
        public DbSet<CatalogBatch> CatalogBatch { get; set; }
        public DbSet<CatalogTicket> CatalogTicket { get; set; }
        public DbSet<CatalogTicketItem> CatalogTicketItem { get; set; }

        //Configuration DbSets
        public DbSet<TicketsConfiguration> TicketsConfiguration { get; set; }

        //Seller and Employee DbSets
        public DbSet<Seller> Seller { get; set; }
        public DbSet<EmployeeUser> EmployeeUser { get; set; }
        public DbSet<Address> Address { get; set; }
        public DbSet<Locale> Locale { get; set; }
        public DbSet<Phonenumber> Phonenumber { get; set; }

        //Sales DbSet
        public DbSet<Sale> Sale { get; set; }

        //History DbSets
        public DbSet<ConnectionHistory> ConnectionHistory { get; set; }
        public DbSet<LogoutHistory> LogoutHistory { get; set; }
        public DbSet<Log> Log { get; set; }

        //Financial DbSets
        public DbSet<CashFlow> CashFlow { get; set; }
        public DbSet<Flow> Flow { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
