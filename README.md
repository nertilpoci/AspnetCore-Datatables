# AspnetCore-Datatables 
Live demo http://dtb.npoci.com
Aspnet core datatables sample 

Download the datatables script and style files. Go to https://datatables.net/download/index and select the files you want to include.
Important are 
Jquery,
Style
Datatables
I use the CDN source in this case it makes it easier to add to a project.
```
<link rel="stylesheet" type="text/css" href="https://cdn.datatables.net/v/bs-3.3.7/jq-2.2.4/dt-1.10.15/cr-1.3.3/r-2.1.1/sc-1.4.2/se-1.2.2/datatables.min.css"/>
 
<script type="text/javascript" src="https://cdn.datatables.net/v/bs-3.3.7/jq-2.2.4/dt-1.10.15/cr-1.3.3/r-2.1.1/sc-1.4.2/se-1.2.2/datatables.min.js"></script>
```

Add these files to _Layout.cshtml

Create a controller.
1. Create
2. Update
3. Delete
4. AjaxDataProvider(for datatable)
```

       [HttpPost]
        public async Task<IActionResult> Create([Bind("FirstName,LastName,City,Country,Phone")] Customer customer)
        {
            if (ModelState.IsValid)
            {   
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

        public JsonResult AjaxDataProvider(DatatableAjaxModel param)
        {
            IEnumerable<Customer> Customers = _context.Customers;
            var totalCustomers = _context.Customers.Count();
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

```

The Create,Edit and Delete actions are the standard mvc actions.
The AjaxDataProvider is what the datatable will call to paginate filter and order data.
1. First we need a total number of results ``` var totalCustomers = _context.Customers.Count(); ``` to know of how many entities we filtered or paged from.
2. We need to get sortDirection ``` var sortDirection = HttpContext.Request.Query["sSortDir_0"]; ``` this gets the sort direction for the first sort column in this case we are only doing sorting for single column so we alwasy need the first there will be only one.
3. We need the column the data is sorted by ``` var sortColumnIndex = Convert.ToInt32(HttpContext.Request.Query["iSortCol_0"]);``` again we only need the first one since we are only supporting 1 column sort at this time.
4. Check the search term ```if (!string.IsNullOrEmpty(param.sSearch)) Customers = Customers.Where(z => z.FirstName.ToLower().Contains(param.sSearch.ToLower()) || z.LastName.ToLower().Contains(param.sSearch.ToLower()) || z.City.ToLower().Contains(param.sSearch.ToLower()) || z.Country.ToLower().Contains(param.sSearch.ToLower()) || z.Phone.ToLower().Contains(param.sSearch.ToLower())); ``` check if its not empty then filter the values
5. Sort data depending on the search index and sort direction sort the data 

 ```
               switch (sortColumnIndex)
            {
                case 1:
                    Customers = sortDirection == "asc" ? Customers.OrderBy(z => z.FirstName) : Customers.OrderByDescending(z =>                             z.FirstName);
                    break;
                case 2:
                    Customers = sortDirection == "asc" ? Customers.OrderBy(z => z.LastName) : Customers.OrderByDescending(z 
                    =>z.LastName);
                    break;
                case 3:
                    Customers = sortDirection == "asc" ? Customers.OrderBy(z => z.City) : Customers.OrderByDescending(z => z.City);
                    break;
                case 4:
                    Customers = sortDirection == "asc" ? Customers.OrderBy(z => z.Country) : Customers.OrderByDescending(z =>z.Country);
                    break;
                case 5:
                    Customers = sortDirection == "asc" ? Customers.OrderBy(z => z.Phone) : Customers.OrderByDescending(z => z.Phone);
                    break;
                default:
                    Customers = Customers.OrderBy(z => z.Id);
                    break;
            }
 ```
            
 6. Get the count of the customers after we filter them ``` var filteredCustomersCount = Customers.Count(); ``` this is so we know how many total customers we have for this filter since we are not going to show them all at once but paginate them.
 7. Paginate customers ```Customers = Customers.Skip(param.iDisplayStart).Take(param.iDisplayLength);``` 
 8. Return a json object to datatable.
            
            
  # Datatable
  ```
  $('#customersTable').DataTable({
                select: {
                    style: 'single'
                }, "ordering": true ,searching: true,
                responsive: true, "bProcessing": true,
                "bServerSide": true,
                "sAjaxSource": '/Customers/AjaxDataProvider',
                "columnDefs": [
                    {
                        "targets": [0],
                        "visible": false,
                        "searchable": false
                    }, {

                        "render": function (data, type, row) {
                            return ` <a class="btn btn-link"  href="/Orders/index/${data.id}" >Orders<a> `
                        },
                        "targets": 6
                    }
                ],
                "columns": [


                    { "data": "id" },
                    { "data": "firstName" },
                    { "data": "lastName" },
                    { "data": "city" },
                    { "data": "country" },
                    { "data": "phone" },
                    {

                        "orderable": false,
                        "searchable": false,
                        "data": null
                    }
                ]
            }); 
  ```
  Important  configuration  "bServerSide": true this tells the datatable that it should get the data from the server itself.
  "sAjaxSource": '/Customers/AjaxDataProvider' this sets the source where it should get the data
            
            
 
