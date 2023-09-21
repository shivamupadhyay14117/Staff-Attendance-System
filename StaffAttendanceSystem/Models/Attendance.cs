using StaffAttendanceSystem.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StaffAttendanceSystem.Models
{
    [Table("AttendanceDetails")]
    public class Attendance
    {
        [Key]
        public int AttendanceId { get; set; }

        [Required]
        public string TeacherId { get; set; } // Reference to the Teacher

        [ForeignKey("TeacherId")]
        public Teacher Teacher { get; set; } // Navigation property

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public AttendanceStatus Status { get; set; } // AttendanceStatus enum

        public TimeSpan? InTime { get; set; }

        public TimeSpan? OutTime { get; set; }

        public string? Reason { get; set; }
    }
}
