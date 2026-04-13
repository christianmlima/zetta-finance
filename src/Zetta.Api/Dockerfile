FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

COPY global.json ./
COPY Zetta.Api/Zetta.Api.csproj Zetta.Api/
COPY Zetta.Application/Zetta.Application.csproj Zetta.Application/
COPY Zetta.Domain/Zetta.Domain.csproj Zetta.Domain/
COPY Zetta.Infrastructure/Zetta.Infrastructure.csproj Zetta.Infrastructure/
COPY Zetta.SharedKernel/Zetta.SharedKernel.csproj Zetta.SharedKernel/

RUN dotnet restore Zetta.Api/Zetta.Api.csproj

COPY . .

RUN dotnet publish Zetta.Api/Zetta.Api.csproj -c Release -o /out --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

COPY --from=build /out .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "Zetta.Api.dll"]
