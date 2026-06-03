using System;
using System.Collections.Generic;

namespace Project_PRN212.Models;

public partial class PriceConfig
{
    public int PriceConfigId { get; set; }

    public int? VehicleTypeId { get; set; }

    public decimal PricePerHour { get; set; }

    public decimal PricePerDay { get; set; }

    public DateTime? ApplyDate { get; set; }

    public virtual VehicleType? VehicleType { get; set; }
}
