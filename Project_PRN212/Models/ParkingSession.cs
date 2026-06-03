using System;
using System.Collections.Generic;

namespace Project_PRN212.Models;

public partial class ParkingSession
{
    public int SessionId { get; set; }

    public string CardCode { get; set; } = null!;

    public string LicensePlate { get; set; } = null!;

    public int? VehicleId { get; set; }

    public int? SlotId { get; set; }

    public DateTime CheckInTime { get; set; }

    public DateTime? CheckOutTime { get; set; }

    public int? StaffInId { get; set; }

    public int? StaffOutId { get; set; }

    public decimal? TotalFee { get; set; }

    public int Status { get; set; }

    public virtual ParkingSlot? Slot { get; set; }

    public virtual User? StaffIn { get; set; }

    public virtual User? StaffOut { get; set; }

    public virtual Vehicle? Vehicle { get; set; }
}
