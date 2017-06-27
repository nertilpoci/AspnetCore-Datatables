using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace AspnetCore_Datatables.Model
{
    public class Order
    {
        public Order() { }
        public Order(int Id,  int CustomerId, double TotalAmount, String OrderNumber)
        {
            this.Id = Id;
            this.OrderDate = DateTime.Now;
            this.CustomerId = CustomerId;
            this.TotalAmount = (decimal)TotalAmount;
            this.OrderNumber = OrderNumber;

        }
        public int Id { get; set; }
        public String OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public int CustomerId { get; set; }
        public decimal TotalAmount { get; set; }
        public virtual Customer Customer { get; set; }
        public HashSet<OrderItem> OrderItems { get; set; } = new HashSet<OrderItem>();

    }
}
