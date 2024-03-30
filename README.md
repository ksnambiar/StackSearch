# StackSearch - Search through stackoverflow posts

This is a basic ASP.NET MVC application used to search for posts from stackoverflow small database.

## Prerequisites

1. Tools
    - Visual Studio or Visual Studio Code: You'll need Visual Studio with ASP.NET and Web Development workload installed.
    - .NET SDK: Make sure you have the latest .NET SDK installed ([Download](https://dotnet.microsoft.com/en-us/download)). I am using net8.0
2. Database
    - public Stack Overflow data export: you can download it from here [https://www.brentozar.com/archive/2015/10/how-to-download-the-stack-overflow-database-via-bittorrent/]
    - Setup MSSQL Database depending on your system.
        -  For Windows and Linux: use this link [link](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
        -  For Mac - use this link. [link](https://medium.com/@ylc2112/the-quick-guide-to-run-sql-server-on-m1-mac-8a25abe52c8b)

## Setting Up

### Clone the Repository:

Clone this repository using your preferred method (e.g., Git).

```bash
git clone [git-url]
```

### Open in Visual Studio Code:

I used Visual Studio Code  as my IDE, but you can use any other IDE of your choice. You will need to have .Net Core installed and then open up the project.

You can download and install it from [here](https://code.visualstudio.com)

### Install NuGet Packages:

1. move into the application root directory.
2.  Run `dotnet restore` command in terminal or VSCode Terminal. This will install all necessary packages defined.
```bash
dotnet restore
```
3. I used the following packages
    - "AutoFixture >= 4.18.1",
    - "Microsoft.EntityFrameworkCore >= 8.0.3",
    - "Microsoft.EntityFrameworkCore.SqlServer >= 8.0.3",
    - "Microsoft.EntityFrameworkCore.Tools >= 8.0.3"

### Replace the database credentials

1. Change the database credentials within the appsettings.json, mainly:
    - Server address
    - Database
    - Username
    - Password

### Run the Application:

- execute the run command.
```bash
dotnet run
```
- The default URL for your application will typically be http://localhost:5164 (adjust the port if necessary).


## Specific changes in the Database

1. Added two indexes on top of Posts (ownerUserId) and Votes (PostId). you execute the following  SQL commands to add them:
```sql
CREATE INDEX IX_OwnerUserId ON dbo.Posts(OwnerUserId);
GO
CREATE INDEX IX_PostId ON dbo.Votes(PostId);
GO
```

