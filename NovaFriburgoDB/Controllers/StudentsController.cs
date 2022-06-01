#nullable disable

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NovaFriburgoDB.DataAccess;
using NovaFriburgoDB.Models.DataModels;
using NovaFriburgoDB.Services;

namespace NovaFriburgoDB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly NovaFriburgoDBContext _context;
        private readonly IStudentsService _studentsService;

        public StudentsController(NovaFriburgoDBContext context, IStudentsService studentsService)
        {
            _context = context;
            _studentsService = studentsService;
        }

        // GET: api/Students
        [HttpGet]

        public async Task<ActionResult<IEnumerable<Student>>> GetStudents()
        {
           
            return await _context.Students
                .Include(c => c.Courses)
                .ToListAsync();
        }

        //GET: api/Students/5
        [HttpGet("{id}")]
        public ActionResult<Student> GetStudent(int id)
        {
            var searchStudent = (from student in _context.Students
                                 where student.Id.Equals(id)
                                 select student).FirstOrDefault();

            if (searchStudent == null)
            {
                return NotFound();
            }

            return searchStudent;
        }

        // PUT: api/Students/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudentCourse(int id, int idCourse)
        {
            var student = await _context.Students
                .Include(c => c.Courses)
                .FirstOrDefaultAsync(s => s.Id == id);
            
            if (id != student.Id || student == null)
            {
                return BadRequest();
            }
            var courses = await _context.Courses.FirstOrDefaultAsync(c => c.Id == idCourse);
            student.Courses.Add(courses);
           
            try
            {
               _context.Students.Update(student);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            //return Ok(searchCourseStudent);

            return NoContent();
        }

        // POST: api/Students
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Student>> PostStudent(Student student)
        {
            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStudent", new { id = student.Id }, student);
        }

        // DELETE: api/Students/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.Id == id);
        }
    }
}