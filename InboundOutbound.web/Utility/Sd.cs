namespace InboundOutbound.web.Utility
{
    public class Sd
    {
        public static string UserAPIBase { get; set; }
        public static string OutboundAPIBase { get; set; }
        public static string InboundAPIBase { get; set; }
        public static string EmailTrailAPIBase { get; set; }

      
        public const string TokenCookie = "JWTToken";
        public enum ApiType
        {
            GET, POST, PUT, DELETE
        }

       
        public enum ContentType
        {
            Json,
            MultipartFormData,
        }
    }
}
