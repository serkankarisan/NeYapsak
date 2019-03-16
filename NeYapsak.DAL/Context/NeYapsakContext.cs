using Microsoft.AspNet.Identity.EntityFramework;
using NeYapsak.DAL.Migrations;
using NeYapsak.Entity.Entity;
using NeYapsak.Entity.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeYapsak.DAL.Context
{
    public class NeYapsakContext : IdentityDbContext<ApplicationUser>
    {
        public NeYapsakContext() : base("NeYapsakContext")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<NeYapsakContext, Configuration>("NeYapsakContext"));
        }
        public virtual DbSet<Etiket> Etiketler{ get; set; }
        public virtual DbSet<Ilan> Ilanlar { get; set; }
        public virtual DbSet<IlanHareket> IlanHareketler { get; set; }
        public virtual DbSet<Katilan> Katilanlar { get; set; }
        public virtual DbSet<SikayetVeOneri> SikayetVeOneriler { get; set; }
    }
}
