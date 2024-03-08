using System.Text.Json;
using System.Text.Json.Serialization;
using Core.Entities;

namespace Infrastructure.Data
{
    public class StoreContextSeed
    {
        public static async Task SeedAsync(StoreContext context)
        {
            if (!context.News.Any())

            {
                var newsData = File.ReadAllText("../Infrastructure/Data/SeedData/news.json");

                var options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve,
                    MaxDepth = 32 // Set a maximum depth to avoid infinite loops
                };

                var newsList = JsonSerializer.Deserialize<List<News>>(newsData, options);

                context.News.AddRange(newsList);
            }
            if (context.ChangeTracker.HasChanges()) await context.SaveChangesAsync();
        }


    }
}