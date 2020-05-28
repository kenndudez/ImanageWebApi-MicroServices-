FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["Auth.Web/Auth.Web.csproj", "Auth.Web/"]
RUN dotnet restore "Auth.Web/Auth.Web.csproj"
COPY . .
WORKDIR "/src/Auth.Web"
RUN dotnet build "Auth.Web.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Auth.Web.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Auth.Web.dll"]