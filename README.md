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
- [Project Structure](#project-structure)
- [Getting Started](#getting-started)
- [Environment Variables](#environment-variables)
- [Roadmap](#roadmap)
- [Team](#team)
- [Data Sources & Acknowledgments](#data-sources--acknowledgments)
- [License](#license)

---

## Overview

Ruya is a mobile AI companion that guides tourists through Egyptian heritage sites using a storytelling, friend-like persona. Point your camera at a monument or ask a question by voice, and Ruya identifies what you're looking at, retrieves verified historical context, and responds conversationally in Arabic or English — grounded entirely in academic sources, never guessed.

At the end of a visit, Ruya compiles the user's journey into a personalized **memory album** — a shareable digital keepsake of the route walked, the stories heard, and the places seen.

## The Problem

Today, a tourist standing in front of a monument in Egypt has three options, and all three fail them:

1. **Read the plaque** — a two-line description with a date and a name. No story, no context.
2. **Hire a guide** — expensive, not always available, delivers the same script regardless of the visitor's interests.
3. **Search it themselves** — a wall of disconnected text that wasn't written for the moment they're standing in.

## The Solution

Ruya replaces all three with a single AI companion in the tourist's pocket:

> "A plaque gives you a fact. A guide gives you a script. Ruya gives you a conversation."

## Key Features

**MVP (Sprint 1–4):**
- [x] Monument registry — admin-managed knowledge base linking dataset labels to canonical monument data
- [x] Role-based authentication (Guest, Tourist, Admin)
- [ ] Site directory — browse and view detail pages for major Egyptian heritage sites
- [ ] Visual recognition — camera-based monument identification
- [ ] Conversational voice Q&A — grounded in verified sources via RAG
- [ ] GPS-triggered narration at site entry points
- [ ] Bilingual support — Egyptian Arabic and English
- [ ] Memory album generation — route, stories, and photos compiled into a shareable keepsake
- [ ] Admin Dashboard — monument registry management, flagged response review, usage analytics

**Post-MVP:**
- [ ] Trip planning & itinerary builder
- [ ] Site ticket booking & booking management
- [ ] Personalized narration depth by user interest
- [ ] Social sharing & PDF export of memory albums
- [ ] Offline mode for low-connectivity areas

See [`docs/backlog.xlsx`](./docs/) for the full prioritized backlog with story points and acceptance criteria.

## Tech Stack

| Layer | Technology |
|---|---|
| **Mobile** | React Native or Flutter |
| **Web (Admin Dashboard)** | Angular |
| **Backend API** | ASP.NET Core Web API (.NET) |
| **AI Orchestration Service** | Python (FastAPI) |
| **Relational Database** | SQL Server |
| **Document/Log Store** | MongoDB |
| **Vector Database** | Pinecone or Chroma |
| **LLM** | GPT-4o / Claude |
| **Vision** | GPT-4V |
| **Text-to-Speech** | ElevenLabs |
| **Observability** | LangFuse |
| **Infrastructure** | Docker, GitHub Actions (CI/CD), Azure |

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

Full architecture diagrams live in [`docs/architecture/`](./docs/architecture/).

## AI Architecture

Ruya's AI layer is intentionally minimal in its use of the word "agent." Only two components make real decisions under ambiguity:

- **Orchestrator Agent** — interprets user intent (a knowledge question? an action request? something unclear?) and decides which tool to invoke
- **Critic Agent** — reviews the Orchestrator's draft response and validates it's actually grounded in retrieved sources before it's spoken aloud

Everything else — vision recognition, GPS triggers, text-to-speech, memory album compilation — is a deterministic **tool** the Orchestrator calls, not an independent agent.

**Knowledge sources (RAG corpus):** UCLA Encyclopedia of Egyptology, Metropolitan Museum Open Access API, Wikipedia (Arabic + English), Egyptian Ministry of Tourism & Antiquities.

## Project Structure

```
ruya/
├── mobile/                 # React Native / Flutter tourist-facing app
├── web-admin/               # Angular admin dashboard
├── backend-api/              # ASP.NET Core Web API
├── ai-service/               # Python FastAPI — Orchestrator, Critic, RAG pipeline
├── docs/
│   ├── architecture/          # System design diagrams
│   ├── backlog.xlsx            # Product backlog
│   └── pitch/                   # Pitch deck & video script
├── .github/workflows/          # CI/CD pipelines
└── README.md
```

> This structure is a starting proposal — update this section once the actual repositories/folders are finalized.

## Getting Started

> **Note:** Setup instructions below are placeholders. Update with real commands once each service is scaffolded.

### Prerequisites
_To be determined._
### Clone the repository
```bash
git clone <repository-url>
cd ruya
```

### Backend API
```bash
cd backend-api
dotnet restore
dotnet run
```

### Run with Docker Compose
```bash
docker-compose up --build
```

## Environment Variables
_To be determined._


## Roadmap
_To be determined._

## Team



## Data Sources & Acknowledgments

## License

_To be determined._

---

