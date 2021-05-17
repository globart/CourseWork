using System;
using System.Collections.Generic;
using System.Linq;

namespace FacultyLib
{
    class Flow
    {
        internal string FlowName;
        internal int Course;
        internal List<Group> groups = new List<Group>();
        internal Dictionary<string, (int, int)> subjects = new Dictionary<string, (int, int)>();

        public Flow(string flowName, int course)
        {
            FlowName = flowName;
            Course = course;
        }

        public void AddGroup(int groupNumber, string[] students)
        {
            groups.Add(new Group(groupNumber, FlowName, Course, students));
            groups[groups.Count - 1].Students = new List<string>(students);
            groups[groups.Count - 1].Students.Sort();
        }

        public void AddSubject(string SubjectName, int LectureHours, int LabHours)
        {
            subjects.Add(SubjectName, (LectureHours, LabHours));
        }
    }
}
