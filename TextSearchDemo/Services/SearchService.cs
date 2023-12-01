using Microsoft.Extensions.Caching.Memory;
using System.Reflection;
using System.Text.Json;
using TextSearchDemo.Interfaces;
using TextSearchDemo.Models;

namespace TextSearchDemo.Services
{
    public class SearchService : ISearchService
    {
        private readonly IMemoryCache memoryCache;
        private static readonly SemaphoreSlim semaphore = new(1, 1);

        public SearchService(IMemoryCache memoryCache)
        {
            this.memoryCache = memoryCache;
        }

        public async Task<IEnumerable<Child>> Search(string searchText, CancellationToken cancellationToken)
        {
            if (!memoryCache.TryGetValue(nameof(Trie), out Trie tree))
            {
                try
                {
                    await semaphore.WaitAsync(cancellationToken);

                    if (!memoryCache.TryGetValue(nameof(Trie), out tree))
                    {
                        tree = await Init();
                        // expiration?
                        memoryCache.Set(nameof(Trie), tree);
                    }
                }
                finally
                {
                    semaphore.Release();
                }
            }

            searchText = searchText.ToLowerInvariant().TrimEnd();
            var node = tree.Node;
            var result = new List<Child>();
            for (int i = 0; i < searchText.Length; i++)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var c = searchText[i];
                if (i == searchText.Length - 1)
                {
                    result = node[c].Children;
                    break;
                }

                if (!node.ContainsKey(c))
                {
                    return new List<Child>();
                }

                node = node[c].Node;
            }

            return result.OrderByDescending(x => x.Weight).ToList();
        }

        private async Task<Trie> Init()
        {
            var jsonString = await File.ReadAllTextAsync("./Files/sv_lsm_data.json");
            var dataModel = JsonSerializer.Deserialize<DataModel>(jsonString, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            var buildings = dataModel.Buildings.ToDictionary(x => x.Id, x => x);
            var groups = dataModel.Groups.ToDictionary(x => x.Id, x => x);
            foreach (var item in dataModel.Locks)
                item.Building = buildings[item.BuildingId];
            foreach (var item in dataModel.Media)
                item.Group = groups[item.GroupId];

            var tree = new Trie();
            Insert(tree, dataModel.Buildings);
            Insert(tree, dataModel.Groups);
            Insert(tree, dataModel.Locks);
            Insert(tree, dataModel.Media);
            return tree;
        }

        private void Insert<T>(Trie tree, List<T> items)
        {
            var className = typeof(T).Name;
            foreach (var item in items)
            {
                Insert(tree, className, item, item);
            }
        }

        private void Insert<T>(Trie tree, string className, T item, T baseItem)
        {
            foreach (PropertyInfo prop in item.GetType().GetProperties())
            {
                var type = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
                var propertyValue = prop.GetValue(item, null);

                if (propertyValue == null) continue;

                if (type == typeof(Building) || type == typeof(Group))
                {
                    var path = $"{className}.{propertyValue}";
                    Insert(tree, path, propertyValue, item);
                }
                else
                {
                    var propertyName = prop.Name;
                    var weight = Weights.GetWeight($"{className}.{propertyName}");
                    if (weight != -1)
                    {
                        Insert(tree, propertyValue.ToString(), propertyName, weight, baseItem);
                    }
                }
            }
        }

        private void Insert(Trie tree, string value, string propertyName, int weight, object result)
        {
            value = value.ToLowerInvariant();
            var child = new Child(value, propertyName, weight, fullWord: false, result);
            var node = tree.Node;
            for (int i = 0; i < value.Length; i++)
            {
                if (i == value.Length - 1)
                {
                    child = new Child(value, propertyName, weight * 10, fullWord: true, result);
                }

                var c = value[i];
                if (!node.ContainsKey(c))
                {
                    var newNode = new Trie();
                    newNode.Children.Add(child);
                    node[c] = newNode;
                }
                else
                {
                    node[c].Children.Add(child);
                }

                node = node[c].Node;
            }
        }
    }
}
