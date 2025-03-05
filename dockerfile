# 1️⃣ Build aşaması
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# 2️⃣ Proje dosyalarını kopyala ve restore et
COPY ["AuthAPI.csproj", "./"]
RUN dotnet restore

# 3️⃣ Tüm kaynak kodlarını kopyala ve publish et
COPY . .
RUN dotnet publish -c Release -o /app/publish

# 4️⃣ Runtime aşaması
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "AuthAPI.dll"]
