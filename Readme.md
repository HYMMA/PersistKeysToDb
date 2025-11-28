# Introduction
[Data Protection Api](https://learn.microsoft.com/en-us/aspnet/core/security/data-protection/using-data-protection) uses symmetric cryptographic algorithms to protect data. So it needs to store and access the key somewhere safe. 
It handles all the key rotation and has a very simple API. When using data protection api in a dotnet project you have the option to store the `keys` in different locations. While there is a EF 
package to store the keys in a database, there was no Dapper provider for this purpose. That's why I made this package.
## How to use
You need a database table that has three columns:
- Id of type `int`
- FriendlyName of type `varchar(max)`
- Xml of type `varchar(max)`

> [!WARNING] 
> If you specify an explicit key persistence location, the data protection system deregisters the default key encryption at rest mechanism, so keys are no longer encrypted at rest. It's recommended that you additionally specify an [explicit key encryption mechanism](https://learn.microsoft.com/en-us/aspnet/core/security/data-protection/implementation/key-encryption-at-rest?view=aspnetcore-9.0) for production deployments.

### Set up the database
The package does not create the database tables. But they are very simple ones.
Lets say you have a SQL database called _DataProtectionKyeDb_, then you can use this script to create the table.
```sql
USE [DataProtectionKeyDb]
GO

/****** Object:  Table [dbo].[DataProtectionKey]    Script Date: 20/12/2024 10:21:09 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DataProtectionKey](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FriendlyName] [varchar](max) NULL,
	[Xml] [varchar](max) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


```

### Register the package
```CSharp
internal class Program
    {
        private static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDataProtection()
            .ProtectKeysWithCertificate("Certificate") // recommended to protect keys at rest 
            .PersistKeysToDb("Connection string"); // extension method from this package

            . . .

        }
    }
```
## Other options
[Key storage providers](https://learn.microsoft.com/en-us/aspnet/core/security/data-protection/implementation/key-storage-providers) discusses multiple package from Microsoft.
