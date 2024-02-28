using Microsoft.AspNetCore.Mvc.Filters;

namespace CoddingGurrus.Infrastructure.CommonHelper
{
    public class CheckTokenAvailability : ActionFilterAttribute, IAsyncAuthorizationFilter
    {
        public Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            return GetTokenModel.GetToken(string.Empty);
        }
    }
}
