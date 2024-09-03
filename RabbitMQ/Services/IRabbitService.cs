using RabbitMQ.Models;
using StatusGeneric;

namespace RabbitMQ.Services
{
    public interface IRabbitService
    {
       void Send(StudentDto dto);
       List<Student> GetStudents();
    }
}
