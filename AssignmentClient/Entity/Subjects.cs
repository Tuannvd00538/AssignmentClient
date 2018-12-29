using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssignmentClient.Entity
{
    public class Subjects
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public string subjects { get; set; }
    }

    public class SubjectsManager
    {
        public static List<Subjects> GetSubject()
        {
            var subjects = new List<Subjects>();

            subjects.Add(new Subjects { Name = "A", Id = 2, subjects = "PHP" });
            subjects.Add(new Subjects { Name = "B", Id = 2, subjects = "PHP" });
            subjects.Add(new Subjects { Name = "C", Id = 2, subjects = "PHP" });
            subjects.Add(new Subjects { Name = "D", Id = 2, subjects = "PHP" });
            subjects.Add(new Subjects { Name = "E", Id = 2, subjects = "PHP" });

            return subjects;
        }
    }
}
