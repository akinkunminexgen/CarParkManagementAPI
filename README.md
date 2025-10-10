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

##API ENDPOINT
| Endpoint             | Method | Description       |
| -------------------- | ------ | ----------------- |
| `/parking`           | GET    | Available space   |
| `/parking`           | POST   | Parking Allocation|
| `/parking/exit`      | POST    | De-allocation    |

