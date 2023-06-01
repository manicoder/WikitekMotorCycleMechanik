using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace WikitekMotorCycleMechanik.StaticInfo
{
    class StaticMethods
    {
        #region Properties
        public static string token = string.Empty;
        public static string last_page = string.Empty;
        public static List<EcuDataSet> ecu_info = new List<EcuDataSet>();
        #endregion

        #region Http Status Code Method

        public static string http_status_code(HttpStatusCode status_code)
        {
            try
            {
                string status_value = string.Empty;
                switch (status_code)
                {
                    case HttpStatusCode.OK:
                        //status_value = 200;
                        break;

                    case HttpStatusCode.Unauthorized:
                        status_value = "Unauthorized";
                        break;

                    case HttpStatusCode.Forbidden:
                        status_value = "Forbidden";
                        break;

                    case HttpStatusCode.NotFound:
                        status_value = "Not Found";
                        break;

                    case HttpStatusCode.MethodNotAllowed:
                        status_value = "Method Not Allowed";
                        break;

                    case HttpStatusCode.NotAcceptable:
                        status_value = "Not Acceptable";
                        break;

                    case HttpStatusCode.RequestTimeout:
                        status_value = "Request Timeout";
                        break;

                    case HttpStatusCode.InternalServerError:
                        status_value = "Internal Server Error";
                        break;

                    case HttpStatusCode.NotImplemented:
                        status_value = "Not Implemented";
                        break;

                    case HttpStatusCode.BadGateway:
                        status_value = "Bad Gateway";
                        break;

                    case HttpStatusCode.ServiceUnavailable:
                        status_value = "Service Unavailable";
                        break;

                    case HttpStatusCode.GatewayTimeout:
                        status_value = "Gateway Timeout";
                        break;

                    case HttpStatusCode.Conflict:
                        status_value = "Conflict";
                        break;

                    case HttpStatusCode.Continue:
                        status_value = "Continue";
                        break;

                    case HttpStatusCode.SwitchingProtocols:
                        status_value = "Switching Protocols";
                        break;

                    case HttpStatusCode.Accepted:
                        status_value = "Gateway Timeout";
                        break;

                    case HttpStatusCode.NonAuthoritativeInformation:
                        status_value = "Non Authoritative Information";
                        break;

                    case HttpStatusCode.NoContent:
                        status_value = "No Content";
                        break;

                    case HttpStatusCode.ResetContent:
                        status_value = "Reset Content";
                        break;

                    case HttpStatusCode.PartialContent:
                        status_value = "Partial Content";
                        break;

                    case HttpStatusCode.MultipleChoices:
                        status_value = "Multiple Choices";
                        break;

                    case HttpStatusCode.MovedPermanently:
                        status_value = "Moved Permanently";
                        break;

                    case HttpStatusCode.Found:
                        status_value = "Found";
                        break;

                    case HttpStatusCode.SeeOther:
                        status_value = "See Other";
                        break;

                    case HttpStatusCode.NotModified:
                        status_value = "Not Modified";
                        break;

                    case HttpStatusCode.UseProxy:
                        status_value = "Use Proxy";
                        break;

                    case HttpStatusCode.Unused:
                        status_value = "Unused";
                        break;

                    case HttpStatusCode.TemporaryRedirect:
                        status_value = "Temporary Redirect";
                        break;

                    case HttpStatusCode.BadRequest:
                        status_value = "Bad Request";
                        break;

                    default:
                        status_value = "Intertnet not connected";
                        break;

                }

                return status_value;
            }
            catch (Exception ex)
            {
                return "Invalid Opration";
            }
        }

        #endregion
    }

    public class EcuDataSet
    {
        public string ecu_name { get; set; }//public static int pid_dataset_id = 0;
        public int dtc_dataset_id { get; set; }
        public int pid_dataset_id { get; set; }
        public string clear_dtc_index { get; set; }
        public string read_dtc_index { get; set; }
        public string read_data_fn_index { get; set; }
        public string tx_header { get; set; }
        public string rx_header { get; set; }
        public string protocol{ get; set; }
        public bool? is_padding { get; set; }
        //public string seed_key_index { get; set; }
    }
}
