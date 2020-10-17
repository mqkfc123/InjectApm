
namespace SkyApm.Transport.Http.Entity
{
    public class ServiceInstancePingPkg
    {
        public int serviceInstanceId { get; set; }
        public long time { get; set; }
        public string serviceInstanceUUID { get; set; }
    }
}
