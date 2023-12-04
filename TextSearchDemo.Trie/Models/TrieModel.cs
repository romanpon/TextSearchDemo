using TextSearchDemo.Models;

namespace TextSearchDemo.Trie.Models
{
    public class TrieModel
    {
        public Dictionary<char, TrieModel> Node { get; } = new();
        public Dictionary<Guid,  Child> Children { get; } = new();
    }
}
