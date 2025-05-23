using Microsoft.AspNetCore.Builder;

namespace THMang1.Server.Services
{
    public static class BackendServer
    {
        public static void Start()
        {
            var builder = WebApplication.CreateBuilder();


            builder.Services.AddControllers(); 

            builder.Services.AddSingleton<IGameService, GameService>();

            builder.Services.AddSignalR(options =>
            {
                options.EnableDetailedErrors = true; 
            });

            builder.Services.AddLogging();


            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/error");
                app.UseHsts();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllers();

            app.MapHub<GameHub>("/gameHub");

            app.RunAsync();
        }
    }
}
