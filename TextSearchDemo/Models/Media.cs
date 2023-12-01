namespace TextSearchDemo.Models
{
    public class Media
    {
        public Guid Id { get; set; }
        public Guid GroupId { get; set; }
        public Group Group { get; set; }
        public string Type { get; set; }
        public string Owner { get; set; }
        public string SerialNumber { get; set; }
        public string Description { get; set; }
    }

}
