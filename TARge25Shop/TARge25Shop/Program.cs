using TARge25Shop.ApplicationServices.Services;
using TARge25Shop.Core;
using TARge25Shop.Data;
using Microsoft.EntityFrameworkCore;
using TARge25Shop.ApplicationServices;
using TARge25Shop.Core.ServiceInterface;

namespace TARge25Shop
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // MVC pattern: https://learn.microsoft.com/en-us/aspnet/core/mvc/overview?view=aspnetcore-10.0
            
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            
            builder.Services.AddScoped<ISpaceshipServiceInterface, SpaceshipServices>();
            builder.Services.AddScoped<IKindergartenServiceInterface, KindergartenServices>();
            builder.Services.AddScoped<IFileServices, FileServices>();
            builder.Services.AddScoped<IRealEstateServices, RealEstateServices>();
            builder.Services.AddScoped<IChuckNorrisServices, ChuckNorrisServices>();
            builder.Services.AddScoped<ICocktailServices, CocktailServices>();
            
            builder.Services.AddDbContext<TARge25ShopContext>(options =>
                options.UseSqlite(
                    builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddHttpClient("ChuckNorrisJokesApi", client =>
            {
                client.BaseAddress = new Uri("https://api.chucknorris.io/");
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            });

            builder.Services.AddHttpClient("CocktailAPI", client =>
            {
                client.BaseAddress = new Uri("https://www.thecocktaildb.com/api/json/v1/1/");
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            });
            
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();
            
            // app.MapGet("/norris-joke", async (ChuckNorrisServices service) =>
            // {
            //     var joke = await service.ChuckNorrisResultHttpClient();
            //     return Results.Ok(joke);
            // });

            app.Run();
        }
    }
}
