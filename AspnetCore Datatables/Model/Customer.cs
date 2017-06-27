using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspnetCore_Datatables.Model
{
    public class Customer
    {
     public Customer() { }
     public Customer(int id,string firstName,string lastName,string city,string country,string phone) {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            City = city;
            Country = country;
            Phone = phone;
        }
        public int Id { get; set; }
     public string FirstName { get; set; }
     public string LastName { get; set; }
     public string City { get; set; }
     public string Country { get; set; }
     public string Phone { get; set; }
     public HashSet<Order> Orders { get; set; } = new HashSet<Order>();
    }
}
