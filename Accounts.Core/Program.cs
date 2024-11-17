using Accounts.Core.DbContext;
using Accounts.Core.Repositories;
using BaseClassLibrary;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddBaseLibraryServices();

builder.Services.AddScoped<ICustomerMasterRepository, CustomerMasterRepository>();
builder.Services.AddScoped<IStockRepository, StockRepository>();
builder.Services.AddScoped<ISalesMasterRepository, SalesMasterRepository>();
builder.Services.AddScoped<ISalesDetailsRepository, SalesDetailsRepository>();

builder.Services.AddDbContext<AppDbContext>(options => 
    //options.UseSqlServer(@"Data Source=103.83.81.7;Initial Catalog=karmajew_SSD;Persist Security Info=True;User ID=karmajew_SSD;Password=Mle^B3n!F1sh$"));
    options.UseSqlServer(@"Data Source=103.83.81.7;Initial Catalog=karmajew_SSD;Persist Security Info=True;User ID=karmajew_SSD;Password=Mle^B3n!F1sh$"));

var app = builder.Build();

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
