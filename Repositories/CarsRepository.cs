



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
    // NOTE bringing in a car and account on each row from this sql statement, in that order
    string sql = @"
    SELECT
    cars.*,
    accounts.*
    FROM cars
    JOIN accounts ON cars.creatorId = accounts.id;";


    // NOTE when there are multiple pieces of data sharing the same row in your sql statements, you must provide Dapper a type for each piece of data in the order they will be coming in.
    // NOTE The first two type arguments are for the pieces of data coming in on the rows
    // NOTE the third type argument will be the return type from our mapping function
    // NOTE our mapping function will be run on each row returned from the sql statement. It must have parameters set up for the first two type arguments passed to Query
    // NOTE our mapping function attacehs a creator object to each car
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
    
    SELECT
    cars.*,
    accounts.*
    FROM cars
    JOIN accounts ON cars.creatorId = accounts.id
    WHERE cars.id = LAST_INSERT_ID();";

    Car car = _db.Query<Car, Account, Car>(sql, (car, account) =>
    {
      car.Creator = account;
      return car;
    },
     carData).FirstOrDefault();
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

  internal void UpdateCar(Car car)
  {
    string sql = @"
    UPDATE cars
    SET
    make = @Make,
    model = @Model,
    year = @Year,
    price = @Price,
    hasCleanTitle = @HasCleanTitle
    WHERE id = @Id
    LIMIT 1;";

    int rowsAffected = _db.Execute(sql, car);

    if (rowsAffected == 0)
    {
      throw new Exception("No cars were updated");
    }

    if (rowsAffected > 1)
    {
      throw new Exception($"{rowsAffected} cars were updated! Uh oh");
    }
  }
}