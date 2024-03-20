using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NileCapitalCruisesBEAPI;
using NileCapitalCruisesBEAPI.Helpers;
using NileCapitalCruisesBEAPI.Models;
using NileCapitalCruisesBEAPI.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpClient();
var ConnectionString = builder.Configuration.GetConnectionString("AppDbConnectionBE");
builder.Services.AddDbContext<NileCapitalCruisesBedbContext>(options => options.UseSqlServer(ConnectionString));

var ConnectionStringOp = builder.Configuration.GetConnectionString("AppDbConnectionBEOP");
builder.Services.AddDbContext<NileCapitalCruisesBeopdbContext>(options => options.UseSqlServer(ConnectionStringOp));



var ConnectionStringUsers = builder.Configuration.GetConnectionString("AppDbConnectionUsersOP");
builder.Services.AddDbContext<NileCapitalCruisesUsersdbContext>(options => options.UseSqlServer(ConnectionStringUsers));
builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<NileCapitalCruisesUsersdbContext>();

builder.Services.Configure<JWT>(builder.Configuration.GetSection("JWT"));

builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.AddTransient<IAuthService, AuthService>();

builder.Services.AddCors(options => {
    options.AddPolicy("AllowAnyOrigin", builder => builder
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());
});


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
                .AddJwtBearer(o =>
                {
                    o.RequireHttpsMetadata = false;
                    o.SaveToken = false;
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidIssuer = builder.Configuration["JWT:Issuer"],
                        ValidAudience = builder.Configuration["JWT:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"])),
                        ClockSkew = TimeSpan.Zero
                    };
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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseCors("AllowAnyOrigin");

app.Run();
