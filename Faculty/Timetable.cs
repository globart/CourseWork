using System;
using System.Collections.Generic;
using System.Linq;

namespace FacultyLib
{
    class Timetable
    {
        private string[] Days = { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday" };
        internal Dictionary<string, Lessons[]> Table = new Dictionary<string, Lessons[]>();
        private Teacher[] Teachers;

        public Timetable(int[] audLec, int[] audLab, Teacher[] teachers)
        {
            Teachers = teachers;
            List<Lessons> FreeDay = new List<Lessons>();
            for (int i = 0; i < 5; i++)
            {
                FreeDay.Add(new Lessons(audLec, audLab));
            }

            Table.Add("Monday", FreeDay.Select(_ => new Lessons(audLec, audLab)).ToArray());
            Table.Add("Tuesday", FreeDay.Select(_ => new Lessons(audLec, audLab)).ToArray());
            Table.Add("Wednesday", FreeDay.Select(_ => new Lessons(audLec, audLab)).ToArray());
            Table.Add("Thursday", FreeDay.Select(_ => new Lessons(audLec, audLab)).ToArray());
            Table.Add("Friday", FreeDay.Select(_ => new Lessons(audLec, audLab)).ToArray());

        }

        private (string, int) FindFreeClass(Group curGroup, Flow curFlow, string subject, string type)
        {
            if (type == "Lecture")
            {
                for (int i = 0; i < 5; i++)
                {
                    bool cond;
                    foreach (string day in Days)
                    {
                        cond = true;
                        if (Table[day][i].AuditoryLec.Count != 0)
                        {
                            foreach (Group gr in curFlow.groups)
                            {
                                if (!(Table[day][i].groups.All(x => x != (gr.FlowName, gr.GroupNumber))))
                                {
                                    cond = false;
                                }
                            }
                        }

                        if (cond)
                        {
                            if (Teachers.Any(x => x.Subject == subject && x.Hours > 0 && !x.Lessons.Contains((day, i))))
                            {
                                return (day, i);
                            }
                        }
                    }
                }
            }


            else if (type == "Laba")
            {
                for (int i = 0; i < 5; i++)
                {
                    foreach (string day in Days)
                    {
                        if (Table[day][i].AuditoryLab.Count != 0)
                        {
                            if (Table[day][i].groups.All(x => x != (curGroup.FlowName, curGroup.GroupNumber)))
                            {
                                if (Teachers.Any(x => x.Subject == subject && x.Hours > 0 && !x.Lessons.Contains((day, i))))
                                {
                                    return (day, i);
                                }
                            }

                        }
                    }
                }
            }
            return ("None", 0);
        }

        public void GenTable(Dictionary<string, (int, int)> subj, Flow curFlow)
        {
            int lecH, labH, classNumber, aud;
            string day;
            Teacher teacher;
            foreach (var item in subj)
            {
                (lecH, labH) = item.Value;
                for (int i = 0; i < lecH; i++)
                {
                    (day, classNumber) = FindFreeClass(curFlow.groups[0], curFlow, item.Key, "Lecture");

                    if (day != "None")
                    {
                        Table[day][classNumber].flows.Add(curFlow.FlowName);

                        aud = Table[day][classNumber].AuditoryLec[0];
                        Table[day][classNumber].AuditoryLec.RemoveAt(0);

                        teacher = Teachers.FirstOrDefault(x => x.Subject == item.Key && x.Hours > 0 && !x.Lessons.Contains((day, classNumber)));
                        teacher.Lessons.Add((day, classNumber));
                        Teachers[Array.IndexOf(Teachers, teacher)].Hours -= 1;
                        Table[day][classNumber].Classes.Add(new Class(item.Key, aud, curFlow.FlowName, "Lecture", teacher.Name));

                        foreach (Group gr in curFlow.groups)
                        {
                            Table[day][classNumber].groups.Add((gr.FlowName, gr.GroupNumber));
                            Table[day][classNumber].Classes[Table[day][classNumber].Classes.Count - 1].Group.Add((curFlow.FlowName, gr.GroupNumber));
                        }
                    }
                    else
                    {
                        throw new InvalidOperationException("There aren`t free classes");
                    }
                }

                for (int i = 0; i < labH; i++)
                {
                    foreach (Group gr in curFlow.groups)
                    {
                        (day, classNumber) = FindFreeClass(gr, curFlow, item.Key, "Laba");
                        if (day != "None")
                        {
                            Table[day][classNumber].flows.Add(curFlow.FlowName);

                            aud = Table[day][classNumber].AuditoryLab[0];
                            Table[day][classNumber].AuditoryLab.RemoveAt(0);

                            teacher = Teachers.FirstOrDefault(x => x.Subject == item.Key && x.Hours > 0 && !x.Lessons.Contains((day, classNumber)));
                            teacher.Lessons.Add((day, classNumber));
                            Teachers[Array.IndexOf(Teachers, teacher)].Hours -= 1;
                            Table[day][classNumber].Classes.Add(new Class(item.Key, aud, curFlow.FlowName, "Laba", teacher.Name));
                            Table[day][classNumber].Classes[Table[day][classNumber].Classes.Count - 1].Group.Add((curFlow.FlowName, gr.GroupNumber));

                            Table[day][classNumber].groups.Add((curFlow.FlowName, gr.GroupNumber));
                        }
                        else
                        {
                            Console.WriteLine(item.Key);
                        }
                    }
                }

            }

        }
    }

    struct Lessons
    {
        internal List<(string, int)> groups;
        internal List<string> flows;
        internal List<Class> Classes;
        internal List<int> AuditoryLec;
        internal List<int> AuditoryLab;

        public Lessons(int[] auditoryLec, int[] auditoryLab)
        {
            AuditoryLec = new List<int>(auditoryLec);
            AuditoryLab = new List<int>(auditoryLab);
            flows = new List<string>();
            groups = new List<(string, int)>();
            Classes = new List<Class>();
        }
    }

    struct Class
    {
        internal string Subject;
        internal int Auditory;
        public List<(string, int)> Group;
        internal string Flow;
        internal string Type;
        internal string Teacher;

        public Class(string subject, int auditory, string flow, string type, string teacher)
        {
            Subject = subject;
            Flow = flow;
            Group = new List<(string, int)>();
            Auditory = auditory;
            Type = type;
            Teacher = teacher;
        }
    }
}
