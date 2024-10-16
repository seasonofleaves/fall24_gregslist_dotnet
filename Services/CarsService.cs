
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
}