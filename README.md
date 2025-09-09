# TeaDiary

Проект TeaDiary — это система для ведения дневника китайского чая, включающая несколько под-проектов:

- **TeaDiary.Api** — RESTful API для управления чайными сортами, пользователями и впечатлениями (в разработке).
- **TeaDiary.Client** — клиентское приложение (в разработке).
- **TeaDiary.Api.Tests** — модульные и интеграционные тесты для API (в разработке).

## Технологии

- ASP.NET Core 8
- Entity Framework Core
- PostgreSQL
- Docker и Docker Compose

## Структура решения

```
TeaDiary/
├── TeaDiary.Api/ # API сервер
├── teadiary.client/ # Клиентская часть (UI)
├── TeaDiary.Api.Tests/ # Тесты
├── docker-compose.yml # Описание контейнеров для БД и API
├── README.md # Этот файл
└── TeaDiary.sln # Решение Visual Studio/VS Code
```

## Запуск проекта

1. Клонируйте репозиторий и перейдите в папку решения:

```bush
git clone https://github.com/yourusername/TeaDiary.git
cd TeaDiary
```

2. Запустите базу данных и API в контейнерах Docker:

```bush
docker-compose up --build
```

3. API будет доступен по адресу: [http://localhost:5000/swagger](http://localhost:5000/swagger) — здесь можно посмотреть и протестировать эндпоинты.

## Локальная разработка

- Для запуска API локально с базой в Docker:  
  Запустите базу командой
  ```bush  
  docker-compose up -d postgres
  ```  
  и настройте строку подключения в `TeaDiary.Api/appsettings.json`, чтобы подключаться к базе на `localhost` и порту `5433`.

- Запускайте API локально командой:  
```bush
  dotnet run --project TeaDiary.Api
```

## Документация API

Подробная документация API находится в папке `TeaDiary.Api` в файле README.md.

## Тестирование

Тесты лежат в проекте `TeaDiary.Api.Tests`. Запуск тестов:

```bush
dotnet test TeaDiary.Api.Tests
```

## Контакты и помощь

Если возникнут вопросы или нужна помощь, пишите: egor05.09.97@gmail.com.
