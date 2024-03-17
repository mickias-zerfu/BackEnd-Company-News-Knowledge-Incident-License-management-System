using System.Text.Json;
using System.Text.Json.Serialization;
using Core.Entities;
using Core.Entities.licenseEntity;

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
            if (!context.KnowledgeBases.Any())

            {
                var KnowledgeData = File.ReadAllText("../Infrastructure/Data/SeedData/knowledge.json");

                var options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve,
                    MaxDepth = 32 // Set a maximum depth to avoid infinite loops
                };

                var KnowledgeList = JsonSerializer.Deserialize<List<KnowledgeBase>>(KnowledgeData, options);
                context.KnowledgeBases.AddRange(KnowledgeList);
            }
            if (!context.Incidents.Any())

            {
                var IncidentData = File.ReadAllText("../Infrastructure/Data/SeedData/incident.json");

                var options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve,
                    MaxDepth = 32 // Set a maximum depth to avoid infinite loops
                };

                var IncidentList = JsonSerializer.Deserialize<List<Incident>>(IncidentData, options);
                context.Incidents.AddRange(IncidentList);
            }


            // if (!context.SharedResources.Any())
            // {
            //     var json = await File.ReadAllTextAsync("../Infrastructure/Data/SeedData/file.json");
            //     var sharedResources = JsonSerializer.Deserialize<List<SharedResource>>(json);

            //     await context.SharedResources.AddRangeAsync(sharedResources);
            //     await context.SaveChangesAsync();
            // }



            if (!context.SoftwareProducts.Any())
            {
                var softwareProductsData = File.ReadAllText("../Infrastructure/Data/SeedData/SoftwareProduct.json");
                var softwareProducts = JsonSerializer.Deserialize<List<SoftwareProduct>>(softwareProductsData);
                context.SoftwareProducts.AddRange(softwareProducts);
            }

            if (!context.Licenses.Any())
            {
                var licensesData = File.ReadAllText("../Infrastructure/Data/SeedData/License.json");
                var licenses = JsonSerializer.Deserialize<List<License>>(licensesData);
                context.Licenses.AddRange(licenses);
            }

            if (!context.LicenseManagers.Any())
            {
                var licenseManagersData = File.ReadAllText("../Infrastructure/Data/SeedData/LicenseManager.json");
                var licenseManagers = JsonSerializer.Deserialize<List<LicenseManager>>(licenseManagersData);
                context.LicenseManagers.AddRange(licenseManagers);
            }
            if (context.ChangeTracker.HasChanges()) await context.SaveChangesAsync();
        }
    }
}