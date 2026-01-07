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

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] YandexRequest request)
        {
            string walAddress = Environment.GetEnvironmentVariable("WAL_ADDRESS");

            var cmd = request.Request.Command.ToLower();
            if (cmd.Contains("включи") && cmd.Contains("пк"))
            {
                WakeOnLan(walAddress);
            }

            var response = new
            {
                version = "1.0",
                session = new
                {
                    session_id = request.Session.SessionId,
                    user_id = request.Session.User.UserId
                },
                response = new
                {
                    text = "готово",
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

            byte[] packet = new byte[102];
            for (int i = 0; i < 6; i++) packet[i] = 0xFF;
            for (int i = 1; i <= 16; i++) Array.Copy(mac, 0, packet, i * 6, 6);

            using (UdpClient client = new UdpClient())
            {
                client.EnableBroadcast = true;
                client.Send(packet, packet.Length, new IPEndPoint(IPAddress.Broadcast, 9));
            }
        }
    }
}
