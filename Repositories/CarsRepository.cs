

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
    string sql = "SELECT * FROM cars;";

    List<Car> cars = _db.Query<Car>(sql).ToList();
    return cars;
  }

  internal Car CreateCar(Car carData)
  {
    string sql = @"
    INSERT INTO 
    cars(make, model, year, price, mileage, engineType, color, imgUrl, hasCleanTitle, creatorId)
    VALUES(@Make, @Model, @Year, @Price, @Mileage, @EngineType, @Color, @ImgUrl, @HasCleanTitle, @CreatorId);
    
    SELECT * FROM cars WHERE id = LAST_INSERT_ID();";

    Car car = _db.Query<Car>(sql, carData).FirstOrDefault();
    return car;
  }
}