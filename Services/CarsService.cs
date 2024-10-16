


namespace gregslist_csharp.Services;

public class CarsService
{
  public CarsService(CarsRepository repository)
  {
    _repository = repository;
  }

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

    _repository.DeleteCar(carId);

    return $"{car.Make} {car.Model} was deleted!";
  }
}