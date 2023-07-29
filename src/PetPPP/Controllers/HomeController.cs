using Microsoft.AspNetCore.Mvc;
using Rebus.Bus;
using ServiceContracts;

namespace PetPPP.Controllers;

[Route("api/[controller]")]
public class HomeController : ControllerBase
{
    private readonly IBus _bus;

    public HomeController(IBus bus)
    {
        _bus = bus;
    }

    [HttpGet]
    public async Task GetAsync([FromQuery] string justName)
    {
        await _bus.Send(new UserRegisteredEvent(justName));
    }
}