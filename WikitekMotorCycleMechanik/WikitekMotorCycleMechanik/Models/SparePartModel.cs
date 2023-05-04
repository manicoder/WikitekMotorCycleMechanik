using System;
using System.Collections.Generic;
using System.Text;

namespace WikitekMotorCycleMechanik.Models
{
    public class SparePartModel
    {
        public string status { get; set; }
        public bool success { get; set; }

        public int count { get; set; }
        public object next { get; set; }
        public object previous { get; set; }
        public List<SparePartResult> results { get; set; }
    }

    public class SparePartResult
    {
        public int oem { get; set; }
        public int model { get; set; }
        public int s_model { get; set; }
        public int year { get; set; }
        public List<SparepartModel> sparepart_model { get; set; }
    }

    public class Sparepart1
    {
        public int id { get; set; }
        public string sparepart_no { get; set; }
        public string is_active { get; set; }
    }

    public class SparepartModel
    {
        public SparepartType sparepart_type { get; set; }
        public Sparepart1 sparepart { get; set; }
        public string charge { get; set; }
        public string is_active { get; set; }
        public bool selected { get; set; }
    }

    public class SparepartType
    {
        public int id { get; set; }
        public string sparepart_category { get; set; }
        public string is_active { get; set; }
    }



    public class AddSparePart
    {
        public string job_card { get; set; }
        public int sparepart { get; set; }
        public int quantity { get; set; }
        public string desc { get; set; }
        public string customer_check { get; set; }
        public string service_check { get; set; }
        public string entry_check { get; set; }
        public string exit_check { get; set; }
        public string status { get; set; }
        public string unit_price { get; set; }
    }

    public class AddSparePartResponse
    {
        public string error { get; set; }
        public bool success { get; set; }
        public string id { get; set; }
        public string job_card { get; set; }
        public int sparepart { get; set; }
        public int quantity { get; set; }
        public string desc { get; set; }
        public string customer_check { get; set; }
        public string service_check { get; set; }
        public string entry_check { get; set; }
        public string exit_check { get; set; }
        public string status { get; set; }
        public int unit_price { get; set; }
    }

    public class UpdateSparePart
    {
        public string id { get; set; }
        public string customer_check { get; set; }
        public string service_check { get; set; }
        public string entry_check { get; set; }
        public string exit_check { get; set; }
    }

    public class UpdateSparePartResponse
    {
        public int id { get; set; }
        public object job_card { get; set; }
        public object symptom { get; set; }
        public string customer_check { get; set; }
        public string entry_check { get; set; }
        public string service_check { get; set; }
        public string exit_check { get; set; }
        public string status { get; set; }

        public bool success { get; set; }
        public string error { get; set; }
    }

    public class AddPickupDropModel
    {
        public string id { get; set; }
        public string technician { get; set; }
        public string panned_time { get; set; }
        public string actual_time { get; set; }
    }

    public class AddPickupModelResponse
    {
        public bool success { get; set; }
        public string status { get; set; }
    }

    public class AddEntryExitCheckModel
    {
        public string job_card { get; set; }
        public string type { get; set; }
        public string technician { get; set; }
        public string panned_time { get; set; }
        public string actual_time { get; set; }
        public string status { get; set; }

    }
}
