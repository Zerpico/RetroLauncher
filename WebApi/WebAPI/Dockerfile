#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for fas$FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build

WORKDIR /src
COPY ["WebApi/WebAPI/WebAPI.csproj", "WebApi/WebAPI/"]
COPY ["WebApi/Application/Application.csproj", "WebApi/Application/"]
COPY ["WebApi/Domain/Domain.csproj", "WebApi/Domain/"]
COPY ["WebApi/Persistence/Persistence.csproj", "WebApi/Persistence/"]

RUN dotnet restore "WebApi/WebAPI/WebAPI.csproj"
COPY . .

#install nodejs
RUN apt-get update -yq
RUN apt-get install curl gnupg -yq
RUN curl -sL https://deb.nodesource.com/setup_16.x | bash -
RUN apt-get install -y nodejs

# set node to production
# ENV NODE_ENV production

# publish
FROM build AS publish
RUN dotnet publish "WebApi/WebAPI/WebAPI.csproj" -c Release -o /app/publish -v m

FROM base AS runtime
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RetroLauncher.WebAPI.dll"]