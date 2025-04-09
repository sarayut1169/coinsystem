using Microsoft.EntityFrameworkCore;
using coinsystem;
using coinsystem.Services;
using MySql.EntityFrameworkCore.Extensions;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddOpenApi();


builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySQL(builder.Configuration.GetConnectionString("DefaultConnection"))); 

builder.Services.AddControllers();


builder.Services.AddScoped<IMoneyService, MoneyService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IMemberService, MemberService>();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers(); 


app.Run();