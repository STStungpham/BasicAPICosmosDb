using BasicAPICosmosDb.Models;
using BasicAPICosmosDb.Services;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

ConfigureServices(builder.Services);

var app = builder.Build();
InitConfigurations(app.Configuration);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


void ConfigureServices(IServiceCollection services)
{
    #region scoped
    services.AddScoped<IDeploymentServices, DeploymentsServices>();
    #endregion
    #region singleton
    services.AddSingleton(typeof(ILogger), typeof(Logger<ICosmosDbServices>));
    services.AddSingleton<ICosmosDbServices, CosmosDbServices>();
    #endregion
}


void InitConfigurations(IConfiguration configuration)
{
    AppSettings.ConnectionString = configuration.GetSection(nameof(AppSettings.ConnectionString)).Get<string>();
    AppSettings.Environment = configuration.GetSection(nameof(AppSettings.Environment)).Get<string>();
    AppSettings.DatabaseName = configuration.GetSection(nameof(AppSettings.DatabaseName)).Get<string>();
}