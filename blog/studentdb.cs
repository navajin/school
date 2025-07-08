using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace CodeFirstBlogDatabase
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var db = new StudentService()) // Creates a new instance of StudentService to interact with the database
            {
                // Prompt the user for student details
                Console.Write("What is your name?: ");
                var name = Console.ReadLine();

                Console.Write("\nWhat is your age?: ");
                var age = Console.ReadLine();

                Console.Write("\nWhat is your major?: ");
                var major = Console.ReadLine();

                // Create a new Student object with the provided details
                Student student = new Student() { Name = name, Age = age, Major = major };
                db.StudentServices.Add(student); // Adds the student object to the Students table in the database
                db.SaveChanges(); // Saves the changes to the database

                // Display all Students from the database
                var studentQuery = from s in db.StudentServices // LINQ query to select all students
                                   orderby s.Name // Orders the students by name
                                   select s; // Selects the student objects

                Console.WriteLine("\n\nAll students in the database:");
                foreach (var s in studentQuery) // Iterates through each student in the studentQuery
                {
                    Console.WriteLine($"Student ID: {s.StudentId}, Name: {s.Name}"); // Displays student ID and name
                }

                // Query to get all posts
                var postQuery = from p in db.Posts
                                orderby p.Title // Orders posts by title
                                select p; // Selects post objects

                Console.WriteLine("\n\nAll posts in the database:");
                foreach (var item in postQuery) // Iterates through each post in the postQuery
                {
                    Console.WriteLine($"\n{item.Title}"); // Displays the title of each post
                }

                Console.WriteLine("\nPress any key to exit...");
                Console.ReadKey(); // Waits for a key press before exiting
            }
        }
    }

    public class Student
    {
        public int StudentId { get; set; } // Unique identifier for the student
        public string Name { get; set; } // Name of the student
        public int Age { get; set; } // Age of the student
        public string Major { get; set; } // Major field of study
    }

    public class StudentServiceContext : DbContext
    {
        public DbSet<Student> Students { get; set; } // Represents the collection of students in the database
    }

    public class StudentService
    {
        public void AddStudent(string name, int age, string major) // Method to add a new student
        {
            using (var context = new StudentService()) // Creates a new instance of StudentSr
            {
                var student = new Student // Creates a new Student object
                {
                    Name = name,
                    Age = age,
                    Major = major
                };

                context.Students.Add(student); // Adds new student to the context
                context.SaveChanges(); // Saves changes to the database
            }
        }
    }

    // Usage
    var studentService = new StudentService();
    studentService.AddStudent("John Doe", 20, "Computer Science"); // Adding a sample student
