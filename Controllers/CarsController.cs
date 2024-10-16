namespace gregslist_csharp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CarsController : ControllerBase
{
  public CarsController(CarsService carsService, Auth0Provider auth0Provider)
  {
    _carsService = carsService;
    _auth0Provider = auth0Provider;
  }

  private readonly CarsService _carsService;
  private readonly Auth0Provider _auth0Provider;


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

  [Authorize]
  [HttpPost]
  public async Task<ActionResult<Car>> CreateCar([FromBody] Car carData)
  {
    try
    {
      Account userInfo = await _auth0Provider.GetUserInfoAsync<Account>(HttpContext); // req.userInfo
      carData.CreatorId = userInfo.Id;
      Car car = _carsService.CreateCar(carData);
      return Ok(car);
    }
    catch (Exception error)
    {
      return BadRequest(error.Message);
    }
  }
}