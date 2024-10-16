using System.ComponentModel.DataAnnotations;

namespace gregslist_csharp.Models;

public class Car
{
  public int Id { get; set; }
  public DateTime CreatedAt { get; set; }
  public DateTime UpdatedAt { get; set; }

  [MinLength(3), MaxLength(50)]
  public string Make { get; set; }

  [MinLength(1), MaxLength(100)]
  public string Model { get; set; }

  [Range(1886, 2025)]
  public uint? Year { get; set; }

  [Range(0, 1000000)]
  // NOTE ? allows property to default to null instead of 0
  public uint? Price { get; set; }

  [MaxLength(500)]
  public string ImgUrl { get; set; }

  [MaxLength(500)]
  public string Description { get; set; }

  public string EngineType { get; set; }

  [MaxLength(50)]
  public string Color { get; set; }

  [Range(0, 1000000)]
  public uint Mileage { get; set; }

  // NOTE defaults to true if not present in object being cast into this class
  // public bool HasCleanTitle { get; set; } = true;
  public bool? HasCleanTitle { get; set; }

  public string CreatorId { get; set; }
  public Account Creator { get; set; }
}