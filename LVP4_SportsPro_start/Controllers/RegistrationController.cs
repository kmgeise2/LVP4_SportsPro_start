using LVP4_SportsPro_start.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace LVP4_SportsPro_start.Controllers
{
    public class RegistrationController : Controller
    {
        private SportsProContext context { get; set; }

        public RegistrationController(SportsProContext ctx)
        {
            context = ctx;
        }

        [HttpGet]
        public IActionResult GetCustomer()
        {
            // Set up a Viewbag in order to populate a drop down list
            ViewBag.Customers = context.Customers
                .OrderBy(c => c.LastName)
                .ToList();

            //Use session state to remember the customer
            int? custID = HttpContext.Session.GetInt32("custID");
            Customer customer;

            // Add new customer if the customer ID doesn't yet exist in session
            // Or go to the database and find the customer associated with the ID
            if (custID == null || custID == 0)
            {
                customer = new Customer();
            }
            else
            {
                customer = context.Customers.Find(custID);
            }

            return View(customer);
        }

        [HttpPost]
        [Route("[controller]s")]
        public IActionResult List(Customer customer)
        {
            // Set the Session ID to the customer that was passed to the List method
            HttpContext.Session.SetInt32("custID", customer.CustomerID);

            if (customer.CustomerID == 0)
            {
                TempData["message"] = "You must select a customer.";
                return RedirectToAction("GetCustomer");
            }
            else
            {
                return RedirectToAction("List", new { id = customer.CustomerID });
            }
        }

        [HttpGet]
        [Route("[controller]s")]
        public IActionResult List(int id)
        {
            /***************************************************
            * Must add the RegistrationViewModel (see video 3/3)
            ****************************************************/
            RegistrationViewModel model = new RegistrationViewModel
            {
                CustomerID = id,
                Customer = context.Customers.Find(id),
                Products = context.Products
                    .OrderBy(p => p.Name)
                    .ToList(),
                Registrations = context.Registrations
                    .Include(r => r.Customer)
                    .Include(r => r.Product)
                    .Where(r => r.CustomerID == id)
                    .ToList()
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult Filter(int customerID = 0)
        {
            return RedirectToAction("List", new { ID = customerID });
        }

        [HttpPost]
        public IActionResult Register(RegistrationViewModel model)
        // The model is passed by the List View when the user selects "Register" button
        {
            if (model.ProductID == 0)
            {
                TempData["message"] = "You must select a product.";
            }
            else
            {
                Registration registration = new Registration
                {
                    CustomerID = model.CustomerID,
                    ProductID = model.ProductID
                };
                context.Registrations.Add(registration);

                try
                {
                    context.SaveChanges();
                }
                catch (DbUpdateException ex)
                {
                    string msg = (ex.InnerException == null) ? ex.Message : ex.InnerException.Message;
                    if (msg.Contains("duplicate key"))
                        TempData["message"] = "This product is already registered to this customer";
                    else
                        TempData["message"] = "Error accessing database: " + msg;
                }
            }
            return RedirectToAction("List", new { ID = model.CustomerID });
        }

        [HttpPost]
        public IActionResult Delete(int customerID, int productID)
        // customerID and productID are hidden input from the List View 
        {
            // initialize 
            Registration registration = new Registration
            {
                CustomerID = customerID,
                ProductID = productID
            };
            // Dbcontext removes the registration from the DB on save
            context.Registrations.Remove(registration);
            context.SaveChanges();
            return RedirectToAction("List", new { ID = customerID });
        }
    }
}