using System.Collections.Generic;
using Contract.DTO;

namespace DeploymentHelperServices.Models
{
    public class GroupingDetailsViewModel
    {
        public List<Step> Steps { get; set; }
        public List<Grouping> Dependencies { get; set; }
        public Grouping Group { get; set; }
        public List<Grouping> AvailableDependencies { get; set; }
    }
}