
namespace gregslist_csharp.Repositories;

public class CarsRepository
{
  public CarsRepository(IDbConnection db)
  {
    _db = db;
  }
  private readonly IDbConnection _db;


  internal List<Car> GetAllCars()
  {
    return [];
  }
}