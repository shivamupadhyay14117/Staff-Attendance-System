using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StaffAttendanceSystem.Models
{
    [Table("TeacherDetails")]
    public class Teacher
    {
        [Key]
        public string Id { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public TeacherStatus Status { get; set; } // AttendanceStatus enum

        [Required]
        public string? Department { get; set; }

        public string? ProfilePictureUrl { get; set; } // Property for profile picture URL

        // Add other properties as needed

        public TimeSpan? InTime { get; set; }

        public TimeSpan? OutTime { get; set; }

        public string? Reason { get; set; }
    }


}
