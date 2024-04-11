using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final_Assignment_Submission_Student
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var dbContext = new StudentDbContext())
            {
                var newStudent = new Student { Name = "John Doe" };

                dbContext.Students.Add(newStudent);
                dbContext.SaveChanges();

                Console.WriteLine("Student added successfully!");
            }

            Console.ReadLine();
        }
    }
}
