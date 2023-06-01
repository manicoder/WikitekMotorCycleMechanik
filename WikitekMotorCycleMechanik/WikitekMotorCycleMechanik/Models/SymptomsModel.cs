using System;
using System.Collections.Generic;
using System.Text;

namespace WikitekMotorCycleMechanik.Models
{
    public class SymptomsModel
    {
        public string status { get; set; }
        public bool success { get; set; }
        public int count { get; set; }
        public object next { get; set; }
        public object previous { get; set; }
        public List<Symptoms> results { get; set; }
    }

    public class Symptoms
    {
        public int id { get; set; }
        public object oem { get; set; }
        public int s_model { get; set; }
        public int year { get; set; }
        public List<SymptomRtModel> symptom_rt_model { get; set; }
    }

    public class SymptomType
    {
        public int id { get; set; }
        public string symptom_type { get; set; }
    }

    public class Symptom
    {
        public int id { get; set; }
        public string symptom_name { get; set; }
    }

    public class SymptomRtModel
    {
        public int id { get; set; }
        public SymptomType symptom_type { get; set; }
        public Symptom symptom { get; set; }

        public bool selected { get; set; }
    }

    public class AddSymptom
    {
        public string job_card { get; set; }
        public int symptom { get; set; }
        public string customer_check { get; set; }
        public string service_check { get; set; }
        public string entry_check { get; set; }
        public string exit_check { get; set; }
        public string status { get; set; }
    }

    public class AddSymptomResponse
    {
        public string error { get; set; }
        public bool success { get; set; }
        public int id { get; set; }
        public string job_card { get; set; }
        public int symptom { get; set; }
        public string customer_check { get; set; }
        public string service_check { get; set; }
        public string entry_check { get; set; }
        public string exit_check { get; set; }
        public string status { get; set; }
    }

    public class UpdateSymptom
    {
        public int id { get; set; }
        public string customer_check { get; set; }
        public string service_check { get; set; }
        public string entry_check { get; set; }
        public string exit_check { get; set; }
    }

    public class UpdateSymptomResponse
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
}

