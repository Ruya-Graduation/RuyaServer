# Ruya (رؤية) — AI Heritage Tour Guide

> An AI-powered companion that replaces short, lifeless plaques and expensive tour guides with a conversational storyteller who brings Egypt's ancient monuments to life.

**Status:** In active development — Sprint 1  
**ITI Graduation Project** — Integrated Development and Architecture Program

---

## Table of Contents

- [Overview](#overview)
- [The Problem](#the-problem)
- [The Solution](#the-solution)
- [Key Features](#key-features)
- [Tech Stack](#tech-stack)
- [System Architecture](#system-architecture)
- [AI Architecture](#ai-architecture)
- [API Endpoints Documentation](#api-endpoints-documentation)
  - [Authentication API (`/api/Auth`)](#1-authentication-api-apiauth)
  - [Chat & AI Conversation API (`/api/Chat`)](#2-chat--ai-conversation-api-apichat)
  - [Admin Artifact Management API (`/api/AdminArtifacts`)](#3-admin-artifact-management-api-apiadminartifacts)
  - [Admin Heritage Site Management API (`/api/AdminSites`)](#4-admin-heritage-site-management-api-apiadminsites)
- [Response Wrapper Format](#response-wrapper-format)
- [Project Structure](#project-structure)
- [Getting Started](#getting-started)
- [Environment Variables](#environment-variables)
- [License](#license)

---

## Overview

Ruya is a mobile AI companion that guides tourists through Egyptian heritage sites using a storytelling, friend-like persona. Point your camera at a monument or ask a question by voice, and Ruya identifies what you're looking at, retrieves verified historical context, and responds conversationally in Arabic or English — grounded entirely in academic sources, never guessed.

At the end of a visit, Ruya compiles the user's journey into a personalized **memory album** — a shareable digital keepsake of the route walked, the stories heard, and the places seen.

---

## The Problem

Today, a tourist standing in front of a monument in Egypt has three options, and all three fail them:

1. **Read the plaque** — a two-line description with a date and a name. No story, no context.
2. **Hire a guide** — expensive, not always available, delivers the same script regardless of the visitor's interests.
3. **Search it themselves** — a wall of disconnected text that wasn't written for the moment they're standing in.

---

## The Solution

Ruya replaces all three with a single AI companion in the tourist's pocket:

> "A plaque gives you a fact. A guide gives you a script. Ruya gives you a conversation."

---

## Key Features

**MVP (Sprint 1–4):**
- [x] Monument registry — admin-managed knowledge base linking dataset labels to canonical monument data
- [x] Role-based authentication (Guest, Tourist, Admin)
- [x] Site directory — browse and view detail pages for major Egyptian heritage sites
- [x] Conversational voice & text Q&A — grounded in verified sources via RAG
- [ ] Visual recognition — camera-based monument identification
- [ ] GPS-triggered narration at site entry points
- [ ] Bilingual support — Egyptian Arabic and English
- [ ] Memory album generation — route, stories, and photos compiled into a shareable keepsake
- [ ] Admin Dashboard — monument registry management, flagged response review, usage analytics

---

## Tech Stack

| Layer | Technology |
|---|---|
| **Mobile** | React Native / Flutter |
| **Web (Admin Dashboard)** | Angular |
| **Backend API** | ASP.NET Core Web API (.NET 10) |
| **Identity & Security** | ASP.NET Core Identity & JWT Bearer Tokens |
| **AI Orchestration Service** | Python (FastAPI) |
| **Relational Database** | SQL Server (Entity Framework Core) |
| **Document/Log Store** | MongoDB |
| **Vector Database** | Pinecone / Chroma |
| **LLM** | GPT-4o / Claude |
| **Vision** | GPT-4V |
| **Text-to-Speech** | ElevenLabs |

---

## System Architecture

```
Client Layer (Mobile App + Web Admin Dashboard)
        │  HTTPS / REST, JWT auth
        ▼
Backend API (.NET Core) — auth, sessions, business logic
        │  Internal HTTPS
        ▼
AI Orchestration Service (Python)
   ├── Orchestrator Agent   — decides which tool to call
   ├── Critic Agent         — validates responses are grounded
   └── Tools                — vision lookup, RAG search, TTS, GPS trigger
        │
        ▼
Data Layer
   ├── SQL Server   — users, sessions, monument registry, bookings
   ├── MongoDB      — conversation logs, agent trace metadata
   └── Vector DB    — knowledge base embeddings
```

---

## AI Architecture

Ruya's AI layer is intentionally minimal in its use of autonomous agents:

- **Orchestrator Agent** — interprets user intent (a knowledge question? an action request? something unclear?) and decides which tool to invoke.
- **Critic Agent** — reviews the Orchestrator's draft response and validates it's grounded in retrieved sources before it's delivered to the user.

Everything else — vision recognition, GPS triggers, text-to-speech, memory album compilation — is a deterministic **tool** the Orchestrator calls.

---

## API Endpoints Documentation

All endpoints are hosted under the base URL `/api`. Protected endpoints require an `Authorization: Bearer <JWT_TOKEN>` header.

### Endpoints Overview Table

| Category | Method | Endpoint | Authorization | Description |
|---|---|---|---|---|
| **Auth** | `POST` | `/api/Auth/register` | Anonymous | Register a new user account |
| **Auth** | `POST` | `/api/Auth/login` | Anonymous | Authenticate user & get JWT token |
| **Auth** | `POST` | `/api/Auth/forgot-password` | Anonymous | Send 6-digit OTP verification email |
| **Auth** | `POST` | `/api/Auth/verify-otp` | Anonymous | Verify OTP code & receive reset session token |
| **Auth** | `POST` | `/api/Auth/reset-password` | Anonymous | Reset user password using token |
| **Chat** | `POST` | `/api/Chat/message` | Authorized | Send text/image query to AI companion |
| **Chat** | `GET` | `/api/Chat` | Authorized | Get all conversations for authenticated user |
| **Chat** | `GET` | `/api/Chat/{conversationId}` | Authorized | Get message history for a conversation |
| **Chat** | `DELETE` | `/api/Chat/{conversationId}` | Authorized | Delete a specific conversation |
| **Admin Artifacts** | `GET` | `/api/AdminArtifacts` | Admin | List all registered historical artifacts |
| **Admin Artifacts** | `GET` | `/api/AdminArtifacts/{id}` | Admin | Get details of an artifact by ID |
| **Admin Artifacts** | `POST` | `/api/AdminArtifacts` | Admin | Create a new historical artifact entry |
| **Admin Artifacts** | `PUT` | `/api/AdminArtifacts/{id}` | Admin | Update an existing artifact entry |
| **Admin Artifacts** | `DELETE` | `/api/AdminArtifacts/{id}` | Admin | Delete an artifact entry |
| **Admin Sites** | `GET` | `/api/AdminSites` | Admin | List all registered Egyptian heritage sites |
| **Admin Sites** | `GET` | `/api/AdminSites/{id}` | Admin | Get details of a heritage site by ID |
| **Admin Sites** | `POST` | `/api/AdminSites` | Admin | Create a new heritage site entry |
| **Admin Sites** | `PUT` | `/api/AdminSites/{id}` | Admin | Update an existing heritage site entry |
| **Admin Sites** | `DELETE` | `/api/AdminSites/{id}` | Admin | Delete a heritage site entry |

---

### 1. Authentication API (`/api/Auth`)

#### `POST /api/Auth/register`
* **Description:** Registers a new tourist account with initial preference settings.
* **Content-Type:** `application/json`
* **Request Body:**
```json
{
  "userName": "johndoe",
  "email": "john.doe@example.com",
  "password": "SecurePassword123!",
  "preferredLanguage": "en",
  "knowledgeLevel": "beginner"
}
```
* **Validation Rules:**
  - `userName`: Required, 3–100 characters.
  - `email`: Required, valid email format, max 200 characters.
  - `password`: Required, minimum 8 characters.
  - `preferredLanguage`: Required, max 50 characters.
  - `knowledgeLevel`: Required, max 50 characters.
* **Success Response (200 OK):** Returns JWT access token upon successful registration.

#### `POST /api/Auth/login`
* **Description:** Authenticates a user with email and password.
* **Content-Type:** `application/json`
* **Request Body:**
```json
{
  "email": "john.doe@example.com",
  "password": "SecurePassword123!"
}
```
* **Validation Rules:**
  - `email`: Required, valid email format.
  - `password`: Required.
* **Success Response (200 OK):** Returns JWT access token.

#### `POST /api/Auth/forgot-password`
* **Description:** Initiates password recovery by sending a 6-digit OTP verification code to the registered email address.
* **Content-Type:** `application/json`
* **Request Body:**
```json
{
  "email": "john.doe@example.com"
}
```
* **Validation Rules:** `email` is required and must be a valid email format.
* **Success Response (200 OK):** Uniform generic success message to protect account privacy.

#### `POST /api/Auth/verify-otp`
* **Description:** Verifies the 6-digit OTP code sent to user's email and issues a temporary password reset session token.
* **Content-Type:** `application/json`
* **Request Body:**
```json
{
  "email": "john.doe@example.com",
  "code": "123456"
}
```
* **Validation Rules:**
  - `email`: Required, valid email format.
  - `code`: Required, valid OTP string.
* **Success Response (200 OK):** Returns `resetToken` and `expiresInSeconds` (300 seconds).

#### `POST /api/Auth/reset-password`
* **Description:** Resets the user's password using the verified reset session token.
* **Content-Type:** `application/json`
* **Request Body:**
```json
{
  "email": "john.doe@example.com",
  "resetToken": "server-issued-reset-token",
  "newPassword": "NewSecurePassword123!",
  "confirmPassword": "NewSecurePassword123!"
}
```
* **Validation Rules:**
  - `email`: Required, valid email format.
  - `resetToken`: Required.
  - `newPassword`: Required, minimum 8 characters.
  - `confirmPassword`: Required, must match `newPassword`.
* **Success Response (200 OK):** Password reset confirmation message.

---

### 2. Chat & AI Conversation API (`/api/Chat`)

#### `POST /api/Chat/message`
* **Description:** Sends a user query (text and optional visual image) to the AI companion. Retains context if `conversationId` is provided, or initializes a new conversation.
* **Content-Type:** `multipart/form-data`
* **Form Parameters:**
  - `ConversationId` *(int, optional)*: ID of existing conversation.
  - `Message` *(string, required)*: Text message / query.
  - `Language` *(string, default "en")*: Target language code (`"en"` or `"ar"`).
  - `Mode` *(string, default "story")*: Interaction mode (`"story"`, `"qa"`, etc.).
  - `Image` *(file, optional)*: Image upload for visual monument recognition.
* **Success Response (200 OK):** Returns AI response, message metadata, and updated conversation ID.

#### `GET /api/Chat`
* **Description:** Retrieves all past chat conversations belonging to the authenticated user.
* **Headers:** `Authorization: Bearer <JWT_TOKEN>`
* **Success Response (200 OK):** List of user conversation summaries.

#### `GET /api/Chat/{conversationId}`
* **Description:** Fetches the full message history for a specific conversation ID.
* **Parameters:** `conversationId` *(int, path parameter)*
* **Headers:** `Authorization: Bearer <JWT_TOKEN>`
* **Success Response (200 OK):** Conversation object including full chronological list of messages.

#### `DELETE /api/Chat/{conversationId}`
* **Description:** Deletes a specific conversation and all associated message history.
* **Parameters:** `conversationId` *(int, path parameter)*
* **Headers:** `Authorization: Bearer <JWT_TOKEN>`
* **Success Response (200 OK):** Deletion confirmation message.

---

### 3. Admin Artifact Management API (`/api/AdminArtifacts`)

#### `GET /api/AdminArtifacts`
* **Description:** Retrieves a list of all historical artifacts in the registry database.
* **Success Response (200 OK):** Array of artifact objects.

#### `GET /api/AdminArtifacts/{id}`
* **Description:** Retrieves details for a specific artifact by its primary key ID.
* **Parameters:** `id` *(int, path parameter)*
* **Success Response (200 OK):** Artifact detail object.

#### `POST /api/AdminArtifacts`
* **Description:** Creates a new historical artifact entry.
* **Content-Type:** `multipart/form-data`
* **Form Parameters:**
  - `SiteId` *(int, required)*: Associated heritage site ID.
  - `Name` *(string, required, max 200 chars)*: Name of the artifact.
  - `Category` *(string, required, max 100 chars)*: Category (e.g., Statue, Papyrus, Jewelry).
  - `Civilization` *(string, required, max 100 chars)*: Historical era/civilization (e.g., Ancient Egyptian).
  - `Period` *(string, required, max 100 chars)*: Dynasty or period designation.
  - `Image` *(file, optional)*: Image file of the artifact.
* **Success Response (201 Created):** Created artifact record with generated ID.

#### `PUT /api/AdminArtifacts/{id}`
* **Description:** Updates an existing artifact record.
* **Content-Type:** `multipart/form-data`
* **Parameters:** `id` *(int, path parameter)*
* **Form Parameters:** Same fields as `CreateArtifactDto`.
* **Success Response (200 OK):** Update status message.

#### `DELETE /api/AdminArtifacts/{id}`
* **Description:** Removes an artifact record from the registry database.
* **Parameters:** `id` *(int, path parameter)*
* **Success Response (200 OK):** Deletion confirmation message.

---

### 4. Admin Heritage Site Management API (`/api/AdminSites`)

#### `GET /api/AdminSites`
* **Description:** Retrieves a list of all registered Egyptian heritage sites.
* **Success Response (200 OK):** Array of heritage site summary objects.

#### `GET /api/AdminSites/{id}`
* **Description:** Retrieves details for a specific heritage site by its ID.
* **Parameters:** `id` *(int, path parameter)*
* **Success Response (200 OK):** Site detail object.

#### `POST /api/AdminSites`
* **Description:** Registers a new heritage site in the database.
* **Content-Type:** `application/json`
* **Request Body:**
```json
{
  "name": "Karnak Temple Complex",
  "city": "Luxor",
  "country": "Egypt",
  "latitude": 25.7188,
  "longitude": 32.6573,
  "hours": "06:00 AM - 05:30 PM",
  "ticket": "450 EGP",
  "crowds": "High in morning",
  "description": "Vast complex of temples, chapels, pylons, and other buildings near Luxor."
}
```
* **Validation Rules:**
  - `name`: Required, max 200 characters.
  - `city`: Required, max 100 characters.
  - `country`: Required, max 100 characters.
  - `latitude`: Required.
  - `longitude`: Required.
  - `hours`, `ticket`, `crowds`: Optional strings (max 100–200 characters).
  - `description`: Optional text (max 2000 characters).
* **Success Response (201 Created):** Created site record with generated ID.

#### `PUT /api/AdminSites/{id}`
* **Description:** Updates an existing heritage site details.
* **Parameters:** `id` *(int, path parameter)*
* **Content-Type:** `application/json`
* **Success Response (200 OK):** Update status message.

#### `DELETE /api/AdminSites/{id}`
* **Description:** Removes a heritage site record from the database.
* **Parameters:** `id` *(int, path parameter)*
* **Success Response (200 OK):** Deletion confirmation message.

---

## Response Wrapper Format

All API responses follow a standardized JSON envelope structure (`ApiResponse<T>`):

```json
{
  "succeeded": true,
  "message": "Operation completed successfully.",
  "data": { ... },
  "errors": null
}
```

* **`succeeded`** *(boolean)*: Indicates if the request executed without error.
* **`message`** *(string)*: User-friendly operational feedback message.
* **`data`** *(object / array)*: Payload returned on successful operations (null on failure).
* **`errors`** *(array of strings)*: List of error messages returned on failure (null on success).

---

## Project Structure

```
RUYA/
├── RUYA_API/                       # ASP.NET Core 10 Web API Solution
│   ├── Application/                # Application Layer (DTOs, Interfaces, Services)
│   │   ├── Common/                 # Cross-cutting interfaces & DTOs
│   │   └── Services/
│   │       ├── Admin/              # Site & Artifact management logic
│   │       ├── Auth/               # Authentication & identity logic
│   │       └── Chat/               # Chat & AI integration logic
│   ├── Controllers/                # REST Controller endpoints
│   │   ├── AdminArtifactsController.cs
│   │   ├── AdminSitesController.cs
│   │   ├── AuthController.cs
│   │   └── ChatController.cs
│   ├── Domain/                     # Core Domain entities & enums
│   ├── ExceptionHandling/          # Global middleware & custom exceptions
│   ├── Infrastructure/             # Database context, EF Core & external integrations
│   ├── Responses/                  # Standardized ApiResponse wrapper factories
│   └── Program.cs                  # Web API configuration & dependency injection
├── docs/                           # Documentation & architecture assets
└── README.md                       # Project documentation
```

---

## Getting Started

### Prerequisites
- [.NET 10.0 SDK](https://dotnet.microsoft.com/download)
- [SQL Server](https://www.microsoft.com/sql-server/)

### Clone & Run Backend API

1. **Clone the repository:**
   ```bash
   git clone <repository-url>
   cd RUYA/RUYA_API
   ```

2. **Restore dependencies:**
   ```bash
   dotnet restore
   ```

3. **Run database migrations & startup server:**
   ```bash
   dotnet run
   ```

4. **Access Swagger UI API Explorer:**  
   Open `https://localhost:7049/swagger` or `http://localhost:5000/swagger` in your browser.

---

## Environment Variables

Configure the following settings in `appsettings.json` or environment variables:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=RuyaDb;Trusted_Connection=True;TrustServerCertificate=True;"
  },
  "Jwt": {
    "Key": "YOUR_SUPER_SECRET_JWT_SIGNING_KEY",
    "Issuer": "RuyaAPI",
    "Audience": "RuyaClients"
  }
}
```

---

## License

This project is developed as part of the **Information Technology Institute (ITI)** Graduation Project. All rights reserved.
