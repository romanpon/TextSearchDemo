using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextSearchDemo.Models;

namespace TextSearchDemo.Trie.Interfaces
{
    public interface IReadService
    {
        Task<DataModel> ReadAsync();
    }
}
