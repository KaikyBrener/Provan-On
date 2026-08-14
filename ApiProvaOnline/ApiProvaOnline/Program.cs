using ApiProvaOnline.Application.Interfaces;
using ApiProvaOnline.Application.Services;
using ApiProvaOnline.Infrastructure.Data;
using ApiProvaOnline.Infrastructure.Repositories;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using ApiProvaOnline.Infrastructure.Middleware;
using Serilog;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

try
{
    Log.Information("Iniciando a API...");
    var builder = WebApplication.CreateBuilder(args);
    builder.Host.UseSerilog();

// Add services to the container.

// 1. Configurar CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirLocalhost",
        policy =>
        {
            policy.SetIsOriginAllowed(origin =>
            {
                var uri = new Uri(origin);
                return uri.Host == "localhost" || uri.Host == "127.0.0.1";
            })
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        });
});

// 2. Configurar Entity Framework Core com MySQL
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ProvaOnDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.Parse("8.0.21-mysql")));

// 3. Registrar Repositories e Unit of Work
builder.Services.AddScoped<IProvaRepository, ProvaRepository>();
builder.Services.AddScoped<IAlunoRepository, AlunoRepository>();
builder.Services.AddScoped<IProfessorRepository, ProfessorRepository>();
builder.Services.AddScoped<IAdministrativoRepository, AdministrativoRepository>();
builder.Services.AddScoped<IPerguntaObjetivaRepository, PerguntaObjetivaRepository>();
builder.Services.AddScoped<IPerguntaRepository, PerguntaRepository>();
builder.Services.AddScoped<IAlunoRespostaObjetivaRepository, AlunoRespostaObjetivaRepository>();
builder.Services.AddScoped<IAlunoRespostaRepository, AlunoRespostaRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// 4. Registrar Services
builder.Services.AddScoped<ProvaService>();
builder.Services.AddScoped<AlunoService>();
builder.Services.AddScoped<ProfessorService>();
builder.Services.AddScoped<AdministrativoService>();
builder.Services.AddScoped<PerguntaObjetivaService>();
builder.Services.AddScoped<PerguntaService>();
builder.Services.AddScoped<AlunoRespostaObjetivaService>();
builder.Services.AddScoped<AlunoRespostaService>();
builder.Services.AddScoped<ICorrecaoService, CorrecaoService>();

// 5. Registrar Segurança e JWT
var jwtKey = builder.Configuration["Jwt:Key"] ?? throw new ArgumentNullException("Jwt:Key is missing");
var key = System.Text.Encoding.ASCII.GetBytes(jwtKey);
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidateLifetime = true
    };
});
builder.Services.AddScoped<ApiProvaOnline.Infrastructure.Security.TokenService>();

// 6. Registrar FluentValidation
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

// 7. Registrar AutoMapper (Resolução explícita para evitar CS1503)
builder.Services.AddAutoMapper(config => { }, typeof(Program).Assembly);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Prova-On API",
        Version = "v1",
        Description = "API para Sistema de Provas Online - Prova-On",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Prova-On Team"
        }
    });
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "JWT Authorization header usando o esquema Bearer. Exemplo: 'Bearer {token}'",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection(); // Desabilitado para desenvolvimento local com frontend em HTTP

// 6. Ativar CORS antes da autorização
app.UseCors("PermitirLocalhost");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Falha crítica ao iniciar a API.");
}
finally
{
    Log.CloseAndFlush();
}


