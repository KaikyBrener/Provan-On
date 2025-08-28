var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// 1. Configurar CORS com múltiplas origens
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        policy =>
        {
            policy.WithOrigins(
                "http://127.0.0.1:5500", // Para seu ambiente de desenvolvimento
                "https://provan-on.vercel.app" // Para o seu frontend em produção
            )
            .AllowAnyHeader()
            .AllowAnyMethod();
            // .AllowCredentials(); // Se não estiver usando cookies ou credenciais, pode ser removido
        });
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
// Move as linhas do Swagger para fora do bloco if
// para que elas funcionem tanto em desenvolvimento quanto em produção.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

// 2. Ativar CORS com a política correta
app.UseCors("CorsPolicy");

app.UseAuthorization();

app.MapControllers();

app.Run();
