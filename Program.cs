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
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

// JWT config
builder.Services.Configure<JwtConfig>(builder.Configuration.GetSection("JwtSettings"));

// Banco Oracle
var conn = builder.Configuration.GetConnectionString("DefaultConnection")
           ?? throw new InvalidOperationException("Connection string 'DefaultConnection' não encontrada.");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseOracle(conn, b => b.MigrationsAssembly("TeamHeartFiap")));

// Repositórios e serviços
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IRecrutamentoServico, ServicoRecrutamento>();
builder.Services.AddScoped<IServicoRelatorio, ServicoRelatorio>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// JWT
var jwtSection = builder.Configuration.GetSection("JwtSettings");
var secret = jwtSection["Secret"];

if (string.IsNullOrWhiteSpace(secret))
    throw new InvalidOperationException("JwtSettings:Secret não foi definido no appsettings.json");

var key = Encoding.UTF8.GetBytes(secret);

// Registro correto do JWT
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

// Cultura para aceitar dd/MM/yyyy nos endpoints
var cultureInfo = new CultureInfo("pt-BR");
cultureInfo.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
cultureInfo.DateTimeFormat.DateSeparator = "/";
CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();

var app = builder.Build();

// Middleware global de exceções
app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication(); 
app.UseAuthorization();

app.MapControllers();

app.Run();
