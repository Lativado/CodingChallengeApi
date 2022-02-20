FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 5196

ENV ASPNETCORE_URLS=http://+:5196

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["CodingChallengeApi.csproj", "./"]
RUN dotnet restore "CodingChallengeApi.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "CodingChallengeApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CodingChallengeApi.csproj" -c Release -o /app/publish /p:UseAppHost=true

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CodingChallengeApi.dll"]
