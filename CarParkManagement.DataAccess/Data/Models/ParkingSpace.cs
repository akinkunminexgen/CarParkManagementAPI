using CarParkManagement.DataAccess.Data.IRepository;
using System;
using System.Collections.Generic;

namespace CarParkManagement.DataAccess.Data.Models;

public partial class ParkingSpace : IAuditable
{
    public int ParkingSpaceId { get; set; }

    public int SpaceNumber { get; set; }

    public bool IsOccupied { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<ParkingAllocation> ParkingAllocations { get; set; } = new List<ParkingAllocation>();
}
