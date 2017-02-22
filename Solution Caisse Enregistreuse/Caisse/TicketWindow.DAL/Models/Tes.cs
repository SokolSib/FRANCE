using System;
using System.Collections.Generic;
using TicketWindow.DAL.Additional;

namespace TicketWindow.DAL.Models
{
    public class Tes
    {
        public Tes()
        {
            TesProducts = new List<TesProduct>();
            TesReglaments = new List<TesReglament>();
        }

        public Guid CustomerId { get; set; }
        public int? Id { get; set; }
        public int Type { get; set; }
        public DateTime DateTime { get; set; }
        public bool Payement { get; set; }
        public bool Livraison { get; set; }
        public short? ACodeFournisseur { get; set; }
        public int? ASex { get; set; }
        public string AName { get; set; }
        public string ASurname { get; set; }
        public string ANameCompany { get; set; }
        public string ASiret { get; set; }
        public string AFrtva { get; set; }
        public string AOfficeAddress { get; set; }
        public string AOfficeZipCode { get; set; }
        public string AOfficeCity { get; set; }
        public string ATelephone { get; set; }
        public string AMail { get; set; }
        public string VNameCompany { get; set; }
        public string VCp { get; set; }
        public string VVille { get; set; }
        public string VAdresse { get; set; }
        public string VPhone { get; set; }
        public string VMail { get; set; }
        public string VSiret { get; set; }
        public string VFrtva { get; set; }
        public string VCodeNaf { get; set; }
        public string VFax { get; set; }
        public decimal? Montant { get; set; }
        public string Description { get; set; }
        public string Nclient { get; set; }
        public List<TesProduct> TesProducts { get; set; }
        public List<TesReglament> TesReglaments { get; set; }
    }
}