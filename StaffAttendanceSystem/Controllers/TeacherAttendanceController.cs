using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StaffAttendanceSystem.Data;
using StaffAttendanceSystem.Models;
using System;

using System.Linq;
using System.Threading.Tasks;

namespace StaffAttendanceSystem.Controllers
{
    public class TeacherAttendanceController : Controller
    {
        private readonly AppDbContext _context;

        public TeacherAttendanceController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("api/getDetails")]
        public async Task<IActionResult> Index(DateTime? startDate = null, DateTime? endDate = null)
        {
            DateTime selectedWeekStart;
            DateTime selectedWeekEnd;

            
            if (startDate.HasValue && endDate.HasValue)
            {
                selectedWeekStart = startDate.Value;
                selectedWeekEnd = endDate.Value;
            }
            else
            {
                selectedWeekStart = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Monday);
                selectedWeekEnd = selectedWeekStart.AddDays(4);
            }

            
            var attendances = await _context.AttendanceDetails
                .Include(a => a.Teacher)
                .Where(a => a.Date >= selectedWeekStart && a.Date <= selectedWeekEnd)
                .ToListAsync();

            foreach (var attendance in attendances)
            {
                attendance.Teacher.Status = IsTeacherAvailableForToday(attendance.TeacherId) ? TeacherStatus.Available : TeacherStatus.NotAvailable;
            }

            await _context.SaveChangesAsync();

            var teachers = await _context.TeacherDetails.ToListAsync();

            var finalList = new AttendanceViewModel
            {
                Teachers = teachers,
                Attendances = attendances,
                Monday = selectedWeekStart
            };

            return View(finalList);
        }

        // GET: Attendance/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attendance = await _context.AttendanceDetails.FindAsync(id);

            // Ensure only the teacher who owns the attendance record can edit it
            if (attendance == null)
            {
                return NotFound();
            }

            return View(attendance);
        }

        // POST: Attendance/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TeacherId,AttendanceId,Date,Status,Reason,InTime,OutTime")] Attendance attendance)
        {
            if (id != attendance.AttendanceId)
            {
                return NotFound();
            }

                try
                {
                    _context.Update(attendance);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AttendanceExists(attendance.AttendanceId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        ModelState.AddModelError("", "The record has been modified by another user. Please refresh and try again.");
                    }
                }
            

            return RedirectToAction("Index");
        }

        private bool AttendanceExists(int id)
        {
            return _context.AttendanceDetails.Any(e => e.AttendanceId == id);
        }




        private bool IsTeacherAvailableForToday(string teacherId)
        {
            var today = DateTime.Today;
            var attendance = _context.AttendanceDetails.FirstOrDefault(a =>
                a.TeacherId == teacherId && a.Date.Date == today.Date);

            // Check if attendance exists
            if (attendance != null)
            {
                if (attendance.Status == AttendanceStatus.Present ||
                    attendance.Status == AttendanceStatus.Late)
                {
                    // Teacher is present or late, so return true
                    return true;
                }
                else if (attendance.Status == AttendanceStatus.Absent ||
                         attendance.Status == AttendanceStatus.NotApplicable)
                {
                    // Teacher is absent or status is not applicable, so return false
                    return false;
                }
            }

            // Teacher's attendance for today is not found, so assuming not available
            return false;
        }




        [HttpGet]
        public IActionResult CreateTeacher()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTeacher(Teacher teacher)
        {
          
                // Add the new teacher to the context
                _context.TeacherDetails.Add(teacher);

                // Create attendance records for the new teacher (you can customize this logic)
                var startDate = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Monday);
                var endDate = startDate.AddDays(4);

                for (var date = startDate; date <= endDate; date = date.AddDays(1))
                {
                    var attendance = new Attendance
                    {
                        TeacherId = teacher.Id,
                        Date = date,
                        Status = AttendanceStatus.NotApplicable
                         // Customize this as needed
                    };
                    _context.AttendanceDetails.Add(attendance);
                }

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            

            
        }



        [Route("api/delete/{teacherId}")]
        public async Task<IActionResult> Delete(string teacherId)
        {
            if (string.IsNullOrEmpty(teacherId))
            {
                return BadRequest("TeacherId is required.");
            }

            // Find and remove teacher details
            var teacher = await _context.TeacherDetails.FindAsync(teacherId);
            if (teacher == null)
            {
                return NotFound("Teacher not found.");
            }

            _context.TeacherDetails.Remove(teacher);

            // Find and remove attendance details for the teacher
            var attendanceDetails = _context.AttendanceDetails.Where(a => a.TeacherId == teacherId).ToList();
            if (attendanceDetails.Count > 0)
            {
                _context.AttendanceDetails.RemoveRange(attendanceDetails);
            }

            try
            {
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Handle any exceptions that may occur during deletion
                return StatusCode(500, $"An error occurred while deleting teacher and attendance details: {ex.Message}");
            }
        }

    }
}
