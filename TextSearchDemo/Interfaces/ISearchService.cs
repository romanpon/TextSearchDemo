using TextSearchDemo.Models;

namespace TextSearchDemo.Interfaces
{
    public interface ISearchService
    {
        Task<IEnumerable<Child>> Search(string searchText, CancellationToken cancellationToken);
    }
}
