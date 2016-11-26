using System;

namespace TicketWindow.DAL.Models
{
    public class XmlFile
    {
        public XmlFile(Guid customerId, string fileName, DateTime date, bool upd, string userName, string data, Guid establishmentCustomerId)
        {
            CustomerId = customerId;
            FileName = fileName;
            Date = date;
            Upd = upd;
            UserName = userName;
            Data = data;
            EstablishmentCustomerId = establishmentCustomerId;
        }

        public Guid CustomerId { get; set; }
        public string FileName { get; set; }
        public DateTime Date { get; set; }
        public bool Upd { get; set; }
        public string UserName { get; set; }
        public string Data { get; set; }
        public Guid EstablishmentCustomerId { get; set; }

        public override string ToString()
        {
            return string.Concat(UserName, " ", FileName);
        }
    }
}