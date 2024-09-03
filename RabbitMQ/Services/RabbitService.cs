using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Models;
using StatusGeneric;

namespace RabbitMQ.Services
{
    public class RabbitService :IRabbitService
    {
        public List<Student> GetStudents()
        {
            var factory = new ConnectionFactory()  //ulanish hosil qilish
            {
                HostName = "localhost",
                VirtualHost = "/"
            };
            List<Student> students = new List<Student>();
            string _message = string.Empty;
            using (var connection = factory.CreateConnection())   //kanalga ulanish
            using (var channel =  connection.CreateModel())
            {
                channel.QueueDeclare("myfirstqueue", durable: true, exclusive: false, autoDelete: false);  //navbat yasash agar yoq bolsa yaratadi bor bolsa yozadi

                var consumer = new EventingBasicConsumer(channel);
                var receivedEvent = new ManualResetEventSlim(false);
                consumer.Received += (model, eventArgs) =>
                {
                    var body = eventArgs.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                   students.Add(JsonConvert.DeserializeObject<Student>(message));
                   _message = message;
                };

                channel.BasicConsume(queue: "myfirstqueue", autoAck: true, consumer: consumer);     //queue tozalash
                receivedEvent.Wait(100);
            }

            return students;
        }

        public void Send(StudentDto dto)
        {
            Student student = new Student()
            {
                Id = Guid.NewGuid(),
                Age = dto.Age,
                Name = dto.Name,

            };

            var factory = new ConnectionFactory()  //ulanish hosil qilish
            {
                HostName = "localhost",
                VirtualHost = "/"
            };

            using (var connection = factory.CreateConnection())   //kanalga ulanish
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare("myfirstqueue", durable: true, exclusive: false, autoDelete: false);  //navbat yasash agar yoq bolsa yaratadi bor bolsa yozadi

                var jsonString = JsonConvert.SerializeObject(student);
                var body = Encoding.UTF8.GetBytes(jsonString);

                channel.BasicPublish("Murojatlar.topic", "jasur", null, body);     //habar yuborish
            }
            
           
        }
    }
}
