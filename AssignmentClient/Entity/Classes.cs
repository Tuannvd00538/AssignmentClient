using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssignmentClient.Entity
{
    public class Classes
    {
        public string ClassName { get; set; }
        public string Links { get; set; }
    }

   


    public class ClassManager
    {
        public static List<Classes> GetClasses()
        {
            var classes = new List<Classes>();
            classes.Add(new Classes { ClassName = "T1707A", Links = "Xem chi tiết"});
            classes.Add(new Classes { ClassName = "T1708E", Links = "Xem chi tiết" });
            classes.Add(new Classes { ClassName = "T505M", Links = "Xem chi tiết" });
            classes.Add(new Classes { ClassName = "T1709M", Links = "Xem chi tiết" });
            classes.Add(new Classes { ClassName = "A1509M", Links = "Xem chi tiết" });
            classes.Add(new Classes { ClassName = "A1707E", Links = "Xem chi tiết" });
            return classes;
        }
    }
}
