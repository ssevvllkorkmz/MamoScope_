using System;
using System.Collections.Generic;
using System.Text;

namespace MaMoScope_.Models
{
    public class MotorDrivers
    {
        public int Id { get; set; }
        public string SerialNumber { get; set; }
        public double Voltage { get; set; }
        public DateTime TestDate { get; set; }
        public DateTime TestDateLocal => DateTime.SpecifyKind(TestDate, DateTimeKind.Utc).ToLocalTime();
        public bool IsPassed { get; set; }

    }
}
