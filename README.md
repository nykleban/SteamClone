# SteamClone
---

# 🎮 SteamClone — Backend

> ASP.NET Core REST API — бекенд для клону платформи Steam.  
> Побудований на тришаровій архітектурі: **API → BLL → DAL**

---

## 🗂 Структура проєкту

```
back/
├── SteamClone/          # Презентаційний шар (API)
├── SteamClone.BLL/      # Бізнес-логіка
└── SteamClone.DAL/      # Доступ до даних (EF Core + PostgreSQL)
```

---

## ⚙️ Технології

| Технологія | Версія |
|---|---|
| .NET | 10.0 |
| ASP.NET Core | 10.0 |
| Entity Framework Core | 10.0.5 |
| PostgreSQL (Npgsql) | 10.0.1 |
| Swagger (Swashbuckle) | 10.1.5 |

---

## 🚀 Запуск проєкту

### 1. Клонувати репозиторій

```bash
git clone <repo-url>
cd back
```

### 2. Налаштувати рядок підключення

Відкрити `SteamClone/appsettings.json` і вказати свої дані для PostgreSQL:

```json
"ConnectionStrings": {
  "DefaultConnection": "User ID=postgres;Password=YOUR_PASSWORD;Host=localhost;Port=5432;Database=SteamClone;"
}
```

### 3. Запустити

```bash
dotnet run --project SteamClone
```

> При першому запуску автоматично застосуються міграції та заповниться БД тестовими даними (Seeder).

### 4. Swagger UI

```
https://localhost:{PORT}/swagger
```

---

<!-- ## 🗄 База даних

### Entities

| Сутність | Опис |
|---|---|
| `GameEntity` | Гра: назва, ціна, дата релізу, опис |
| `DeveloperEntity` | Розробник: ім'я, зображення |
| `GenreEntity` | Жанр: назва |
| `GameImageEntity` | Зображення гри: назва, флаг прев'ю |

### Зв'язки

- `Developer` → `Games` — один до багатьох
- `Game` → `Images` — один до багатьох (cascade delete)
- `Game` ↔ `Genres` — багато до багатьох (таблиця `GameGenres`)

### Міграції

```bash
# Застосувати міграції вручну
dotnet ef database update --project SteamClone.DAL --startup-project SteamClone

# Створити нову міграцію
dotnet ef migrations add MigrationName --project SteamClone.DAL --startup-project SteamClone
```

---

## 🌐 API Endpoints

### Games — `/api/game`

| Метод | Endpoint | Опис |
|---|---|---|
| `GET` | `/api/game` | Отримати всі ігри |
| `GET` | `/api/game/{id}` | Отримати гру за ID |
| `POST` | `/api/game` | Створити гру |
| `PUT` | `/api/game` | Оновити гру |
| `PATCH` | `/api/game` | Частково оновити гру |
| `DELETE` | `/api/game` | Видалити гру |

### Genres — `/api/genre`

| Метод | Endpoint | Опис |
|---|---|---|
| `GET` | `/api/genre` | Отримати всі жанри |
| `GET` | `/api/genre/{id}` | Отримати жанр за ID |

---

## 🧱 Репозиторії (DAL)

Всі репозиторії наслідують `GenericRepository<T>`, який надає базові CRUD-операції.

### `GenericRepository<T>`
Базові методи: `GetAll()`, `GetByIdAsync()`, `CreateAsync()`, `UpdateAsync()`, `DeleteAsync()`, `CreateRangeAsync()`, `DeleteRangeAsync()`

### `GameRepository`
| Метод | Опис |
|---|---|
| `GetCheaperThan(decimal price)` | Ігри дешевші за вказану ціну |
| `GetByGenre(int genreId)` | Ігри вказаного жанру |

### `DeveloperRepository`
| Метод | Опис |
|---|---|
| `GetByNameAsync(string name)` | Пошук розробника за ім'ям |
| `IsExistsAsync(string name)` | Перевірка існування розробника |

### `GameImageRepository`
| Метод | Опис |
|---|---|
| `GetByGameId(int gameId)` | Всі зображення конкретної гри |

### `GenreRepository`
| Метод | Опис |
|---|---|
| `GetByNameAsync(string name)` | Пошук жанру за назвою |
| `IsExitsAsync(string name)` | Перевірка існування жанру |

--- -->

## 🌱 Seeder

При першому запуску БД автоматично заповнюється початковими даними:

- **10 жанрів**: Спорт, Стратегії, Жахи, Казуальні, Перегони, Симулятори, Виживання, Космос, Аніме, Шутери
- **10 розробників** з іграми: Rockstar Games, Ubisoft, EA, CD Projekt RED, Capcom, Bethesda, Sega, FromSoftware, Square Enix, Valve

<!-- ---

## 🔧 CORS

Налаштований дозволяючий CORS-policy (`allowAll`) для зручності розробки:

```
AllowAnyOrigin + AllowAnyMethod + AllowAnyHeader
```

> ⚠️ Перед деплоєм в продакшн замінити на конкретні origins. -->