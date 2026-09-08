FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ArgosApi/ArgosApi.csproj ArgosApi/
RUN dotnet restore ArgosApi/ArgosApi.csproj

COPY ArgosApi/ ArgosApi/
RUN dotnet publish ArgosApi/ArgosApi.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
USER root

RUN apt-get update \
    && apt-get install -y --no-install-recommends curl ca-certificates gnupg \
    && mkdir -p /etc/apt/keyrings \
    && curl -fsSL https://deb.nodesource.com/gpgkey/nodesource-repo.gpg.key | gpg --dearmor -o /etc/apt/keyrings/nodesource.gpg \
    && echo "deb [signed-by=/etc/apt/keyrings/nodesource.gpg] https://deb.nodesource.com/node_22.x nodistro main" > /etc/apt/sources.list.d/nodesource.list \
    && apt-get update \
    && apt-get install -y --no-install-recommends nodejs \
    && rm -rf /var/lib/apt/lists/*

WORKDIR /app
COPY --from=build /app/publish .

ARG ARGOS_AVALIADOR_VERSION=1.2.0
ENV PLAYWRIGHT_BROWSERS_PATH=/ms-playwright

WORKDIR /app/avaliador
RUN npm init -y \
    && npm install argos-avaliador-acessibilidade@${ARGOS_AVALIADOR_VERSION}

WORKDIR /app/avaliador/node_modules/argos-avaliador-acessibilidade
RUN npx playwright install --with-deps chromium

WORKDIR /app
RUN mkdir -p /ms-playwright /home/app \
    && chown -R app:app /app /ms-playwright /home/app

ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:8080
ENV HOME=/home/app

USER $APP_UID
EXPOSE 8080

ENTRYPOINT ["dotnet", "ArgosApi.dll"]
