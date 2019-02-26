FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY AzureWebAppCompose/AzureWebAppCompose.csproj AzureWebAppCompose/
RUN dotnet restore AzureWebAppCompose/AzureWebAppCompose.csproj
COPY . .
WORKDIR /src/AzureWebAppCompose
RUN dotnet build AzureWebAppCompose.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish AzureWebAppCompose.csproj -c Release -o /app

FROM base AS final

ARG BUILD_NUMBER=
ENV BUILD_NUMBER=$BUILD_NUMBER

WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "AzureWebAppCompose.dll"]
