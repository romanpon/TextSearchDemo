using System.Text.Json.Serialization;
using TextSearchDemo.Trie.Interfaces;

namespace TextSearchDemo.Models
{
    public class Lock : IEntity
    {
        public Guid Id { get; set; }
        public Guid BuildingId { get; set; }
        [JsonIgnore]
        public Building Building { get; set; }
        public string Type { get; set; }
        public string SerialNumber { get; set; }
        public string Floor { get; set; }
        public string RoomNumber { get; set; }
        public string Description { get; set; }
    }

}
