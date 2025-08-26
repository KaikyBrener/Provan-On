# Usa a imagem oficial do .NET 8.0 SDK para o build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Define o diretório de trabalho no container
WORKDIR /app

# Copia os arquivos de projeto (o mais aninhado)
COPY ApiProvaOnline/ApiProvaOnline/ApiProvaOnline.csproj ApiProvaOnline/ApiProvaOnline/
COPY ApiProvaOnline/ApiProvaOnline.sln ApiProvaOnline/

# Restaura as dependências
RUN dotnet restore ApiProvaOnline/ApiProvaOnline.sln

# Copia todo o código-fonte restante
COPY . .

# Publica a aplicação
RUN dotnet publish "ApiProvaOnline/ApiProvaOnline.csproj" -c Release -o out

# Usa uma imagem leve para rodar a aplicação
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .

# Comando para iniciar a aplicação
ENTRYPOINT ["dotnet", "ApiProvaOnline.dll"]
