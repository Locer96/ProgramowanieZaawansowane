# InventoryApp

## Technologies used
- .NET 8.0
- ASP.NET Core MVC
- Entity Framework Core
- Github Actions

## First Launch
### Local Development
1. Clone the repository to your local machine.
2. Ensure you have the necessary environment variables set up, including `AZURE_SQL_CONNECTIONSTRING`.
3. Run the application using your preferred IDE or `dotnet run` command.
4. On the first launch, the application will automatically apply any pending migrations to the database.
5. Multiple accounts with roles will be created on start of the application:
- Admin (Sufixes from 1 to 3)
   - **Email**: admin1@admin.com
   - **Password**: Admin@123
- Support (Sufixes from 1 to 3)
   - **Email**: support1@admin.com
   - **Password**: Support@123
- User (Sufixes from 1 to 25)
   - **Email**: user1@admin.com
   - **Password**: User@123
6. Use the admin account to log in and access administrator-only features.
7. Use the support account to log in and access support-only features.
8. Use the user account to log in and access authentication-only features.
9. Register other account to test registration

## Project Structure
This project implements MVC (Model-View-Controller) using ASP.NET Core:
- **Models** are defined in the *Models* directory.
- **Views** are defined in the *Views* directory.
- **Controllers** are defined in the *Controllers* directory.
- **Migrations** for Database are defined in *Data/Migrations* directory.

Additionally, the project uses ASP.NET Core Identity for authentication and authorization.

## Object-Relational Mapping
The project extensively uses Object-Relational Mapping (ORM) with Entity Framework Core, following the Code First approach as visible in `ApplicationDbContext.cs`

## Field Restrictions

### AspNetUsers Fields
- **Email**: Must be a valid email address.
- **Password**: Must be at least 6 characters long and max 100 characters long, have at least one non alphanumeric character, have at least one lowercase ('a'-'z') and have at least one uppercase ('A'-'Z').

### WorkStation Fields
- **Id**: Auto-incremented integer.
- **WorkStationNumber**: Required unique string, maximum length 20 characters.
- **UserEmail**: Selectable from the list of exisiting users. Possible to set None to remove assigned user.
- **PCSerialNumber**: Optional field with maximum length of 100 characters.
- **PC**: Selectable from the list of exisiting DeviceModels of PC type. Possible to set None to remove assigned PC.
- **Display**: Selectable from the list of exisiting DeviceModels of Display type. Possible to set None to remove assigned Display.
- **Keyboard**: Optional boolean field.
- **Mouse**: Optional boolean field.
- **AdditionalInfo**: Optional field with maximum length of 400 characters, only displayed in Details.

### DeviceModel Fields
- **Id**: Auto-incremented integer.
- **Name**: Required string, maximum length 100 characters.
- **Type**: Required string, maximum length 50 characters. PC or Display. Left as a string for possible future expansion of more types

Field restrictions are also visible in UI

### Database Connection Configuration
- The file `Program.cs` includes the configuration for connecting to a SQL Server database using `UseSqlServer`, indicating the use of Entity Framework Core for database interactions. The connection string is retrieved from the environment variable `AZURE_SQL_CONNECTIONSTRING`.
- `DatabaseInitializer.cs` contains generation of Dummy data for testing purposes

## User Roles and their accesses

- Views `Device Models` are accessible only to users with the "Administrator" role and those handle management tasks such as creating, editing, and deleting Device Models available to use in WorkStations. 
On those views all items can be sorted by any of the columns. Also you can see aggregation of number of models already assigned.
- Views `Work stations` are accessible only to users with the "Support" or "Administrator" role and those handle workstation management tasks such as creating, editing, and deleting and assigning users to workstations.
On those views all items can be sorted by any of the columns.
- View `My Workspace` accessible only to logged in user provide information about assigned workspace but it does not allow user to edit anything there. 