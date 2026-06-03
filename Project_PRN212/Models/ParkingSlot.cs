using System;
using System.Collections.Generic;

namespace Project_PRN212.Models;

public partial class ParkingSlot
{
    public int SlotId { get; set; }

    public string SlotCode { get; set; } = null!;

    public int? ZoneId { get; set; }

    public int Status { get; set; }

    public virtual ICollection<ParkingSession> ParkingSessions { get; set; } = new List<ParkingSession>();

    public virtual Zone? Zone { get; set; }
}
