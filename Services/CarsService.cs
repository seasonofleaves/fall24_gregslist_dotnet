


namespace gregslist_csharp.Services;

public class CarsService
{
  public CarsService(CarsRepository repository)
  {
    _repository = repository;
  }

  // NOTE Each service should only have one dedicated repository
  private readonly CarsRepository _repository;


  internal List<Car> GetAllCars()
  {
    List<Car> cars = _repository.GetAllCars();
    return cars;
  }

  internal Car CreateCar(Car carData)
  {
    Car car = _repository.CreateCar(carData);
    return car;
  }

  internal Car GetCarById(int carId)
  {
    Car car = _repository.GetCarById(carId);

    if (car == null)
    {
      throw new Exception($"Invalid car id: {carId}");
    }

    return car;
  }

  internal string DeleteCar(int carId, string userId)
  {
    Car car = GetCarById(carId);

    if (car.CreatorId != userId)
    {
      throw new Exception("That ain't your car, pal");
    }

    // NOTE no need to return data here, we just need to update the database
    _repository.DeleteCar(carId);

    return $"{car.Make} {car.Model} was deleted!";
  }

  internal Car UpdateCar(int carId, string userId, Car carUpdateData)
  {
    Car car = GetCarById(carId);

    if (car.CreatorId != userId)
    {
      throw new Exception("Not your car, bud");
    }
    // null coalescing operator (??) determines if the left-hand side value is null. If it is, it defaults to the right-hand side
    car.Make = carUpdateData.Make ?? car.Make;
    car.Model = carUpdateData.Model ?? car.Model;
    // NOTE must allow number values to be nullable in the Car model for this check to work
    car.Price = carUpdateData.Price ?? car.Price;
    car.Year = carUpdateData.Year ?? car.Year;
    // NOTE must allow boolean values to be nullable in the Car model for this check to work
    car.HasCleanTitle = carUpdateData.HasCleanTitle ?? car.HasCleanTitle;

    _repository.UpdateCar(car);

    return car;
  }
}