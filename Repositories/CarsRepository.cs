



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
    string sql = @"
    SELECT
    cars.*,
    accounts.*
    FROM cars
    JOIN accounts ON cars.creatorId = accounts.id;";

    List<Car> cars = _db.Query<Car, Account, Car>(sql, (car, account) =>
    {
      car.Creator = account;
      return car;
    }).ToList();
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

  internal Car GetCarById(int carId)
  {
    string sql = @"
    SELECT
    cars.*,
    accounts.*
    FROM cars
    JOIN accounts ON cars.creatorId = accounts.id
    WHERE cars.id = @carId;";

    Car car = _db.Query<Car, Account, Car>(sql, (car, account) =>
    {
      car.Creator = account;
      return car;
    }, new { carId }).FirstOrDefault();
    return car;
  }

  internal void DeleteCar(int carId)
  {
    string sql = "DELETE FROM cars WHERE id = @carId LIMIT 1;";

    int rowsAffected = _db.Execute(sql, new { carId });

    if (rowsAffected == 0)
    {
      throw new Exception("No cars were deleted");
    }

    if (rowsAffected > 1)
    {
      throw new Exception($"{rowsAffected} cars were deleted! Uh oh");
    }
  }
}