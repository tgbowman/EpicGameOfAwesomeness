# EpicGameOfAwesomeness API

### About the API:
This Web API was built using C# .NET and SQLite.  When you launch the API for the first time it runs the DBInitializer file and seeds the database with the story data for "A Strange Request."  The Database is a SQLite database.  The ERD for the database is linked below.

### Installing the API:
1. Clone the repo onto your machine.
2. From the project directory type ```dotnet ef database update``` into your terminal.
3. From the project directory type ```dotnet run``` into your terminal.
4. This will start running the API on http://localhost:5000

### API XHR Request Syntax:
1. http://localhost:5000/api/{DatabaseTableName} .    
*** More syntax examples for specific request can be seen on that tables Controller within the source code.***
