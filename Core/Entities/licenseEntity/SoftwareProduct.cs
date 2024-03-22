

using System.Text.Json.Serialization;

namespace Core.Entities.licenseEntity
{
  public class SoftwareProduct
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Version { get; set; }
    public string Description { get; set; }
    public string Vendor { get; set; }
    public DateTime ReleaseDate { get; set; } 
    // EF Relation 
}

}