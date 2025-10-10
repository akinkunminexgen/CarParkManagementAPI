using CarParkManagement.DataAccess.Data.IRepository;
using System;
using System.Collections.Generic;

namespace CarParkManagement.DataAccess.Data.Models;

public partial class ParkingAllocation : IAuditable
{
    public int ParkingAllocationId { get; set; }

    public int VehicleId { get; set; }

    public int ParkingSpaceId { get; set; }

    public DateTime TimeIn { get; set; }

    public DateTime? TimeOut { get; set; }

    public decimal? Charge { get; set; }

    public bool? IsAvailable { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ParkingSpace ParkingSpace { get; set; } = null!;

    public virtual Vehicle Vehicle { get; set; } = null!;
}
