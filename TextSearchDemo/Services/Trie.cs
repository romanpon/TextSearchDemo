using TextSearchDemo.Models;

namespace TextSearchDemo.Services
{
    // Should be made read only to avoid accident edits after initializing 
    public class Trie
    {
        public Dictionary<char, Trie> Node { get; } = new();
        public List<Child> Children { get; } = new();
    }
}
