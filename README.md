# 🚗 ParkEase Backend

> **Find. Reserve. Park. Effortlessly.**
> A production-grade **Smart Parking Management Platform** built with **.NET 8 Microservices**, **PostgreSQL**, **Docker**, and modern backend engineering practices.

---

# ✨ Why This Project Matters

ParkEase is not another CRUD parking app. It is a distributed backend system designed to simulate a real-world parking ecosystem with multiple actors, independent services, role-based workflows, analytics, notifications, and intelligent operational features.

It supports:

* **Drivers** searching and reserving parking spots
* **Managers** operating parking lots
* **Admins** controlling the platform
* **System jobs** handling automation in background

---

# 🧠 Core Highlights

## 🔹 Microservices Architecture

Each domain is isolated as an independently deployable service with its own business logic and database ownership.

## 🔹 Production-Style Backend

* JWT Authentication
* Role-based Authorization
* Swagger Documentation
* Dockerized Runtime
* Structured Logging
* Global Exception Handling
* Health Checks

## 🔹 Intelligent Features

* Trust Score Engine
* Carbon Savings Metrics
* Memory Parking Recall
* Silent Demand Heatmap

---

# 🏗️ System Architecture

```text
                    ┌──────────────────────┐
                    │   Angular Frontend   │
                    └──────────┬───────────┘
                               │
                         API Requests
                               │
     ┌─────────────────────────────────────────────────┐
     │                ParkEase Backend                │
     └─────────────────────────────────────────────────┘
         │        │        │        │        │
         ▼        ▼        ▼        ▼        ▼
      Auth   ParkingLot   Spot   Booking   Payment
         │        │        │        │        │
         ▼        ▼        ▼        ▼        ▼
      Vehicle  Notification  Analytics  Background Jobs
```

---

# 🧩 Microservices / Branch Strategy

Each microservice is also managed as a dedicated feature branch during development.

```text
main
└── dev
    ├── auth-service
    ├── parkinglot-service
    ├── spot-service
    ├── booking-service
    ├── payment-service
    ├── vehicle-service
    ├── notification-service
    ├── analytics-service
    └── parkease-web
```

## Workflow

1. Create service branch from `dev`
2. Build feature completely
3. Test locally
4. Merge into `dev`
5. Final merge `dev → main`

---

# ⚙️ Services Breakdown

| Service                | Responsibility                                    |
| ---------------------- | ------------------------------------------------- |
| `auth-service`         | Register, Login, JWT, Roles, Profile Management   |
| `parkinglot-service`   | Lot CRUD, Approval, Search, Availability Metadata |
| `spot-service`         | Spot CRUD, Types, Status, EV / Reserved Flags     |
| `booking-service`      | Reserve, Cancel, Extend, Check-In, Check-Out      |
| `payment-service`      | Payments, Refunds, Receipts, Revenue Sync         |
| `vehicle-service`      | User Vehicle Management                           |
| `notification-service` | Email, SMS, In-App Alerts                         |
| `analytics-service`    | Occupancy, Revenue, Trends, Reports               |
| `parkease-web`         | Frontend Integration Layer                        |

---

# 🛠️ Tech Stack

## Backend

* .NET 8
* ASP.NET Core Web API
* Entity Framework Core
* Repository Pattern
* Clean Layered Architecture

## Security

* ASP.NET Core Identity
* JWT Bearer Tokens
* Role Claims
* Protected Endpoints

## Database

* PostgreSQL
* Per-Service Database Strategy
* Code-First Migrations

## DevOps

* Docker
* Docker Compose
* Render Deployment Ready

## Observability

* Serilog Logging
* Health Endpoints
* Exception Middleware

## Docs / Testing

* Swagger / OpenAPI
* Manual API Validation

---

# 📁 Project Structure

```text
ParkEase-Backend/
│── docker-compose.yml
│── init-db.sql
│── README.md
│
├── AuthService/
│   ├── Controllers/
│   ├── Services/
│   ├── Repository/
│   ├── Models/
│   ├── Data/
│   ├── Middleware/
│   └── Program.cs
│
├── ParkingLotService/
├── SpotService/
├── BookingService/
├── PaymentService/
├── VehicleService/
├── NotificationService/
├── AnalyticsService/
└── ParkEase.Web/
```

---

# 🔐 Authentication Flow

```text
Register → Login → Receive JWT → Access Protected APIs → Refresh Token → Logout
```

## Roles

* DRIVER
* MANAGER
* ADMIN

---

# 🚘 Main Business Flows

## Driver Flow

```text
Search Lot → View Spots → Book Spot → Check-In → Pay → Check-Out
```

## Manager Flow

```text
Create Lot → Add Spots → View Bookings → Track Revenue
```

## Admin Flow

```text
Approve Lots → Manage Users → Platform Analytics → Reports
```

---

# 🌟 Unique Features

## Trust Score Engine

Behavior-based scoring using cancellations, no-shows, overstays, timely exits, payment discipline.

## Carbon Savings Dashboard

Estimate emissions reduced by reducing parking search time.

## Memory Parking Recall

Remember where the user parked: lot, floor, section, spot.

## Silent Demand Heatmap

Analyze failed searches and full-lot demand patterns.

---

# 🚀 Quick Start

## 1. Clone Repo

```bash
git clone <repo-url>
cd ParkEase-Backend
```

## 2. Start Containers

```bash
docker compose up -d --build
```

## 3. Verify Running Containers

```bash
docker ps
```

## 4. Open Swagger

```text
http://localhost:5001/swagger
```

---

# 🧪 API Testing

Use Swagger UI to test:

## Auth APIs

* Register
* Login
* Profile
* Change Password
* Validate Token

## Booking APIs

* Create Booking
* Cancel Booking
* Extend Booking
* Checkout

## Manager APIs

* Create Lot
* Add Spots
* Revenue Reports

## Admin APIs

* Users Management
* Lot Approval
* Platform Analytics

---

# 📈 Non-Functional Design Goals

* Scalable independent services
* Secure token-based auth
* Fast search responses
* Prevent double booking
* Fault isolation between services
* Clean maintainable codebase

---

# ☁️ Deployment

## Local

Docker Compose

## Cloud Ready

* Render
* Railway
* Azure
* AWS

---

# 🧭 Future Enhancements

* Redis Caching
* RabbitMQ Event Bus
* SignalR Real-Time Updates
* CI/CD Pipelines
* xUnit Test Coverage
* Rate Limiting
* OpenTelemetry Monitoring

---

# 👩‍💻 Developer Notes

This project was built as a serious backend system to demonstrate:

* System Design
* Distributed Architecture
* API Engineering
* Security
* Scalability Thinking
* Product Thinking

---

# 📚 References

## Standards & Practices Referenced

* REST API Design Principles
* OpenAPI / Swagger Specification
* OAuth 2.0 / JWT Authentication Patterns
* Microservices Domain Separation Principles
* Twelve-Factor App Concepts
* Clean Architecture / Layered Architecture
* Docker Containerization Practices
* CI/CD Automation Standards
* Structured Logging & Observability Patterns

## Project Source Reference

This README was enriched using the ParkEase internal case study document covering requirements, actors, use cases, class diagrams, architecture, non-functional requirements, technology stack, and glossary. fileciteturn12file0

---

# 📖 Glossary

| Term           | Meaning                                         |
| -------------- | ----------------------------------------------- |
| Driver         | End user who books parking spots                |
| Manager        | Operator who manages parking lots and spots     |
| Admin          | Platform authority managing users and approvals |
| Parking Lot    | Physical parking facility onboarded to ParkEase |
| Spot           | Individual parking unit inside a lot            |
| Booking        | Reservation tied to user, spot, and time window |
| Pre-Booking    | Advance reservation before arrival              |
| Walk-In        | Immediate booking created on arrival            |
| Check-In       | User arrival confirmation                       |
| Check-Out      | Exit confirmation with fare settlement          |
| Occupancy Rate | Occupied spots ÷ total spots                    |
| Peak Hours     | Highest demand time periods                     |
| Grace Period   | Allowed delay before auto-cancellation          |
| JWT            | Token used for stateless authentication         |
| POCO           | Plain C# domain model class                     |
| EF Core        | Entity Framework Core ORM                       |
| IHostedService | .NET background task interface                  |
| SignalR        | Real-time communication library                 |
| Haversine      | Distance formula for geo search                 |

---

# 🎯 Goals & Objectives

## Business Goals

* Reduce time spent searching for parking
* Improve parking lot utilization
* Create revenue channels for lot owners
* Provide transparent digital payments
* Build scalable smart-city infrastructure

## Engineering Goals

* Independent service deployment
* Clean maintainable codebase
* Secure role-based access
* High concurrency booking safety
* Fast geo-search responses
* Reliable analytics and reporting
* Production-ready DevOps pipeline

## Portfolio Goals

* Demonstrate enterprise backend capability
* Showcase microservices architecture skill
* Prove system design competence
* Display real-world product thinking

---

# 📌 Assumptions

* Each service can own its database.
* Inter-service communication may be sync or async.
* Payment gateway integration is abstracted.
* GPS coordinates are available for nearby search.
* Managers only manage authorized lots.
* Users authenticate before protected actions.

---

# ⚠️ Risks & Engineering Challenges

* Double booking under concurrency
* Cross-service transaction consistency
* Notification delivery failures
* Payment reconciliation mismatches
* Search latency at scale
* Data drift across services
* Monitoring distributed failures

## Mitigations

* Optimistic concurrency / row versioning
* Idempotent APIs
* Retry policies
* Structured logs + tracing
* Cache hot reads
* Background recovery jobs

---

# 📑 Bibliography

1. Microsoft. *ASP.NET Core Documentation*.
2. Microsoft. *Entity Framework Core Documentation*.
3. PostgreSQL Global Development Group. *PostgreSQL Documentation*.
4. Docker Inc. *Docker & Docker Compose Documentation*.
5. OpenAPI Initiative. *OpenAPI Specification*.
6. Swagger. *Swagger UI Documentation*.
7. OWASP Foundation. *API Security Top 10*.
8. Martin Fowler. *Microservices Architecture Patterns*.
9. Sam Newman. *Building Microservices*.
10. ParkEase Internal Case Study Document (2026). fileciteturn12file0

---

# 📜 License

MIT License

---

# ⭐ Final Statement

**ParkEase is not just parking software. It is a scalable mobility infrastructure backend.**
