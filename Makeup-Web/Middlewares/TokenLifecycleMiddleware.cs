using Domain_Layer.DTOs.AthanticationDtos;
using Domain_Layer.Interfaces.ServiceInterfaces;
using NuGet.Common;
using System.IdentityModel.Tokens.Jwt;

namespace Makeup_Web.Middlewares
{
    public class TokenLifecycleMiddleware
    {
        private readonly RequestDelegate next;

        public TokenLifecycleMiddleware(RequestDelegate  next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context,IAuthService authService)
        {
            var accessToken = context.Request.Cookies["AccessToken"];
            var refreshToken = context.Request.Cookies["RefreshToken"];

            if (!string.IsNullOrEmpty(accessToken))
            {
                try
                {
                    var handler = new JwtSecurityTokenHandler();
                    var jwtToken = handler.ReadJwtToken(accessToken);
                    if (jwtToken.ValidTo<=DateTime.UtcNow.AddSeconds(-30))
                    {
                        if (!string.IsNullOrEmpty(refreshToken))
                        {
                            var RefreshTokenRespone = await authService.RefreshTokenAsync(refreshToken);
                            if (RefreshTokenRespone.IsAuthenticated)
                            {
                                AppendAuthCookies(context, RefreshTokenRespone);

                                context.Request.Headers["Authorization"] = $"Bearer {RefreshTokenRespone.Token}";
                            }

                        }
                        else 
                        {
                            ClearAuthCookies(context);

                        }

                    }
                    else 
                    {
                        context.Request.Headers["Authorization"] = $"Bearer {accessToken}";
                    }
                }
                catch (Exception)
                {

                    throw;
                }
                

            }

            await next(context);
        }

        private void AppendAuthCookies(HttpContext context, AuthModleDto authModle  ) 
        {
            var options = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = authModle.TokenExpiresOn
            };
            context.Response.Cookies.Append("AccessToken", authModle.Token!, options);
            context.Response.Cookies.Append("RefreshToken", authModle.RefreshToken!, options);

        }

        private void ClearAuthCookies(HttpContext context) 
        {
            context.Response.Cookies.Delete("AccessToken");
            context.Response.Cookies.Delete("RefreshToken");
        }
    }
}
