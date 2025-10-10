using CarParkManagement.DataAccess.Data.Enums;
using CarParkManagement.DataAccess.Data.IRepository;
using System;
using System.Collections.Generic;

namespace CarParkManagement.DataAccess.Data.Models;

public partial class ChargeRate : IAuditable
{
    public int ChargeRateId { get; set; }

    public string? Size { get; set; }

    public decimal RatePerMinute { get; set; }

    public decimal ExtraChargePer5Min { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
}
