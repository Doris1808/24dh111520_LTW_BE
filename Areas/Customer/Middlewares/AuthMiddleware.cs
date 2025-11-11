using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace _24dh111520_LTW.Middleware
{
    public class AuthMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path.ToString().ToLower();

            // ✅ Cho phép truy cập các trang không cần đăng nhập
            if (
                path.Contains("/account/login") ||
                path.Contains("/account/register") ||
                path.Contains("/customer/home/index") ||
                path.Contains("/customer/home/productdetail")
            )
            {
                await _next(context);
                return;
            }

            // ✅ Chỉ CHẶN khi vào giỏ hàng hoặc thanh toán
            if (path.Contains("/cart"))
            {
                if (string.IsNullOrEmpty(context.Session.GetString("Username")))
                {
                    // Lưu trang hiện tại để quay lại sau khi đăng nhập
                    context.Session.SetString("ReturnUrl", path + context.Request.QueryString);
                    context.Response.Redirect("/Customer/Account/Login");
                    return;
                }
            }

            await _next(context);
        }
    }

    public static class AuthMiddlewareExtensions
    {
        public static IApplicationBuilder UseAuthMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AuthMiddleware>();
        }
    }
}
