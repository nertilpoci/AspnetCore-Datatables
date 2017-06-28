using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspnetCore_Datatables.Model
{
    public class Product
    {
        public Product() { }
        public Product(int id,string productName, int supplierId, double unitPrice,string package , bool isDiscountinued)
        {
            Id = id;
            SupplierId = supplierId;
            ProductName = productName;
            Package = package;
            UnitPrice = (decimal)unitPrice;
            IsDiscountinued = isDiscountinued;
        }
        public int Id { get; set; }
        public string ProductName { get; set; }
        public int SupplierId { get; set; }
        public virtual Supplier Supplier { get; set; }
        public decimal UnitPrice { get; set; }
        public string Package { get; set; }
        public bool IsDiscountinued { get; set; }
        public HashSet<OrderItem> OrderItems { get; set; } = new HashSet<OrderItem>();
    }
}
