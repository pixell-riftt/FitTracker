# FitTracker

A fitness workout logging web application built with ASP.NET Core MVC. FitTracker allows users to create, manage, and track their workouts, categorize them by exercise type, and save their favorite workouts for quick access.

## Main Features

- **Workout Management** — Full CRUD (Create, Read, Update, Delete) operations for workouts.
- **Exercise Categories** — Workouts are organized by exercise type (Cardio, Strength, Flexibility, HIIT, CrossFit, Yoga).
- **My Workouts** — Personal dashboard showing only your workouts with stats (total workouts, total minutes, total saves).
- **Browse & Search** — Browse all workouts with search by title and filter by exercise type.
- **Favorites** — Users can save and remove workouts from their personal favorites list. You cannot save your own workouts — only other users' workouts can be added to favorites.
- **Image Upload** — Upload images from your device or provide an image URL for each workout.
- **User Authentication** — Register and login functionality powered by ASP.NET Identity.
- **Dark Mode** — Toggle between light (earth tones) and dark mode using the moon/sun icon in the navbar.
- **Toast Notifications** — Success messages appear when creating, editing, deleting, or saving workouts.
- **Responsive Design** — Clean, mobile-friendly UI built with Bootstrap 5.

## Technologies Used

- ASP.NET Core 8.0 (MVC)
- Entity Framework Core 8.0
- Microsoft SQL Server (LocalDB)
- ASP.NET Identity
- Razor Views (with Layout, Partial Views, and Sections)
- Bootstrap 5
- jQuery Unobtrusive Validation

## Project Structure

The solution follows a multi-layered architecture:

- **FitTracker.Web** — Controllers, Views, and application startup configuration.
- **FitTracker.Data** — Database context, migrations, and seed data.
- **FitTracker.Data.Models** — Entity models (Workout, ExerciseType, UserWorkout).
- **FitTracker.ViewModels** — View models for each view.
- **FitTracker.Services.Core** — Business logic with service interfaces and implementations.
- **FitTracker.Common** — Shared validation constants.

## Setup Instructions

### Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) or higher
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (LocalDB, Express, or full edition)
- Visual Studio 2022+ or JetBrains Rider (recommended) / VS Code with C# extension

### Installation

1. **Clone the repository:**
   ```bash
   git clone https://github.com/pixell-riftt/FitTracker.git
   cd FitTracker
   ```

2. **Check the connection string** in `FitTracker.Web/appsettings.json`:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=FitTracker;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
   }
   ```
   Adjust the `Server` value if your SQL Server instance name is different.

3. **Apply database migrations:**
   ```bash
   dotnet ef database update --startup-project FitTracker.Web --project FitTracker.Data
   ```

4. **Run the application:**
   ```bash
   cd FitTracker.Web
   dotnet run
   ```

5. **Open your browser** and navigate to the URL shown in the terminal (e.g., `http://localhost:5276`).

### Test Accounts

**Admin account (seeded):**
- Email: admin@fittracker.com
- Password: Admin123!

**Test user account (register manually to test favorites):**
- Email: testuser@fittracker.com
- Password: Test123!

> Note: You cannot save your own workouts to favorites. Log in with a different account to test the favorites feature.

## License

This project was developed as a course assignment for the ASP.NET Fundamentals module at SoftUni.
