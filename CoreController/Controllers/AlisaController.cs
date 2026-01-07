using CoreController.Commands;
using CoreController.Contracts;
using CoreController.Utils;
using Microsoft.AspNetCore.Mvc;

namespace CoreController.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AlisaController : ControllerBase
    {
        private readonly CommandRegistry _registry;
        private readonly ILogger<AlisaController> _logger;
        public AlisaController(ILogger<AlisaController> logger, CommandRegistry registry)
        {
            _logger = logger;
            _registry = registry;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] YandexRequest request)
        {
            _logger.LogInformation($"Received command: {request.Request.Command}");
            var response = new AlisaResponse
            {
                Version = "1.0",
                Session = request.Session,
                Response = new Response
                {
                   End_Session = false,
                   Text = ""
                }
            };        

            if (request.Session.New)
            {
                response.Response.Text = "Привет!";
                return Ok(response);
            }

            var cmdKey =  CommandParser.ParseCommand(request.Request.Command);
            var command = _registry.GetCommand(cmdKey);
            response.Response.Text = command != null
            ? await command.ExecuteAsync()
            : "Неизвестная команда";

            return Ok(response);

        }
    }
}
