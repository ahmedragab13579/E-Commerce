# EcommerceStoreApi

Comprehensive, well-organized README describing project features, architecture, API surface, DTOs, error codes, testing and operational notes.

---

## Project overview

EcommerceStoreApi is a .NET 8 REST API that implements the backend for an e-commerce store.  
The project is organized by feature (Controllers, Services, Domain, Infrastructure) and follows a layered approach: Controllers → Application Services → Repositories → Domain models. It uses a Result<T> wrapper for service responses and a centralized mapper to convert those into HTTP responses.

Key technical highlights:
- .NET 8, C# 12
- Layered architecture with DI for services
- JWT-based authentication (tokens used in integration tests)
- Audit logging via a SaveChanges interceptor
- Event publishing for order workflows (integration tests show a message consumer)
- Action result mapping: service Result<T> → IActionResult via ActionResultStatus helper

---

## Main features

- User registration, login and profile management
- Product browsing and product management endpoints
- Cart management (add/remove items)
- Checkout / Orders workflow with event publishing
- Payment endpoints (payment processing orchestration)
- Reviews: create, update, delete with business rules (duplicate review prevention, authorization)
- Audit logging of data changes (who changed what and when)
- Centralized service results and error codes to standardize API responses
- Integration tests covering scenarios (login → add to cart → checkout → event publishing)

---

## Architecture & folders (high level)

- Controllers: API surface (examples under MainControllers/CustomerControllers)
- Application: DTOs, service interfaces, Result<T> wrapper
- E_Infrastructure: implementations, EF Core data layer, interceptors (e.g., AuditInterceptor)
- Domain (E_Domain): domain models and business logic entities
- Tests: integration tests (use WebApplicationFactory, test consumers for message bus)

---

## Important shared types

Result<T>
- Uniform service response wrapper:
  - Success (bool)
  - Data (T)
  - Message (string)
  - Code (string)
- Factory helpers: Result<T>.Ok(...) and Result<T>.Fail(code, message)

AddUserDto (used by registration)
- FirstName (required, max 100)
- LastName (required, max 100)
- UserName (required, max 100)
- Email (required, max 200, [EmailAddress])
- Phone (optional, regex validated)
- Password
- FullName (NotMapped)

IUserService (selected surface)
- Add, Login, ChangePassword, ForgotPassword, GetById, Update, GetAll, BlockUser, ActivateUser, SoftDeleteUser, RestoreUser, etc.

Current user helper
- Service to retrieve current user id used by services (e.g., review authorization)

---

## API surface (controllers & important endpoints)

Routes use the pattern: `api/shop/[controller]` (controller name is lower-cased in routes by convention in many examples).

RegistrationController
- POST /api/shop/Registration/Registration
  - Body: AddUserDto
  - Success: Result<int> with created user id
  - Errors: duplicate email, validation errors, Result codes from service

LoginController (auth)
- POST /api/shop/login
  - Body: LoginDto (username/email + password)
  - Success: token (used by tests)
  - Protected endpoints require Bearer token in Authorization header

CustomersController
- GET /api/shop/Customers/{id}
- GET /api/shop/Customers
- PUT /api/shop/Customers/{id}
- Actions follow Result<T> response pattern and enforce authorization for protected actions.

ProductsController
- GET /api/shop/Products (list)
- GET /api/shop/Products/{id}
- Query/filtering endpoints expected (pagination/sorting likely available in service layer)

Cart endpoints (inferred from tests)
- POST /api/shop/Cart/AddItem
  - Body: AddCartItemDto (ProductId, Quantity)
  - Requires Authorization (Bearer token used in tests)

OrderController
- POST /api/shop/order/CheckOUt (example path from tests)
  - Protected: Requires authenticated user
  - Publishes an order-placed event (RabbitMQ or other bus) upon successful checkout

PaymentController
- Payment-related orchestration (create payment intent, confirm, webhook handling)

ReviewController
- POST /api/shop/Review (add a review)
  - Business rules: duplicate review prevention → returns code "DUPLICATE_REVIEW"
- PUT /api/shop/Review/{id} (update)
  - Authorization: only reviewer can update → returns "FORBIDDEN" if not allowed
- DELETE /api/shop/Review/{id}
  - Authorization: only reviewer can delete
- Common error codes used by ReviewService: UNAUTHORIZED, NOT_FOUND, FORBIDDEN, SAVE_FAILED, DELETE_FAILED, SERVER_ERROR

Action result mapping
- Controllers call services, receive Result<T>, then `ActionResultStatus.MapResult(Result)` converts to proper IActionResult and HTTP status codes (200, 400, 401, 403, 404, 500 etc.) with code/message in body.

---

## Error codes and conventions

Services return standardized codes via Result<T>.Fail(code, message). Examples seen in the codebase:
- DUPLICATE_REVIEW
- SAVE_FAILED
- DELETE_FAILED
- NOT_FOUND
- UNAUTHORIZED
- FORBIDDEN
- SERVER_ERROR
- INVALID_DATA
- BUSINESS_RULE_VIOLATION

Include code and message in the API response to make it machine-readable and easy for clients to handle.

---

## Audit logging

AuditInterceptor (E_Infrastructure.Data.AuditInterceptor)
- Intercepts EF Core SaveChanges
- Records audit entries for Added/Modified/Deleted entities (skips AuditLog entity itself)
- Captures TableName, OperationType, ChangeDate, UserId, RecordId
- For newly added entities record IDs are handled after save and linked with a temporary in-memory map
- Uses ICurrentUserService to associate changes with a user id (or 0 for anonymous/seeded operations)

---

## Events & asynchronous flows

- Checkout publishes an order-placed event. An integration test uses a test consumer (TestOrderPlacedConsumer) to assert that an event is published.
- Tests show use of a message consumer and waiting for messages with a timeout; this indicates the project integrates with a message broker (commonly RabbitMQ or similar).

---

## Testing

- Integration tests use CustomWebApplicationFactory and HttpClient to exercise routes.
- Example scenario: login → set Authorization header → add to cart → checkout → assert published event using a test consumer.
- To run tests locally: `dotnet test` (run from solution or test project). Tests expect a running test double/consumer for messaging or in-memory test harness.

---

## Configuration & running locally

Typical required configuration (set as environment variables or in appsettings):
- Database connection: e.g., `ConnectionStrings:DefaultConnection`
- JWT settings: e.g., secret key, issuer, expiry (the tests use tokens so JWT configuration is required)
- Message broker: host, username, password (for order events)
- Any third-party keys (payment provider) if PaymentController integrates with external payment gateway

Run locally:
- Build: `dotnet build`
- Run: `dotnet run --project EcommerceStoreApi` (or start from Visual Studio)
- Tests: `dotnet test`

If using Visual Studio, run the API with the debugger or __F5__ (or use __Debug > Start Debugging__).

---

## Contributing

- Follow existing coding style and layering.
- Use Result<T> for service responses and return consistent error codes.
- Add unit/integration tests for new features and business rules.
- Keep controllers thin: business rules belong to services.

---

## Notes & pointers

- Endpoints return a uniform Result<T> payload. Clients should check the `Success` flag, `Code`, and `Message`.
- Authenticated endpoints rely on getting a Bearer token from login.
- Audit logs provide traceability for DB changes; ensure sensitive data is not logged.
- ReviewService enforces ownership checks; follow the same pattern for other resources with per-user ownership.

---

## Contact / Maintainers

For questions about implementation details, tests or environment setup, inspect:
- Main controllers under `MainControllers/CustomerControllers`
- Application services interfaces under `Application\Services\InterFaces`
- Infrastructure implementations under `E_Infrastructure`

---

License: Project-specific (check repository root for LICENSE file).
