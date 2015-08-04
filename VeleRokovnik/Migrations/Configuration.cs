using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using VeleRokovnik.Entities;
using VeleRokovnik.Models;
using VeleRokovnik.Util;
using System.Data.Entity.Migrations;

namespace VeleRokovnik.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<RokovnikContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "VeleRokovnik.Entities.RokovnikContext";
        }

        protected override void Seed(RokovnikContext context)
        {
            const string defaultPass = "lozinka";
            RokovnikUserManager userManager = new RokovnikUserManager(new UserStore<RokovnikUser>(context));

            context.Database.ExecuteSqlCommand("DELETE FROM ObvezaEntities");
            context.Database.ExecuteSqlCommand("DELETE FROM OsobaEntities");
            context.Database.ExecuteSqlCommand("DELETE FROM AspNetUsers");
            
            Random r = new Random(Environment.TickCount);
            var imena = new[] { "Pero", "Marko", "Joso", "Ivo", "Vedran", "Krešo", "Luka", "Nikola", "Dolan", "Sigmund", "Adolf" };

            var prezimena = new[] { "Bariæ", "Mehiæ", "Mudriæ", "Lutiæ", "Karalin", "Suniæ", "Bociæ", "Ergela", "Tarandek", "Dulèiæ" };

            var naslovi = new[] { "Projekt", "Roðendan", "Spoj", "Dogovor", "Intervju", "Conference call", "Kava" };

            for (int i = 0; i < 25; i++)
            {
                OsobaEntity oso = new OsobaEntity
                {
                    Ime = imena[r.Next(0, imena.Length)],
                    Prezime = prezimena[r.Next(0, prezimena.Length)],
                    DatumRodjenja = DateTime.Now.AddYears(-25).AddDays(r.Next(0, 250)),
                    NazivRokovnika = "Novi rokovnik",
                    OpisRokovnika = "Poèetni opis rokovnika",
                };

                var numOfObveze = r.Next(2, 15);

                for (int j = 0; j < numOfObveze; j++)
                {
                    ObvezaEntity obv = new ObvezaEntity
                    {
                        Datum = DateTime.Now.AddDays(r.Next(0, 10)),
                        Naslov = naslovi[r.Next(0, naslovi.Length)],
                        JeHitno = Convert.ToBoolean(r.Next(0, 2)),
                        VrstaObveze = (VrstaObveze) r.Next(0, 5),
                        Opis = Utilities.LoremIpsum(8, 15),
                        JeObavljeno = Convert.ToBoolean(r.Next(0, 2))
                    };
                    oso.Obveze.Add(obv);
                }
                context.Osobe.Add(oso);

                context.SaveChanges();

                RokovnikUser rUser = new RokovnikUser()
                {
                    UserName = Utilities.CreateUsername(oso.Ime, oso.Prezime),
                    OsobaId = oso.OsobaId
                };

                userManager.Create(rUser, defaultPass);

            }
        }
    }
}
