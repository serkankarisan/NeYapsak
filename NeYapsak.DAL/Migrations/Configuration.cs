namespace NeYapsak.DAL.Migrations
{
    using Microsoft.AspNet.Identity.EntityFramework;
    using NeYapsak.Entity.Entity;
    using NeYapsak.Entity.Identity;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<NeYapsak.DAL.Context.NeYapsakContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            ContextKey = "NeYapsak.DAL.Context.NeYapsakContext";
        }

        protected override void Seed(NeYapsak.DAL.Context.NeYapsakContext context)
        {

            IdentityUserRole UserRole;

            ApplicationRole ApRoleAdmin = new ApplicationRole() { Id = "5e8edacb-d8f9-47b3-91f1-028643139e1d", Name = "Admin", Description = "Yönetici" };
            ApplicationRole ApRoleUser = new ApplicationRole() { Id = "3e8edacb-d8f9-47b3-91f1-028643139e1d", Name = "User", Description = "Kullanýcý" };

            ApplicationUser ApUserAdmin = new ApplicationUser() { Id = "6e8edacb-d8f9-47b3-91f1-028643139e1d", Name = "Mehmet ali", Surname = "Coþar", Email = "memo@gmail.com", UserName = "mehmetcosar", DogumTarihi = DateTime.Now, EmailConfirmed = true, PasswordHash = "ABjmuPvN9Zaw/+7SM5GYHUZXWDahYkc48tBpmNNlVVgM5Rj02N9rVzeMzPGAlzJeXA==", SecurityStamp = "d6b253a0-6325-410b-bed4-796ba916e443", PhoneNumber = null, PhoneNumberConfirmed = false, TwoFactorEnabled = false, LockoutEndDateUtc = null, LockoutEnabled = false, AccessFailedCount = 0 };//burada ýd,password ve securitystamp deðiþecek.
            ApplicationUser ApUserUser1 = new ApplicationUser() { Id = "90fd7acd-9a43-488b-80bb-d6084507c441", Name = "Oya", Surname = "Koþar", Email = "oya@gmail.com", UserName = "oyakosar", DogumTarihi = DateTime.Now, EmailConfirmed = true, PasswordHash = "AJLZWCBM0sIffqRzoPgim0OV1tomiWFDAG/6r/ARWA9IbTc4vF+9r8CuZSEBpvwTsg==", SecurityStamp = "f67c8d73-6d11-4e4f-b9fc-4cfbb6a4dadc", PhoneNumber = null, PhoneNumberConfirmed = false, TwoFactorEnabled = false, LockoutEndDateUtc = null, LockoutEnabled = false, AccessFailedCount = 0 };
            ApplicationUser ApUserUser2 = new ApplicationUser() { Id = "7e8edacb-d8f9-47b3-91f1-028643139e1d", Name = "Ali", Surname = "Uçar", Email = "ali@gmail.com", UserName = "aliucar", DogumTarihi = DateTime.Now, EmailConfirmed = false, PasswordHash = "ABjmuPvN9Zaw/+7SM5GYHUZXWDahYkc48tBpmNNlVVgM5Rj02N9rVzeMzPGAlzJeXA==", SecurityStamp = "d6b253a0-6325-410b-bed4-796ba916e443", PhoneNumber = null, PhoneNumberConfirmed = false, TwoFactorEnabled = false, LockoutEndDateUtc = null, LockoutEnabled = false, AccessFailedCount = 0 };


            UserRole = new IdentityUserRole() { RoleId = "5e8edacb-d8f9-47b3-91f1-028643139e1d", UserId = "6e8edacb-d8f9-47b3-91f1-028643139e1d" };

            ApUserAdmin.Roles.Add(UserRole);
            ApRoleAdmin.Users.Add(UserRole);


            UserRole = new IdentityUserRole() { RoleId = "3e8edacb-d8f9-47b3-91f1-028643139e1d", UserId = "90fd7acd-9a43-488b-80bb-d6084507c441" };

            ApUserUser1.Roles.Add(UserRole);
            ApRoleUser.Users.Add(UserRole);


            UserRole = new IdentityUserRole() { RoleId = "3e8edacb-d8f9-47b3-91f1-028643139e1d", UserId = "7e8edacb-d8f9-47b3-91f1-028643139e1d" };

            ApUserUser2.Roles.Add(UserRole);
            ApRoleUser.Users.Add(UserRole);


            context.Users.AddOrUpdate(
             ApUserAdmin, ApUserUser1, ApUserUser2
            );

            context.Roles.AddOrUpdate(
                ApRoleUser, ApRoleAdmin
                );

            context.Etiketler.AddOrUpdate(
             new Etiket { EtiketAdi = "Halý saha", Aciklama = "Maç mý yapsak", Silindi = false },
             new Etiket { EtiketAdi = "Batak", Aciklama = "Batak mý yapsak", Silindi = false },
             new Etiket { EtiketAdi = "101", Aciklama = "101 mi yapsak", Silindi = false }
            );

            context.Ilanlar.AddOrUpdate(
            new Ilan { Id = 1, Baslik = "Halýsaha Maçý mý yapsak.", BaslangicTarihi = DateTime.Now, GoruntulenmeSayaci = 1, Il = "istanbul", Ilce = "beþiktaþ", Kontenjan = 2, KullaniciId = "90fd7acd-9a43-488b-80bb-d6084507c441", OlusturmaTarihi = DateTime.Now, Ozet = "Hele  gelde maç yapak.kaleye adam lazým :).loyloy.", Silindi = false, Yayindami = true }
            );

            context.IlanHareketler.AddOrUpdate(
            new IlanHareket { IlanId = 1, IlgilenenId = 1, KatilanId = 0, KullaniciId = "90fd7acd-9a43-488b-80bb-d6084507c441", Tarih = DateTime.Now, Silindi = false }
            );

            context.Ilgilenenler.AddOrUpdate(
             new Ilgilenen { Id = 1, IlanId = 1, KullaniciId = "90fd7acd-9a43-488b-80bb-d6084507c441", Tarih = DateTime.Now, Silindi = false }
            );

            context.Katilanlar.AddOrUpdate(
              new Katilan { Id = 1, IlanId = 1, KullaniciId = "90fd7acd-9a43-488b-80bb-d6084507c441", Tarih = DateTime.Now, Silindi = false }
            );

            context.SikayetVeOneriler.AddOrUpdate(
              new SikayetVeOneri { Aciklama = "kendi ilanýmdan þikayetçiyim.hele.hele.", Id = 1, IlanId = 1, KullaniciId = "90fd7acd-9a43-488b-80bb-d6084507c441", Tarih = DateTime.Now, Silindi = false }
            );
        }
    }
}
