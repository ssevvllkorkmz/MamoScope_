# MaMoScope

MaMoScope, motor sürücülerinin test edilmesi, test sonuçlarının kaydedilmesi ve geçmiş test kayıtlarının görüntülenmesi amacıyla geliştirilmiş bir masaüstü uygulamasıdır.

Proje, WPF tabanlı bir istemci uygulaması ile ASP.NET Core Web API ve PostgreSQL veritabanından oluşan katmanlı bir mimariye sahiptir. Uygulama içerisinde gerçekleştirilen motor testleri API aracılığıyla veritabanına kaydedilmekte ve geçmiş test sonuçları tekrar görüntülenebilmektedir.

---

## Projenin Amacı

MaMoScope projesinin temel amacı, motor sürücülerinin test süreçlerinin bilgisayar ortamında gerçekleştirilmesini ve test sonuçlarının düzenli bir şekilde saklanmasını sağlamaktır.

Uygulama ile;

* Motor sürücüsünün seri numarası doğrulanabilir.
* Motor sürücüsüne ait test verileri oluşturulabilir.
* Motor sürücüsünün voltaj değeri kontrol edilebilir.
* Test sonucu başarılı veya başarısız olarak belirlenebilir.
* Test sonuçları veritabanına kaydedilebilir.
* Daha önce gerçekleştirilen testler görüntülenebilir.
* WPF istemcisi ile Web API arasında HTTP/JSON üzerinden iletişim kurulabilir.

---

## Kullanılan Teknolojiler

| Teknoloji             | Kullanım Alanı                             |
| --------------------- | ------------------------------------------ |
| C#                    | Ana programlama dili                       |
| .NET                  | Uygulama altyapısı                         |
| WPF                   | Masaüstü kullanıcı arayüzü                 |
| ASP.NET Core Web API  | REST API                                   |
| Entity Framework Core | Veritabanı işlemleri ve ORM                |
| PostgreSQL            | İlişkisel veritabanı                       |
| CommunityToolkit.Mvvm | WPF MVVM yapısı                            |
| Dependency Injection  | Bağımlılık yönetimi                        |
| HTTP / JSON           | WPF - API iletişimi                        |
| Docker                | API'nin container ortamında çalıştırılması |
| Git / GitHub          | Versiyon kontrolü ve proje yönetimi        |

---

## Sistem Mimarisi

MaMoScope, istemci ile veritabanı arasındaki iletişimi doğrudan gerçekleştirmek yerine ASP.NET Core Web API üzerinden gerçekleştirecek şekilde tasarlanmıştır.

```text
┌──────────────────────────────┐
│       MAMOSCOPE WPF          │
│          CLIENT              │
│                              │
│  Views                       │
│  ViewModels                  │
│  Services                    │
│  Models                      │
└──────────────┬───────────────┘
               │
               │ HTTP / JSON
               ▼
┌──────────────────────────────┐
│    ASP.NET CORE WEB API      │
│                              │
│    MotorTestsController      │
└──────────────┬───────────────┘
               │
               ▼
┌──────────────────────────────┐
│        APPLICATION           │
│                              │
│  MotorDriverService          │
│  DTOs                        │
│  Repository Interfaces       │
└──────────────┬───────────────┘
               │
               ▼
┌──────────────────────────────┐
│       INFRASTRUCTURE         │
│                              │
│  AppDbContext                │
│  MotorDriversRepository      │
│  EF Core Migrations          │
└──────────────┬───────────────┘
               │
               ▼
┌──────────────────────────────┐
│         POSTGRESQL           │
│                              │
│       MotorDrivers           │
│           Table              │
└──────────────────────────────┘
```

### Önemli Mimari Ayrım

API projesinde herhangi bir Service sınıfı veya Service klasörü bulunmamaktadır.

ASP.NET Core API projesi Presentation katmanı olarak görev yapmakta ve temel olarak Controller yapısını içermektedir.

`MotorDriverService` Application katmanında bulunurken, WPF projesindeki `MotorDriversService` farklı bir sorumluluğa sahiptir.

* **WPF `MotorDriversService`**: API ile HTTP üzerinden iletişim kurar.
* **Application `MotorDriverService`**: Uygulamanın iş mantığını yönetir.
* **API `MotorTestsController`**: HTTP isteklerini karşılar ve ilgili Application bileşenlerine yönlendirir.
* **Infrastructure `MotorDriversRepository`**: Veritabanı işlemlerini gerçekleştirir.

---

## Clean Architecture

Proje, sorumlulukların birbirinden ayrılması amacıyla Clean Architecture prensiplerinden yararlanılarak yapılandırılmıştır.

### Domain

Domain katmanı uygulamanın temel varlıklarını içerir.

Örneğin:

```text
MotorDrivers
```

Motor sürücüsüne ait temel verileri temsil eden entity bu katmanda bulunmaktadır.

### Application

Application katmanı uygulamanın iş mantığını içerir.

Bu katmanda;

* `MotorDriverService`
* DTO sınıfları
* Repository interface'leri

bulunmaktadır.

Bu katman, uygulamanın iş kurallarının yönetilmesini ve diğer katmanlarla olan bağımlılıkların soyutlanmasını sağlar.

### Infrastructure

Infrastructure katmanı veri erişimi ile ilgili işlemleri gerçekleştirir.

Bu katmanda;

* `AppDbContext`
* `MotorDriversRepository`
* Entity Framework Core migrations

bulunmaktadır.

Repository yapısı üzerinden PostgreSQL veritabanına erişim sağlanmaktadır.

### Presentation

Presentation katmanı ASP.NET Core Web API projesidir.

Bu katmanda;

```text
MotorTestsController
```

bulunmaktadır.

Controller, istemciden gelen HTTP isteklerini karşılayarak Application katmanındaki işlemlerin yürütülmesini sağlar.

---

## WPF Client

WPF uygulaması MVVM (Model-View-ViewModel) mimarisi kullanılarak geliştirilmiştir.

Temel klasör yapısı:

```text
WPF Client
│
├── Core
├── Factories
├── Models
├── Resources
├── Services
├── ViewModels
└── Views
```

### Views

Kullanıcı arayüzünü oluşturan XAML dosyalarını içerir.

### ViewModels

Arayüz ile uygulama mantığı arasındaki bağlantıyı sağlar.

Projede `CommunityToolkit.Mvvm` kullanılarak MVVM yapısının daha düzenli bir şekilde uygulanması sağlanmıştır.

### Services

WPF tarafındaki servisler;

* API ile haberleşme
* navigasyon
* dialog işlemleri
* uygulama içi veri yönetimi

gibi işlemleri gerçekleştirir.

Özellikle `MotorDriversService`, WPF uygulamasının Web API ile HTTP üzerinden iletişim kurmasını sağlar.

---

## Motor Test Süreci

Uygulamadaki temel işlem motor sürücüsünün test edilmesidir.

Test süreci genel olarak aşağıdaki şekilde ilerler:

```text
Motor sürücüsü seri numarası
          │
          ▼
Seri numarası doğrulama
          │
          ▼
Voltaj değeri alınması
          │
          ▼
Voltaj aralığının kontrol edilmesi
          │
          ▼
Test sonucunun belirlenmesi
          │
          ▼
API'ye HTTP POST isteği
          │
          ▼
Application katmanı
          │
          ▼
Repository
          │
          ▼
PostgreSQL
```

Seri numarası için aşağıdaki format kullanılmaktadır:

```text
OPT-DRV-XXXX
```

Örneğin:

```text
OPT-DRV-1234
```

Geçersiz seri numaraları test işlemine alınmamaktadır.

---

## Voltaj Kontrolü

Motor testinde voltaj değeri belirlenen aralık içerisinde değerlendirilir.

Test sonucunun başarılı kabul edilmesi için voltaj değerinin:

```text
23.5 V ≤ Voltaj ≤ 24.5 V
```

aralığında olması gerekmektedir.

Bu aralık dışındaki değerler başarısız test olarak değerlendirilir.

---

## API

ASP.NET Core Web API, WPF istemcisi ile veritabanı arasında iletişim sağlayan katmandır.

Temel endpointler:

### Tüm test kayıtlarını getirme

```http
GET /api/motortests
```

Bu endpoint veritabanında bulunan motor test kayıtlarını getirir.

### Yeni test kaydı oluşturma

```http
POST /api/motortests
```

Bu endpoint yeni motor test kaydının oluşturulmasını sağlar.

API ile WPF arasındaki veri alışverişinde JSON formatı kullanılmaktadır.

---

## Veritabanı

Projede PostgreSQL ilişkisel veritabanı kullanılmaktadır.

Veritabanı işlemleri Entity Framework Core aracılığıyla gerçekleştirilmektedir.

Temel tablo:

```text
MotorDrivers
```

Tablo içerisinde motor sürücüsünün seri numarası, test sırasında kullanılan voltaj değeri ve test sonucu gibi bilgiler tutulmaktadır.

Entity Framework Core Migration yapısı kullanılarak veritabanı şemasının yönetilmesi sağlanmıştır.

---

## Repository Yapısı

Veritabanı işlemlerinin doğrudan uygulama mantığı içerisinde gerçekleştirilmemesi amacıyla Repository yapısı kullanılmıştır.

`MotorDriversRepository` temel olarak;

* Tüm motor kayıtlarını getirme
* Seri numarasına göre kayıt arama
* Yeni motor test kaydı ekleme

işlemlerini gerçekleştirmektedir.

Veritabanı bağlantısının yönetiminde `AppDbContext` ve `IDbContextFactory` kullanılmaktadır.

---

## Dependency Injection

Projede bağımlılıkların yönetilmesi amacıyla Dependency Injection yaklaşımı kullanılmıştır.

WPF tarafında gerekli servis ve ViewModel bağımlılıkları uygulamanın başlangıcında yapılandırılmaktadır.

Bu yaklaşım sayesinde sınıfların bağımlılıkları doğrudan oluşturmak yerine dışarıdan sağlanmakta ve uygulamanın daha modüler ve test edilebilir olması amaçlanmaktadır.

---

## Veri Akışı

Yeni bir test kaydı oluşturulduğunda veri akışı aşağıdaki şekilde gerçekleşir:

```text
WPF Kullanıcı Arayüzü
        ↓
ViewModel
        ↓
MotorDriversService
        ↓
HTTP POST
        ↓
MotorTestsController
        ↓
MotorDriverService
        ↓
MotorDriversRepository
        ↓
AppDbContext
        ↓
PostgreSQL
```

Geçmiş kayıtların görüntülenmesinde ise veri akışı ters yönde gerçekleşmektedir:

```text
PostgreSQL
    ↓
Repository
    ↓
Application
    ↓
Controller
    ↓
HTTP JSON Response
    ↓
WPF MotorDriversService
    ↓
MotorDriversStore
    ↓
ViewModel
    ↓
View
```

---

## Docker

ASP.NET Core Web API'nin container ortamında çalıştırılabilmesi için Docker yapılandırması hazırlanmıştır.

Projede bulunan `Dockerfile`, API projesinin Docker image haline getirilmesini ve container içerisinde çalıştırılmasını sağlar.

Bu yapı sayesinde uygulamanın farklı ortamlarda daha kolay çalıştırılması ve dağıtılması amaçlanmıştır.

---

## Proje Yapısı

Genel proje yapısı aşağıdaki gibidir:

```text
MaMoScope
│
├── WPF Client
│   ├── Core
│   ├── Factories
│   ├── Models
│   ├── Resources
│   ├── Services
│   ├── ViewModels
│   └── Views
│
├── API
│   └── Controllers
│       └── MotorTestsController
│
├── Application
│   ├── DTOs
│   ├── Interfaces
│   └── Services
│       └── MotorDriverService
│
├── Domain
│   └── Entities
│       └── MotorDrivers
│
└── Infrastructure
    ├── Data
    ├── Repositories
    └── Migrations
```

---

## Kurulum

Projeyi bilgisayara almak için:

```bash
git clone <REPOSITORY_URL>
```

Daha sonra proje Visual Studio ile açılabilir.

Gerekli NuGet paketlerinin yüklenmesinin ardından API ve WPF projeleri çalıştırılabilir.

### Veritabanı

PostgreSQL kurulumu gerçekleştirildikten sonra proje için gerekli veritabanı oluşturulmalıdır.

Connection string, uygulamanın yapılandırma dosyasında kullanılan PostgreSQL sunucusuna göre düzenlenmelidir.

Migration'lar kullanılarak veritabanı şeması oluşturulabilir.

---

## Çalıştırma

Projeyi çalıştırmak için öncelikle API'nin çalışır durumda olması gerekir.

API çalıştırıldıktan sonra WPF Client başlatılır.

WPF uygulaması API'nin adresine HTTP üzerinden bağlanarak motor test kayıtlarını oluşturabilir ve geçmiş kayıtları görüntüleyebilir.

---

## Projenin Geliştirilebilecek Yönleri

Projenin ilerleyen aşamalarında aşağıdaki özellikler eklenebilir:

* Gerçek motor sürücüsü donanımı ile haberleşme
* Otomatik test senaryolarının oluşturulması
* Kullanıcı giriş ve yetkilendirme sistemi
* Daha ayrıntılı test raporları
* PDF veya Excel formatında test çıktısı alma
* Grafiksel test sonuçları
* Gelişmiş filtreleme ve arama
* Test kayıtlarının tarih aralığına göre filtrelenmesi
* Merkezi ve uzak veritabanı kullanımı
* API'nin güvenli HTTPS bağlantısı üzerinden yayınlanması
* Otomatik CI/CD süreçlerinin oluşturulması
* Birim ve entegrasyon testlerinin genişletilmesi

---

## Sonuç

MaMoScope projesi kapsamında WPF tabanlı bir masaüstü uygulaması ile ASP.NET Core Web API ve PostgreSQL veritabanı arasında çalışan katmanlı bir sistem geliştirilmiştir.

Proje içerisinde MVVM, Clean Architecture, Repository Pattern, Dependency Injection ve REST API gibi yazılım geliştirme yaklaşımlarından yararlanılmıştır.

WPF istemcisi kullanıcı etkileşimlerini yönetirken, ASP.NET Core Web API istemci ile uygulama katmanları arasında iletişim sağlamaktadır. Application katmanı iş mantığını yönetmekte, Infrastructure katmanı ise veritabanı erişimini gerçekleştirmektedir.

Bu yapı sayesinde uygulamanın sorumlulukları katmanlara ayrılmış ve daha düzenli, sürdürülebilir ve geliştirilebilir bir yazılım mimarisi oluşturulmuştur.

---

## Geliştirici

**Şevval Korkmaz**

Bilgisayar Mühendisliği

MaMoScope Projesi
