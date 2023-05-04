using System;
using System.Collections.Generic;
using System.Text;

namespace WikitekMotorCycleMechanik.Models
{
    public class ServicesModel
    {
        public string status { get; set; }
        public bool success { get; set; }
        public int count { get; set; }
        public object next { get; set; }
        public object previous { get; set; }
        public List<ServicesResult> results { get; set; }
    }

    public class ServicesResult
    {
        public int oem { get; set; }
        public int model { get; set; }
        public int s_model { get; set; }
        public int year { get; set; }
        public List<ServiceModel> service_model { get; set; }
    }

    public class ServiceModel
    {
        public ServiceType service_type { get; set; }
        public Service1 service { get; set; }
        public string charge { get; set; }
        public string is_active { get; set; }
        public bool selected { get; set; }
    }

    public class ServiceType
    {
        public int id { get; set; }
        public string service_category { get; set; }
        public string is_active { get; set; }
    }

    public class Service1
    {
        public int id { get; set; }
        public string service_name { get; set; }
        public string is_active { get; set; }
    }

    public class AddService
    {
        public string job_card { get; set; }
        public int service { get; set; }
        public int quantity { get; set; }
        public string customer_check { get; set; }
        public string service_check { get; set; }
        public string entry_check { get; set; }
        public string exit_check { get; set; }
        public string status { get; set; }
        public string unit_price { get; set; }
    }

    public class AddServiceResponse
    {
        public string id { get; set; }
        public string job_card { get; set; }
        public int service { get; set; }
        public int quantity { get; set; }
        public string customer_check { get; set; }
        public string service_check { get; set; }
        public string entry_check { get; set; }
        public string exit_check { get; set; }
        public string status { get; set; }
        public int unit_price { get; set; }

        public string error { get; set; }
        public bool success { get; set; }
    }

    public class UpdateServices
    {
        public string id { get; set; }
        public string customer_check { get; set; }
        public string service_check { get; set; }
        public string entry_check { get; set; }
        public string exit_check { get; set; }
    }

    public class UpdateServicesResponse
    {
        public string id { get; set; }
        public string customer_check { get; set; }
        public string service_check { get; set; }
        public string entry_check { get; set; }
        public string exit_check { get; set; }

        public bool success { get; set; }
        public string error { get; set; }
    }
}
