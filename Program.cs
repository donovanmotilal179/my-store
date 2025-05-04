using MyStore.Interfaces;
using MyStore.Implementation;
using Microsoft.Extensions.Configuration;
using RedisCaching.CachingServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddScoped<IGetAllClientSqlData, GetAllClientSqlData>();
builder.Services.AddScoped<IGetAllAnimalSqlData, GetAllAnimalSqlData>();
builder.Services.AddScoped<ICacheService, CacheService>();
builder.Services.AddScoped<IUpdateClientData, UpdateClientData>();
builder.Services.AddScoped<IUpdateAnimalData, UpdateAnimalData>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();