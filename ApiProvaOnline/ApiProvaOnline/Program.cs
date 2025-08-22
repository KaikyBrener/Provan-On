var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// 1. Configurar CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirLocalhost",
        policy =>
        {
            policy.WithOrigins("http://127.0.0.1:5500")
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();  // Permite envio de credenciais (cookies, etc)

            // Para liberar todas as origens (apenas para teste, não recomendado em produção):
            // policy.SetIsOriginAllowed(_ => true)
            //       .AllowAnyHeader()
            //       .AllowAnyMethod()
            //       .AllowCredentials();
        });
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// 2. Ativar CORS antes da autorização
app.UseCors("PermitirLocalhost");

app.UseAuthorization();

app.MapControllers();

app.Run();
