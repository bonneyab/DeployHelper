namespace Contract.DTO
{
    public class Dependency
    {
        public int DependencyId { get; set; }
        public int DependentGroupingId { get; set; }
        public int DependentOnGroupingId{ get; set; }
    }
}
