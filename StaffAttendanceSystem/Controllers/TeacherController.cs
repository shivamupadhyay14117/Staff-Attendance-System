using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StaffAttendanceSystem.Data;
using StaffAttendanceSystem.Models;
using System;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace StaffAttendanceSystem.Controllers
{
    public class TeacherController : Controller
    {
        private readonly AppDbContext _context;

        public TeacherController(AppDbContext context)
        {
            _context = context;
        }

        // GET: TimeOff
        public async Task<IActionResult> Index()
        {
            var viewModel = new AttendanceViewModel
            {
                // Initialize the Teachers and Attendances properties as needed
                Teachers = await _context.TeacherDetails.ToListAsync(),
                Attendances = await _context.AttendanceDetails.ToListAsync()
            };

            return View(viewModel);
        }

        // GET: TimeOff/Create
        public async Task<IActionResult> Create()
        {
            var viewModel = new AttendanceViewModel
            {
                // Initialize the Teachers and Attendances properties as needed
                Teachers = await _context.TeacherDetails.ToListAsync(),
                Attendances = await _context.AttendanceDetails.ToListAsync()
            };

            return View(viewModel);
        }

        // POST: TimeOff/Create
        [Route("api/SubmitTimeOff")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string teacherId, DateTime date, string reason)
        {
            if (ModelState.IsValid)
            {
                // Check if the teacher with the provided ID exists
                var teacher = await _context.TeacherDetails.FindAsync(teacherId);

                if (teacher != null)
                {
                    // Create a new time-off request (Attendance) with the provided details
                    var existingAttendance = await _context.AttendanceDetails
             .Where(a => a.TeacherId == teacherId && a.Date == date)
             .FirstOrDefaultAsync();

                    if (existingAttendance != null)
                    {
                        // Update the existing attendance record with the provided reason and status
                        existingAttendance.Reason = reason;
                        existingAttendance.Status = AttendanceStatus.Absent;

                        await _context.SaveChangesAsync();

                        // Redirect to the desired URL (e.g., "/api/getDetails")
                        return Redirect("/api/getDetails");
                    }

                }
                else
                {
                    // Handle the case where the provided teacher ID is not found
                    ModelState.AddModelError("teacherId", "Teacher with the provided ID does not exist.");
                }
            }

            // If the model state is invalid or teacher not found, return to the form with validation errors
            var viewModel = new AttendanceViewModel
            {
                // Initialize the Teachers and Attendances properties as needed
                Teachers = await _context.TeacherDetails.ToListAsync(),
                Attendances = await _context.AttendanceDetails.ToListAsync()
            };

            return View("Create", viewModel); // Return to the Create view with validation errors
        }

        public async Task<IActionResult> Details(string? id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest(); // Handle invalid teacher ID
            }

            // Find the teacher by ID in the database
            var teacher = await _context.TeacherDetails.FirstOrDefaultAsync(t => t.Id == id);

            if (teacher == null)
            {
                return NotFound(); // Handle teacher not found
            }

            // Get the current month and year
            var currentDate = DateTime.Now;
            var currentYear = currentDate.Year;
            var currentMonth = currentDate.Month;

            // Calculate the total working days for the current month and year
            var totalWorkingDays = GetTotalWorkingDays(currentYear, currentMonth);

            // Calculate the counts of month attendance and year attendance for the teacher
            var monthAttendanceCount = await _context.AttendanceDetails
                .Where(a => a.TeacherId == id && a.Date.Year == currentYear && a.Date.Month == currentMonth)
                .CountAsync();

            var yearAttendanceCount = await _context.AttendanceDetails
                .Where(a => a.TeacherId == id && a.Date.Year == currentYear)
                .CountAsync();

            // Calculate the attendance percentages
            var monthAttendancePercentage = (int)monthAttendanceCount / totalWorkingDays;
            var yearAttendancePercentage = (int)yearAttendanceCount / totalWorkingDays;

            var viewModel = new AttendanceViewModel
            {
                Teacher = teacher,
                CurrentMonth = monthAttendancePercentage * 100, // Convert to percentage
                CurrentYear = yearAttendancePercentage * 100,   // Convert to percentage
            };

            return View(viewModel); // Display teacher details using a view
        }

        // Create a method to calculate total working days
        private int GetTotalWorkingDays(int year, int month)
        {
            int totalWorkingDays = 0;
            int daysInMonth = DateTime.DaysInMonth(year, month);

            // Define a list of holidays
            var holidays = new List<DateTime>
    {
        new DateTime(year, 9, 1),   // Start of the academic year
        new DateTime(year, 12, 1),  // Commemoration Day
        new DateTime(year, 3, 10),  // Ramadan starts
        new DateTime(year, 4, 9),   // Eid al-Fitr
        new DateTime(year, 6, 17),  // Eid al-Adha
    };

            for (int day = 1; day <= daysInMonth; day++)
            {
                var currentDate = new DateTime(year, month, day);

                // Check if the current date is a holiday or weekend (Friday or Saturday)
                if (!holidays.Contains(currentDate) && currentDate.DayOfWeek != DayOfWeek.Friday && currentDate.DayOfWeek != DayOfWeek.Saturday)
                {
                    totalWorkingDays++;
                }
            }

            return totalWorkingDays;
        }


    }
}
