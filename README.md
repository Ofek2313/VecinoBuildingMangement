# Vecino – Smart Building Management System

A full-stack building management platform designed to streamline communication and administration between building managers and residents.

> ⚠️ This project is currently under active development as a 5-point school project.

## About

Vecino is a multi-client system that digitizes the day-to-day management of a residential building (ועד בית). It provides a dedicated desktop application for the building manager and a web application for residents, both connected through a shared web service backend.

## Features

- **Payments** — Track and manage tenant payment status
- **Maintenance Requests** — Residents can submit requests; managers can track and update them
- **Announcements** — Building manager can broadcast announcements to all residents
- **Polls** — Create and vote on building decisions
- **Events** — Schedule and display upcoming building events

## Architecture

Vecino is split into multiple components:

| Component | Technology | Role |
|-----------|------------|------|
| `VecinoWpfApp` | C# / WPF | Desktop app for the building manager |
| `VecinoWebApplication` | C# / HTML / CSS | Web app for residents |
| `VecinoBuildingMangementWebService` | C# | Backend web service (shared API) |
| `BuildingManagementWsClient` | C# | WebSocket client |
| `VecinoBuildingMangement` | C# | Core logic and data models |
| `Testing` | C# | Unit tests |

## Requirements

- Windows
- Visual Studio 2022 (or later)
- .NET (version used in the project)
- SQL Server or LocalDB (if applicable)

## Building

1. Clone the repository:
   ```
   git clone https://github.com/Ofek2313/VecinoBuildingMangement.git
   ```
2. Open `VecinoBuildingMangement.sln` in Visual Studio.
3. Restore NuGet packages if prompted.
4. Set the startup project based on what you want to run:
   - **Manager interface** → `VecinoWpfApp`
   - **Resident interface** → `VecinoWebApplication`
   - **Backend** → `VecinoBuildingMangementWebService`
5. Press **Ctrl+Shift+B** to build.

## Project Structure

```
VecinoBuildingMangement/
├── VecinoWpfApp/                        # Desktop app for building manager
├── VecinoWebApplication/                # Web app for residents
├── VecinoBuildingMangementWebService/   # Shared backend web service
├── BuildingManagementWsClient/          # WebSocket client
├── VecinoBuildingMangement/             # Core logic and models
├── Testing/                             # Unit tests
└── VecinoBuildingMangement.sln          # Visual Studio solution file
```
