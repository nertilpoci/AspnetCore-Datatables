using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspnetCore_Datatables.Model
{
    public class OrderItem
    {
        public OrderItem() { }
        public OrderItem(int id, int orderId, int productId, double unitPrice, int quantity)
        {
            Id = id;
            OrderId = orderId;
            ProductId = productId;
            UnitPrice = (decimal)unitPrice;
            Quantity = quantity;
        }
        public int Id { get; set; }
        public int OrderId { get; set; }
        public virtual Order Order { get; set; }
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
    }
}
