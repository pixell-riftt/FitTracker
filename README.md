# FitTracker

A fitness workout logging web application built with ASP.NET Core MVC. FitTracker allows users to create, manage, and track their workouts, categorize them by exercise type, save favorites, leave comments, and manage user profiles — all within a supportive fitness community.

## Project Concept

FitTracker was designed as a community-driven fitness platform where users can log their workouts, share them with others, and discover new routines. The application supports two roles — regular users and administrators — with a dedicated admin panel for site management.

## Main Features

- **Workout Management** — Full CRUD (Create, Read, Update, Delete) operations for workouts with soft delete.
- **Exercise Categories** — Workouts organized by type: Cardio, Strength, Flexibility, HIIT, CrossFit, Yoga.
- **My Workouts** — Personal dashboard showing only your workouts with statistics (total workouts, total minutes, total saves by others).
- **Browse & Search** — Browse all workouts with search by title, filter by exercise type, and pagination (6 per page).
- **Favorites** — Save and remove other users' workouts from your personal favorites list.
- **Comments** — Leave comments on workouts to engage with the community. Authors and admins can delete comments.
- **User Profiles** — View and edit profiles with display name, bio, and profile picture. See other users' profiles and their workouts.
- **Image Upload** — Upload images from your device or provide an image URL for each workout.
- **User Authentication & Roles** — Register/login with ASP.NET Identity. Two roles: User and Admin.
- **Admin Panel** — Dedicated admin area with dashboard, user management (promote/demote), workout management, and comment moderation.
- **Dark Mode** — Toggle between light (earth tones) and dark (warm brown) mode. Preference saved in localStorage.
- **Toast Notifications** — Success and error messages for all major actions.
- **Custom Error Pages** — Styled pages for 404 Not Found, 500 Server Error, and 403 Access Denied.
- **Delete Confirmations** — Confirmation dialogs before deleting workouts or removing favorites.
- **Responsive Design** — Mobile-friendly UI built with Bootstrap 5.

## Architecture & Design Decisions

### Multi-Layered Architecture

The solution follows clean separation of concerns with 7 projects:

- **FitTracker.Web** — Presentation layer: Controllers, Views, Areas, and application startup configuration. Handles HTTP requests, routing, and UI rendering.
- **FitTracker.Data** — Data access layer: Entity Framework Core DbContext, migrations, seed data, and database configuration using Fluent API.
- **FitTracker.Data.Models** — Domain layer: Entity models representing database tables (Workout, ExerciseType, UserWorkout, Comment, UserProfile).
- **FitTracker.ViewModels** — Transfer layer: View models tailored for each view, separating UI concerns from database entities.
- **FitTracker.Services.Core** — Business logic layer: Service interfaces and implementations containing all application logic. Controllers never access the database directly.
- **FitTracker.Common** — Shared layer: Validation constants used across models and view models to ensure consistency.
- **FitTracker.Tests** — Test layer: Unit tests using xUnit and InMemory database for testing service methods.

### Key Design Patterns

- **Dependency Injection** — All services registered in Program.cs with `AddScoped`. Controllers depend on interfaces, not concrete classes.
- **Repository Pattern** — DbContext acts as the repository; services encapsulate all data access logic.
- **Soft Delete** — Workouts are never physically deleted; `IsDeleted` flag preserves data integrity.
- **MVC Areas** — Admin functionality separated into its own area with dedicated layout and controllers.

### Entity Relationships

- Workout → ExerciseType: Many-to-One
- Workout → User (Author): Many-to-One
- Workout ↔ User (Favorites): Many-to-Many via UserWorkout join table
- Comment → Workout: Many-to-One
- Comment → User (Author): Many-to-One
- UserProfile → User: One-to-One

### Validation Strategy

- **Server-side**: Data annotations on ViewModels ([Required], [StringLength], [Range]) checked via ModelState.IsValid
- **Client-side**: jQuery Unobtrusive Validation for instant feedback before form submission
- **Database-level**: Entity model annotations ([Required], [MaxLength]) enforced by EF Core migrations

### Seeding

The database is seeded with:
- 2 roles (Admin, User)
- 1 admin user with Admin role assigned
- 1 admin profile
- 6 exercise types
- 3 sample workouts
- 2 sample comments

### Security

- CSRF protection via [ValidateAntiForgeryToken] on all POST actions
- Authorization checks in services (only authors can edit/delete their workouts)
- [Authorize] attribute on all controllers except Home
- [Authorize(Roles = "Admin")] on the Admin area
- Custom Access Denied page for unauthorized access attempts
- Razor automatically HTML-encodes output to prevent XSS

## Technologies Used

- ASP.NET Core 8.0 (MVC)
- Entity Framework Core 8.0
- Microsoft SQL Server (LocalDB)
- ASP.NET Identity with Roles
- Razor Views (Layout, Partial Views, Sections)
- Bootstrap 5
- Bootstrap Icons
- jQuery Unobtrusive Validation
- xUnit (Unit Testing)
- InMemory Database (Testing)

## Test Coverage

44 unit tests covering the service layer:

- **WorkoutServiceTests** — CRUD operations, invalid data handling
- **WorkoutServiceEditDeleteTests** — Edit/delete operations, authorization checks
- **WorkoutServiceFavoritesTests** — Save/remove favorites, duplicate prevention, MyWorkouts
- **CommentServiceTests** — Add/delete comments, authorization, ordering
- **ProfileServiceTests** — Profile CRUD, auto-creation, validation

Run tests with:
```bash
dotnet test
```

## Setup Instructions

### Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) or higher
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (LocalDB, Express, or full edition)
- Visual Studio 2022+ or JetBrains Rider / VS Code with C# extension

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

**Test user account (register manually to test favorites and comments):**
- Email: testuser@fittracker.com
- Password: Test123!

> Note: You cannot save your own workouts to favorites. Log in with a different account to test the favorites and comments features. The admin account has access to the Admin panel via the shield icon in the navbar.

## License

This project was developed as a course assignment for the ASP.NET Fundamentals and ASP.NET Advanced modules at SoftUni.
