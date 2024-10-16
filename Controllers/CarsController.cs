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

  // NOTE you can have multiple dependencies for a single class. Make sure their values are assigned in the constructor
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

  [HttpGet("{carId}")]
  public ActionResult<Car> GetCarById(int carId)
  {
    try
    {
      Car car = _carsService.GetCarById(carId);
      return Ok(car);
    }
    catch (Exception exception)
    {
      return BadRequest(exception.Message);
    }
  }

  // NOTE Authorize denotes that you must have a valid bearer token to access this endpoint
  [Authorize] // .use(Auth0Provider)
  [HttpPost]
  public async Task<ActionResult<Car>> CreateCar([FromBody] Car carData)
  {
    try
    {
      // NOTE GetUserInfoAsync method allows us to decode a bearer token
      // Must be awaited, as this is an external call to an API
      // HttpContext has information about our request and response. Includes headers from the request that contain bearer tokens from client
      Account userInfo = await _auth0Provider.GetUserInfoAsync<Account>(HttpContext); // req.userInfo
      // REVIEW never ever trust the client
      carData.CreatorId = userInfo.Id;
      Car car = _carsService.CreateCar(carData);
      return Ok(car);
    }
    catch (Exception error)
    {
      return BadRequest(error.Message);
    }
  }

  [Authorize]
  [HttpDelete("{carId}")]
  public async Task<ActionResult<string>> DeleteCar(int carId)
  {
    try
    {
      Account userInfo = await _auth0Provider.GetUserInfoAsync<Account>(HttpContext);
      string message = _carsService.DeleteCar(carId, userInfo.Id);
      return Ok(message);
    }
    catch (Exception error)
    {
      return BadRequest(error.Message);
    }
  }

  [Authorize]
  [HttpPut("{carId}")]
  public async Task<ActionResult<Car>> UpdateCar(int carId, [FromBody] Car carUpdateData)
  {
    try
    {
      Account userInfo = await _auth0Provider.GetUserInfoAsync<Account>(HttpContext);
      Car car = _carsService.UpdateCar(carId, userInfo.Id, carUpdateData);
      return Ok(car);
    }
    catch (Exception error)
    {
      return BadRequest(error.Message);
    }
  }
}