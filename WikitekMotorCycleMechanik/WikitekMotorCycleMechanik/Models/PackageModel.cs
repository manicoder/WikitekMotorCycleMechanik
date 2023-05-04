using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace WikitekMotorCycleMechanik.Models
{
    public class PackageModel
    {
        public int count { get; set; }
        public object next { get; set; }
        public object previous { get; set; }
        public List<PackageResult> results { get; set; }
        public string error { get; set; }
        public HttpStatusCode status_code { get; set; }
    }

    public class Country
    {
        public int id { get; set; }
        public string name { get; set; }
        public object country_code { get; set; }
        public object currency_code { get; set; }
        public string is_active { get; set; }
        public object slug { get; set; }
    }

    public class PackageResult
    {
        public string id { get; set; }
        public object slug { get; set; }
        public string code { get; set; }
        public Country country { get; set; }
        //public int packages { get; set; }
        public Segments segments { get; set; }
        public PackageParms packages { get; set; }
        public double amount { get; set; }
        public object cost { get; set; }
        public int recurrence_period { get; set; }
        public string recurrence_unit { get; set; }
        public string description { get; set; }
        public int grace_period { get; set; }
        public bool is_active { get; set; }
        public List<int> oem { get; set; }
        public bool oem_specific { get; set; }
        public int usability { get; set; }
        public int package_visibility { get; set; }
    }

    //public class Segments
    //{
    //    public int id { get; set; }
    //    public string segment_name { get; set; }
    //}

    public class PackageParms
    {
        public List<int> runtime_params { get; set; }
    }

    //public class PackageResult
    //{
    //    public string id { get; set; }
    //    public object slug { get; set; }
    //    public string code { get; set; }
    //    public Country country { get; set; }
    //    public int packages { get; set; }
    //    public string segments { get; set; }
    //    public double amount { get; set; }
    //    public object cost { get; set; }
    //    public int recurrence_period { get; set; }
    //    public string recurrence_unit { get; set; }
    //    public string description { get; set; }
    //    public int grace_period { get; set; }
    //    public bool is_active { get; set; }
    //    public List<Oem> oem { get; set; }
    //    public bool oem_specific { get; set; }
    //    public int usability { get; set; }
    //    public int package_visibility { get; set; }
    //}


    #region [Get Subcription Models inside Invantab]
    public class SubcribtionModel
    {
        public bool status { get; set; }
        public string message { get; set; }
        public int count { get; set; }
        public object next { get; set; }
        public object previous { get; set; }
        public List<SubcribtionResult> results { get; set; }
    }

    public class SubcribtionResult
    {
        public PartNo part_no { get; set; }
        public string fg_serial_no { get; set; }
        public List<BatchSerialno> batch_serialno { get; set; }
    }

    public class BatchSerialno
    {
        public PartNo part_no { get; set; }
        public string serial_number { get; set; }
    }

    public class PartNo
    {
        public string id { get; set; }
        public string part_number { get; set; }
    }

    //public class BatchName
    //{
    //    public string id { get; set; }
    //    public string fg_serial_no { get; set; }
    //    public List<BatchSerialno> batch_serialno { get; set; }
    //}

    #endregion

    #region ["Subscriptions Link Multiple Subscription]
    public class UserSubsLink
    {
        public bool success { get; set; }
        public string status { get; set; }
    }

    #endregion
}
