using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using AutoMapper;
using TeamHeartFiap.Infrastructure.Data;
using TeamHeartFiap.Infrastructure;
using TeamHeartFiap.Repositories;
using TeamHeartFiap.Services;
using Oracle.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

builder.Services.Configure<JwtConfig>(builder.Configuration.GetSection("JwtSettings"));

var conn = builder.Configuration.GetConnectionString("DefaultConnection")
           ?? throw new InvalidOperationException("Connection string 'DefaultConnection' não encontrada.");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseOracle(conn, b => b.MigrationsAssembly("TeamHeartFiap")));

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IRecrutamentoServico, ServicoRecrutamento>();
builder.Services.AddScoped<IServicoRelatorio, ServicoRelatorio>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// JWT
var jwtSection = builder.Configuration.GetSection("JwtSettings");
var secret = jwtSection["Secret"];
var key = Encoding.UTF8.GetBytes(secret);

// REGISTRO CORRETO DO JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidIssuer = jwtSection["Issuer"],
            ValidAudience = jwtSection["Audience"],
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };
    });

// Roles
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdmin", policy => policy.RequireRole("Admin"));
});

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication(); // obrigatório antes do UseAuthorization
app.UseAuthorization();

app.MapControllers();

app.Run();
