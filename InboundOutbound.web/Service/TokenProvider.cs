
using InboundOutbound.web.Service.IService;
using InboundOutbound.web.Utility;
using Newtonsoft.Json.Linq;

namespace InboundOutbound.web.Service
{
    public class TokenProvider : ITokenProvider
    {
        private readonly IHttpContextAccessor _contextAccessor;
        public TokenProvider(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }
        public void ClearToken()
        {
            _contextAccessor.HttpContext?.Response.Cookies.Delete(Sd.TokenCookie);
        }

        public string? GetToken()
        {
            string? token = null;
            bool? hasToken=_contextAccessor.HttpContext?.Request.Cookies.TryGetValue(Sd.TokenCookie, out token);
            return hasToken is true ? token : null;
        }

        public void SetToken(string token)
        {
           _contextAccessor.HttpContext?.Response.Cookies.Append(Sd.TokenCookie, token);
        }
    }
}
