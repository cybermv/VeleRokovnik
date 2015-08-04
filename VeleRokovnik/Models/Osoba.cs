using System;
using System.Collections.Generic;
using System.Web.Mvc;
using VeleRokovnik.Entities;

namespace VeleRokovnik.Models
{
    public class Osoba
    {
        public Osoba()
        {
            OsobaId = -1;
            Obveze = new List<Obveza>();
        }

        public int? OsobaId { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public DateTime? DatumRodjenja { get; set; }
        public IList<Obveza> Obveze { get; set; }
        public string NazivRokovnika { get; set; }
        public string OpisRokovnika { get; set; }

        public static Osoba ConvertEntityToModel(OsobaEntity entity, bool fetchObveze = false)
        {
            Osoba o = new Osoba
            {
                OsobaId = entity.OsobaId,
                Ime = entity.Ime,
                Prezime = entity.Prezime,
                DatumRodjenja = entity.DatumRodjenja,
                NazivRokovnika = entity.NazivRokovnika,
                OpisRokovnika = entity.OpisRokovnika,
                Obveze = new List<Obveza>(),
            };
            if (fetchObveze)
            {
                foreach (var obvezaEntity in entity.Obveze)
                {
                    o.Obveze.Add(Obveza.ConvertEntityToModel(obvezaEntity));
                }
            }
            return o;
        }

        public static OsobaEntity ConvertModelToEntity(Osoba model)
        {
            var entity = new OsobaEntity
            {
                OsobaId = model.OsobaId ?? -1,
                Ime = model.Ime,
                Prezime = model.Prezime,
                DatumRodjenja = model.DatumRodjenja,
                NazivRokovnika = model.NazivRokovnika,
                OpisRokovnika = model.OpisRokovnika,
            };
            foreach (var obvezaModel in model.Obveze)
            {
                entity.Obveze.Add(Obveza.ConvertModelToEntity(obvezaModel));
            }
            return entity;
        }

        public SelectList GetObvezeSelectList()
        {
            return new SelectList(Obveze, "ObvezaId", "Naslov");
        }
    }
}