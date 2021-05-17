using System;
using System.Collections.Generic;

namespace FacultyLib
{
    class Group
    {
        private int Course;
        internal string FlowName;
        internal int GroupNumber;
        internal List<string> Students;

        public Group(int course, string flowName, int groupNumber, string[] students)
        {
            Course = course;
            FlowName = flowName;
            GroupNumber = groupNumber;
            Students = new List<string>(students);
        }

        public void RemoveStudent(string studentName)
        {
            if (Students.Contains(studentName))
            {
                Students.Remove(studentName);
            }
        }

        public void AddStudent(string name)
        {
            Students.Add(name);
            Students.Sort();
        }
    }
}
