# TeaDiary.Api

RESTful API для трекинга китайского чая, впечатлений и сортов.

## Описание

Технологии: ASP.NET Core, Entity Framework Core, PostgreSQL, Docker.

## Запуск проекта

1. Клонируйте репозиторий:

```bush
git clone https://github.com/EgorTolbaev/TeaDiary.Api.git
cd TeaDiary.Api
```

2. Запустите через Docker Compose (PostgreSQL + API):

```bush
docker-compose up --build
```

3. Swagger доступен на `http://localhost:5000/swagger`.

---

## Основные эндпоинты

- `/api/tea`
- `/api/teaType`
- `/api/user`
- `/api/impression`

---

## Примеры запроса/ответа

### Создать чай (POST /api/tea)

```json
{
"name": "Da Hong Pao",
"teaTypeId": "...",
"yearCollection": "2024",
"quantity": 2,
"userId": "...",
"description": "Плотный аромат, красный чай."
}
```

---

## Тестирование

Запуск интеграционных тестов:

```bush
dotnet test TeaDiary.Api.Tests
```

---

## Swagger и документация

XML-комментарии включены — все модели и методы подробно задокументированы.

---

## Установка зависимостей

Для работы с Entity Framework Core и PostgreSQL установите пакеты:

```bush
dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL
dotnet add package Microsoft.EntityFrameworkCore.Tools
```


- **Microsoft.EntityFrameworkCore** — основной пакет EF Core.
- **Npgsql.EntityFrameworkCore.PostgreSQL** — провайдер PostgreSQL.
- **Microsoft.EntityFrameworkCore.Tools** — инструменты для миграций.

---

## Работа с миграциями

- Добавить новую миграцию с именем `<MigrationName>`:

```bush
dotnet ef migrations add <MigrationName>
```

- Обновить базу данных до последней миграции:

```bush
dotnet ef database update
```

---

## Docker и docker-compose

### Dockerfile (папка TeaDiary.Api)

```Docker
# укажем, на основе какого образа будем 
# делать наш контейнер. Для сборки приложения используем dotnet-sdk 
# и назовём этот образ builder
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS builder 
# укажем директорию для нашего приложения внутри контейнера
WORKDIR /Application

# Скопируем все файлы из проекта в файловую систему контейнера
COPY . ./
# Запустим restore для загрузки зависимостей
RUN dotnet restore
# Опубликуем собранный dll в папку "output"
RUN dotnet publish -c Release -o output

# Теперь соберём образ, в котором наше приложение 
# будет запущено. Для запуска приложения достаточно
# среды выполнения aspnet, без sdk
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /Application
# Скопируем файлы приложения из предыдущего образа 
COPY --from=builder /Application/output .
# укажем команду, которая будет запускать приложение
ENTRYPOINT ["dotnet", "TeaDiary.Api.dll"]

```

---

### docker-compose.yml (в корне TeaDiary)

```docker-compose
services:
  postgres:
    container_name: postgres
    image: postgres
    restart: always
    environment:
      POSTGRES_DB: TeaDiaryDB
      POSTGRES_USER: postgres
      POSTGRES_PASSWORD: postgres
    ports:
      - 5433:5432
    volumes:
      - postgres-data:/var/lib/postgresql/data

  pgadmin:
    image: dpage/pgadmin4
    environment:
      PGADMIN_DEFAULT_EMAIL: admin@admin.com
      PGADMIN_DEFAULT_PASSWORD: password
      PGADMIN_LISTEN_PORT: 80
    ports:
      - 15432:80
    depends_on:
      - postgres

  teadiary-api:
    build:
      context: ./TeaDiary.Api
      dockerfile: Dockerfile
    depends_on:
      - postgres
    environment:
      - ConnectionStrings__DefaultConnection=Host=postgres;Database=TeaDiaryDB;Username=postgres;Password=postgres
    ports:
      - 5000:8080

volumes:
  postgres-data:

```

---

### Запуск проекта вручную с миграциями

Добавьте в `Program.cs` вызов миграций при старте:

```CSharp
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<TeaDiaryContext>();
    db.Database.Migrate();
}
```

---

### Запуск локально с базой в Docker

Если хотите запускать API локально (из кода), а базу использовать из Docker:

- Запустите базу:

```bush
docker-compose up -d postgres
```

- В `appsettings.json` для API настройте строку подключения:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5433;Database=TeaDiaryDB;Username=postgres;Password=postgres"
  }
}
```
- Запускайте API локально из IDE или командой:

```bush
dotnet run --project TeaDiary.Api
```

