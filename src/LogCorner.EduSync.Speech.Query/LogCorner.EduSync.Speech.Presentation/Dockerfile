#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["LogCorner.EduSync.Speech.Query/LogCorner.EduSync.Speech.Presentation/LogCorner.EduSync.Speech.Presentation.csproj", "LogCorner.EduSync.Speech.Query/LogCorner.EduSync.Speech.Presentation/"]
COPY ["LogCorner.EduSync.Speech.Query/LogCorner.EduSync.Speech.ReadModel/LogCorner.EduSync.Speech.ReadModel.csproj", "LogCorner.EduSync.Speech.Query/LogCorner.EduSync.Speech.ReadModel/"]
COPY ["LogCorner.EduSync.Speech.Query/LogCorner.EduSync.Speech.Application/LogCorner.EduSync.Speech.Application.csproj", "LogCorner.EduSync.Speech.Query/LogCorner.EduSync.Speech.Application/"]
COPY ["LogCorner.EduSync.Speech.Query/LogCorner.EduSync.Speech.Infrastructure/LogCorner.EduSync.Speech.Infrastructure.csproj", "LogCorner.EduSync.Speech.Query/LogCorner.EduSync.Speech.Infrastructure/"]
RUN dotnet restore "LogCorner.EduSync.Speech.Query/LogCorner.EduSync.Speech.Presentation/LogCorner.EduSync.Speech.Presentation.csproj"
COPY . .
WORKDIR "/src/LogCorner.EduSync.Speech.Query/LogCorner.EduSync.Speech.Presentation"
RUN dotnet build "LogCorner.EduSync.Speech.Presentation.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LogCorner.EduSync.Speech.Presentation.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LogCorner.EduSync.Speech.Presentation.dll"]