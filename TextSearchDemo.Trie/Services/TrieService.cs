using System.Reflection;
using TextSearchDemo.Models;
using TextSearchDemo.Trie.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using TextSearchDemo.Trie.Models;

namespace TextSearchDemo.Trie.Services
{
    public class TrieService(IReadService readService, IMemoryCache memoryCache) : ITrieService
    {
        private readonly IReadService _readService = readService;
        private readonly IMemoryCache _memoryCache = memoryCache;

        private static readonly SemaphoreSlim _semaphore = new(1, 1);

        public async Task<IEnumerable<Child>> SearchAsync(string searchText, CancellationToken cancellationToken) 
        {
            searchText = searchText.ToLowerInvariant().TrimEnd();
            var trie = await GetTrieAsync(cancellationToken);
            var node = trie.Node;
            var result = new List<Child>();
            for (int i = 0; i < searchText.Length; i++)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var c = searchText[i];
                if (!char.IsLetterOrDigit(c))
                {
                    continue;
                }

                if (!node.ContainsKey(c))
                {
                    return new List<Child>();
                }

                if (i == searchText.Length - 1)
                {
                    result = node[c].Children.Values.ToList();
                    break;
                }

                node = node[c].Node;
            }

            // improvement: this sorting can be avoided by keeping list sorted beforehand
            return result.OrderByDescending(x => x.Weight).Take(20).ToList();
        }

        private async Task<TrieModel> GetTrieAsync(CancellationToken cancellationToken)
        {
            if (!_memoryCache.TryGetValue(nameof(Trie), out TrieModel tree))
            {
                try
                {
                    await _semaphore.WaitAsync(cancellationToken);

                    if (!_memoryCache.TryGetValue(nameof(Trie), out tree))
                    {
                        tree = await InitAsync();
                        // expiration?
                        _memoryCache.Set(nameof(Trie), tree);
                    }
                }
                finally
                {
                    _semaphore.Release();
                }
            }

            return tree;
        }

        private async Task<TrieModel> InitAsync()
        {
            var dataModel = await _readService.ReadAsync();
            var buildings = dataModel.Buildings.ToDictionary(x => x.Id, x => x);
            var groups = dataModel.Groups.ToDictionary(x => x.Id, x => x);
            foreach (var item in dataModel.Locks)
                item.Building = buildings[item.BuildingId];
            foreach (var item in dataModel.Media)
                item.Group = groups[item.GroupId];

            var trie = new TrieModel();
            Insert(trie, dataModel.Buildings);
            Insert(trie, dataModel.Groups);
            Insert(trie, dataModel.Locks);
            Insert(trie, dataModel.Media);
            return trie;
        }

        private void Insert<T>(TrieModel trie, List<T> items) 
        {
            var className = typeof(T).Name;
            foreach (var item in items)
            {
                Insert(trie, className, item, item);
            }
        }

        private void Insert(TrieModel trie, string className, object item, object baseItem) 
        {
            foreach (PropertyInfo prop in item.GetType().GetProperties())
            {
                var type = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
                var propertyValue = prop.GetValue(item, null);

                if (propertyValue == null) continue;

                if (type == typeof(Building) || type == typeof(Group))
                {
                    var path = $"{className}.{prop.Name}";
                    Insert(trie, path, propertyValue, item);
                }
                else
                {
                    var propertyName = prop.Name;
                    var weight = Weights.GetWeight($"{className}.{propertyName}");
                    if (weight != -1)
                    {
                        var suffixes = GetSuffixes(propertyValue.ToString());
                        var isSuffix = false;
                        foreach (var suffix in suffixes)
                        {
                            Insert(trie, suffix, propertyName, weight, baseItem, isSuffix);
                            isSuffix = true;
                        }
                    }
                }
            }
        }

        private string[] GetSuffixes(string text)
        {
            var n = text.Length;
            var suffixes = new string[n];

            for (var i = 0; i < n; i++)
            {
                suffixes[i] = text.Substring(i);
            }

            return suffixes;
        }

        private void Insert(TrieModel trie, string value, string propertyName, int weight, object entity, bool isSuffix) 
        {
            value = value.ToLowerInvariant();
            var child = new Child(value, propertyName, weight, fullWord: false, entity);
            var node = trie.Node;
            for (int i = 0; i < value.Length; i++)
            {
                if (i == value.Length - 1 && !isSuffix)
                {
                    child = new Child(value, propertyName, weight * 10, fullWord: true, entity);
                }

                var c = value[i];
                if (!char.IsLetterOrDigit(c))
                {
                    continue;
                }

                var childEntity = (IEntity)child.entity;
                if (!node.ContainsKey(c))
                {
                    var newNode = new TrieModel();
                    newNode.Children[childEntity.Id] = child;
                    node[c] = newNode;
                }
                else
                {
                    // store entities only by their the most high weighted property
                    if (node[c].Children.TryGetValue(childEntity.Id, out var existingChild))
                    {
                        if (existingChild.Weight < child.Weight)
                        {
                            node[c].Children[childEntity.Id] = child;
                        }
                    }
                    else
                    {
                        node[c].Children[childEntity.Id] = child;
                    }
                }

                node = node[c].Node;
            }
        }
    }
}
