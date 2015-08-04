using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using VeleRokovnik.Entities;

namespace VeleRokovnik.Models
{
    using System.Linq;

    public class Obveza
    {
        public Obveza()
        {
            OsobaId = -1;
            Datum = DateTime.Today;
        }

        public int? OsobaId { get; set; }
        public int? ObvezaId { get; set; }
        public string Naslov { get; set; }
        public string Opis { get; set; }
        public DateTime Datum { get; set; }
        public bool JeHitno { get; set; }
        public bool JeObavljeno { get; set; }
        public VrstaObveze VrstaObveze { get; set; }

        public static Obveza ConvertEntityToModel(ObvezaEntity entity)
        {
            return new Obveza
            {
                OsobaId = entity.Osoba.OsobaId,
                ObvezaId = entity.ObvezaId,
                Naslov = entity.Naslov,
                Opis = entity.Opis,
                Datum = entity.Datum,
                JeHitno = entity.JeHitno,
                JeObavljeno = entity.JeObavljeno,
                VrstaObveze = entity.VrstaObveze
            };
        }

        public static ObvezaEntity ConvertModelToEntity(Obveza model)
        {
            return new ObvezaEntity
            {
                ObvezaId = model.ObvezaId ?? -1,
                Naslov = model.Naslov,
                Opis = model.Opis,
                Datum = model.Datum,
                JeHitno = model.JeHitno,
                JeObavljeno = model.JeHitno,
                VrstaObveze = model.VrstaObveze,
            };
        }

        public static SelectList GetVrstaObvezeSelectList()
        {
            Array enumVals = Enum.GetValues(typeof(VrstaObveze));
            List<ListItem> items = new List<ListItem>(enumVals.Length);
            items.AddRange(from object i in enumVals select new ListItem { Text = Enum.GetName(typeof(VrstaObveze), i), Value = ((int)i).ToString("N0") });

            return new SelectList(items, "Value", "Text");
        }
    }
}