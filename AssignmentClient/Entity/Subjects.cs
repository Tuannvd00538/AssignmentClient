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

            subjects.Add(new Subjects { Name = "PHP"});
            subjects.Add(new Subjects { Name = "ASP.NET Core MVC"});
            subjects.Add(new Subjects { Name = "Java"});
            subjects.Add(new Subjects { Name = "C#"});
            subjects.Add(new Subjects { Name = "C++"});

            return subjects;
        }
    }
}
