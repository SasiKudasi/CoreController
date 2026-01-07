using System.Net;
using System.Net.Sockets;

namespace CoreController.Commands
{
    public class WakeOnLanCommand : ICommand
    {
        public string Name => "wake_pc";
        public async Task<string> ExecuteAsync()
        {
            string walAddress = Environment.GetEnvironmentVariable("WAL_ADDRESS") ?? "";
            if (string.IsNullOrEmpty(walAddress))
            {
                return "Не настроен адрес Wake on LAN";
            }
            await WakeUpPC(walAddress);
            return "Включение компьютера";
        }


        public async Task WakeUpPC(string macAddress)
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
                var bts = await client.SendAsync(packet, packet.Length, new IPEndPoint(IPAddress.Broadcast, 9));
            }
        }

    }
}
