using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hotelservices.UserAuth.Controllers
{
    [Authorize]
    [Controller]
    [Route("[controller]")]
    public class TestContoller : Controller
    {
        [HttpGet]
        public IActionResult Hello()
        {
            return StatusCode(200);
        }
    }
}
