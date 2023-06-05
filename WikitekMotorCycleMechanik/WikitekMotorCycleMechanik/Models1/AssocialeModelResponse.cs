using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace WikitekMotorCycleMechanik.Models1
{


    public class AssocialeModelResponse
    {
        public int id { get; set; }
        public int workshop { get; set; }
        public string vehicle { get; set; }
        public string user { get; set; }
        public string message { get; set; }
        public HttpStatusCode status_code { get; set; }

    }


}
