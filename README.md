### dotnet-core-dbcontext-extensions

In this repository, I will share the extensions I use for database management when developing projects with .Net Core.

#### AddDynamicDbContext 
[![NuGet](https://img.shields.io/nuget/v/AddDynamicDbContext.svg)](https://www.nuget.org/packages/AddDynamicDbContext)

This extension aims to simplify ConnectionString definition for **Development** and **Production** environments when adding DbContext to our .Net Core project.

```
Install-Package AddDynamicDbContext -Version 1.0.0
```

You can use this plugin when adding **DbContext** to services in **Startup**. ConnectionString to be used when accessing your database will be determined automatically according to your environment.

It is simple to use, we call the **`AddDDbContext`** extension when adding dbcontext in the **Startup.cs** `ConfigureServices` method.

```csharp
services.AddDDbContext<SampleDbContext>("Sample");
```

We add our **Development** and **Production** environment information to the **ConnectionStrings** section of our **`appsettings.json`** file.

```json
"ConnectionStrings": {
  "Sample": {
    "Development": "Development ConnectionString",
    "Production": "Production ConnectionString"
  }
}
```

##### Parameters

Descriptions and default values for the extension parameters.

| Parameter | Type | Required | Default | Description |
|--------------------------|---------------------|----------|---------|-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| connectionStringKey | string | yes |  | ConnectionString key at the **appsettins.json** file. |
| provider | Provider | no | Provider.SqlServer | Select the type of provider to use when connecting. You can choose [**SqlServer** or **MySQLServer**]. |
| debug | bool | no | false | Use the Production environment while in Debug mode. |


##### Dockerfile

If you don't add the following line to Dockerfile for the production environment, the environment variable returns null. The extension overcomes this situation. I leave it as information.

```yaml
ENV ASPNETCORE_ENVIRONMENT=Production
```
