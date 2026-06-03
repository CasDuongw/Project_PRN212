using System;
using System.Collections.Generic;

namespace Project_PRN212.Models;

public partial class Zone
{
    public int ZoneId { get; set; }

    public string ZoneName { get; set; } = null!;

    public int? VehicleTypeId { get; set; }

    public virtual ICollection<ParkingSlot> ParkingSlots { get; set; } = new List<ParkingSlot>();

    public virtual VehicleType? VehicleType { get; set; }
}
