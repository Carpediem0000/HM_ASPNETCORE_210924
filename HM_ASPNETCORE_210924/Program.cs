using System.Diagnostics;
using System.Text.Json;

namespace HM_ASPNETCORE_210924
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var app = builder.Build();

            app.Use(async (context, next) =>
            {
                var stopwatch = Stopwatch.StartNew();
                await next.Invoke();
                stopwatch.Stop();
                Console.WriteLine($"{stopwatch.ElapsedMilliseconds} ms");
            });

            app.MapGet("/", () => "Hello World!");

            app.MapGet("/about", () => { 
                Thread.Sleep(1000); // Для проверки логирования
                return "Это приложение написано при использовании ASP.NET Core";
            });

            app.MapPost("/echo", async (HttpContext context) =>
            {
                var reqBody = await JsonSerializer.DeserializeAsync<object>(context.Request.Body);
                return Results.Json(reqBody);
            });

            app.Run();
        }
    }
}
