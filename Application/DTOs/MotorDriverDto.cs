using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs
{
    public class MotorDriverDto
    {
        public int Id { get; set; }
        public string SerialNumber { get; set; }
        public double Voltage { get; set; }
        public DateTime TestDate { get; set; }
        public bool IsPassed { get; set; }
    }
}