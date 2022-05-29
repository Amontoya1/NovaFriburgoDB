using NovaFriburgoDB.Models.DataModels;

namespace NovaFriburgoDB.Services
{
    public interface IStudentsService
    {
        IEnumerable<Student> GetStudentsWithCourses();
        IEnumerable<Student> GetStudentsWithNoCourses();
    }
}
