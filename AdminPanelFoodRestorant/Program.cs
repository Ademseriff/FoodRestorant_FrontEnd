using AdminPanelFoodRestorant.Consumers;
using MassTransit;
using Shared;

namespace AdminPanelFoodRestorant
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
         
            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddMassTransit(configurator =>
            {
                configurator.AddConsumer<OrderViewResponseEventConsumer>();
                configurator.UsingRabbitMq((contex, _configure) =>
                {
                    _configure.Host(builder.Configuration["RabbitMq"]);
                    _configure.ReceiveEndpoint(RabbitMQSettings.Admin_ViewResponseEventQueue, e => e.ConfigureConsumer<OrderViewResponseEventConsumer>(contex));
                });
            });
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
