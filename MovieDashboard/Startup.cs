using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MovieDashboard.Data;
using MovieDashboard.Services;
using Microsoft.EntityFrameworkCore;
using MovieDashboard.Hubs;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

        services.AddControllersWithViews();
        services.AddSignalR();
        services.AddScoped<CsvService>();
        services.AddScoped<MoviePredictionService>();
        services.AddScoped<CsvExportService>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceScopeFactory scopeFactory)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            endpoints.MapControllerRoute(
                name: "test",
                pattern: "test",
                defaults: new { controller = "Test", action = "Index" });
            endpoints.MapHub<DashboardHub>("/dashboardHub");
            endpoints.MapControllers(); // Para API endpoints
        });

        // Usa un ámbito para inicializar CsvService
        using (var scope = scopeFactory.CreateScope())
        {
            var csvService = scope.ServiceProvider.GetRequiredService<CsvService>();
            var filePath = Path.Combine(env.ContentRootPath, "C:/Users/clobo/Documents/WorkSpace/cubo/MovieDashboard/imdb_top_250 (1).csv"); // Ruta absoluta al archivo CSV
            csvService.LoadMoviesFromCsv(filePath);
        }
    }
}
