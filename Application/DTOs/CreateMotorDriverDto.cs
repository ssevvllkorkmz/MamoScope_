using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs
{
    public class CreateMotorDriverDto
    {
        public string SerialNumber { get; set; }
        public double Voltage { get; set; }
    }
}