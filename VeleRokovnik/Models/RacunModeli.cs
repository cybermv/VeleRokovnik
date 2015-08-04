using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mime;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace VeleRokovnik.Models
{
    public class PrijavaModel
    {
        [Required]
        [Display(Name = "Korisničko ime")]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Lozinka")]
        public string Password { get; set; }
    }

    public class NoviKorisnikModel
    {
        [Required(ErrorMessage = "Moraš unijeti korisničko ime!")]
        [Display(Name = "Korisničko ime:")]
        [StringLength(50, ErrorMessage = "Korisničko ime ne smije biti duže od 50 znakova")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Moraš unijeti ime!")]
        [Display(Name = "Ime:")]
        [StringLength(50, ErrorMessage = "Ime ne smije biti duže od 50 znakova")]
        public string Ime { get; set; }

        [Required(ErrorMessage = "Moraš unijeti prezime!")]
        [Display(Name = "Prezime:")]
        [StringLength(50, ErrorMessage = "Prezime ne smije biti duže od 50 znakova")]
        public string Prezime { get; set; }

        [Required(ErrorMessage = "Moraš unijeti lozinku!")]
        [DataType(DataType.Password)]
        [Display(Name = "Lozinka:")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Moraš ponovno unijeti lozinku!")]
        [DataType(DataType.Password)]
        [Display(Name = "Još jednom lozinka:")]
        [Compare("Password", ErrorMessage = "Lozinke se ne podudaraju!")]
        public string RepeatPassword { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Datum rođenja (neobavezno):")]
        public DateTime? DatumRodjenja { get; set; }
    }

    public class PromjenaLozinkeModel
    {
        [Required(ErrorMessage = "Moraš unijeti staru lozinku!")]
        [Display(Name = "Stara lozinka:")]
        public string StaraLozinka { get; set; }

        [Required(ErrorMessage = "Moraš unijeti novu lozinku!")]
        [Display(Name = "Nova lozinka:")]
        public string NovaLozinka { get; set; }

        [Required(ErrorMessage = "Moraš ponovno unijeti novu lozinku!")]
        [Display(Name = "Još jednom nova lozinka:")]
        [Compare("NovaLozinka", ErrorMessage = "Lozinke se ne podudaraju!")]
        public string NovaLozinkaPonovno { get; set; }
    }

    public class ResetLozinkeModel
    {
        [Required(ErrorMessage = "Moraš unijeti korisničko ime!")]
        [Display(Name = "Korisničko ime:")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Moraš unijeti novu lozinku!")]
        [Display(Name = "Nova lozinka:")]
        public string NovaLozinka { get; set; }

        [Required(ErrorMessage = "Moraš ponovno unijeti novu lozinku!")]
        [Display(Name = "Još jednom nova lozinka:")]
        [Compare("NovaLozinka", ErrorMessage = "Lozinke se ne podudaraju!")]
        public string NovaLozinkaPonovno { get; set; }
    }

    public class OsobniPodaciModel
    {
        public OsobniPodaciModel()
        {
            
        }

        public OsobniPodaciModel(Osoba osoba)
        {
            this.Ime = osoba.Ime;
            this.Prezime = osoba.Prezime;
            this.DatumRodjenja = osoba.DatumRodjenja;
            this.NazivRokovnika = osoba.NazivRokovnika;
            this.OpisRokovnika = osoba.OpisRokovnika;
        }

        [Required(ErrorMessage = "Moraš unijeti ime!")]
        [Display(Name = "Ime:")]
        [StringLength(50, ErrorMessage = "Ime ne smije biti duže od 50 znakova")]
        public string Ime { get; set; }

        [Required(ErrorMessage = "Moraš unijeti prezime!")]
        [Display(Name = "Prezime:")]
        [StringLength(50, ErrorMessage = "Prezime ne smije biti duže od 50 znakova")]
        public string Prezime { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Datum rođenja:")]
        public DateTime? DatumRodjenja { get; set; }

        [Required(ErrorMessage = "Moraš unijeti naziv rokovnika!")]
        [Display(Name = "Naziv rokovnika:")]
        public string NazivRokovnika { get; set; }

        [Required(ErrorMessage = "Moraš unijeti opis rokovnika!")]
        [Display(Name = "Opis rokovnika:")]
        public string OpisRokovnika { get; set; }

        public void UpdateOsoba(ref Osoba osobaToEdit)
        {
            osobaToEdit.Ime = this.Ime;
            osobaToEdit.Prezime = this.Prezime;
            osobaToEdit.DatumRodjenja = this.DatumRodjenja;
            osobaToEdit.NazivRokovnika = this.NazivRokovnika;
            osobaToEdit.OpisRokovnika = this.OpisRokovnika;
        }
    }
}