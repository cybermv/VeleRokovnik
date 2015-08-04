using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using VeleRokovnik.Models;

namespace VeleRokovnik.DAL
{
    public interface IRokovnikRepository
    {
        IList<Obveza> GetAllObveze();
        IList<Obveza> GetObvezeFor(int personId, Func<Obveza, bool> predicate = null);
        Obveza GetObvezaByKey(int obvezaId);
        int AddObvezaFor(int personId, Obveza model);
        bool UpdateObveza(Obveza model);
        bool DeleteObveza(int obvezaId);

        IList<Osoba> GetAllOsobe(bool fetchObveze = false);
        Osoba GetOsobaByKey(int osobaId, bool fetchObveze = false);
        int AddOsoba(Osoba osoba);
        bool UpdateOsoba(Osoba osoba);
        bool DeleteOsoba(int osobaId);

        bool SaveChanges();
    }
}