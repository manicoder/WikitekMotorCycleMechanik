using System;
using System.Collections.Generic;
using System.Text;

namespace WikitekMotorCycleMechanik.Models
{
    public class CollaborateModel
    {
        public string type { get; set; }
    }

    public class CreateCollaborateModel
    {
        public string user { get; set; }
        public string Pid_tag { get; set; }
        public string Tweet_tag { get; set; }
        public string hashtags { get; set; }
        public string video { get; set; }
        public string image { get; set; }
        public string parent { get; set; }
        public string reply_to { get; set; }
        public string timestamp { get; set; }
    }
}
