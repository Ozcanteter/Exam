# Employee Wellness Program - Architecture

## Overview
.NET 8 Web API built on ABP Framework for managing company wellness challenges, progress tracking, and leaderboards. Designed for high concurrency and scalability.

## Technology Stack

| Component | Technology | Version |
|-----------|-----------|---------|
| Framework | ABP Framework | 8.0.1 |
| Runtime | .NET | 8.0 |
| ORM | Entity Framework Core | 8.0 |
| Database | SQL Server | 2019+ |
| Cache | Redis | 7.0+ |
| Message Queue | RabbitMQ | 3.10+ |
| Logging | Serilog (ABP) | 2.0+ |
---

## API Endpoints
- `POST /api/challenges/create` - Create challenge
- `POST /api/challenges/{id}/progress` - Submit progress (async)
- `GET /api/challenges/{id}/leaderboard` - Get top 10 (cached)
- `GET /api/users/{id}/active` - Get user's active challenges

---

## Architecture

### Layered Design
```
Controllers (ChallengePublicController)
    ↓
App Services (ChallengePublicAppService)
    ↓
Domain Layer (Managers, Repositories, Entities)
    ↓
Infrastructure (EF Core, Repositories)
```

### Domain Entities
- **Challenge** - Aggregate root (Name, StartDate, EndDate, Goal, IsActive)
- **ProgressEntry** - High-volume writes (ChallengeId, UserId, Value)
- **ChallengeUserTotal** - Denormalized aggregates for fast reads
- **Participant** - User-challenge enrollment (tracks active status)

---

## High-Volume Processing

### Async Progress Flow
Progress submissions don't block on DB writes:

1. Client submits progress → API
2. API publishes event to RabbitMQ via outbox pattern (transactional)
3. API returns 200 immediately (~50ms)
4. Background worker consumes from queue asynchronously
5. Worker updates DB + Redis cache in batch

**ProgressEntryConsumerWorker** flow:
- Receives `ProgressEntryCreateEto` from queue
- Auto-enrolls user in challenge (Participant)
- Creates ProgressEntry record
- Updates ChallengeUserTotal (increment or create)
- Updates Redis leaderboard cache

**Benefits:** No DB bottleneck, fast API response 

---

## Performance & Caching

### Redis Cache (Leaderboard)
- Stores dictionary: `Dictionary<UserId, TotalScore>` per challenge
- Background worker updates after each batch

---

## Concurrency & Consistency

### Optimistic Concurrency Control
- All updates include `ConcurrencyStamp` with ABP
- EF Core detects conflicts, throws exception
- Prevents race conditions on Challenge updates


## API Examples

**Create Challenge:**
```json
POST /api/challenges/create
{
  "name": "Step Challenge 2024",
  "startDate": "2024-01-01T00:00:00Z",
  "endDate": "2024-03-31T23:59:59Z",
  "goal": 10000.0
}
```

**Submit Progress (async):**
```json
POST /api/challenges/{id}/progress
{
  "userId": "550e8400-e29b-41d4-a716-446655440000",
  "value": 8500.0
}
// Returns 200 immediately, processed in background
```

**Get Leaderboard:**
```json
GET /api/challenges/{id}/leaderboard
// Response (from Redis cache):
[
  { "placeIndex": 0, "userName": "john.doe", "totalProgress": 85000.0 },
  { "placeIndex": 1, "userName": "jane.smith", "totalProgress": 78000.0 }
]
```

---

## Key Components

**ChallengePublicAppService** - Public API logic
- CreateAsync, GetActiveChallengesAsync, GetLeaderboardAsync, ProgressEntryCreateAsync

**ProgressEntryConsumerWorker** - Background service
- RabbitMQ consumer (IHostedService)
- Processes events, updates DB + cache
- Error handling with BasicNack + requeue

**Database Tables**
- AppChallenges, AppProgressEntries, AppChallengeUserTotals, AppParticipants

  -----
 <img width="709" height="836" alt="diagfl" src="https://github.com/user-attachments/assets/2d497e33-dbe5-4c95-8cd6-474dd745fe84" />

