using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ArgosApi.Infrastructure.Swagger
{
    public class AuthorizeOperationFilter : IOperationFilter
    { 
        public void Apply(
            OpenApiOperation operation,
            OperationFilterContext context)
        {
            var hasAllowAnonymous =
                context.MethodInfo
                    .GetCustomAttributes(true)
                    .OfType<AllowAnonymousAttribute>()
                    .Any()
                ||
                context.MethodInfo.DeclaringType?
                    .GetCustomAttributes(true)
                    .OfType<AllowAnonymousAttribute>()
                    .Any() == true;


            if (hasAllowAnonymous)
                return;


            var hasAuthorize =
                context.MethodInfo
                    .GetCustomAttributes(true)
                    .OfType<AuthorizeAttribute>()
                    .Any()
                ||
                context.MethodInfo.DeclaringType?
                    .GetCustomAttributes(true)
                    .OfType<AuthorizeAttribute>()
                    .Any() == true;


            if (!hasAuthorize)
                return;

            //operation.Security =
            //[
            //    new OpenApiSecurityRequirement
            //    {
            //        [
            //            new OpenApiSecuritySchemeReference(
            //                JwtBearerDefaults.AuthenticationScheme)
            //        ] = []
            //    }
            //];
        }
    }
}
