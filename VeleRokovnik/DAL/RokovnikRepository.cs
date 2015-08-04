using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using VeleRokovnik.Entities;
using VeleRokovnik.Models;

namespace VeleRokovnik.DAL
{
    public class RokovnikRepository : IRokovnikRepository, IDisposable
    {
        private readonly RokovnikContext _context;

        public RokovnikRepository(RokovnikContext context)
        {
            _context = context;
        }

        public IList<Obveza> GetAllObveze()
        {
            var list = new List<Obveza>();
            var obveze = _context.Obveze;
            foreach (var obvezaEntity in obveze)
            {
                list.Add(Obveza.ConvertEntityToModel(obvezaEntity));
            }
            return list;
        }

        public IList<Obveza> GetObvezeFor(int personId, Func<Obveza, bool> predicate = null)
        {
            var list = new List<Obveza>();
            var person = _context.Osobe.Find(personId);
            if (person == null)
                return list;
            foreach (var obvezaEntity in person.Obveze)
            {
                list.Add(Obveza.ConvertEntityToModel(obvezaEntity));
            }

            if (predicate != null)
                list = list.Where(predicate).ToList();

            return list.OrderBy(p => p.Datum).ThenBy(p => p.Naslov).ToList();
        }

        public Obveza GetObvezaByKey(int obvezaId)
        {
            var obvezaEntity = _context.Obveze.Find(obvezaId);
            if (obvezaEntity == null)
                return null;
            return Obveza.ConvertEntityToModel(obvezaEntity);
        }

        public int AddObvezaFor(int personId, Obveza model)
        {
            var entity = Obveza.ConvertModelToEntity(model);
            var osoba = _context.Osobe.Find(personId);
            if (osoba == null)
                return -1;
            entity.Osoba = osoba;
            _context.Obveze.Add(entity);
            _context.SaveChanges();
            return entity.ObvezaId;
        }

        public bool UpdateObveza(Obveza model)
        {
            var entity = _context.Obveze.Find(model.ObvezaId);
            if (entity == null)
                return false;
            entity.Naslov = model.Naslov;
            entity.Opis = model.Opis;
            entity.Datum = model.Datum;
            entity.JeHitno = model.JeHitno;
            entity.VrstaObveze = model.VrstaObveze;
            return true;
        }

        public bool DeleteObveza(int obvezaId)
        {
            var toDelete = _context.Obveze.Find(obvezaId);
            if (toDelete == null)
                return false;
            _context.Obveze.Remove(toDelete);
            return true;
        }

        public IList<Osoba> GetAllOsobe(bool fetchObveze = false)
        {
            var list = new List<Osoba>();

            foreach (var osobaEntity in _context.Osobe)
            {
                list.Add(Osoba.ConvertEntityToModel(osobaEntity, fetchObveze));
            }

            return list.OrderBy(p => p.Prezime).ThenBy(p => p.Ime).ToList();
        }

        public Osoba GetOsobaByKey(int osobaId, bool fetchObveze = false)
        {
            var osobaEntity = _context.Osobe.Find(osobaId);
            if (osobaEntity == null)
                return null;
            return Osoba.ConvertEntityToModel(osobaEntity, fetchObveze);

        }

        public int AddOsoba(Osoba osoba)
        {
            var entity = Osoba.ConvertModelToEntity(osoba);
            _context.Osobe.Add(entity);
            _context.SaveChanges();
            return entity.OsobaId;

        }

        public bool UpdateOsoba(Osoba osoba)
        {
            var entity = _context.Osobe.Find(osoba.OsobaId);
            if (entity == null)
                return false;

            entity.Ime = osoba.Ime;
            entity.Prezime = osoba.Prezime;
            entity.DatumRodjenja = osoba.DatumRodjenja;
            entity.NazivRokovnika = osoba.NazivRokovnika;
            entity.OpisRokovnika = osoba.OpisRokovnika;

            var oldObveze = entity.Obveze.ToList();
            var newObveze = osoba.Obveze;
            foreach (var obvezaEntity in oldObveze)
            {
                if (newObveze.Any(p => p.ObvezaId == obvezaEntity.ObvezaId))
                {
                    var newObveza = newObveze.Single(p => p.ObvezaId == obvezaEntity.ObvezaId);
                    UpdateObveza(newObveza);
                    newObveze.Remove(newObveza);
                }
                else
                {
                    DeleteObveza(obvezaEntity.ObvezaId);
                }
            }

            foreach (var obvezaModel in newObveze)
            {
                AddObvezaFor(entity.OsobaId, obvezaModel);
            }

            return true;
        }

        public bool DeleteOsoba(int osobaId)
        {
            var osoba = _context.Osobe.Find(osobaId);
            if (osoba == null)
                return false;

            _context.Osobe.Remove(osoba);
            return true;

        }

        public bool SaveChanges()
        {
            return _context.SaveChanges() > 0;
        }

        private bool _isDisposed;

        protected virtual void Dispose(bool disposing)
        {
            if (!this._isDisposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this._isDisposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}