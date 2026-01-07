using CoreController.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Sockets;
using static System.Net.Mime.MediaTypeNames;

namespace CoreController.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AlisaController : ControllerBase
    {
        private readonly ILogger<AlisaController> _logger;
        public AlisaController(ILogger<AlisaController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] YandexRequest request)
        {
            string walAddress = Environment.GetEnvironmentVariable("WAL_ADDRESS");
            var text = "";
            var cmd = request.Request.Command.ToLower();
            if (cmd.Contains("включи") && cmd.Contains("пк"))
            {
                _logger.LogInformation("Отправка запроса на включение");
                WakeOnLan(walAddress);
                text = "готово";

            }
            else
            {
                text = "Неизвестная команда";
            }

            var response = new
            {
                version = "1.0",
                session = new
                {
                    session_id = request.Session.session_id,
                    user_id = request.Session.User.user_id
                },
                response = new
                {
                    text,
                    end_session = false
                }
            };
            return Ok(response);

        }

        private void WakeOnLan(string macAddress)
        {

            byte[] mac = macAddress
                .Split(':')
                .Select(b => Convert.ToByte(b, 16))
                .ToArray();
            _logger.LogInformation($"МАК АДРЕС :  {macAddress}");

            byte[] packet = new byte[102];
            for (int i = 0; i < 6; i++) packet[i] = 0xFF;
            for (int i = 1; i <= 16; i++) Array.Copy(mac, 0, packet, i * 6, 6);

            using (UdpClient client = new UdpClient())
            {
                client.EnableBroadcast = true;
                var bts =  client.Send(packet, packet.Length, new IPEndPoint(IPAddress.Broadcast, 9));
                _logger.LogInformation($"Bytes send :  {bts}");
            }
        }
    }
}
