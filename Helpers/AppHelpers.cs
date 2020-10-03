using GSCrm.Data;
using GSCrm.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Security.Claims;
using static GSCrm.Utils.AppUtils;

namespace GSCrm.Helpers
{
    public static class AppHelpers
    {
        /// <summary>
        /// Метод осуществляет подготовку приложения
        /// </summary>
        public static void Prepare(this IApplicationBuilder app) => InitializeLocations();

        /// <summary>
        /// Метод возвращает текущего пользователя из HTTP контекста
        /// </summary>
        /// <param name="httpContext">HTTP контекст</param>
        /// <param name="context">Контекст приложения</param>
        /// <returns></returns>
        public static User GetCurrentUser(this HttpContext? httpContext, ApplicationDbContext context)
        {
            if (httpContext == null) return null;
            if (httpContext.User.Identity.IsAuthenticated)
                return context.Users.FirstOrDefault(n => n.UserName == httpContext.User.Identity.Name);
            return null;
        }

        /// <summary>
        /// Возвращает модель пользователя
        /// </summary>
        /// <param name="User"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static User GetUserModel(this ClaimsPrincipal User, ApplicationDbContext context)
            => context.Users.FirstOrDefault(n => n.UserName == User.Identity.Name);
    }
}
