# BillZip

You must have postresql installed with a user as follows on localhost:
jebel
password

To Add Migrations, navigate to the project directory where the models reside and run: 
_dotnet ef migrations add InitialMigration -s ../BillZip --context IdentityContext_
...where "IdentityContext" is replaced by the context you want to run operations on.

To run migrations, navigate to the project directory where models reside and run:
_dotnet ef database update -s ../BillZip --context IdentityContext_
...where "IdentityContext" is replaced by the context you want to run operations on.

Swagger UI is enabled in Debug/dev...  Access this as follows:
http://localhost:34791/swagger/
...or the raw json:
http://localhost:34791/swagger/v1/swagger.json


__To run this app for the first time:__
1. Download .net core 2.0 for your operating system
2. Navigate to the project directory and type:
	_dotnet restore_
	_dotnet build_
3. cd to Identity directory and run:
	_dotnet ef database update -s ../BillZip --context IdentityContext_
4. cd to Building_Management directory and run:
	_dotnet ef database update -s ../BillZip --context BuildingManagementContext_
5. cd to BillZip directory and run:
	_dotnet run_

That's it!