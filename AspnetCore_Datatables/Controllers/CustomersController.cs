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
    public class CustomersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CustomersController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: Customers
        public  IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("FirstName,LastName,City,Country,Phone")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                customer.Id = _context.Customers.LastOrDefault()?.Id +1 ?? 1;//set the identity to the last value +1 since we added values manually by id entityframework doesn't seem to know were the last one was left off
               _context.Add(customer);
                await _context.SaveChangesAsync();
                return Json(customer);
            }
            else
            {
                return BadRequest(ModelState.Values.SelectMany(z => z.Errors).FirstOrDefault().ErrorMessage);
            }
           
        }

        
        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,City,Country,Phone")] Customer customer)
        {
            if (id != customer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                    
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
             return Ok();
            }
            else
            {
                return BadRequest(ModelState.Values.SelectMany(z => z.Errors).FirstOrDefault().ErrorMessage);
            }
        }

   
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var customer = await _context.Customers.SingleOrDefaultAsync(m => m.Id == id);
            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
            return Ok();
        }

        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.Id == id);
        }

        public JsonResult AjaxDataProvider(DatatableAjaxModel param)
        {
            IEnumerable<Customer> Customers = _context.Customers;
            var totalCustomers = _context.Customers.Count();
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(param);
            var sortDirection = HttpContext.Request.Query["sSortDir_0"]; // asc or desc
            var sortColumnIndex = Convert.ToInt32(HttpContext.Request.Query["iSortCol_0"]);
            if (!string.IsNullOrEmpty(param.sSearch)) Customers = Customers.Where(z => z.FirstName.ToLower().Contains(param.sSearch.ToLower()) || z.LastName.ToLower().Contains(param.sSearch.ToLower()) || z.City.ToLower().Contains(param.sSearch.ToLower()) || z.Country.ToLower().Contains(param.sSearch.ToLower()) || z.Phone.ToLower().Contains(param.sSearch.ToLower()));

            switch (sortColumnIndex)
            {
                case 1:
                    Customers = sortDirection == "asc" ? Customers.OrderBy(z => z.FirstName) : Customers.OrderByDescending(z => z.FirstName);
                    break;
                case 2:
                    Customers = sortDirection == "asc" ? Customers.OrderBy(z => z.LastName) : Customers.OrderByDescending(z => z.LastName);
                    break;
                case 3:
                    Customers = sortDirection == "asc" ? Customers.OrderBy(z => z.City) : Customers.OrderByDescending(z => z.City);
                    break;
                case 4:
                    Customers = sortDirection == "asc" ? Customers.OrderBy(z => z.Country) : Customers.OrderByDescending(z => z.Country);
                    break;
                case 5:
                    Customers = sortDirection == "asc" ? Customers.OrderBy(z => z.Phone) : Customers.OrderByDescending(z => z.Phone);
                    break;
                default:
                    Customers = Customers.OrderBy(z => z.Id);
                    break;


            }
            var filteredCustomersCount = Customers.Count();
            Customers = Customers.Skip(param.iDisplayStart).Take(param.iDisplayLength);
            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = totalCustomers,
                iTotalDisplayRecords = filteredCustomersCount,
                aaData = Customers
            });
        }
    }
}
