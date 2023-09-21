using StaffAttendanceSystem.Models;
using System;
using System.Collections.Generic;

namespace StaffAttendanceSystem.Models
{
    public class AttendanceViewModel
    {
        public List<Teacher> Teachers { get; set; }
        public List<Attendance> Attendances { get; set; }
        public DateTime Monday { get; set; } // Add this property
        public Teacher Teacher { get; set; }

        public int CurrentMonth { get; set; } // Add this property
        public int CurrentYear { get; set; } // Add this property
    }
}
