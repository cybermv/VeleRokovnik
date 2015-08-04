using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.DynamicData;
using System.Web.Services.Description;

namespace VeleRokovnik.Entities
{
    public enum VrstaObveze
    {
        Posao, Faks, Sastanak, Izlazak, Ostalo
    }

    [TableName("Obveze")]
    public class ObvezaEntity
    {
        [Key]
        public int ObvezaId { get; set; }

        [StringLength(50)]
        public string Naslov { get; set; }

        [StringLength(250)]
        public string Opis { get; set; }

        public DateTime Datum { get; set; }

        public bool JeHitno { get; set; }

        public bool JeObavljeno { get; set; }

        public VrstaObveze VrstaObveze { get; set; }

        public virtual OsobaEntity Osoba { get; set; }
    }
}