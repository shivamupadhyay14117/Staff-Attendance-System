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

        //public async Task<IActionResult> Details(string? id)
        //{
        //    if (string.IsNullOrEmpty(id))
        //    {
        //        return BadRequest(); // Handle invalid teacher ID
        //    }

        //    // Find the teacher by ID in the database
        //    var teacher = await _context.TeacherDetails.FirstOrDefaultAsync(t => t.Id == id);

        //    if (teacher == null)
        //    {
        //        return NotFound(); // Handle teacher not found
        //    }

        //    // Get the current month and year
        //    var currentDate = DateTime.Now;
        //    var currentYear = currentDate.Year;
        //    var currentMonth = currentDate.Month;

        //    // Calculate the counts of month attendance and year attendance for the teacher
        //    var monthAttendanceCount = await _context.AttendanceDetails
        //        .Where(a => a.TeacherId == id && a.Date.Year == currentYear && a.Date.Month == currentMonth)
        //        .CountAsync();

        //    var yearAttendanceCount = await _context.AttendanceDetails
        //        .Where(a => a.TeacherId == id && a.Date.Year == currentYear)
        //        .CountAsync();

        //    // Calculate the total working days in the current month and year
        //    //var totalWorkingDays = GetTotalWorkingDays(currentYear, currentMonth);

        //    // Calculate the attendance percentages
        //    var monthAttendancePercentage = (double)monthAttendanceCount / 100;
        //    var yearAttendancePercentage = (double)yearAttendanceCount /  100;

        //    var viewModel = new AttendanceViewModel
        //    {
        //        Teacher = teacher,
        //        CurrentMonth = monthAttendancePercentage,
        //        CurrentYear = yearAttendancePercentage,
        //    };

        //    return View(viewModel); // Display teacher details using a view
        //}


    }
}
