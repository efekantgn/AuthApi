# Kimlik Doğrulama ve Yetkilendirme API'si - Docker Destekli

Bu proje, ASP.NET Core kullanarak JWT tabanlı kimlik doğrulama ve yetkilendirme sistemi geliştirmektedir. Docker desteği eklenmiştir, böylece uygulama Docker konteyneri içinde çalıştırılabilir.

## Özellikler

- **Kullanıcı Kayıt ve Giriş Sistemi:** Kullanıcılar kayıt olabilir, giriş yapabilir.
- **JWT Token ile Kimlik Doğrulama:** Kullanıcıların API'lere güvenli bir şekilde erişebilmesi için JWT token kullanılmaktadır.
- **Roller:** Kullanıcılar farklı rollerle eşleştirilir (Admin, User vb.).
- **Yetkilendirme:** Admin rolüne sahip kullanıcılar, yalnızca admin yetkili endpoint'lere erişebilir.
- **Swagger Entegrasyonu:** API dokümantasyonu için Swagger entegrasyonu yapılmıştır.
- **Docker Desteği:** Proje Docker konteyneri içinde çalıştırılabilir.

## Kullanılan Teknolojiler

- ASP.NET Core
- SQLite
- JWT (JSON Web Token)
- Identity
- Swagger
- Docker

## Kurulum

1. **Projeyi Klonlayın:**

   ```bash
   git clone https://github.com/yourusername/your-repository.git
   ```

2. **Docker Konteynerini Çalıştırın:**

   Projenin kök dizininde `docker-compose.yml` dosyasını kullanarak Docker konteynerini başlatın:

   ```bash
   docker-compose up --build
   ```

   Bu komut, uygulamayı Docker konteyneri içinde çalıştıracaktır.

3. **Veritabanı Migrasyonlarını Uygulayın:**

   Docker konteyneri çalışırken, veritabanı migrasyonlarını uygulamak için aşağıdaki komutu kullanın:

   ```bash
   docker exec -it <container_name> dotnet ef database update
   ```

4. **Projeyi Çalıştırın:**

   Docker konteyneri başlatıldığında, API'niz `http://localhost:8080` adresinde çalışacaktır.

5. **Swagger UI'ye Erişim:**

   API dökümantasyonu ve test için Swagger UI'ye `http://localhost:8080/swagger` üzerinden erişebilirsiniz.

## Kimlik Doğrulama ve Yetkilendirme

- Kullanıcı kaydı ve giriş işlemleri için `/api/auth/register` ve `/api/auth/login` endpoint'leri kullanılmaktadır.
- JWT token ile kimlik doğrulama için Authorization başlığına `Bearer <token>` eklemeniz gerekmektedir.
- Admin yetkili endpoint'lerine erişim için admin rolüne sahip olmanız gerekmektedir.

## Katkı

Eğer projeye katkıda bulunmak isterseniz, öneri ve düzeltmeleri pull request olarak gönderebilirsiniz.

## Lisans

Bu proje MIT Lisansı ile lisanslanmıştır.

# Kimlik Doğrulama ve Yetkilendirme API'si

Bu proje, ASP.NET Core kullanarak JWT tabanlı kimlik doğrulama ve yetkilendirme sistemi geliştirmektedir. Proje, SQLite veritabanı kullanarak kullanıcı kayıt ve giriş işlemlerini, roller (Admin, User) ile yetkilendirmeyi ve Swagger entegrasyonunu sağlamaktadır.

## Özellikler

- **Kullanıcı Kayıt ve Giriş Sistemi:** Kullanıcılar kayıt olabilir, giriş yapabilir.
- **JWT Token ile Kimlik Doğrulama:** Kullanıcıların API'lere güvenli bir şekilde erişebilmesi için JWT token kullanılmaktadır.
- **Roller:** Kullanıcılar farklı rollerle eşleştirilir (Admin, User vb.).
- **Yetkilendirme:** Admin rolüne sahip kullanıcılar, yalnızca admin yetkili endpoint'lere erişebilir.
- **Swagger Entegrasyonu:** API dokümantasyonu için Swagger entegrasyonu yapılmıştır.

## Kullanılan Teknolojiler

- ASP.NET Core
- SQLite
- JWT (JSON Web Token)
- Identity
- Swagger

## Kurulum

1. **Projeyi Klonlayın:**

   ```bash
   git clone https://github.com/yourusername/your-repository.git
   ```

2. **Bağımlılıkları Yükleyin:**

   ```bash
   cd your-repository
   dotnet restore
   ```

3. **Veritabanı Migrasyonlarını Uygulayın:**

   ```bash
   dotnet ef database update
   ```

4. **Projeyi Çalıştırın:**

   ```bash
   dotnet run
   ```

   API'niz `http://localhost:5296` adresinde çalışacaktır.

5. **Swagger UI'ye Erişim:**

   API dökümantasyonu ve test için Swagger UI'ye `http://localhost:5296/swagger` üzerinden erişebilirsiniz.

## Kimlik Doğrulama ve Yetkilendirme

- Kullanıcı kaydı ve giriş işlemleri için `/api/auth/register` ve `/api/auth/login` endpoint'leri kullanılmaktadır.
- JWT token ile kimlik doğrulama için Authorization başlığına `Bearer <token>` eklemeniz gerekmektedir.
- Admin yetkili endpoint'lerine erişim için admin rolüne sahip olmanız gerekmektedir.

## Katkı

Eğer projeye katkıda bulunmak isterseniz, öneri ve düzeltmeleri pull request olarak gönderebilirsiniz.

## Lisans

Bu proje MIT Lisansı ile lisanslanmıştır.
