using TextSearchDemo.Configuration;
using TextSearchDemo.Trie.Interfaces;
using Microsoft.Extensions.Options;
using TextSearchDemo.Models;
using System.Text.Json;

namespace TextSearchDemo.Trie.Services
{
    public class ReadService : IReadService
    {
        private readonly Settings settings;

        public ReadService(IOptions<Settings> settings)
        {
            this.settings = settings.Value;
        }

        public async Task<DataModel> ReadAsync()
        {
            var jsonString = await File.ReadAllTextAsync(settings.FilePath);
            var dataModel = JsonSerializer.Deserialize<DataModel>(jsonString, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return dataModel;
        }
    }
}
