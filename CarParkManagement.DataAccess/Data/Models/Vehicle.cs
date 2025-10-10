using CarParkManagement.DataAccess.Data.IRepository;
using CarParkManagement.DataAccess.Data.Enums;
using System;
using System.Collections.Generic;

namespace CarParkManagement.DataAccess.Data.Models;

public partial class Vehicle : IAuditable
{
    public int VehicleId { get; set; }

    public string VehicleReg { get; set; } = null!;

    public int ChargeRateId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string? VehicleType { get; set; }

    public virtual ChargeRate ChargeRate { get; set; } = null!;

    public virtual ICollection<ParkingAllocation> ParkingAllocations { get; set; } = new List<ParkingAllocation>();
}
