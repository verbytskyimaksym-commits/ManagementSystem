# Система медичних записів (ManagementSystem)

Система для управління записами до лікаря (`Doctors`, `Schedules`, `Appointments`) з MVC-адмін-панеллю та клієнтом на .NET MAUI.

## Структура проєкту
- `Server/ServerApp` — ASP.NET Core 8 (MVC-адмін-панель, REST API, Swagger, JWT, EF Core + SQLite)
- `Client/ClientApp` — .NET MAUI 8 (десктопний клієнт для лікаря на Windows, патерн MVVM)
- `Database/init.sql` — SQL-скрипт таблиць, зв'язків, індексів і тестових даних
- `Docs/` — `swagger.json`, `init.sql` та скріншоти тестування (`Docs/screenshots/`)

## Запуск
- **Сервер:** `dotnet run --project Server/ServerApp` (адмінка: `http://localhost:5254/`, логін `admin` / `admin123`, Swagger: `/swagger`)
- **Клієнт (MAUI):** `dotnet run --project Client/ClientApp --no-launch-profile` (вхід лікаря: `kovalenko@clinic.ua` / `hash_doctor_1`)

## API-ендпоінти
- `POST /api/auth/login` — логін лікаря, видача JWT-токена
- `GET /api/schedules` — графік лікаря (Bearer JWT)
- `GET /api/appointments` — записи лікаря (Bearer JWT)
- `POST /api/appointments` — створення та підтвердження запису (Bearer JWT)