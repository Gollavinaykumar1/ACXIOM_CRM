# AcxiomCRM — ASP.NET Core MVC implementation

Separate ASP.NET Core MVC implementation intended to meet the technology requirements in the supplied AcxiomCRM assignment. It does not overwrite the original Node.js project. The UI retains the existing visual direction: navy sidebar, teal accents, white cards and light workspace.

## Required local software
- .NET 8 SDK (8.0.x)
- Visual Studio 2022 or VS Code with C# Dev Kit
- Internet access on first run to restore NuGet packages

**Testing disclosure:** The build environment used to prepare this archive did not have the .NET SDK installed. This source has therefore not been compiled or browser-tested here. Run the commands below locally before treating it as submission-ready.

## Run in VS Code (Windows)
1. Extract the ZIP and open the folder containing `AcxiomCRM.csproj` in VS Code.
2. Open Terminal → New Terminal.
3. Run `dotnet --version` and confirm it reports `8.0.x`.
4. Run `dotnet restore`.
5. Run `dotnet build`.
6. Run `dotnet run`.
7. Open the URL printed in the terminal.

The app creates a local SQLite database named `acxiomcrm.db` on first launch and seeds demo accounts.

## Demo accounts
| Role | Email | Password |
|---|---|---|
| Admin | admin@acxiomcrm.demo | Admin@123 |
| Manager | manager@acxiomcrm.demo | Manager@123 |
| Sales Executive | sales@acxiomcrm.demo | Sales@123 |

New self-registered accounts receive the Sales Executive role and are not automatically signed in; registration redirects to Login. Password fields have show/hide controls.

## Included implementation
- ASP.NET Core MVC / Razor views
- ASP.NET Core Identity with EF Core SQLite persistence, unique email, password policy, lockout and secure cookies
- Seeded Admin, Manager and Sales Executive roles
- Customer create/list/edit, scoped visibility, search, validation and audit events
- Lead create/list, status transitions and qualified-lead conversion to customer
- Opportunity create/list, stage changes, amount/probability/date validation and weighted value
- Follow-up create/list/status changes and date validation
- Admin user activation/deactivation page
- Audit log view and event writes for authentication and core CRM mutations
- Role-scoped dashboard KPIs and pipeline report with customer CSV export
- Authenticated REST endpoints for login/logout, customers, leads, opportunities, follow-ups and pipeline data
- DTO-based API input/output, model validation, role restrictions and authentication rate limiter
- Anti-forgery validation on MVC state-changing forms
- Responsive layout, inline validation, toast notifications and password visibility

## Known gaps — do not claim these are complete
- Manager team scoping currently uses manager-wide visibility; no team/department entity or assignment workflow.
- Admin user creation, editing, primary-role reassignment and secure admin password-reset workflow are not implemented; current admin page supports activation/deactivation.
- Lead edit/delete, opportunity edit/delete, customer history timeline and full related-activity timeline need extension.
- Reports currently provide pipeline summary and customer CSV export; the full set of separate PDF reports and their filters/pagination/export formats remains to implement.
- API login/logout use Identity cookies; bearer/JWT authentication is not configured.
- Audit events are append-oriented in the UI, but database-level immutability/retention controls are not configured.
- Full automated unit/integration tests, security testing and browser acceptance tests remain to be run.

## Before submission — required local verification
Run `dotnet restore` and `dotnet build`. Then test: demo and invalid login; logout and lockout; registration followed by separate login; role access by manually entering `/Users` and `/Audit` as Sales Executive; customer ownership; qualified lead conversion; opportunity amount/probability/date validation; follow-up date/status validation; API status codes and DTO fields; role-scoped dashboard and report data; desktop/mobile layouts.

SQLite database is created in the project working directory as `acxiomcrm.db`. Back it up before resetting. Do not use demo passwords in production.
