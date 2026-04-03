//Problem 3- LSP – Liskov Substitution Principle
//Scenario: Shape Area Calculator
//A graphics application calculates the area of different shapes.
//Any derived class should be able to replace the base class without breaking functionality.

//Requirements:
//1.Create a base class or interface:
//•	Shape

//2.Derived classes:
//•	Rectangle
//•	Circle

//3.Each shape should implement:
//•	CalculateArea()

//4.A method should accept Shape object and calculate area.
//Technical Constraints:
//•	Use method overriding
//•	Derived classes must not break base class behavior

//Expectations:
//Students should demonstrate that the program works correctly when:
//•	Rectangle object is used
//•	Circle object is used

//Program Flow Diagram:
//Shape -> (Rectangle, Circle) Both OverRide CalculateArea()

//Learning Outcome:
//Students will learn:
//•	Meaning of substitutability
//•	Importance of correct inheritance
//•	How polymorphism supports LSP


using System;
using System.Collections.Generic;
using System.Text;

namespace upGrad_Week6_Day1.Problem_Statement_3
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Choose Shape:");
            Console.WriteLine("1. Rectangle");
            Console.WriteLine("2. Circle");

            Console.Write("Enter choice: ");
            int choice = Convert.ToInt32(Console.ReadLine());

            Shape? shape = null;

            if (choice == 1)
            {
                Console.Write("Enter Length: ");
                double length = Convert.ToDouble(Console.ReadLine());

                Console.Write("Enter Width: ");
                double width = Convert.ToDouble(Console.ReadLine());

                shape = new Rectangle(length, width);
            }
            else if (choice == 2)
            {
                Console.Write("Enter Radius: ");
                double radius = Convert.ToDouble(Console.ReadLine());

                shape = new Circle(radius);
            }
            else
            {
                Console.WriteLine("Invalid choice!");
                return;
            }

            PrintArea(shape);
        }

        static void PrintArea(Shape shape)
        {
            Console.WriteLine("Area: " + shape.CalculateArea());
        }
    }
}
