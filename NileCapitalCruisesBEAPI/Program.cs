using Microsoft.EntityFrameworkCore;
using NileCapitalCruisesBEAPI.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpClient();
var ConnectionString = builder.Configuration.GetConnectionString("AppDbConnectionBE");
builder.Services.AddDbContext<NileCapitalCruisesBedbContext>(options => options.UseSqlServer(ConnectionString));

var ConnectionStringOp = builder.Configuration.GetConnectionString("AppDbConnectionBEOP");
builder.Services.AddDbContext<NileCapitalCruisesBeopdbContext>(options => options.UseSqlServer(ConnectionStringOp));




builder.Services.AddCors(options => {
    options.AddPolicy("AllowAnyOrigin", builder => builder
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());
});
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseCors("AllowAnyOrigin");

app.Run();
