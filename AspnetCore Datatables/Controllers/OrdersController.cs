using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AspnetCore_Datatables.Data;
using AspnetCore_Datatables.Model;

namespace AspnetCore_Datatables.Controllers
{
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context)
        {
            _context = context;    
        }


        public IActionResult Index([FromRoute]int id)
        {
           var customer= _context.Customers.Find(id);
            if (customer == null) return BadRequest();
            return View(customer);
        }

        public JsonResult AjaxDataProvider([FromRoute]int id,DatatableAjaxModel param)
        {
            IEnumerable<Order> Orders = _context.Orders.Include(z=>z.OrderItems).ThenInclude(o=>o.Product).Where(z=>z.CustomerId==id);
            var totalOrders = _context.Orders.Count(z => z.CustomerId == id);
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(param);
            var sortDirection = HttpContext.Request.Query["sSortDir_0"]; // asc or desc
            var sortColumnIndex = Convert.ToInt32(HttpContext.Request.Query["iSortCol_0"]);
            if (!string.IsNullOrEmpty(param.sSearch)) Orders = Orders.Where(z => z.OrderNumber.Contains(param.sSearch) || z.OrderDate.ToString().Contains(param.sSearch) || z.TotalAmount.ToString().Contains(param.sSearch));

            switch (sortColumnIndex)
            {
                case 1:
                    Orders = sortDirection == "asc" ? Orders.OrderBy(z => z.OrderNumber) : Orders.OrderByDescending(z => z.OrderNumber);
                    break;
                case 2:
                    Orders = sortDirection == "asc" ? Orders.OrderBy(z => z.OrderDate) : Orders.OrderByDescending(z => z.OrderDate);
                    break;
                case 3:
                    Orders = sortDirection == "asc" ? Orders.OrderBy(z => z.TotalAmount) : Orders.OrderByDescending(z => z.TotalAmount);
                    break;
               
                default:
                    Orders = Orders.OrderBy(z => z.Id);
                    break;


            }
            var filteredOrdersCount = Orders.Count();
            Orders = Orders.Skip(param.iDisplayStart).Take(param.iDisplayLength);
            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = totalOrders,
                iTotalDisplayRecords = filteredOrdersCount,
                aaData = Orders
            });
        }
    }
}
