using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using WikitekMotorCycleMechanik.Models;
using WikitekMotorCycleMechanik.Models1;
using Xamarin.Essentials;

namespace WikitekMotorCycleMechanik
{
    public class ApiServices1
    {
        //string base_url = "https://wikitek.io/api/v1/";//Original Server
        //string base_url = "http://128.199.17.43/api/v1/";//Test Server
        string url = string.Empty;
        string invantab_url = "http://inventab.io/";
        string inventabBaseUrl = "http://inventab.io/api/v1/";
        HttpClient client;


        public ApiServices1()
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            client = new HttpClient(clientHandler);
            //client = new HttpClient(new System.Net.Http.HttpClientHandler());
        }
        public async Task<AssocialeModelResponse> AssociateVehicle(AssociatevehicleModel model)
        {
            HttpResponseMessage httpResponse = new HttpResponseMessage();
            try
            {
                var token = Preferences.Get("token", null);

                string Data = string.Empty;
                //client = new HttpClient();
                var json = JsonConvert.SerializeObject(model);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("JWT", token);
                httpResponse = await client.PostAsync($"{App.base_url}workshops/associate-vehicle/", content);
                Data = httpResponse.Content.ReadAsStringAsync().Result;
                var countries = JsonConvert.DeserializeObject<AssocialeModelResponse>(Data);
                countries.status_code = httpResponse.StatusCode;
                Preferences.Set("associatevehicle", countries.id);
                return countries;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<AssocialeModelResponse> AddTechnician(TechnicianvehicleModel model)
        {
            HttpResponseMessage httpResponse = new HttpResponseMessage();
            try
            {
                var token = Preferences.Get("token", null);

                string Data = string.Empty;
                //client = new HttpClient();
                var json = JsonConvert.SerializeObject(model);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("JWT", token);
                httpResponse = await client.PostAsync($"{App.base_url}workshops/associate-technician/", content);
                Data = httpResponse.Content.ReadAsStringAsync().Result;
                var countries = JsonConvert.DeserializeObject<AssocialeModelResponse>(Data);
                countries.status_code = httpResponse.StatusCode;
                Preferences.Set("associatevehicle", countries.id);
                return countries;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public async Task<AssocialeModelResponse> AssignTechnician(AssignTechnicianVehicleModel model)
        {
            HttpResponseMessage httpResponse = new HttpResponseMessage();
            try
            {
                var token = Preferences.Get("token", null);

                string Data = string.Empty;
                //client = new HttpClient();
                var json = JsonConvert.SerializeObject(model);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("JWT", token);
                httpResponse = await client.PostAsync($"{App.base_url}workshops/vehicle-technician-associate/", content);
                Data = httpResponse.Content.ReadAsStringAsync().Result;
                var countries = JsonConvert.DeserializeObject<AssocialeModelResponse>(Data);
                countries.status_code = httpResponse.StatusCode;
                Preferences.Set("associatevehicle", countries.id);
                return countries;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<AssocialeModelResponse> VehicleTechnicianAssociation(AssignTechnicianVehicleModel model)
        {
            HttpResponseMessage httpResponse = new HttpResponseMessage();
            try
            {
                var token = Preferences.Get("token", null);

                string Data = string.Empty;
                //client = new HttpClient();
                var json = JsonConvert.SerializeObject(model);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("JWT", token);
                httpResponse = await client.PostAsync($"{App.base_url}workshops/vehicle-technician-associate/", content);
                Data = httpResponse.Content.ReadAsStringAsync().Result;
                var countries = JsonConvert.DeserializeObject<AssocialeModelResponse>(Data);
                countries.status_code = httpResponse.StatusCode;
                Preferences.Set("associatevehicle", countries.id);
                return countries;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<ErroMsg> ConfirmAssociateVehicle(SentOtpVehicle model)
        {
            HttpResponseMessage httpResponse = new HttpResponseMessage();
            try
            {
                var token = Preferences.Get("token", null);

                string Data = string.Empty;
                //client = new HttpClient();
                var json = JsonConvert.SerializeObject(model);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("JWT", token);
                httpResponse = await client.PostAsync($"{App.base_url}workshops/confirm-vehicle-association/", content);
                Data = httpResponse.Content.ReadAsStringAsync().Result;
                var countries = JsonConvert.DeserializeObject<ErroMsg>(Data);
               
                 return countries;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<ErroMsg> ConfirmTechnicianAssociateVehicle(SentOtpTechnician model)
        {
            HttpResponseMessage httpResponse = new HttpResponseMessage();
            try
            {
                var token = Preferences.Get("token", null);

                string Data = string.Empty;
                //client = new HttpClient();
                var json = JsonConvert.SerializeObject(model);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("JWT", token);
                httpResponse = await client.PostAsync($"{App.base_url}workshops/confirm-technician-association/", content);
                Data = httpResponse.Content.ReadAsStringAsync().Result;
                var countries = JsonConvert.DeserializeObject<ErroMsg>(Data);

                return countries;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<ErroMsg> ConfirmVehicleTechnicianAssociateVehicle(SentOtpVehicle model)
        {
            HttpResponseMessage httpResponse = new HttpResponseMessage();
            try
            {
                var token = Preferences.Get("token", null);

                string Data = string.Empty;
                //client = new HttpClient();
                var json = JsonConvert.SerializeObject(model);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("JWT", token);
                httpResponse = await client.PostAsync($"{App.base_url}workshops/confirm-vehicle-technician-association/", content);
                Data = httpResponse.Content.ReadAsStringAsync().Result;
                var countries = JsonConvert.DeserializeObject<ErroMsg>(Data);

                return countries;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<ErroMsg> ConfirmVehicleTechnicianAssociation(SentOtpVehicle model)
        {
            HttpResponseMessage httpResponse = new HttpResponseMessage();
            try
            {
                var token = Preferences.Get("token", null);

                string Data = string.Empty;
                //client = new HttpClient();
                var json = JsonConvert.SerializeObject(model);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("JWT", token);
                httpResponse = await client.PostAsync($"{App.base_url}workshops/confirm-vehicle-technician-association/", content);
                Data = httpResponse.Content.ReadAsStringAsync().Result;
                var countries = JsonConvert.DeserializeObject<ErroMsg>(Data);

                return countries;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<ErroMsg> ConfirmDeAssociateVehicle(SentOtpVehicle model)
        {
            HttpResponseMessage httpResponse = new HttpResponseMessage();
            try
            {
                var token = Preferences.Get("token", null);

                string Data = string.Empty;
                //client = new HttpClient();
                var json = JsonConvert.SerializeObject(model);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("JWT", token);
                httpResponse = await client.PostAsync($"{App.base_url}workshops/confirm-vehicle-disassociation/", content);
                Data = httpResponse.Content.ReadAsStringAsync().Result;
                var countries = JsonConvert.DeserializeObject<ErroMsg>(Data);

                return countries;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<NewVehicle> VehicleList()
        {
            HttpResponseMessage httpResponse = new HttpResponseMessage();
            try
            {
                var token = Preferences.Get("token", null);

                string Data = string.Empty;
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("JWT", token);
                httpResponse = await client.GetAsync($"{App.base_url}vehicles/list/");
                Data = httpResponse.Content.ReadAsStringAsync().Result;
                var vehicles = JsonConvert.DeserializeObject<NewVehicle>(Data);
                return vehicles;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<NewTechnicanModel> TechnicianList()
        {
            HttpResponseMessage httpResponse = new HttpResponseMessage();
            try
            {
                var token = Preferences.Get("token", null);

                string Data = string.Empty;
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("JWT", token);
                httpResponse = await client.GetAsync($"{App.base_url}users/get-unassociated-user");
                Data = httpResponse.Content.ReadAsStringAsync().Result;
                var Technicans = JsonConvert.DeserializeObject<NewTechnicanModel>(Data);
                return Technicans;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<ErroMsg> ConfirmDeAssociateTechnician(SentOtpVehicle model)
        {
            HttpResponseMessage httpResponse = new HttpResponseMessage();
            try
            {
                var token = Preferences.Get("token", null);

                string Data = string.Empty;
                //client = new HttpClient();
                var json = JsonConvert.SerializeObject(model);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("JWT", token);
                httpResponse = await client.PostAsync($"{App.base_url}workshops/confirm-vehicle-disassociation/", content);
                Data = httpResponse.Content.ReadAsStringAsync().Result;
                var countries = JsonConvert.DeserializeObject<ErroMsg>(Data);

                return countries;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<ErroMsg> Techniciandisassociate(DeAssociatevehicleModel model)
        {
            HttpResponseMessage httpResponse = new HttpResponseMessage();
            try
            {
                var token = Preferences.Get("token", null);

                string Data = string.Empty;
                //client = new HttpClient();
                var json = JsonConvert.SerializeObject(model);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("JWT", token);
                httpResponse = await client.PostAsync($"{App.base_url}workshops/technician-disassociate/", content);
                Data = httpResponse.Content.ReadAsStringAsync().Result;
                var countries = JsonConvert.DeserializeObject<ErroMsg>(Data);

                return countries;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<ErroMsg> VechicleDeAssociate(DeAssociatevehicleModel model)
        {
            HttpResponseMessage httpResponse = new HttpResponseMessage();
            try
            {
                var token = Preferences.Get("token", null);

                string Data = string.Empty;
                //client = new HttpClient();
                var json = JsonConvert.SerializeObject(model);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("JWT", token);
                httpResponse = await client.PostAsync($"{App.base_url}workshops/vehicle-disassociate/", content);
                Data = httpResponse.Content.ReadAsStringAsync().Result;
                var countries = JsonConvert.DeserializeObject<ErroMsg>(Data);

                return countries;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        //techincian
        //public async Task<AssocialeModelResponse> AssociateTechnicial(technicai model)
        //{
        //    HttpResponseMessage httpResponse = new HttpResponseMessage();
        //    try
        //    {
        //        var token = Preferences.Get("token", null);

        //        string Data = string.Empty;
        //        //client = new HttpClient();
        //        var json = JsonConvert.SerializeObject(model);
        //        var content = new StringContent(json, Encoding.UTF8, "application/json");
        //        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("JWT", token);
        //        httpResponse = await client.PostAsync($"{App.base_url}workshops/associate-vehicle/", content);
        //        Data = httpResponse.Content.ReadAsStringAsync().Result;
        //        var countries = JsonConvert.DeserializeObject<AssocialeModelResponse>(Data);
        //        countries.status_code = httpResponse.StatusCode;
        //        Preferences.Set("associatevehicle", countries.id);
        //        return countries;
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}
    }
}
