# ASP WebApi With Hangfire And Mysql Database

DotNet Core 8

## Install Nuget Packages

### Require

```console
nuget install Hangfire -Version 1.8.17
nuget install Hangfire.MySqlStorage -Version 2.0.3
nuget install Pomelo.EntityFrameworkCore.MySql -Version 8.0.2
nuget install Microsoft.EntityFrameworkCore -Version 8.0.11
nuget install Microsoft.EntityFrameworkCore.Design -Version 8.0.11
nuget install Microsoft.EntityFrameworkCore.Tools -Version 8.0.11
```

### Optional

```console
nuget install Microsoft.EntityFrameworkCore.SqlServer -Version 8.0.11
nuget install System.Data.SqlClient -Version 4.8.6
nuget install Microsoft.VisualStudio.Web.CodeGeneration.Design -Version 8.0.7
nuget install Swashbuckle.AspNetCore -Version 6.4.0
```

## CSharp Code In File

WebApiWithHangfire\WebApiWithHangfire.csproj

```csharp
  <ItemGroup>
    <PackageReference Include="Hangfire" Version="1.8.17" />
    <PackageReference Include="Hangfire.MySqlStorage" Version="2.0.3" />
    <PackageReference Include="Microsoft.EntityFrameworkCore" Version="8.0.11" />
    <PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="8.0.11">
      <PrivateAssets>all</PrivateAssets>
      <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
    </PackageReference>
    <PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="8.0.11" />
    <PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="8.0.11">
      <PrivateAssets>all</PrivateAssets>
      <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
    </PackageReference>
    <PackageReference Include="Microsoft.VisualStudio.Web.CodeGeneration.Design" Version="8.0.7" />
    <PackageReference Include="Pomelo.EntityFrameworkCore.MySql" Version="8.0.2" />
    <PackageReference Include="Swashbuckle.AspNetCore" Version="6.4.0" />
    <PackageReference Include="System.Data.SqlClient" Version="4.8.6" />
  </ItemGroup>
```

## Config AppSetting.json

* For SQL Server Complete DefaultConnection
* For MySQL Complete MysqlConnection

```json
  {
    "ConnectionStrings": {
        "DefaultConnection": "Server=(local); Database = HangfireDb; Trusted_Connection=true; Trust Server Certificate=true;",
        "MysqlConnection": "server=localhost;uid=UserName;pwd=Password;database=HangfireDb;Allow User Variables=True"
    },
  }
```

## Run Update Database

```console
Update-Database
```
