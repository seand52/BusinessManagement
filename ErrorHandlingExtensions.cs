using Microsoft.AspNetCore.Diagnostics;
using System.Diagnostics;
    
namespace BusinessManagementApi.Extensions
{
    
    public static class ErrorHandlingExtensions
    {
        public static void ConfigureExtensionHandler(this IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                await Results.Problem(
                    title: "Unexpected error handling the request",
                    statusCode: StatusCodes.Status500InternalServerError,
                    extensions: new Dictionary<string, object?>
                    {
                        { "traceId", Activity.Current?.Id }
                    }
                ).ExecuteAsync(context);
            });
        }
    }
}
