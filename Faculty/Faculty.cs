using System;
using System.Collections.Generic;
using System.Linq;

namespace FacultyLib
{
    public class Faculty
    {
        private List<Speciality> Specialities = new List<Speciality>();
        private int[] LectureAuditories;
        private int[] LabsAuditories;
        private Teacher[] Teachers;
        private Timetable Table;

        public Faculty(int[] aud_lec, int[] aud_lab, Teacher[] teachers)
        {
            LectureAuditories = (int[])aud_lec.Clone();
            LabsAuditories = (int[])aud_lab.Clone();
            Teachers = (Teacher[])teachers.Clone();
            Table = new Timetable(LectureAuditories, LabsAuditories, teachers);
        }

        public int SpecialityNum()
        {
            return Specialities.Count();
        }

        public int FlowsNum()
        {
            int specs = Specialities.Count();
            int flows = 0;
            for (int i = 0; i < specs; i++)
            {
                flows += Specialities[i].Flows.Count();
            }
            return flows;
        }

        public int GroupsNum()
        {
            int specs = Specialities.Count();
            int flows = 0;
            int groups = 0;

            for (int i = 0; i < specs; i++)
            {
                flows += Specialities[i].Flows.Count();
            }

            for (int i = 0; i < specs; i++)
            {
                for (int j = 0; j < flows; j++)
                {
                    groups += Specialities[i].Flows[j].groups.Count();
                }
            }
            return groups;
        }

        public void AddSpeciality(string specName)
        {
            Specialities.Add(new Speciality(specName));
        }

        public void AddFlow(string specName, string flowName, int course)
        {
            int index = Specialities.FindIndex(spec => spec.SpecialityName == specName);
            Specialities[index].AddFlow(flowName, course);
        }

        public void AddGroup(string specName, string flowName, int groupName, int course, string[] students)
        {
            int index1 = Specialities.FindIndex(spec => spec.SpecialityName == specName);
            int index2 = Specialities[index1].Flows.FindIndex(flow => flow.FlowName == flowName && flow.Course == course);
            Specialities[index1].Flows[index2].AddGroup(groupName, students);
        }

        public void AddStudent(string specName, string flowName, int groupName, int course, string student)
        {
            int index1 = Specialities.FindIndex(spec => spec.SpecialityName == specName);
            int index2 = Specialities[index1].Flows.FindIndex(flow => flow.FlowName == flowName && flow.Course == course);
            int index3 = Specialities[index1].Flows[index2].groups.FindIndex(gr => gr.GroupNumber == groupName);
            Specialities[index1].Flows[index2].groups[index3].AddStudent(student);
        }

        public void RemoveStudent(string specName, string flowName, int groupName, int course, string student)
        {

            int index1 = Specialities.FindIndex(spec => spec.SpecialityName == specName);
            int index2 = Specialities[index1].Flows.FindIndex(flow => flow.FlowName == flowName && flow.Course == course);
            int index3 = Specialities[index1].Flows[index2].groups.FindIndex(gr => gr.GroupNumber == groupName);
            if (Specialities[index1].Flows[index2].groups[index3].Students.Contains(student))
            {
                Specialities[index1].Flows[index2].groups[index3].RemoveStudent(student);
            }
            else
            {
                throw new System.InvalidOperationException("There isn`t such a student");
            }
        }

        public void SetSubjects(string specName, string flowName, int course, string[] subjects, int[] lec, int[] lab)
        {
            int index1 = Specialities.FindIndex(spec => spec.SpecialityName == specName);
            int index2 = Specialities[index1].Flows.FindIndex(flow => flow.FlowName == flowName && flow.Course == course);
            for (int i = 0; i < subjects.Length; i++)
            {
                Specialities[index1].Flows[index2].AddSubject(subjects[i], lec[i], lab[i]);
            }
        }

        public int AllStudents()
        {
            int total = 0;
            foreach (Speciality spec in Specialities)
            {
                foreach (Flow flow in spec.Flows)
                {
                    foreach (Group group in flow.groups)
                    {
                        total += group.Students.Count();
                    }
                }
            }
            return total;
        }

        public void SetSchedule(string specName, string flowName, int course)
        {
            int index1 = Specialities.FindIndex(spec => spec.SpecialityName == specName);
            int index2 = Specialities[index1].Flows.FindIndex(flow => flow.FlowName == flowName && flow.Course == course);
            Table.GenTable(Specialities[index1].Flows[index2].subjects, Specialities[index1].Flows[index2]);
        }

        public string GetTable(string flow, int group, string day, int classNumber)
        {
            string info = "";
            if (Table.Table[day][classNumber].groups.Contains((flow, group)))
            {
                Class lesson = Table.Table[day][classNumber].Classes.Find(x => x.Group.Any(y => y == (flow, group)));
                info = lesson.Subject + " " + lesson.Type + " Aud: " + lesson.Auditory + " Teacher: " + lesson.Teacher;
            }
            return info;
        }
    }

    public struct Teacher
    {
        internal string Name;
        internal string Subject;
        internal int Hours;
        internal List<(string, int)> Lessons;
        public Teacher(string name, string subject, int hours)
        {
            Lessons = new List<(string, int)>();
            Name = name;
            Subject = subject;
            Hours = hours;
        }
    }
}
