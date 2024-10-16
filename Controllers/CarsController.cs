namespace gregslist_csharp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CarsController : ControllerBase
{
  public CarsController(CarsService carsService)
  {
    _carsService = carsService;
  }

  private readonly CarsService _carsService;


  [HttpGet]
  public ActionResult<List<Car>> GetAllCars()
  {
    try
    {
      List<Car> cars = _carsService.GetAllCars();
      return Ok(cars);
    }
    catch (Exception error)
    {
      return BadRequest(error.Message);
    }
  }
}