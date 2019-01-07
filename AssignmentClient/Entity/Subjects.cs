using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssignmentClient.Entity
{
    public class Subjects
    {
        public string SubjectName { get; set; }
    }

    public class SubjectViewModel
    {
        private Subjects defaultRecording = new Subjects();
        public Subjects DefaultRecording { get { return this.defaultRecording; } }

        private ObservableCollection<Subjects> recordings = new ObservableCollection<Subjects>();
        public ObservableCollection<Subjects> Recordings { get { return this.recordings; } }
        public SubjectViewModel()
        {
            this.recordings.Add(new Subjects()
            {
                SubjectName = "PHP",
               
            });
            this.recordings.Add(new Subjects()
            {
                SubjectName = "Java"
              
            });
            this.recordings.Add(new Subjects()
            {
                SubjectName = "ASP.NET Core MVC"
                
            });
        }
    }
}
