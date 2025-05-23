# 🧠 ASP.NET Core Learning Project

Welcome to my hands-on ASP.NET Core learning repository! 🚀  
This project is a collection of concepts and mini-features I've built while exploring **.NET 8**, **Entity Framework Core**, and clean backend architecture.

## 📚 What This Project Covers

This project is designed as a backend API using:

- ✅ ASP.NET Core Minimal APIs
- ✅ Entity Framework Core (EF Core) with SQLite
- ✅ Clean Architecture practices
- ✅ DTOs and Mapping logic
- ✅ Dependency Injection
- ✅ Separation of Endpoints using Extension Methods
- ✅ Async/Await and EF querying patterns
- ✅ Proper HTTP status codes and responses

---

## 🧩 Modules Implemented

### 🎮 Game API

A fully functional GameStore backend:

- **Endpoints:**
  - `GET /games/` — Get all games
  - `GET /games/{id}` — Get a game by ID
  - `POST /games/add` — Add a new game
  - `PATCH /games/{id}` — Update an existing game
  - `DELETE /games/{id}` — Delete a game
  - `GET /games/active` — List all active games
  - `GET /games/inActive` — List all inactive games

- **Includes:**
  - Game and Genre models
  - DTO mapping (`CreateGameDto`, `GameSummaryDto`, `GameDetailsDto`)
  - SQLite integration with `DbContext`
  - Proper response handling (`404`, `201`, `200`, etc.)

---

### 🏷️ Genre API

- `GET /genre/` — Get all genres
- `GET /genre/{id}` — Get a specific genre by ID
- AsNoTracking usage for performance
- Graceful handling of not found responses

---

## 🛠️ Technologies Used

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download)
- ASP.NET Core Minimal APIs
- Entity Framework Core
- SQLite (local development)
- Visual Studio Code (IDE)

---

## 🧪 How to Run

1. **Clone the repo**
   ```bash
   https://github.com/Chebaleomkar/asp.net-core-learning.git
   cd asp.net-core-learning
