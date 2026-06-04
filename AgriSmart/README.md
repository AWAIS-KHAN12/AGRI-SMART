# AgriSmart — Login & Register

Blazor Server + ASP.NET Core Identity project with **Login** and **Register** pages only.

## How to Run

```bash
cd AgriSmart.Web
dotnet run
```

Then open: `https://localhost:5001` (or `http://localhost:5000`)

## Default Admin Account
- **Username:** admin
- **Password:** Admin@1234

## Register a New Farmer
- Go to `/register`
- Fill in the form
- After registration, you'll be redirected to `/farmer/dashboard` (not implemented in this demo)

## Tech Stack
- ASP.NET Core 3.1 Blazor Server
- ASP.NET Core Identity
- Entity Framework Core (SQLite by default)

## Database
SQLite is used by default (`agrismart.db` is auto-created on first run).
To switch to SQL Server, update `appsettings.json`:
```json
"Database": { "Provider": "SqlServer" }
```
