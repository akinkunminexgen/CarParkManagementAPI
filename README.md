

# AsimpleCarParkApi

A simple .NET Framework API project. This guide explains how to set it up locally.  

---

## Prerequisites

- [.NET Framework 6.x](https://dotnet.microsoft.com/en-us/download/dotnet-framework)  
- [Visual Studio](https://visualstudio.microsoft.com/) or any IDE that supports .NET Framework  
- SQL Server (or your preferred database)  
- Git (optional, if cloning from GitHub)  

---

## Setup Instructions

1. **Clone the repository**

```bash
git clone https://github.com/akinkunminexgen/CarParkManagementAPI.git
cd CarParkManagementAPI

Update-Package -reinstall
Build Solution or use the command line msbuild CarParkManagementAPI.sln
```

## Database setup

Locate the file named script.sql in the project root.
Run it in SQL Server Management Studio (SSMS) to generate the database and all required tables.
The script also inserts default data for the ChargeRate table.

## Run the API

Set the project as the startup project in Visual Studio.
Press F5 or run from the command line using:

```bash
dotnet run
```

## API ENDPOINT
| Endpoint             | Method | Description       |
| -------------------- | ------ | ----------------- |
| `/parking`           | GET    | Available space   |
| `/parking`           | POST   | Parking Allocation|
| `/parking/exit`      | POST    | De-allocation    |


### Notes

Default charge rates are automatically inserted from the database script.
You can modify connection strings and environment settings in appsettings.json.
