namespace DataAccess.Models
{
    public class Step
    {
        public int GroupingId { get; set; }
        public int StepId { get; set; }
        public int Order { get; set; }
        public string Description { get; set; }
        public bool Complete { get; set; }
    }
}
