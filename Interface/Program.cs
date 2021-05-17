using System;
using System.Linq;
using FacultyLib;

namespace Interface
{
    class Program
    {
        private static Faculty faculty;

        static void Main()
        {
            var mathTeacher = new Teacher("mathTeacher", "math", 200);
            var chemistryTeacher = new Teacher("chemistryTeacher", "chemistry", 300);
            var englishTeacher = new Teacher("englishTeacher", "english", 400);
            var audLec = new[] { 101, 103, 105, 107 };
            var audLab = new[] { 102, 104, 106, 108 };
            var teachers = new[] { mathTeacher, chemistryTeacher, englishTeacher };
            faculty = new Faculty(audLec, audLab, teachers);
            while (true)
            {
                WriteOptions();
                if (ChooseOptions() == 'e')
                {
                    break;
                }
            }
        }

        private static void WriteOptions()
        {
            Console.WriteLine("Choose your option:");
            Console.WriteLine("1. Add speciality");
            Console.WriteLine("2. Add flow");
            Console.WriteLine("3. Add group");
            Console.WriteLine("4. Add student");
            Console.WriteLine("5. Delete student");
            Console.WriteLine("6. Set subjects");
            Console.WriteLine("7. Total number of students");
            Console.WriteLine("8. Set a schedule");
            Console.WriteLine("9. Details of a class");
            Console.WriteLine("e. Exit");
        }

        private static char ChooseOptions()
        {
            Console.Write("\nYour option: ");
            var option = Console.ReadKey().KeyChar;
            Console.WriteLine();

            try
            {
                switch (option)
                {
                    case '1':
                        AddSpeciality();
                        return '1';
                    case '2':
                        if (faculty.SpecialityNum() == 0)
                        {
                            throw new ArgumentException("There aren't any specialities");
                        }
                        else
                        {
                            AddFlow();
                        }
                        return '2';
                    case '3':
                        if (faculty.FlowsNum() == 0)
                        {
                            throw new ArgumentException("There aren't any flows");
                        }
                        else
                        {
                            AddGroup();
                        }
                        return '3';
                    case '4':
                        if (faculty.GroupsNum() == 0)
                        {
                            throw new ArgumentException("There aren't any groups");
                        }
                        else
                        {
                            AddStudent();
                        }
                        return '4';
                    case '5':
                        if(faculty.AllStudents() == 0)
                        {
                            throw new ArgumentException("There aren't any students");
                        }
                        else
                        {
                            RemoveStudent();
                        }
                        return '5';
                    case '6':
                        if (faculty.GroupsNum() == 0)
                        {
                            throw new ArgumentException("There aren't any groups");
                        }
                        else
                        {
                            SetSubjects();
                        }
                        return '6';
                    case '7':
                        AmountOfStudents();
                        return '7';
                    case '8':
                        if (faculty.GroupsNum() == 0)
                        {
                            throw new ArgumentException("There aren't any groups");
                        }
                        else
                        {
                            SetSchedule();
                        }
                        return '8';
                    case '9':
                        if (faculty.GroupsNum() == 0)
                        {
                            throw new ArgumentException("There aren't any groups");
                        }
                        else
                        {
                            GetClassDetails();
                        }
                        return '9';
                    case 'e':
                        return 'e';
                    default:
                        Console.WriteLine("Please, choose the correct option!");
                        return '0';
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return '0';
        }

        private static void AddSpeciality()
        {
            Console.WriteLine("Enter name of a speciality: ");
            string specialityName = Console.ReadLine();
            faculty.AddSpeciality(specialityName);
            Console.WriteLine("Speciality added");
        }
        private static void AddFlow()
        {
            Console.WriteLine("Enter name of a speciality: ");
            string specialityFlowName = Console.ReadLine();
            Console.WriteLine("Enter name of a flow: ");
            string flowName = Console.ReadLine();
            Console.WriteLine("Enter a course number: ");
            int flowCourse = Convert.ToInt32(Console.ReadLine());
            faculty.AddFlow(specialityFlowName, flowName, flowCourse);
            Console.WriteLine("Flow added");
        }
        private static void AddGroup()
        {
            Console.WriteLine("Enter name of a speciality: ");
            string specialityName = Console.ReadLine();
            Console.WriteLine("Enter name of a flow: ");
            string flowName = Console.ReadLine();
            Console.WriteLine("Enter group number: ");
            int groupName = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Enter course number: ");
            int course = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Enter list of students, separated by commas(without spaces after them)");
            string[] students = Console.ReadLine().Split(",");
            faculty.AddGroup(specialityName, flowName, groupName, course, students);
            Console.WriteLine("Group added");
        }
        private static void AddStudent()
        {
            Console.WriteLine("Enter name of a speciality: ");
            string specialityName = Console.ReadLine();
            Console.WriteLine("Enter name of a flow: ");
            string flowName = Console.ReadLine();
            Console.WriteLine("Enter group number: ");
            int groupName = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Enter course number: ");
            int course = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Enter student`s surname, name and patronymic: ");
            string student = Console.ReadLine();
            faculty.AddStudent(specialityName, flowName, groupName, course, student);
            Console.WriteLine("Student added");
        }
        private static void RemoveStudent()
        {
            Console.WriteLine("Enter name of a speciality: ");
            string specialityName = Console.ReadLine();
            Console.WriteLine("Enter name of a flow: ");
            string flowName = Console.ReadLine();
            Console.WriteLine("Enter group number: ");
            int groupName = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Enter course number: ");
            int course = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Enter student`s surname, name and patronymic: ");
            string student = Console.ReadLine();
            faculty.RemoveStudent(specialityName, flowName, groupName, course, student);
            Console.WriteLine("Student deleted");
        }
        private static void SetSubjects()
        {
            Console.WriteLine("Enter name of a speciality: ");
            string specialityName = Console.ReadLine();
            Console.WriteLine("Enter name of a flow: ");
            string flowName = Console.ReadLine();
            Console.WriteLine("Enter course number: ");
            int course = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Enter subjects, separated by commas(without spaces after them): ");
            string[] subjects = Console.ReadLine().Split(",");
            Console.WriteLine("Enter lecture hours, separated by commas(without spaces after them): ");
            int[] lecs = Console.ReadLine().Split(",").Select(x => Convert.ToInt32(x)).ToArray();
            Console.WriteLine("Enter lab hours, separated by commas(without spaces after them): ");
            int[] labs = Console.ReadLine().Split(",").Select(x => Convert.ToInt32(x)).ToArray();
            faculty.SetSubjects(specialityName, flowName, course, subjects, lecs, labs);
            Console.WriteLine("Subjects set");
        }
        private static void AmountOfStudents()
        {
            Console.WriteLine("Amount of students: " + faculty.AllStudents());
        }
        private static void SetSchedule()
        {
            Console.WriteLine("Enter name of a speciality: ");
            string specialityName = Console.ReadLine();
            Console.WriteLine("Enter name of a flow: ");
            string flowName = Console.ReadLine();
            Console.WriteLine("Enter course number: ");
            int course = Convert.ToInt32(Console.ReadLine());
            faculty.SetSchedule(specialityName, flowName, course);
            Console.WriteLine("Schedule set");
        }
        private static void GetClassDetails()
        {
            Console.WriteLine("Enter name of a flow: ");
            string flowName = Console.ReadLine();
            Console.WriteLine("Enter group number: ");
            int groupName = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Enter day: ");
            string day = Console.ReadLine();
            Console.WriteLine("Enter class number: ");
            int classNum = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Class details: " + faculty.GetTable(flowName, groupName, day, classNum));
        }
    }
}
