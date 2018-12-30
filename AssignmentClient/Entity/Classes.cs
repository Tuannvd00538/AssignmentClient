using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssignmentClient.Entity
{
    public class Classes
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string IntendTime { get; set; }
        public int Status { get; set; }

    }

    public class ClassManager
    {
        public static List<Classes> GetClasses()
        {
            var classes = new List<Classes>();

            classes.Add(new Classes { Name = "A", Id = 2, StartTime = "0000", EndTime = "10-10-2012", IntendTime = "000000" });
            classes.Add(new Classes { Name = "B", Id = 3, StartTime = "10-11-2011", EndTime = "10-10-2012", IntendTime = "10-05-2012" });
            classes.Add(new Classes { Name = "B", Id = 4, StartTime = "10-11-2011", EndTime = "10-10-2012", IntendTime = "10-05-2012" });
            classes.Add(new Classes { Name = "C", Id = 5, StartTime = "10-11-2011", EndTime = "10-10-2012", IntendTime = "10-05-2012" });
            classes.Add(new Classes { Name = "D", Id = 6, StartTime = "10-11-2011", EndTime = "10-10-2012", IntendTime = "10-05-2012" });
            classes.Add(new Classes { Name = "E", Id = 7, StartTime = "10-11-2011", EndTime = "10-10-2012", IntendTime = "10-05-2012" });
            return classes;
        }
    }
}
