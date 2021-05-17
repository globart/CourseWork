using System;
using System.Collections.Generic;
using System.Linq;

namespace FacultyLib
{
    class Speciality
    {
        internal string SpecialityName;
        internal List<Flow> Flows = new List<Flow>();

        public Speciality(string specialityName)
        {
            SpecialityName = specialityName;
        }

        public void AddFlow(string FlowName, int Course)
        {
            Flows.Add(new Flow(FlowName, Course));
        }
    }
}
