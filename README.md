# AgentDesk Web Service

ASP.NET Web API for AgentDesk, an insurance agency management system. It holds the business logic and is the only layer that talks to the SQL Server database. The **AgentDesk Web Console** (MVC site) calls it over HTTP.

## Features

- JWT authentication (login returns a token)
- REST endpoints for Company, User, Agency, Group, Client, Address, Policy, Policy Nominee and Policy Premium
- Policy import: creates or reuses group, client and address, then saves the policy and nominee
- FUP update: verifies the policy number, saves the premium, marks it adjusted and moves the policy FUP date to the adjustment date
- Maturity date calculation (commencement date plus term in years)
- Paged list queries with search criteria

## Tech stack

| Area | Technology |
|---|---|
| Framework | ASP.NET Web API 2 (.NET Framework 4.x) |
| Data access | Entity Framework (database first, `AgentDeskDBEntities`) |
| Database | SQL Server (`AgentDeskDB`) |
| Dependency injection | Unity |
| Auth | JWT bearer tokens |
| Serialization | System.Text.Json (mapping from EF results to domain classes) |

## Project structure

```
AgentDeskWebService/
├── api/                    API controllers (Policy, PolicyPremium, User, ...)
├── Domain/                 Domain classes (Policy, Client, Group, Address, PolicyNominee, PolicyPremium, User)
├── Infrastructure/
│   ├── IService/           Service interfaces
│   ├── IRepository/        Repository interfaces
│   ├── Service/            Business logic
│   └── Repository/         EF queries and saves
├── Persistence/            EF model (AgentDeskDBEntities)
├── Helper/                 StaticMethods, PasswordEncryptor, JWT service
└── Web.config
```

Flow: `Controller -> Service -> Repository -> Database`.

## Getting started

### Requirements

- Visual Studio 2019 or later
- .NET Framework 4.x
- SQL Server 2016 or later
- IIS 10 for deployment

### Database

1. Create the database (restore a backup or run your schema script).
2. Make sure the lookup rows exist, because the import depends on them:
   - `PolicyStatus` (the import uses `PolicyStatusId = 1`)
   - `PolicyTypes` (the import uses `PolicyTypeId = 1`)
   - Roles, at least one Company and one System Owner user
3. Required recent columns: `Clients.MiddleName`, `Policies.Plan`, `Policies.Term`, `Policies.PPT`, `CompanyId` on Policy, Client, Group, Address, User and Agency.

### Configuration (`Web.config`)

```xml
<connectionStrings>
  <add name="AgentDeskDBEntities" connectionString="...initial catalog=AgentDeskDB;..." providerName="System.Data.EntityClient" />
</connectionStrings>

<appSettings>
  <add key="Salt" value="YOUR_SALT" />
  <!-- JWT settings used by the token service -->
</appSettings>
```

- The `Salt` value is used to encrypt user passwords. A different salt means existing users cannot log in.
- Do not commit real connection strings, salts or JWT keys. Use config transforms or an ignored local config.

### Run

1. Open the `.sln`, restore NuGet packages and build.
2. Run with IIS Express (F5) or publish to IIS.
3. Register every repository and service interface in the Unity configuration. A missing registration returns a 500 with `Resolution failed ... No public constructor is available for type I...Repository`.

## Key business rules

### Policy import (`api/Policy/ImportPolicy`)

Order of work for each row:

1. **Group**: looked up by Family value (group name) within the company. Created if missing, using the client name as head name. No auto-generated code during import.
2. **Client**: split name into first, middle and last name (Pascal case, `/LA` removed). Matched by name plus phone or DOB within the company, and created if not found. An existing client keeps its group.
3. **Address**: only when the row has an address. Split into Line1, Line2, Area, City, ZipCode and linked to the client.
4. **Policy**: saved with the client id. `Plan/Term/PPT` is split (for example `178/99/20`) and the **maturity date** is calculated as commencement date plus term years. Terms of 99 or more are treated as whole life (no maturity date).
5. **Nominee**: saved when a nominee name is present.

All created rows get `CompanyId` and `CreatedBy` from the logged-in user, set by the console before the call.

### Group code generation

Groups created from the group screen with a blank code get an auto-generated code in the format `FirstName + LastName + 12 random characters`. Imported groups are not affected.

### FUP update (`PolicyPremium` upsert)

In one transaction:

1. Find the policy by policy number (unique).
2. Save the premium row (the same policy number and adjustment date updates the existing row).
3. Set `IsAdjusted = true`.
4. Set `Policy.FUPDate` to the adjustment date.

An unknown policy number returns `Policy number not found!` and saves nothing.

## Testing the API

```bash
curl -X POST "https://your-api-host/api/PolicyPremium/Upsert" \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer YOUR_TOKEN" \
  -d '{"PolicyNumber":"866222990","DueDate":"2026-10-09","AdjDate":"2026-11-09","Premium":445,"CreatedBy":1}'
```

Get the token from the login endpoint (`Authenticate`) first.

## Troubleshooting

| Problem | Check |
|---|---|
| 500 on a controller | Missing Unity registration, or a column in the EF model that is missing in the database |
| `CompanyId` is 0 | The domain class in this project has no `CompanyId` property, so it is dropped when the request is bound |
| Foreign key error on policy insert | `PolicyStatus` or `PolicyTypes` rows (id 1) do not exist |
| Login returns `RoleId` / `CompanyId` as 0 | The user query projection (`GetBy`) must include both fields |

## License

Add your license here.
