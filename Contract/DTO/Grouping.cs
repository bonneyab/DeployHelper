namespace Contract.DTO
{
    public class Grouping
    {
        public string Name { get; set; }
        public int Weight { get; set; }
        public int GroupingId { get; set; }
        public int DeploymentId { get; set; }

        public string Notes { get; set; }
        public string Owner { get; set; }
        public string VendorNotes { get; set; }
        public bool VendorActionRequired { get; set; }
        public bool VendorActionComplete { get; set; }
        public bool Completed { get; set; }
    }
}
