using TextSearchDemo.Models;

namespace TextSearchDemo.Trie.Interfaces
{
    public interface ITrieService
    {
        Task<IEnumerable<Child>> SearchAsync(string searchText, CancellationToken cancellationToken);
    }
}
