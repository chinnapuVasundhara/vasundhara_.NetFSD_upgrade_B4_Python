using System;
using System.Collections.Generic;
using System.Text;
using upGrad_Week6_Day1.Problem_Statement_1;

namespace upGrad_Week6_Day1.Problem_Statement_1
{
    public class ReportGenerator
    {
        public void GenerateReport(List<Student> students)
        {
            Console.WriteLine("\n===== STUDENT REPORT =====");

            foreach (var student in students)
            {
                Console.WriteLine($"ID: {student.StudentId}");
                Console.WriteLine($"Name: {student.StudentName}");
                Console.WriteLine($"Marks: {student.Marks}");
                Console.WriteLine($"Result: {GetResult(student.Marks)}");
                Console.WriteLine("----------------------------");
            }
        }

        private string GetResult(int marks)
        {
            return marks >= 40 ? "Pass" : "Fail";
        }
    }
}