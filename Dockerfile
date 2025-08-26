FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app
COPY ApiProvaOnline/ApiProvaOnline.csproj ./ApiProvaOnline/
RUN dotnet restore ApiProvaOnline/ApiProvaOnline.csproj
COPY . /app/
RUN dotnet publish "ApiProvaOnline/ApiProvaOnline.csproj" -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "ApiProvaOnline.dll"]
