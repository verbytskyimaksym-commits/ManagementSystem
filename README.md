# Система медичних записів (ManagementSystem)

Система для управління записами до лікаря (`Doctors`, `Schedules`, `Appointments`) з MVC-адмін-панеллю та MAUI-застосунком.

## Структура проєкту
- `Server/ServerApp` — ASP.NET Core 8 (MVC адмін-панель + REST API)
- `Client/ClientApp` — .NET MAUI 8 (клієнт для лікаря, MVVM)

## Запуск (CLI / VS Code)
- **Сервер:** `dotnet run --project Server/ServerApp --launch-profile https`
- **Клієнт (Windows):** `dotnet build Client/ClientApp -t:Run -f net8.0-windows10.0.19041.0`