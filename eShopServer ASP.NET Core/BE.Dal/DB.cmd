dotnet ef database drop --force
dotnet ef migrations add InitialCreate73
dotnet ef database update
dotnet sql-cache create "Server=.; Database=BED; MultipleActiveResultSets=true; integrated security=true" "dbo" "SessionData"