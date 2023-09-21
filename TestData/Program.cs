using Microsoft.EntityFrameworkCore;
using StaffAttendanceSystem.Data;
using StaffAttendanceSystem.Models;
using System;
using System.Collections.Generic;

namespace StaffAttendanceSystem.TestDataGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseSqlServer("Server=DESKTOP-01QMQ7F;Database=AttendanceManagement;Trusted_Connection=True;");

            using (var context = new AppDbContext(optionsBuilder.Options))
            {
                // Add test data for teachers
                var teachers = new List<Teacher>
                {
                    new Teacher
                    {
                        Id = "1",
                        FullName = "John Doe",
                        Email = "johndoe@example.com",
                        PhoneNumber = "123-456-7890",
                        Status = TeacherStatus.NotAvailable,
                        Department = "Math",
                        ProfilePictureUrl = "\"https://source.unsplash.com/500x700/?profilepicture" // Add a sample profile picture URL
                    },
                    new Teacher
                    {
                        Id = "2",
                        FullName = "Jane Smith",
                        Email = "janesmith@example.com",
                        PhoneNumber = "987-654-3210",
                        Status = TeacherStatus.NotAvailable,
                        Department = "Science",
                        ProfilePictureUrl = "https://source.unsplash.com/500x700/?profilepicture" // Add a sample profile picture URL
                    },

                      new Teacher
                    {
                        Id = "3",
                        FullName = "Shiv Smith",
                        Email = "shiv@example.com",
                        PhoneNumber = "987-654-3210",
                        Status = TeacherStatus.NotAvailable,
                        Department = "Maths",
                        ProfilePictureUrl = "https://source.unsplash.com/500x700/?profilepicture" // Add a sample profile picture URL
                    },

                        new Teacher
                    {
                        Id = "4",
                        FullName = "Shiv Shankar",
                        Email = "shankr@example.com",
                        PhoneNumber = "127-654-3210",
                        Status = TeacherStatus.NotAvailable,
                        Department = "School",
                        ProfilePictureUrl = "https://source.unsplash.com/500x700/?profilepicture" // Add a sample profile picture URL
                    }, new Teacher

                        {
                        Id = "5",
                        FullName = "Karn Shankar",
                        Email = "karn@example.com",
                        PhoneNumber = "127-654-3210",
                        Status = TeacherStatus.NotAvailable,
                        Department = "STEM",
                        ProfilePictureUrl = "https://source.unsplash.com/500x700/?profilepicture" // Add a sample profile picture URL
                    },
                    // Add more teacher records as needed
                };

                context.TeacherDetails.AddRange(teachers);
                context.SaveChanges();

                // Add test data for attendance records
                var attendanceRecords = new List<Attendance>();
                var today = DateTime.Today;

                foreach (var teacher in teachers)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        var attendance = new Attendance
                        {
                            TeacherId = teacher.Id,
                            Date = today.AddDays(i),
                            Status = i % 2 == 0 ? AttendanceStatus.Present : AttendanceStatus.Absent,
                            InTime = TimeSpan.FromHours(8 + i), // Example in-time
                            OutTime = TimeSpan.FromHours(16 + i), // Example out-time
                            Reason = i % 2 == 0 ? null : "Reason for absence on " + today.AddDays(i).ToString("dd MMM")
                        };

                        attendanceRecords.Add(attendance);
                    }
                }

                context.AttendanceDetails.AddRange(attendanceRecords);
                context.SaveChanges();
            }

            Console.WriteLine("Test data for teachers and attendance has been added to the database.");
        }
    }
}
