using System.Text.Json.Serialization;
using TextSearchDemo.Trie.Interfaces;

namespace TextSearchDemo.Models
{
    public class Media : IEntity
    {
        public Guid Id { get; set; }
        public Guid GroupId { get; set; }
        [JsonIgnore]
        public Group Group { get; set; }
        public string Type { get; set; }
        public string Owner { get; set; }
        public string SerialNumber { get; set; }
        public string Description { get; set; }
    }

}
