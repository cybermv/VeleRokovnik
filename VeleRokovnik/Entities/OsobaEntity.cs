using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.DynamicData;

namespace VeleRokovnik.Entities
{
    [TableName("Osobe")]
    public class OsobaEntity
    {
        public OsobaEntity()
        {
            Obveze = new List<ObvezaEntity>();
        }
        [Key]
        public int OsobaId { get; set; }

        [StringLength(50)]
        public string Ime { get; set; }

        [StringLength(50)]
        public string Prezime { get; set; }

        public DateTime? DatumRodjenja { get; set; }

        [StringLength(50)]
        public string NazivRokovnika { get; set; }

        [StringLength(150)]
        public string OpisRokovnika { get; set; }

        public virtual ICollection<ObvezaEntity> Obveze { get; set; }
    }
}