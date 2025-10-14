

# AsimpleCarParkApi

This is a simple car park management API designed to manage vehicle parking spaces, allocate spaces to vehicles, calculate parking charges, and de-allocate spaces when vehicles exit. The API follows best practices in software design, including Service and Repository layers, to ensure readability, maintainability, and testability of the code. 

---
## Project Overview
The API handles the following scenarios:
- Allocating vehicles to the first available parking space.
- Retrieving the number of available and occupied spaces.
- Calculating the parking charge when a vehicle exits.
- De-allocating the parking space once a vehicle exits.
---

## Parking Charges
- Small Car - £0.10/minute
- Medium Car - £0.20/minute
- Large Car - £0.40/minute
- Additional Charges: Every 5 minutes, an additional £1 is added to the parking charge.

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

- Locate the file named script.sql in the project root.
- Run it in SQL Server Management Studio (SSMS) to generate the database and all required tables.
- The script also inserts default data for the ChargeRate table.

## Run the API

- Set the project as the startup project in Visual Studio.
- Press F5 or run from the command line using:

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
Unit tests have been included to validate the core functionality of the API, including:

- Vehicle allocation.
- Charge calculation.
- Parking space deallocation.

To run the tests, use the following command:
```bash
dotnet test
```

### Notes

- The API assumes that parking spaces are limited. It allocates vehicles to the first available space.
- A vehicle can only park in one space at a time, and it must exit when done.
- The additional £1 charge is added every 5 minutes a vehicle stays in the parking space.
- The database used is either in-memory or any relational database (MSSQL/Postgres). The solution is designed to be easily configurable.

## Benefits of the Architecture Used
- **Maintainability**: The separation of concerns into layers (Repository, Service, and Controller) makes it easy to change or extend functionality without affecting other parts of the application. For example, changes to the parking logic would be isolated in the service layer without needing to modify the controller.

- **Testability**: This design allows for easy unit testing. You can mock the IParkingRepository in your service layer tests, ensuring that you are testing business logic without depending on a database.

- **Scalability**: If new features or requirements arise (like adding new parking charges or handling more complex vehicle types), they can be added to the service layer without impacting the controllers.

- **Readability**: Each layer has a clear responsibility, and the controller code remains focused on HTTP concerns, while the service and repository layers handle business logic and data access.
