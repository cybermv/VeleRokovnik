using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Web;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity.EntityFramework;
using VeleRokovnik.Models;
using VeleRokovnik.Util;

namespace VeleRokovnik.Entities
{
    public class RokovnikContext : IdentityDbContext<RokovnikUser>
    {
        public RokovnikContext()
            : base("RokovnikDb")
        {
        }

        public DbSet<OsobaEntity> Osobe { get; set; }
        public DbSet<ObvezaEntity> Obveze { get; set; }

        public static RokovnikContext Create()
        {
            return new RokovnikContext();
        }
    }
}