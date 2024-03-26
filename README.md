# FitnessAppV2
C# and .NET - ASP.NET Core Web App (Model-View-Controller)

"applicationUrl": "http://localhost:5088",
-connection is not secured (no https:// )

V2 updates:
-better models structure

Dependencies:
-Entity - keeps database infos in sync with the app
NuGet packages:
-Microsoft.EntityFrameworkCore v8.0.3
Entity Framework Core is a modern object-database mapper for .NET. It supports LINQ queries, change tracking, updates, and schema migrations. EF Core works with SQL Server, Azure SQL Database, SQLite, Azure Cosmos DB, MySQL, PostgreSQL, and other databases through a provider plugin API.
-Microsoft.EntityFrameworkCore.SqlServer v8.0.3
Microsoft SQL Server database provider for Entity Framework Core.
-Microsoft.EntityFrameworkCore.Tools v8.0.3  <<<--- used for Code First approach
Entity Framework Core Tools for the NuGet Package Manager Console in Visual Studio.
-Bootstrap v5.3.3
The most popular front-end framework for developing responsive, mobile first projects on the web.

Persistent storage:
-SQL Server 2022 - locahost machine - db (will be using the db named 'FitnessData3' with x tables)
-SSMS v 19.3.4.0 - db management tool

Connection string:
-establish a connection to SQL Server db

FE:
-Bootstrap
-JavaScript

Tested on browser:
-Chrome Version 122.0.6261.131

Work steps: for Code First approach
1) create empty project
2) create model classes in folder Models->DbEntities (used to save infos in db)
3) create application DbContext class in folder Data (added 4 props reprezenting the 4 tables in db)
4) create connection string with its authentication details in file appsettings.json
5) add Dependency Injection in Program.cs file to be able to use the connection to SQL Server at db 'FitnessData3'
6) in PackageManagerConsole run commands 'Add-Migration <name>' and 'Update-Database' to create all needed tables in db based on model classes from folder Models->DbEntities
7) refresh SQL Server and check the db and tables are created (check also all PK and FK are created correctly)
8) create Controllers - add at the beginning initialization of DbContext so we can have access to db data
9) create ViewModels
10) create Views
11) add all Business Logic requirements to Models classes
12) for saving data to db use Server-Side Validation with Model.IsValid (validation done on the server is called server-side validation) 
13) for entering data by user in boxex of webpage use Client-Side Validation with form validation (Validation done in the browser is called client-side validation)

