# AcxiomCRM security patch — apply to your existing project

This patch preserves the existing Razor UI and database structure. It contains targeted security and workflow corrections based on the uploaded source.

## Changed files
- Controllers/LeadsController.cs — checks lead ownership before conversion.
- Controllers/OpportunitiesController.cs — restricts customer selection to records the current user can access; validates probability and customer ownership on create.
- Controllers/FollowUpsController.cs — validates related Customer/Lead existence and ownership before creating follow-ups.
- Controllers/Api/CrmApiController.cs — checks customer ownership when creating opportunities; validates related Customer/Lead ownership for API follow-ups; writes audit events for API create operations.
- Views/Leads/Index.cshtml — keeps the current lead status selected in the workflow dropdown.
- Views/Opportunities/Index.cshtml — keeps the current opportunity stage selected.
- Views/FollowUps/Index.cshtml — keeps the current follow-up status selected.

## Apply
1. Stop the app with Ctrl+C.
2. Back up your current folder and `acxiomcrm.db`.
3. Extract this ZIP.
4. Copy the contents of `AcxiomCRM_ASPNETCore` over your existing project folder, allowing these listed files to be replaced.
5. Open a new terminal in the folder containing `AcxiomCRM.csproj`.
6. Set your .NET environment if needed:
   ```powershell
   $env:DOTNET_ROOT = "F:\dotnet"
   $env:PATH = "F:\dotnet;$env:PATH"
   $env:TEMP = "F:\Temp"
   $env:TMP = "F:\Temp"
   $env:NUGET_PACKAGES = "F:\NuGet"
   $env:DOTNET_CLI_HOME = "F:\dotnet-home"
   ```
7. Run:
   ```powershell
   dotnet build
   ```
8. Only if the build succeeds, run:
   ```powershell
   dotnet run
   ```

## Tests after applying
- Log in as Sales and attempt to convert a lead owned by Admin. It should return Forbidden.
- Log in as Sales and attempt to create an opportunity linked to an Admin-owned customer. It should be rejected.
- Try creating a follow-up linked to another user's customer/lead. It should be rejected.
- Change a Lead, Opportunity and Follow-up status, submit, and confirm the dropdown still shows the saved status.
- Create a normal Sales-owned customer, lead, opportunity and follow-up; all should still work.
- Check Audit Trail for API create events after testing API create routes.

## Remaining work — NOT included in this patch
This is not a claim of full assignment completion. The uploaded project still needs:
- Lead edit/delete screens and actions.
- Opportunity edit/delete screens and actions.
- Customer details/history and related activity timeline.
- Full activity module (calls, meetings, emails, tasks).
- Admin user creation, role reassignment and secure password reset.
- Chart.js dashboard charts and any missing KPIs required by the assignment.
- Additional report types, filters, pagination and export formats.
- API PUT/PATCH/DELETE operations and more complete API tests.
- Stronger audit coverage for all updates/deletes/security events and production retention controls.
- Automated tests and full browser acceptance testing.
