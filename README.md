# CSW Microservices

This project implements a microservices architecture for managing students, disciplines, and enrollments using C# and .NET.

## Services

- **StudentService** (Port 5001): Manages student registration and queries.
- **DisciplineService** (Port 5002): Manages discipline registration.
- **EnrollmentService** (Port 5003): Handles student enrollments in disciplines.

## Running Locally

1. Ensure .NET 8+ is installed.
2. Navigate to each service directory and run `dotnet run`.
3. Services will run on ports 5001, 5002, 5003.

## Running with Docker

1. Ensure Docker and Docker Compose are installed.
2. Run `docker-compose up --build` from the root directory.

## API Endpoints

### StudentService
- `POST /students` - Register a student (body: { "name": "string", "registrationNumber": "string" })
- `GET /students/{registrationNumber}` - Get student by registration number
- `GET /students/search/{namePart}` - Search students by name part

### DisciplineService
- `POST /disciplines` - Register a discipline (body: { "code": "string", "name": "string", "schedule": "char" })
- `GET /disciplines/{code}/{schedule}` - Get discipline by code and schedule
- `GET /disciplines` - Get all disciplines

### EnrollmentService
- `POST /enrollments` - Enroll student (body: { "studentRegistrationNumber": "string", "disciplineCode": "string", "schedule": "char" })

## Architecture

Each service has its own database (SQLite) and communicates via HTTP APIs.