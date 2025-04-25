using lockserver;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class WorkController : ControllerBase
{
    private readonly ILockService _lockService;

    public WorkController(ILockService lockService)
    {
        _lockService = lockService;
    }

    [HttpPost("do-locked-work")]
    public async Task<IActionResult> DoWorkAsync()
    {
        var success = await _lockService.ExecuteWithLockAsync("chave-nfe", TimeSpan.FromSeconds(30), async () =>
        {
            // Protected critical section
            await Task.Delay(30000); // simulate work
            Console.WriteLine("Work done inside lock");
        });

        if (!success)
            return Conflict("Could not acquire lock. Try again later.");

        return Ok("Work done safely!");
    }
}
