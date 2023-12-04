using System.Text.Json.Serialization;

namespace TextSearchDemo.Models
{
    public class Child(string value, string propertyName, int weight, bool fullWord, object result)
    {
        public string EntityType { get; } = result.GetType().Name;
        public string Value { get; } = value;
        public string PropertyName { get; } = propertyName;
        public int Weight { get; } = weight;
        public object entity { get; } = result;
        public bool FullWord { get; } = fullWord;
    }
}
