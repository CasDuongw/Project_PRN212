using System;
using System.Collections.Generic;

namespace Project_PRN212.Models;

public partial class User
{
    public int UserId { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public int? RoleId { get; set; }

    public virtual ICollection<ParkingSession> ParkingSessionStaffIns { get; set; } = new List<ParkingSession>();

    public virtual ICollection<ParkingSession> ParkingSessionStaffOuts { get; set; } = new List<ParkingSession>();

    public virtual Role? Role { get; set; }

    public virtual ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
}
