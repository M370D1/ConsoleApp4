using System;
using System.Collections.Generic;
using System.Linq;

class StudentDataManagementSystem
{
    private static Dictionary<string, Dictionary<string, List<double>>> students = new Dictionary<string, Dictionary<string, List<double>>>();
    private static readonly List<string> allowedSubjects = new List<string> { "Math", "Biology", "History", "English", "Sport", "Physics" };

    static void Main(string[] args)
    {
        Console.WriteLine("Student Data Management System!");

        while (true)
        {
            DisplayMenu();
            UserChoice(Console.ReadLine());
        }
    }

    static void DisplayMenu()
    {
        Console.WriteLine("\nChoose an option:");
        Console.WriteLine("1. Add a new student");
        Console.WriteLine("2. Remove a student");
        Console.WriteLine("3. Assign student to subject");
        Console.WriteLine("4. Update a student's grades");
        Console.WriteLine("5. Display all students");
        Console.WriteLine("6. Exit");
        Console.Write("Enter your choice: ");
    }

    static void UserChoice(string choice)
    {
        switch (choice)
        {
            case "1":
                AddStudent();
                break;
            case "2":
                RemoveStudent();
                break;
            case "3":
                AssignSubjectToStudent();
                break;
            case "4":
                UpdateStudentGrades();
                break;
            case "5":
                DisplayAllStudents();
                break;
            case "6":
                Console.WriteLine("Exiting the system. Goodbye!");
                Environment.Exit(0);
                break;
            default:
                Console.WriteLine("Invalid option. Please try again.");
                break;
        }
    }

    static void AddStudent()
    {
        Console.Write("Enter student name (use only small and capital Latin letters): ");
        string studentName = Console.ReadLine();

        if (!IsValidStudentName(studentName, out string errorMessage))
        {
            Console.WriteLine(errorMessage);
            return;
        }

        if (StudentExists(studentName))
        {
            Console.WriteLine($"Student {studentName} already exists.");
        }
        else
        {
            students[studentName] = new Dictionary<string, List<double>>();
            Console.WriteLine($"Student {studentName} added successfully!");
        }
    }

    static void RemoveStudent()
    {
        Console.Write("Enter student name to remove: ");
        string studentName = Console.ReadLine();

        if (!StudentExists(studentName))
        {
            Console.WriteLine($"Student {studentName} does not exist.");
            return;
        }

        students.Remove(studentName);
        Console.WriteLine($"Student {studentName} removed successfully!");
    }

    static void AssignSubjectToStudent()
    {
        Console.Write("Enter student and subject (format: StudentName-Subject): ");
        string input = Console.ReadLine();

        var parts = input.Split('-');
        if (!IsValidInput(parts, out string errorMessage))
        {
            Console.WriteLine(errorMessage);
            return;
        }

        string studentName = parts[0].Trim();
        string subject = parts[1].Trim();

        if (!StudentExists(studentName))
        {
            Console.WriteLine($"Student {studentName} does not exist.");
            return;
        }

        if (!IsValidSubject(subject, out string subjectErrorMessage))
        {
            Console.WriteLine(subjectErrorMessage);
            return;
        }

        if (!students[studentName].ContainsKey(subject))
        {
            students[studentName][subject] = new List<double>();
            Console.WriteLine($"Student {studentName} has successfully enrolled in the {subject} class.");
        }
        else
        {
            Console.WriteLine($"Student {studentName} is already enrolled in the {subject} class.");
        }
    }

    static void UpdateStudentGrades()
    {
        Console.Write("Enter student name: ");
        string studentName = Console.ReadLine();


        if (!StudentExists(studentName))
        {
            Console.WriteLine($"Student {studentName} does not exist.");
            return;
        }

        Console.Write("Enter subject and grade (format: Subject-Grade): ");
        string input = Console.ReadLine();

        var parts = input.Split('-');
        if (!IsValidInput(parts, out string errorMessage))
        {
            Console.WriteLine(errorMessage);
            return;
        }

        string subject = parts[0].Trim();
        if (!students[studentName].ContainsKey(subject))
        {
            Console.WriteLine($"Student {studentName} is not enrolled in {subject}.");
            return;
        }

        if (TryParseGrade(parts[1].Trim(), out double grade))
        {
            students[studentName][subject].Add(grade);
            Console.WriteLine($"Grade {grade} added for {studentName} in {subject}.");
        }
        else
        {
            Console.WriteLine("Invalid grade. Grade must be a number between 2 and 6.");
        }
    }

    static void DisplayAllStudents()
    {
        if (students.Count == 0)
        {
            Console.WriteLine("No students found.");
            return;
        }

        Console.WriteLine("\nDisplaying all students:");
        foreach (var student in students)
        {
            string name = student.Key;
            var subjects = student.Value;

            string subjectList = string.Join(", ", subjects.Keys);
            double averageGrade = subjects.Values.SelectMany(grades => grades).DefaultIfEmpty(0).Average();

            Console.WriteLine($"{name}, Subjects: {subjectList}, Average Grade: {averageGrade:F2}");
        }
    }

    static bool IsValidStudentName(string name, out string errorMessage)
    {
        errorMessage = string.Empty;

        if (string.IsNullOrWhiteSpace(name))
        {
            errorMessage = "Student name cannot be empty.";
            return false;
        }

        if (!name.All(char.IsLetter))
        {
            errorMessage = "Invalid name. Use only letters.";
            return false;
        }

        return true;
    }

    static bool IsValidInput(string[] parts, out string errorMessage)
    {
        errorMessage = string.Empty;

        if (parts.Length != 2)
        {
            errorMessage = "Invalid format. Please use the correct format.";
            return false;
        }

        return true;
    }

    static bool StudentExists(string studentName)
    {
        return students.ContainsKey(studentName);
    }

    static bool IsValidSubject(string subject, out string errorMessage)
    {
        errorMessage = string.Empty;

        if (string.IsNullOrWhiteSpace(subject))
        {
            errorMessage = "Subject cannot be empty.";
            return false;
        }

        if (!allowedSubjects.Contains(subject))
        {
            errorMessage = $"Invalid subject. Allowed subjects are: {string.Join(", ", allowedSubjects)}.";
            return false;
        }

        return true;
    }

    static bool TryParseGrade(string input, out double grade)
    {
        grade = 0;
        return double.TryParse(input, out grade) && grade >= 2 && grade <= 6;
    }
}
