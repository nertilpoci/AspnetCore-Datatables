using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspnetCore_Datatables.Model
{
    public class Supplier
    {
        public Supplier() { }
        public Supplier(int Id, string CompanyName, string ContactName, string ContactTitle, string City, string Phone, string Fax)
        {
            this.Id = Id;
            this.CompanyName = CompanyName;
            this.ContactName = ContactName;
            this.ContactTitle = ContactTitle;
            this.City = City;
            this.Phone = Phone;
            this.Fax = Fax;
        }
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string ContactName { get; set; }
        public string ContactTitle { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public HashSet<Product> Products { get; set; } = new HashSet<Product>();
    }
}
