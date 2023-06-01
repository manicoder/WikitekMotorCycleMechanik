using SQLite;
//using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace WikitekMotorCycleMechanik.Models
{
    public class OemModel
    {
        public int count { get; set; }
        public string next { get; set; }
        public object previous { get; set; }
        public List<OemResult> results { get; set; }
        public string detail { get; set; }
        public HttpStatusCode status_code { get; set; }
    }

    public class OemResult
    {
        [PrimaryKey, AutoIncrement]
        public int pk_id { get; set; } 
        public int id { get; set; }
        public string name { get; set; }

        //[OneToMany(CascadeOperations = CascadeOperation.All)]
        public Segment segment_name { get; set; }
        //public object model_year { get; set; }

        //[OneToMany(CascadeOperations = CascadeOperation.All)]
        public Oem oem { get; set; }
    }

    public class Oem
    {

        public int id { get; set; }
        public string oem_file { get; set; }
        public byte[] oem_file_local { get; set; }
        public string name { get; set; }
        public object slug { get; set; }
        public AttachmentModel attachment { get; set; }

        public Uri oemUri { get; set; }

        //[ForeignKey(typeof(OemResult))]
        public int fk_model_id { get; set; }
    }

    public class Segment
    {
        public int id { get; set; }
        public string segment_name { get; set; }

        //[ForeignKey(typeof(OemResult))]
        public int fk_model_id { get; set; }
    }

}
