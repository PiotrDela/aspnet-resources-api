Solution contains implementation of API and persistence layer using .NET 6.

Few assumptions:
- Used Dapper as a simple ORM
- I put everything under the same project, grouped apprioprate types/services into Folders instead for the simplicity
- database.sql script contains sql commands needed to initialize an empty database
- I'm using SQL Database hosted in Azure on my private, free subscription
- ResourcesManagement.http file contains HTTP requests defined as code so it's easier to build and send requests (I'm using VS Code Rest Client extension for that purpose)

Suggested further improvements:
- Connection string to DB should be securely stored outside of repository, same with the JWT signing key, however I left it directly in the source code just for the simplicity
- When storing passwords, keep it as hash(plain text + salt). Salt should be unique value for every User entry to increase security
- Write unit tests for the Command Handlers
- Write integration tests for ResourcesController, one that will initialize and run the host "in memory"
- Write integration tests for types/services connecting to database, possibly could utilize in memory database in this case
- Consider introducing retries for DbConcurrencyException to decrease number of of 409s returned to the client
