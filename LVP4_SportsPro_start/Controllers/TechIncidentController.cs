using LVP4_SportsPro_start.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LVP4_SportsPro_start.Controllers
{
    public class TechIncidentController : Controller
    {
        private SportsProContext context { get; set; }

        public TechIncidentController(SportsProContext ctx)
        {
            context = ctx;
        }

        [HttpGet]
        public IActionResult Get()
        {
            ViewBag.Technicians = context.Technicians
                .OrderBy(c => c.Name)
                .ToList();

            int? techID = HttpContext.Session.GetInt32("techID");
            Technician technician;
            if (techID == null)
            {
                technician = new Technician();
            }
            else
            {
                technician = context.Technicians
                    .Where(t => t.TechnicianID == techID)
                    .FirstOrDefault();
            }

            return View(technician);
        }

        [HttpPost]
        public IActionResult List(Technician technician)
        {
            HttpContext.Session.SetInt32("techID", technician.TechnicianID);

            if (technician.TechnicianID == 0)
            {
                TempData["message"] = "You must select a technician.";
                return RedirectToAction("Get");
            }
            else
            {
                return RedirectToAction("List", new { id = technician.TechnicianID });
            }
        }

        [HttpGet]
        public IActionResult List(int id)
        {
            var model = new TechIncidentViewModel
            {
                Technician = context.Technicians.Find(id),

                Incidents = context.Incidents
                    .Include(i => i.Customer)
                    .Include(i => i.Product)
                    .OrderBy(i => i.DateOpened)
                    .Where(i => i.TechnicianID == id)
                    .Where(i => i.DateClosed == null)
                    .ToList()
            };
            return View(model);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            int? techID = HttpContext.Session.GetInt32("techID");
            var model = new TechIncidentViewModel
            {
                Technician = context.Technicians.Find(techID),

                Incident = context.Incidents
                    .Include(i => i.Customer)
                    .Include(i => i.Product)
                    .FirstOrDefault(i => i.IncidentID == id)
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(IncidentViewModel model)
        {
            Incident i = context.Incidents.Find(model.Incident.IncidentID);
            i.Description = model.Incident.Description;
            i.DateClosed = model.Incident.DateClosed;

            context.Incidents.Update(i);
            context.SaveChanges();

            int? techID = HttpContext.Session.GetInt32("techID");
            return RedirectToAction("List",
                new { id = techID });
        }
    }
}