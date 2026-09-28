# News Portal

Учебный проект: веб-приложение для публикации и чтения новостей на ASP.NET Core MVC.

## Стек

- ASP.NET Core 9 MVC
- Entity Framework Core + PostgreSQL (Npgsql)
- ASP.NET Core Identity (аутентификация, роли)
- AutoMapper
- Bootstrap 5

## Возможности

- Регистрация и вход, роли: Admin, Moderator, User
- CRUD новостей, категории, теги, источники
- Модерация статей и комментариев
- Реакции, подписки на категории, аналитика просмотров
- Загрузка изображений, пагинация, поиск
- Слоистая архитектура: WebAppUI → BLL → DAL

## Структура решения

- `DAL` — DbContext, репозитории, миграции EF Core
- `BLL` — DTO, сервисы, интерфейсы, профили AutoMapper
- `WebAppUI` — контроллеры, представления, Identity, конфигурация

## Запуск локально

1. Установить [.NET 9 SDK](https://dotnet.microsoft.com/download) и PostgreSQL.

2. Создать базу данных:

   ```
   CREATE DATABASE kursuchigi2;
   ```

3. Настроить строку подключения. Рекомендуется через User Secrets:

   ```
   dotnet user-secrets set "ConnectionStrings:postgresConnection" "Host=localhost;Port=5432;Database=kursuchigi2;Username=postgres;Password=ВАШ_ПАРОЛЬ" --project WebAppUI
   ```

   Или отредактировать `WebAppUI/appsettings.Development.json` (в `.gitignore`, в репозиторий не попадёт).

4. Применить миграции (они уже есть в `DAL/Migrations`):

   ```
   dotnet ef database update --project DAL --startup-project WebAppUI
   ```

5. Запустить:

   ```
   dotnet run --project WebAppUI
   ```

## Автор

Vladimir — [GitHub](https://github.com/id-relog)

## Демо
https://news-portal-qo2j.onrender.com