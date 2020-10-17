
namespace SkyApm.Transport.Http.Entity
{
    public enum SpanType
    {
        Entry = 0,
        Exit = 1,
        Local = 2
    }

    public enum SpanLayer
    {
        Unknown = 0,
        Database = 1,
        Rpcframework = 2,
        Http = 3,
        Mq = 4,
        Cache = 5
    }

    public enum RefType
    {
        CrossProcess = 0,
        CrossThread = 1
    }

    public enum ServiceType
    {
       
        Normal = 0,
      
        Database = 1,
    
        Mq = 2,
      
        Cache = 3,
       
        Browser = 4,
    }


}
