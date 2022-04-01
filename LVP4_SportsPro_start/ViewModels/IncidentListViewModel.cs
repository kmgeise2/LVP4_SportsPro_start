using System.Collections.Generic;

namespace LVP4_SportsPro_start.Models
{
    public class IncidentListViewModel
    {
        public string Filter { get; set; }
        public IEnumerable<Incident> Incidents { get; set; }
    }
}
