using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tik4net;
using tik4net.Objects;
using tik4net.Objects.Ip.Hotspot;

namespace Hotspot.Tools.Mikrotik
{
    public class MikrotikHandler
    {
        private string MkUsername { get; set; }
        private string MkPassword { get; set; }

        public MikrotikHandler()
        {
            MkUsername = "sistema";
            MkPassword = "134system";
        }

        public async Task CreateHotspotUser(string user, string ip, TimeSpan time, long franchise, string profile)
        {
            try
            {
                ITikConnection conn = await ConnectionFactory.OpenConnectionAsync(TikConnectionType.Api, ip, this.MkUsername, this.MkPassword);

                var hotspotUser = new HotspotUser()
                {
                    Name = user,
                    LimitUptime = time.TotalSeconds.ToString(),
                    LimitBytesTotal = franchise,
                    Profile = profile
                };

                conn.Save(hotspotUser);
            }
            catch (Exception e)
            {
                try
                {
                    ITikConnection conn = await ConnectionFactory.OpenConnectionAsync(TikConnectionType.Api, ip, this.MkUsername, this.MkPassword);

                    var hotspotUser = new HotspotUser()
                    {
                        Name = user,
                        LimitUptime = time.TotalSeconds.ToString(),
                        LimitBytesTotal = franchise
                    };

                    conn.Save(hotspotUser);
                }
                catch(Exception e2)
                {

                }
            }
        }

        public async Task CreateProfile(int bandwith, string ip, string name, string server)
        {
            try
            {
                ITikConnection conn = await ConnectionFactory.OpenConnectionAsync(TikConnectionType.Api, ip, this.MkUsername, this.MkPassword);

                var hotspotProfile = new HotspotUserProfile()
                {
                    Name = name,
                    RateLimit = bandwith + "m/" + bandwith + "m",
                    SharedUsers = "1",
                    KeepaliveTimeout = "00:02:00",
                    StatusAutorefresh = "00:01:00",
                    OnLogin = "/ip hotspot user remove [find name=$user]",
                    OnLogout = "/tool fetch url=\"http://" + server + "/mikrotik/logout?password=$user&time=$\"uptime-secs\"&franchise=$\"bytes-total\"\" mode=https keep-result=no"
                };

                conn.Save(hotspotProfile);
            }
            catch(Exception e) { }
        }

        public async Task DeleteProfile(string ip, string name)
        {
            try
            {
                ITikConnection conn = await ConnectionFactory.OpenConnectionAsync(TikConnectionType.Api, ip, this.MkUsername, this.MkPassword);
                var profile = conn.LoadByName<HotspotUserProfile>(name);
                conn.Delete<tik4net.Objects.Ip.Hotspot.HotspotUserProfile>(profile);
            }
            catch (Exception e) { }
        }

        public async Task DeleteActiveConnection(string ip, string name)
        {
            try
            {
                ITikConnection conn = await ConnectionFactory.OpenConnectionAsync(TikConnectionType.Api, ip, this.MkUsername, this.MkPassword);
                var activeConnection = conn.LoadAll<HotspotActive>().Where(o => o.UserName == name).FirstOrDefault();
                conn.CreateCommand("ip/hotspot/active/remove", conn.CreateParameter(".id", activeConnection.Id)).ExecuteNonQuery();
            }
            catch (Exception e) { }
        }

        public async Task<List<HotspotActive>> UpdateTimeAndFranchise(string ip)
        {
            List<HotspotActive> r = new List<HotspotActive>();
            try
            {
                ITikConnection conn = await ConnectionFactory.OpenConnectionAsync(TikConnectionType.Api, ip, this.MkUsername, this.MkPassword);
                r = conn.LoadAll<HotspotActive>().ToList();
            }
            catch (Exception e) { }

            return r;
        }
    }
}
