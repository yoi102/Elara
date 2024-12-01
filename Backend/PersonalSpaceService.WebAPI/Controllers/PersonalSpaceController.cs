using Microsoft.AspNetCore.Mvc;
using PersonalSpaceService.WebAPI;

namespace PersonalSpaceService.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PersonalSpaceController : ControllerBase
    {
     
        private readonly ILogger<PersonalSpaceController> _logger;

        public PersonalSpaceController(ILogger<PersonalSpaceController> logger)
        {
            _logger = logger;
        }

     





    }
}
