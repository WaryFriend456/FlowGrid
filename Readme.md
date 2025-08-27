# FlowGrid - A Trello-like Project Management App

FlowGrid is a full-stack project management application built with ASP.NET Core and React. It allows users to manage their projects using boards, lists, and cards in a real-time, collaborative environment.

## Tech Stack

- **Backend**: C# 13, ASP.NET Core 9 Web API
- **Frontend**: React 19, Vite, Bun
- **Database**: MySQL
- **Real-time**: SignalR
- **Authentication**: ASP.NET Core Identity with JWTs

## Getting Started

### Prerequisites
- .NET 9 SDK
- Bun.js
- MySQL Server

### Backend Setup
1.  Navigate to the `backend` folder.
2.  Update the `ConnectionStrings` in `appsettings.Development.json`.
3.  Run `dotnet run`. The API will be available at `https://localhost:7226`.

### Frontend Setup
1.  Navigate to the `frontend` folder.
2.  Run `bun install` to install dependencies.
3.  Run `bun run dev`. The application will be available at `http://localhost:5173`.