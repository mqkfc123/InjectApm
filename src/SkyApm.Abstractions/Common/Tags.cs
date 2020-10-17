
namespace SkyApm.Abstractions.Common
{
    public static class Tags
    {
        public static readonly string URL = "url";

        public static readonly string PATH = "path";


        public static readonly string HTTP_METHOD = "http.method";

        public static readonly string STATUS_CODE = "status_code";

        public static readonly string DB_TYPE = "db.type";

        public static readonly string DB_INSTANCE = "db.instance";

        public static readonly string DB_STATEMENT = "db.statement";

        public static readonly string DB_BIND_VARIABLES = "db.bind_vars";

        public static readonly string MQ_TOPIC = "mq.topic";

        public static readonly string MQ_BROKER = "mq.broker";

        public static readonly string GRPC_METHOD_NAME = "grpc.method";

        public static readonly string GRPC_STATUS = "grpc.status";
    }
}
