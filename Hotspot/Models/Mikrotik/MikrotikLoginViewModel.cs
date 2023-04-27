using System;

namespace Hotspot.Models.Mikrotik
{
    public class MikrotikLoginViewModel
    {
        public string Password { get; set; }
        public string Ip { get; set; }
        public TimeSpan Time { get; set; }
        public string Bandwith { get; set; }
        public string Franchise { get; set; }

        public MikrotikLoginViewModel()
        {
        }

        public MikrotikLoginViewModel(string ip, string password, TimeSpan time)
        {
            Password = password;
            Ip = ip;
            Time = time;
        }
    }
}
