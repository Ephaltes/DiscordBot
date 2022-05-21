FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "DiscordBot.ConsoleApp/DiscordBot.ConsoleApp.csproj"

WORKDIR "/src/DiscordBot.ConsoleApp"
RUN dotnet build "DiscordBot.ConsoleApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DiscordBot.ConsoleApp.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENV DOTNET_RUNNING_IN_CONTAINER=true
ENV TZ=Europe/Berlin
ENV LANG de_AT.UTF-8
ENV LANGUAGE ${LANG}
ENV LC_ALL ${LANG}

ENTRYPOINT ["dotnet", "DiscordBot.ConsoleApp.dll"]
