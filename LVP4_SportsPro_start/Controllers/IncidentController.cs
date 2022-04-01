using LVP4_SportsPro_start.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LVP4_SportsPro_start.Controllers
{
    public class IncidentController : Controller
    {
        private SportsProContext context { get; set; }

        public IncidentController(SportsProContext ctx)
        {
            context = ctx;
        }

        [Route("[controller]s")]
        public IActionResult List(string filter = "all")
        {
            IncidentListViewModel model = new IncidentListViewModel
            {
                Filter = filter
            };

            IQueryable<Incident> query = context.Incidents
                .Include(i => i.Customer)
                .Include(i => i.Product)
                .OrderBy(i => i.DateOpened);

            if (filter == "unassigned")
            {
                query = query.Where(i => i.TechnicianID == null);
            }

            if (filter == "open")
            {
                query = query.Where(i => i.DateClosed == null);
            }

            List<Incident> incidents = query.ToList();
            model.Incidents = incidents;

            return View(model);
        }

        // helper method
        private IncidentViewModel GetViewModel()
        {
            IncidentViewModel model = new IncidentViewModel
            {
                Customers = context.Customers
                    .OrderBy(c => c.FirstName)
                    .ToList(),

                Products = context.Products
                    .OrderBy(c => c.Name)
                    .ToList(),

                Technicians = context.Technicians
                    .OrderBy(c => c.Name)
                    .ToList(),
            };

            return model;
        }

        public IActionResult Filter(string id)
        {
            return RedirectToAction("List", new { Filter = id });
        }

        [HttpGet]
        public IActionResult Add()
        {
            IncidentViewModel model = GetViewModel();
            model.Incident = new Incident();
            model.Action = "Add";

            return View("AddEdit", model);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            IncidentViewModel model = GetViewModel();
            var incident = context.Incidents.Find(id);
            model.Incident = incident;
            model.Action = "Edit";

            return View("AddEdit", model);
        }

        [HttpPost]
        public IActionResult Save(Incident incident)
        {
            IncidentViewModel model = GetViewModel();
            if (incident.IncidentID == 0)
            {
                model.Action = "Add";
            }
            else
            {
                model.Action = "Edit";
            }

            if (ModelState.IsValid)
            {
                if (model.Action == "Add")
                {
                    context.Incidents.Add(incident);
                }
                else
                {
                    context.Incidents.Update(incident);
                }
                context.SaveChanges();
                return RedirectToAction("List");
            }
            else
            {
                model.Incident = incident;
                return View("AddEdit", model);
            }
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var incident = context.Incidents.Find(id);
            return View(incident);
        }

        [HttpPost]
        public IActionResult Delete(Incident incident)
        {
            context.Incidents.Remove(incident);
            context.SaveChanges();
            return RedirectToAction("List");
        }

    }
}