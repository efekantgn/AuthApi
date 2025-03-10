# 1. Çalışma zamanı için .NET 9 ASP.NET Core imajını kullan
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app

# 2. Gerekli portları aç
EXPOSE 5000

# 3. SQLite veritabanı dosyasının saklanacağı klasörü oluştur
RUN mkdir -p /app/db
VOLUME /app/db

# 4. Build işlemleri için .NET SDK 9 kullan
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# 5. Projeyi kopyala ve bağımlılıkları yükle
COPY ./AuthAPI.csproj ./
RUN dotnet restore "AuthAPI.csproj"

# 6. Tüm kaynak kodunu kopyala ve projeyi yayınla
COPY . .
RUN dotnet publish "AuthAPI.csproj" -c Release -o /app/publish

# 7. Çalıştırma aşaması için en hafif imaja geç
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .

# 8. Ortam değişkenlerini tanımla
ENV ASPNETCORE_URLS=http://+:5000
ENV ASPNETCORE_ENVIRONMENT=Production

# 9. Çalıştırılacak API dosyasını belirle
ENTRYPOINT ["dotnet", "AuthAPI.dll"]
