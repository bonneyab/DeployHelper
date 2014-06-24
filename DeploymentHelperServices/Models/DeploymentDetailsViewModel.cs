using System.Collections.Generic;

namespace DeploymentHelperServices.Models
{
    public class DeploymentDetailsViewModel
    {
        public Contract.DTO.Deployment Deployment { get; set; }
        public IEnumerable<GroupViewModel> Groupings { get; set; }
    }
}