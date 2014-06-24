namespace DeploymentHelperServices.Models
{
    public class GroupViewModel
    {
        public string Name { get; set; }
        public int Weight { get; set; }
        public int GroupingId { get; set; }
        public int DeploymentId { get; set; }

        public string Owner { get; set; }
        public bool VendorActionRequired { get; set; }
        public bool VendorActionComplete { get; set; }
        public string Status { get; set; }
        public string CssClass { get; set; }
    }
}