You must have postresql installed with a user as follows on localhost:
jebel
password

To Add Migrations, navigate to the project directory where the models reside and run: 
dotnet ef migrations add InitialMigration -s ../BillZip

TO run migrations, navigate to the project directory where models reside and run:
dotnet ef database update -s ../BillZip

Swagger UI is enabled in Debug/dev...  Access this as follows:
http://localhost:34791/swagger/
...or the raw json:
http://localhost:34791/swagger/v1/swagger.json