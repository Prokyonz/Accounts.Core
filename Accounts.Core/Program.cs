using Accounts.Core.DbContext;
using Accounts.Core.Repositories;
using BaseClassLibrary;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "DiamondTrade.API", Version = "v1" });
});

builder.Services.AddBaseLibraryServices();

builder.Services.AddScoped<ICustomerMasterRepository, CustomerMasterRepository>();
builder.Services.AddScoped<IStockRepository, StockRepository>();
builder.Services.AddScoped<ISalesMasterRepository, SalesMasterRepository>();
builder.Services.AddScoped<ISalesDetailsRepository, SalesDetailsRepository>();
builder.Services.AddScoped<IUserMasterRepository, UserMasterRepository>();
builder.Services.AddScoped<IItemMasterRepository, ItemMasterRepository>();
builder.Services.AddScoped<IPurchaseMasterRepository, PurchaseMasterRepository>();

builder.Services.AddDbContext<AppDbContext>(options => 
    //options.UseSqlServer(@"Data Source=103.83.81.7;Initial Catalog=karmajew_SSD;Persist Security Info=True;User ID=karmajew_SSD;Password=Mle^B3n!F1sh$"));
    options.UseSqlServer(@"Data Source=103.83.81.7;Initial Catalog=karmajew_SSD;Persist Security Info=True;User ID=karmajew_SSD;Password=Mle^B3n!F1sh$"));

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.IgnoreNullValues = true;
    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
    options.JsonSerializerOptions.MaxDepth = 10485760; // add your desired limit here
}); ;

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.AllowAnyHeader()
                   .AllowAnyOrigin()
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});

var app = builder.Build();
app.UseCors("AllowAllOrigins");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("../swagger/v1/swagger.json", "DiamondTrade.API v1"));

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
