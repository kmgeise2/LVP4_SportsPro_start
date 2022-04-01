using System.Collections.Generic;

namespace LVP4_SportsPro_start.Models
{
    public class TechIncidentViewModel
    {
        public Technician Technician { get; set; }
        public Incident Incident { get; set; }
        public IEnumerable<Incident> Incidents { get; set; }
    }
}
