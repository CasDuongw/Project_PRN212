using System;
using System.Collections.Generic;

namespace Project_PRN212.Models;

public partial class Vehicle
{
    public int VehicleId { get; set; }

    public string LicensePlate { get; set; } = null!;

    public int? VehicleTypeId { get; set; }

    public int? OwnerId { get; set; }

    public virtual User? Owner { get; set; }

    public virtual ICollection<ParkingSession> ParkingSessions { get; set; } = new List<ParkingSession>();

    public virtual VehicleType? VehicleType { get; set; }
}
