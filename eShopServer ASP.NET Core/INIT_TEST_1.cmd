cd BE.Dal
dotnet ef migrations add InitialCreate
dotnet ef database update
dotnet sql-cache create "Server=.; Database=BED; MultipleActiveResultSets=true; integrated security=true" "dbo" "SessionData
cd..
cd BE.Service
dotnet watch run
