using WikitekMotorCycleMechanik.Models;
using Newtonsoft.Json;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Diagnostics;
using Xamarin.Essentials;
using WikitekMotorCycleMechanik.Interfaces;
using FFImageLoading;
using Plugin.Connectivity;

namespace WikitekMotorCycleMechanik.Services
{
    public   class ApiServices
    {
        //string base_url = "https://wikitek.io/api/v1/";//Original Server
        //string base_url = "http://128.199.17.43/api/v1/";//Test Server
        string url = string.Empty;
        string invantab_url = "http://inventab.io/";
        string inventabBaseUrl = "http://inventab.io/api/v1/";
        HttpClient client;


        public ApiServices()
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            client = new HttpClient(clientHandler);
            //client = new HttpClient(new System.Net.Http.HttpClientHandler());
        }

        #region Correct Login Method before integrate MAC ID code
        public async Task<LoginResponse> UserLogin1(LoginModel model)
        {
            #region Old Code
            HttpResponseMessage httpResponse = new HttpResponseMessage();
            LoginResponse loginResponse = new LoginResponse();
            string Data = string.Empty;


            try
            {
                //var is_network = NetworkConnection();
                //bool is_online = false;
                //if(!string.IsNullOrEmpty(Preferences.Get("token", null)))
                //{
                //    is_online = true;
                //}
                ////if (is_network)
                //{
                var network = await NetworkConnection();
                if (network)
                {
                    var json = JsonConvert.SerializeObject(model);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    httpResponse = await client.PostAsync($"{App.base_url}users/login/", content);
                    Data = httpResponse.Content.ReadAsStringAsync().Result;
                    loginResponse = JsonConvert.DeserializeObject<LoginResponse>(Data);

                    loginResponse.picture_local = await DownloadImageAsync(loginResponse.picture);
                    if (loginResponse.subscriptions != null && loginResponse.subscriptions.Any())
                    {
                        foreach (var item2 in loginResponse.subscriptions?.ToList())
                        {
                            if (item2.oem != null && item2.oem.Any())
                            {
                                foreach (var item in item2.oem.ToList())
                                {
                                    if (item.attachment != null)
                                    {
                                        //item2.attachment.attachment = "https://wikitek.io" + submodel.attachment.attachment;
                                        item.attachment.attachmentfile_local = await DownloadImageAsync(item.attachment.attachment);
                                    }
                                    else
                                    {
                                        item.attachment = new AttachmentModel();
                                    }
                                    //item.oem_file_local = await DownloadImageAsync("https://wikitek.io"+item.oem_file);
                                }
                            }
                        }
                    }
                    Data = JsonConvert.SerializeObject(loginResponse);

                    Preferences.Set("LoginResponse", Data);
                }
                else
                {
                    loginResponse.error = "No internet connection";
                }
                loginResponse = JsonConvert.DeserializeObject<LoginResponse>(Data);
                loginResponse.status_code = httpResponse.StatusCode;
                Preferences.Set("user_name", loginResponse?.email);
                Preferences.Set("password", model.password);
                Preferences.Set("token", loginResponse.token?.access);
                Preferences.Set("refresh_token", loginResponse.token?.refresh);
                App.user_first_name = loginResponse?.first_name;
                App.user_last_name = loginResponse?.last_name;
                await Application.Current.SavePropertiesAsync();
                App.user_id = loginResponse?.user_id;
                App.user_type = loginResponse?.user_type;
                App.country_id = loginResponse?.agent?.workshop?.country;

                //if (is_online)
                //{
                if (loginResponse.subscriptions != null)
                {
                    App.is_update = false;
                    Preferences.Set("IsUpdate", "false");
                    if (loginResponse.subscriptions.Count > 0)
                    {
                        App.is_update = false;
                        Preferences.Set("IsUpdate", "false");
                    }
                    else
                    {
                        App.is_update = true;
                        Preferences.Set("IsUpdate", "true");
                    }
                }
                else
                {
                    App.is_update = true;
                    Preferences.Set("IsUpdate", "true");
                }

                if (loginResponse.dongles != null)
                {
                    App.is_update = false;
                    Preferences.Set("IsUpdate", "false");
                    if (loginResponse.dongles.Count > 0)
                    {
                        App.is_update = false;
                        Preferences.Set("IsUpdate", "false");
                    }
                    else
                    {
                        App.is_update = true;
                        Preferences.Set("IsUpdate", "true");
                    }
                }
                else
                {
                    App.is_update = true;
                    Preferences.Set("IsUpdate", "true");
                }
                //}
                return loginResponse;

                //}
                //else
                //{
                //    loginResponse.error = "No internet connection";
                //    return loginResponse;
                //}
            }
            catch (Exception ex)
            {
                loginResponse.error = Data;
                loginResponse.status_code = httpResponse.StatusCode;
                //if(ex.Message.Contains("Failed to connect"))
                //{
                //    //loginResponse.status_code;
                //    loginResponse.error = "Please check device internet connection.";

                //}
                //else
                //{
                //    loginResponse.status_code = httpResponse.StatusCode;
                //    loginResponse.error = "Error inside login service";
                //}

                //Debug.WriteLine("Error - inside login api");
                return loginResponse;
            }
            #endregion
        }
        #endregion

        public async Task<LoginResponse> UserLogin(LoginModel model)
        {
            #region Old Code
            HttpResponseMessage httpResponse = new HttpResponseMessage();
            LoginResponse loginResponse = new LoginResponse();
            string Data = string.Empty;


            try
            {
                //var is_network = NetworkConnection();
                //bool is_online = false;
                //if(!string.IsNullOrEmpty(Preferences.Get("token", null)))
                //{
                //    is_online = true;
                //}
                ////if (is_network)
                //{
                var network = await NetworkConnection();
                if (network)
                {
                    var json = JsonConvert.SerializeObject(model);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    httpResponse = await client.PostAsync($"{App.base_url}users/login/new", content);
                    Data = httpResponse.Content.ReadAsStringAsync().Result;
                    loginResponse = JsonConvert.DeserializeObject<LoginResponse>(Data);

                    loginResponse.picture_local = await DownloadImageAsync(loginResponse.picture);

                    if (loginResponse.subscriptions != null && loginResponse.subscriptions.Any())
                    {
                        foreach (var item2 in loginResponse.subscriptions?.ToList())
                        {
                            if (item2.oem != null && item2.oem.Any())
                            {
                                foreach (var item in item2.oem.ToList())
                                {
                                    if (item.attachment != null)
                                    {
                                        //item2.attachment.attachment = "https://wikitek.io" + submodel.attachment.attachment;
                                        item.attachment.attachmentfile_local = await DownloadImageAsync(item.attachment.attachment);
                                    }
                                    else
                                    {
                                        item.attachment = new AttachmentModel();
                                    }
                                    //item.oem_file_local = await DownloadImageAsync("https://wikitek.io"+item.oem_file);
                                }
                            }
                        }
                    }

                    Data = JsonConvert.SerializeObject(loginResponse);

                    Preferences.Set("LoginResponse", Data);
                }
                else
                {
                    loginResponse.error = "No internet connection";
                }
                loginResponse = JsonConvert.DeserializeObject<LoginResponse>(Data);
                loginResponse.status_code = httpResponse.StatusCode;
                //Preferences.Set("user_name", loginResponse?.email);
                //Preferences.Set("password", model.password);
                //Preferences.Set("token", loginResponse.token?.access);
                //Preferences.Set("refresh_token", loginResponse.token?.refresh);
                App.user_first_name = loginResponse?.first_name;
                App.user_last_name = loginResponse?.last_name;
                await Application.Current.SavePropertiesAsync();
                App.user_id = loginResponse?.user_id;
                App.user_type = loginResponse?.user_type;
                App.country_id = loginResponse?.agent?.workshop?.country;

                //if (is_online)
                //{
                if (loginResponse.subscriptions != null)
                {
                    App.is_update = false;
                    Preferences.Set("IsUpdate", "false");
                    if (loginResponse.subscriptions.Count > 0)
                    {
                        App.is_update = false;
                        Preferences.Set("IsUpdate", "false");
                    }
                    else
                    {
                        App.is_update = true;
                        Preferences.Set("IsUpdate", "true");
                    }
                }
                else
                {
                    App.is_update = true;
                    Preferences.Set("IsUpdate", "true");
                }

                if (loginResponse.dongles != null)
                {
                    App.is_update = false;
                    Preferences.Set("IsUpdate", "false");
                    if (loginResponse.dongles.Count > 0)
                    {
                        App.is_update = false;
                        Preferences.Set("IsUpdate", "false");
                    }
                    else
                    {
                        App.is_update = true;
                        Preferences.Set("IsUpdate", "true");
                    }
                }
                else
                {
                    App.is_update = true;
                    Preferences.Set("IsUpdate", "true");
                }
                //}
                return loginResponse;

                //}
                //else
                //{
                //    loginResponse.error = "No internet connection";
                //    return loginResponse;
                //}
            }
            catch (Exception ex)
            {
                loginResponse.error = Data;
                loginResponse.status_code = httpResponse.StatusCode;
                //if(ex.Message.Contains("Failed to connect"))
                //{
                //    //loginResponse.status_code;
                //    loginResponse.error = "Please check device internet connection.";

                //}
                //else
                //{
                //    loginResponse.status_code = httpResponse.StatusCode;
                //    loginResponse.error = "Error inside login service";
                //}

                //Debug.WriteLine("Error - inside login api");
                return loginResponse;
            }
            #endregion
        }

        public async Task<CountyModel> Countries(string name, bool is_updated)
        {
            HttpResponseMessage httpResponse = new HttpResponseMessage();
            try
            {
                string Data = string.Empty;
                //await NetworkConnection();
                //if (App.is_update)
                //{
                url = $"{App.base_url}locations/get-country/";
                httpResponse = await client.GetAsync(url);
                //httpResponse = await client.GetAsync($"{App.base_url}locations/get-country/?name={name}");
                Data = httpResponse.Content.ReadAsStringAsync().Result;
                Preferences.Set("CountryList", Data);
                //}
                //else
                //{
                //    Data = Preferences.Get("CountryList", null);
                //}

                var countries = JsonConvert.DeserializeObject<CountyModel>(Data);
                countries.status_code = httpResponse.StatusCode;
                countries.results = countries.results.Where(x => x.name == name).ToList();
                return countries;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error - inside get country api");
                return null;
            }
        }

        //public async Task<VehicleModel> GetAllModels(string token)
        //{
        //    HttpResponseMessage httpResponse = new HttpResponseMessage();
        //    try
        //    {
        //        string Data = string.Empty;
        //        //client = new HttpClient();
        //        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("JWT", token);
        //        httpResponse = await client.GetAsync($"{App.base_url}models/models");
        //        Data = httpResponse.Content.ReadAsStringAsync().Result;
        //        var agents = JsonConvert.DeserializeObject<VehicleModel>(Data);
        //        agents.status_code = httpResponse.StatusCode;
        //        return agents;
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine("Error - inside get agent api");
        //        return null;
        //    }
        //}

        public async Task<AgentModel> GetAgentList(string type, int country, string pin_code, bool is_updated)
        {
            HttpResponseMessage httpResponse = new HttpResponseMessage();
            try
            {
                string Data = string.Empty;
                //await NetworkConnection();
                if (App.is_update)
                {
                    //client = new HttpClient();
                    httpResponse = await client.GetAsync($"{App.base_url}workshops/get-agent-list/?name={type}&country={country}&pincode={pin_code}");
                    Data = httpResponse.Content.ReadAsStringAsync().Result;
                    Preferences.Set("AgentList", Data);
                }
                else
                {
                    Data = Preferences.Get("AgentList", null);
                }
                var agents = JsonConvert.DeserializeObject<AgentModel>(Data);
                agents.status_code = httpResponse.StatusCode;
                return agents;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error - inside get agent api");
                return null;
            }
        }

        public async Task<DistricModel> DistricAndState(string pin_code, int country_id, string name)
        {
            HttpResponseMessage httpResponse = new HttpResponseMessage();
            try
            {
                string Data = string.Empty;
                ////client = new HttpClient();
                //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("JWT", token);
                httpResponse = await client.GetAsync($"{App.base_url}locations/get-geography-list/?name={name}&pincode={pin_code}&country={country_id}");
                //http://15.206.157.39:8080/api/v1/locations/get-geography-list/?name=515413&country=4&name=other
                Data = httpResponse.Content.ReadAsStringAsync().Result;
                var distric_state = JsonConvert.DeserializeObject<DistricModel>(Data);
                distric_state.status_code = httpResponse.StatusCode;
                var val = distric_state;//results.First().rs_user_type_pincode.First();
                return val;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error - inside get distric and state api");
                return null;
            }
        }

        public async Task<PinCode> DistricAndStateInfo(string pin_code)
        {
            HttpResponseMessage httpResponse = new HttpResponseMessage();
            try
            {
                string Data = string.Empty;
                ////client = new HttpClient();
                //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("JWT", token);
                httpResponse = await client.GetAsync($"{App.base_url}locations/pincode/?name={pin_code}");
                Data = httpResponse.Content.ReadAsStringAsync().Result;
                var distric_state = JsonConvert.DeserializeObject<PinCode>(Data);
                distric_state.status_code = httpResponse.StatusCode;
                var val = distric_state;//results.First().rs_user_type_pincode.First();
                return val;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error - inside get distric and state api");
                return null;
            }
        }

        public async Task<CreateRsAgentResponse> CreateRSAgent(CreateRSAgentModel model)
        {
            HttpResponseMessage httpResponse = new HttpResponseMessage();
            try
            {
                string Data = string.Empty;
                //client = new HttpClient();
                var json = JsonConvert.SerializeObject(model);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("JWT", token);
                httpResponse = await client.PostAsync($"{App.base_url}workshops/agent-create/", content);
                Data = httpResponse.Content.ReadAsStringAsync().Result;
                var countries = JsonConvert.DeserializeObject<CreateRsAgentResponse>(Data);
                countries.status_code = httpResponse.StatusCode;
                return countries;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #region Old
        //public async Task<VehicleModel> GetModelByOem(string token, int oem_id, bool is_update)
        //{
        //    HttpResponseMessage http_response = new HttpResponseMessage();
        //    try
        //    {
        //        string Data = string.Empty;
        //        VehicleModel models = new VehicleModel();
        //        //await NetworkConnection();
        //        if (App.is_update)
        //        {
        //            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("JWT", token);
        //            url = $"{App.base_url}models/models-local/";
        //            http_response = await client.GetAsync(url);
        //            //http_response = await client.GetAsync($"{App.base_url}models/models/?oem={oem_name}");
        //            Data = http_response.Content.ReadAsStringAsync().Result;
        //            models = JsonConvert.DeserializeObject<VehicleModel>(Data);

        //            foreach (var item in models.data)
        //            {
        //                item.model_file_lacal= await DownloadImageAsync(item.model_file);
        //            }

        //            Data = JsonConvert.SerializeObject(models);

        //            Preferences.Set("ModelList", Data);
        //        }
        //        else
        //        {
        //            Data = Preferences.Get("ModelList", null);
        //            models = JsonConvert.DeserializeObject<VehicleModel>(Data);
        //        }
        //        models.status_code = http_response.StatusCode;

        //        models.data = models.results.Where(x => x.oem.id == oem_id).ToList();
        //        return models;//.results.FirstOrDefault(x => x.id == id);
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}
        #endregion

        public async Task<VehicleModel> GetModelByOem(string token, ViewModels.GetModel model, bool is_update)
        {
            HttpResponseMessage http_response = new HttpResponseMessage();
            try
            {
                string Data = string.Empty;
                VehicleModel models = new VehicleModel();
                //await NetworkConnection();
                if (App.is_update)
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("JWT", token);
                    //url = $"{App.base_url}models/models/";
                    //url = $"{App.base_url}models/models-local/";
                    url = $"{App.base_url}models/list/oem";
                    var json = JsonConvert.SerializeObject(model);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    http_response = await client.PostAsync(url, content);
                    //http_response = await client.GetAsync($"{App.base_url}models/models/?oem={oem_name}");
                    Data = http_response.Content.ReadAsStringAsync().Result;
                    models = JsonConvert.DeserializeObject<VehicleModel>(Data);

                    //models.results= new List<VehicleModelResult>(models.results.Where(x=>x.oem.id == oem_id));



                    foreach (VehicleModelResult item in models.data)
                    {
                        if (item.attachment != null)
                        {
                            item.attachment.attachment = "https://wikitek.io" + item.attachment.attachment;
                            // item.attachment.attachmentfile_local = await DownloadImageAsync(item.attachment.attachment);
                        }
                        else
                        {
                            item.attachment = new AttachmentModel();
                        }
                        foreach (VehicleSubModel submodel in item.sub_models.ToList())
                        {
                            if (submodel.attachment != null)
                            {
                                submodel.attachment.attachment = "https://wikitek.io" + submodel.attachment.attachment;
                                // submodel.attachment.attachmentfile_local = await DownloadImageAsync(submodel.attachment.attachment);
                            }
                            else
                            {
                                submodel.attachment = new AttachmentModel();
                            }
                        }
                    }

                    Data = JsonConvert.SerializeObject(models);
                    Preferences.Set("ModelList", Data);
                }
                else
                {
                    Data = Preferences.Get("ModelList", null);
                    models = JsonConvert.DeserializeObject<VehicleModel>(Data);
                }
                //var vahicle_all_models_list = JsonConvert.DeserializeObject<VehicleModel>(Data);
                models.status_code = http_response.StatusCode;

                //models.results = models.results.Where(x => x.oem.id == oem_id).ToList();
                return models;//.results.FirstOrDefault(x => x.id == id);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<SegmentModel> GetAllSegment(string token)
        {
            HttpResponseMessage http_response = new HttpResponseMessage();
            try
            {
                string Data = string.Empty;
                ////client = new HttpClient();
                //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("JWT", token);
                http_response = await client.GetAsync($"{App.base_url}models/segments/");
                Data = http_response.Content.ReadAsStringAsync().Result;
                var segment_list = JsonConvert.DeserializeObject<SegmentModel>(Data);
                segment_list.status_code = http_response.StatusCode;
                return segment_list;//.results.FirstOrDefault(x => x.id == id);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<PackageModel> GetPackageBySegment(string token, string user_type, string package_type, int? country_id, int segment_id)
        {
            HttpResponseMessage http_response = new HttpResponseMessage();
            try
            {
                string Data = string.Empty;
                //client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("JWT", token);
                url = $"{App.base_url}packages/list/?segment={segment_id}&country={country_id}&type={package_type}&user_type={user_type}&active=True";
                http_response = await client.GetAsync(url);
                Data = http_response.Content.ReadAsStringAsync().Result;
                var oems = JsonConvert.DeserializeObject<PackageModel>(Data);
                oems.status_code = http_response.StatusCode;
                return oems;//.results.FirstOrDefault(x => x.id == id);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<PidResult> GetPid(string token, int id, bool is_updated)
        {
            HttpResponseMessage http_response = new HttpResponseMessage();
            try
            {
                string Data = string.Empty;

                //await NetworkConnection();
                if (App.is_update)
                {
                    //client = new HttpClient();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("JWT", token);
                    url = $"{App.base_url}datasets/pid-dataset/";
                    http_response = await client.GetAsync(url);
                    Data = http_response.Content.ReadAsStringAsync().Result;
                    Preferences.Set("PidList", Data);
                }
                else
                {
                    Data = Preferences.Get("PidList", null);
                }

                var pid_list = JsonConvert.DeserializeObject<PidModel>(Data);
                return pid_list.results.FirstOrDefault(x => x.id == id);//.results.FirstOrDefault(x => x.id == id);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<DtcResult> GetDtc(string token, int id, bool is_updated)
        {
            HttpResponseMessage http_response = new HttpResponseMessage();
            try
            {
                string Data = string.Empty;
                //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("JWT", token);
                //http_response = await client.GetAsync($"{App.base_url}datasets/dtc-dataset/{id}/");
                //Data = http_response.Content.ReadAsStringAsync().Result;

                //await NetworkConnection();
                if (App.is_update)
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("JWT", token);
                    url = $"{App.base_url}datasets/dtc-dataset/";
                    http_response = await client.GetAsync(url);
                    Data = http_response.Content.ReadAsStringAsync().Result;
                    Preferences.Set("DtcList", Data);
                }
                else
                {
                    Data = Preferences.Get("DtcList", null);
                }

                var dtc_list = JsonConvert.DeserializeObject<DtcModel>(Data);
                return dtc_list.results.FirstOrDefault(x => x.id == id);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<DtcModel> GetFullDtc(string token, int id, bool is_updated)
        {
            HttpResponseMessage http_response = new HttpResponseMessage();
            try
            {
                string Data = string.Empty;

                App.is_update = true;

                if (App.is_update)
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("JWT", token);
                    url = $"{App.base_url}datasets/dtc-dataset/";
                    http_response = await client.GetAsync(url);
                    Data = http_response.Content.ReadAsStringAsync().Result;
                    Preferences.Set("DtcList", Data);
                }
                else
                {
                    Data = Preferences.Get("DtcList", null);
                }
                App.is_update = false;
                var dtc_list = JsonConvert.DeserializeObject<DtcModel>(Data);
                return dtc_list;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //public async Task<OemModel> GetVehicleBrand(string token, int segments_id)
        //{
        //    HttpResponseMessage http_response = new HttpResponseMessage();
        //    try
        //    {
        //        string Data = string.Empty;
        //        ////client = new HttpClient();
        //        //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("JWT", token);
        //        //http_response = await client.GetAsync($"{App.base_url}models/get-oems/?segments_id={segments_id}");
        //        Data = ReadTextFile("OemList.txt");//http_response.Content.ReadAsStringAsync().Result;
        //        var oem_list = JsonConvert.DeserializeObject<OemModel>(Data);
        //        return oem_list;//.results.FirstOrDefault(x => x.id == id);
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}

        #region old GetAllOems
        //public async Task<OemModel> GetAllOems(string token, int segment_id, bool is_updated)
        //{
        //    HttpResponseMessage http_response = new HttpResponseMessage();
        //    OemModel oems = new OemModel();
        //    int pagination = 0;
        //    try
        //    {
        //        string Data = string.Empty;
        //        //await NetworkConnection();
        //        if (App.is_update)
        //        {
        //            //List<OemImageLocalModel> local_images = new List<OemImageLocalModel>();
        //            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("JWT", token);
        //            url = $"{App.base_url}models/segments-oem/";
        //            http_response = await client.GetAsync(url);
        //            //http_response = await client.GetAsync($"{App.base_url}models/segments-oem/?segments_id={segment_id}");
        //            Data = http_response.Content.ReadAsStringAsync().Result;
        //            oems = JsonConvert.DeserializeObject<OemModel>(Data);
        //            if (oems.count > 1000)
        //            {
        //                pagination = oems.count / 1000;
        //            }
        //            int index = 0;
        //            for (int i = 0; i < pagination; i++)
        //            {
        //                index += 1000;
        //                url = $"{App.base_url}models/segments-oem/?offset={index}";
        //                http_response = await client.GetAsync(url);
        //                Data = http_response.Content.ReadAsStringAsync().Result;
        //                var more_oems = JsonConvert.DeserializeObject<OemModel>(Data);
        //                if (more_oems != null)
        //                {
        //                    foreach (var item in more_oems.results)
        //                    {
        //                        oems.results.Add(item);
        //                    }
        //                }

        //            }

        //            foreach (var item2 in oems.results)
        //            {
        //                item2.oem.oem_file_local = await DownloadImageAsync(item2.oem.oem_file);
        //            }


        //            Data = JsonConvert.SerializeObject(oems);
        //            Preferences.Set("OemList", Data);
        //        }
        //        else
        //        {
        //            Data = Preferences.Get("OemList", null);
        //            oems = JsonConvert.DeserializeObject<OemModel>(Data);
        //        }

        //        oems.status_code = http_response.StatusCode;
        //        oems.results = oems.results.Where(x => x.segment_name.id == segment_id).ToList();
        //        return oems;//.results.FirstOrDefault(x => x.id == id);
        //    }
        //    catch (Exception ex)
        //    {

        //        return oems;
        //    }
        //}
        #endregion

        public async Task<OemModel> GetAllOems(string token, int segment_id, bool is_updated)
        {
            HttpResponseMessage http_response = new HttpResponseMessage();
            OemModel oems = new OemModel();
            int pagination = 0;
            try
            {
                string Data = string.Empty;
                if (is_updated)
                {
                    if (await NetworkConnection())
                    {
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("JWT", token);
                        url = $"{App.base_url}models/segments-oem/?segments_id={segment_id}";
                        http_response = await client.GetAsync(url);
                        Data = http_response.Content.ReadAsStringAsync().Result;
                        oems = JsonConvert.DeserializeObject<OemModel>(Data);
                        if (http_response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                        {
                            oems.status_code = http_response.StatusCode;
                        }
                        else
                        {
                            if (oems.count > 1000)
                            {
                                pagination = oems.count / 1000;
                            }

                            int index = 0;
                            for (int i = 0; i < pagination; i++)
                            {
                                index += 1000;
                                url = $"{App.base_url}models/segments-oem/?offset={index}";
                                http_response = await client.GetAsync(url);
                                Data = http_response.Content.ReadAsStringAsync().Result;
                                var more_oems = JsonConvert.DeserializeObject<OemModel>(Data);
                                if (more_oems != null)
                                {
                                    foreach (var item in more_oems.results)
                                    {
                                        oems.results.Add(item);
                                    }
                                }
                            }

                            var group = oems.results.GroupBy(x => x.oem.id).Select(x => x.First());
                            oems.results = new List<OemResult>(group);

                            //foreach (var item2 in oems.results)
                            //{
                            //    if (item2.oem.attachment != null)
                            //    {
                            //        //item2.attachment.attachment = "https://wikitek.io" + submodel.attachment.attachment;
                            //        item2.oem.attachment.attachmentfile_local = await DownloadImageAsync(item2.oem.attachment.attachment);
                            //    }
                            //    else
                            //    {
                            //        item2.oem.attachment = new AttachmentModel();
                            //    }
                            //    //item2.oem.oem_file_local = await DownloadImageAsync(item2.oem.oem_file);
                            //}
                            Data = JsonConvert.SerializeObject(oems);
                            Preferences.Set("OemList", Data);
                        }
                    }
                    else
                    {
                        oems.detail = "Please check your internet connection.";
                    }
                }
                else
                {
                    //foreach (var item in oems.results)
                    //{
                    //    if (item.oem.attachment != null)
                    //    {
                    //        ImageService.Instance.LoadUrl(item.oem.attachment.attachment)
                    //            .Retry(3, 200)
                    //            .DownSample(400, 400)



                    //    }
                    //}
                    Data = Preferences.Get("OemList", null);
                    oems = JsonConvert.DeserializeObject<OemModel>(Data);



                }
                oems.detail = "Success";
                oems.status_code = http_response.StatusCode;
                return oems;
            }
            catch (Exception ex)
            {
                oems.status_code = http_response.StatusCode;
                oems.detail = ex.Message;
                return oems;
            }
        }
        //public async Task<VehicleModelModel> GetVehicleModel(string token, string oem_name)
        //{
        //    HttpResponseMessage http_response = new HttpResponseMessage();
        //    try
        //    {
        //        string Data = string.Empty;
        //        //client = new HttpClient();
        //        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("JWT", token);
        //        http_response = await client.GetAsync($"{App.base_url}models/models/?oem={oem_name}");
        //        Data = http_response.Content.ReadAsStringAsync().Result;
        //        var model_list = JsonConvert.DeserializeObject<VehicleModelModel>(Data);
        //        //model_list.status_code = 
        //        return model_list;//.results.FirstOrDefault(x => x.id == id);
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}

        //public async Task<SubModel> GetVehicleSubModel(string token, int model_id)
        //{
        //    HttpResponseMessage http_response = new HttpResponseMessage();
        //    try
        //    {
        //        string Data = string.Empty;
        //        ////client = new HttpClient();
        //        //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("JWT", token);
        //        //http_response = await client.GetAsync($"{App.base_url}models/get-oems/?segments_id={segments_id}");
        //        Data = ReadTextFile("SubModelList.txt");//http_response.Content.ReadAsStringAsync().Result;
        //        var sub_model_list = JsonConvert.DeserializeObject<SubModel>(Data);
        //        return sub_model_list;//.results.FirstOrDefault(x => x.id == id);
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}
        public async Task<RegistrationResponseModel> UserRegistration(MediaFile file, UserModel model)
        {
            HttpResponseMessage http_response = new HttpResponseMessage();
            RegistrationResponseModel registrationResponseModel = new RegistrationResponseModel();
            string Data = string.Empty;
            try
            {
                string js = JsonConvert.SerializeObject(model);
                var content = new MultipartFormDataContent();

                string segmentId = JsonConvert.SerializeObject(model.segment_id);

                StringContent first_name = new StringContent(model.first_name);
                StringContent last_name = new StringContent(model.last_name);
                StringContent email = new StringContent(model.email);
                StringContent mobile = new StringContent(model.mobile);
                StringContent password = new StringContent(model.password);
                StringContent device_type = new StringContent(model.device_type);
                StringContent mac_id = new StringContent(model.mac_id);
                StringContent rs_agent_id = new StringContent(model.rs_agent_id);
                StringContent segment = new StringContent(model.segment);
                StringContent user_type = new StringContent(model.user_type);
                StringContent segment_id = new StringContent(segmentId);
                StringContent role = new StringContent(model.role);
                StringContent pin_code = new StringContent(model.pin_code);
                StringContent coutry_id = new StringContent(model.country_name);

                content.Add(new StreamContent(file.GetStream()), "\"user_profile_pic\"", $"{file.Path}");

                content.Add(first_name, "first_name");
                content.Add(last_name, "last_name");
                content.Add(email, "email");
                content.Add(mobile, "mobile");
                content.Add(password, "password");
                content.Add(device_type, "device_type");
                content.Add(mac_id, "mac_id");
                content.Add(rs_agent_id, "rsagent_id");
                content.Add(segment, "segment");
                content.Add(segment_id, "segment_id");
                content.Add(user_type, "user_type");
                content.Add(role, "role");
                content.Add(pin_code, "pincode");
                content.Add(coutry_id, "country");

                //var http//client = new System.Net.Http.HttpClient();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var url = $"{App.base_url}users/register/";
                http_response = await client.PostAsync(url, content);

                Data = http_response.Content.ReadAsStringAsync().Result;
                if (http_response.StatusCode == System.Net.HttpStatusCode.OK || http_response.StatusCode == System.Net.HttpStatusCode.Created)
                {
                    registrationResponseModel.userResponse = JsonConvert.DeserializeObject<UserResponse>(Data);
                }
                else
                {
                    registrationResponseModel.registrationError = JsonConvert.DeserializeObject<RegistrationError>(Data);
                }
                registrationResponseModel.status_code = http_response.StatusCode;
                return registrationResponseModel;
            }
            catch (Exception ex)
            {
                //Debug.WriteLine("Error - inside get user regis api");
                registrationResponseModel.null_error = Data;
                return registrationResponseModel;
            }
        }

        public async Task<RegistrationResponseModel> UserUpdate(MediaFile file, string userid, string email)
        {
            HttpResponseMessage http_response = new HttpResponseMessage();
            RegistrationResponseModel registrationResponseModel = new RegistrationResponseModel();
            string Data = string.Empty;
            try
            {
                var content = new MultipartFormDataContent();

                StringContent user_email = new StringContent(email);

                content.Add(new StreamContent(file.GetStream()), "\"user_profile_pic\"", $"{file.Path}");
                content.Add(user_email, "email");

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var url = $"{App.base_url}users/update-user/{userid}";
                http_response = await client.PutAsync(url, content);
                Data = http_response.Content.ReadAsStringAsync().Result;
                if (http_response.StatusCode == System.Net.HttpStatusCode.OK || http_response.StatusCode == System.Net.HttpStatusCode.Created)
                {
                    registrationResponseModel.userResponse = JsonConvert.DeserializeObject<UserResponse>(Data);
                }
                else
                {
                    registrationResponseModel.registrationError = JsonConvert.DeserializeObject<RegistrationError>(Data);
                }
                registrationResponseModel.status_code = http_response.StatusCode;
                return registrationResponseModel;
            }
            catch (Exception ex)
            {
                //Debug.WriteLine("Error - inside get user regis api");
                registrationResponseModel.null_error = Data;
                return registrationResponseModel;
            }
        }

        public async Task<RegDongleRespons> RegisterDongle(RegisterDongleModel registerDongleModel, string token)
        {
            HttpResponseMessage http_response = new HttpResponseMessage();
            try
            {
                ////client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("JWT", token);
                var json = JsonConvert.SerializeObject(registerDongleModel);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                http_response = await client.PostAsync($"{App.base_url}devices/register/odb-device/", content);
                var Data = http_response.Content.ReadAsStringAsync().Result;
                var dongle_respons = JsonConvert.DeserializeObject<RegDongleRespons>(Data);
                dongle_respons.status_code = http_response.StatusCode;
                return dongle_respons;
            }
            catch (Exception ex)
            {
                //Application.Current.MainPage.DisplayAlert("Alert", ex.StackTrace, "Ok");
                return null;
            }
        }

        public async Task<DongleStatusModel> GetDongleStatus(string token, string mac_id, bool is_updated)
        {
            HttpResponseMessage http_response = new HttpResponseMessage();
            try
            {
                string Data = string.Empty;
                //is_updated = true;
                //await NetworkConnection();
                if (App.is_update)
                {
                    ////client = new HttpClient();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("JWT", token);
                    http_response = await client.GetAsync($"{App.base_url}devices/devices-status-list/?mac_id={mac_id}");
                    Data = http_response.Content.ReadAsStringAsync().Result;
                    var ser = JsonConvert.DeserializeObject<DongleStatusModel>(Data);
                    var dong = ser.results.FirstOrDefault();
                    dong.mac_id = mac_id;
                    ser.results = new List<DongleResult> { new DongleResult { is_active = dong.is_active, mac_id = dong.mac_id } };
                    var json = JsonConvert.SerializeObject(ser);
                    Preferences.Set("DongleStatusList", json);
                }
                else
                {
                    Data = Preferences.Get("DongleStatusList", null);
                }
                var dongle = JsonConvert.DeserializeObject<DongleStatusModel>(Data);
                //dongle.results = dongle.results.Where(x => x.mac_id == mac_id).ToList();
                dongle.status_code = http_response.StatusCode;
                return dongle;//.results.FirstOrDefault(x => x.id == id);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<PartsModel> GetMarketPlaceList(string token)
        {
            HttpResponseMessage http_response = new HttpResponseMessage();
            try
            {
                token = Preferences.Get("tokenIT",String.Empty);
                string Data = string.Empty;
                ////client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("JWT", token);
                //http_response = await client.GetAsync($"{App.base_url}service_lead/all_parts/");
                http_response = await client.GetAsync($"{inventabBaseUrl}parts/fetch/parts/?market_place=wikitek1");
                //http_response = await client.GetAsync($"{inventabBaseUrl}parts/fetch/wikitek/parts/");
                Data = http_response.Content.ReadAsStringAsync().Result;
                var oems = JsonConvert.DeserializeObject<PartsModel>(Data);
                oems.status_code = http_response.StatusCode;
                return oems;//.results.FirstOrDefault(x => x.id == id);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<PackageModel> GetPackageBySegmentId(string token, int segment_id)
        {
            HttpResponseMessage http_response = new HttpResponseMessage();
            try
            {
                string Data = string.Empty;
                //client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("JWT", token);
                url = $"{App.base_url}models/segments-oem/?segments_id={segment_id}";
                http_response = await client.GetAsync(url);
                Data = http_response.Content.ReadAsStringAsync().Result;
                var oems = JsonConvert.DeserializeObject<PackageModel>(Data);
                oems.status_code = http_response.StatusCode;
                return oems;//.results.FirstOrDefault(x => x.id == id);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #region Cart Section
        public async Task<CreateRsAgentResponse> AddToCart(string token, AddCartModel model)
        {
            HttpResponseMessage httpResponse = new HttpResponseMessage();
            try
            {
                token = Preferences.Get("tokenIT", String.Empty);
                string Data = string.Empty;
                //client = new HttpClient();
                var json = JsonConvert.SerializeObject(model);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("JWT", token);
                httpResponse = await client.PostAsync($"{inventabBaseUrl}carts/add_to_cart/", content);
                Data = httpResponse.Content.ReadAsStringAsync().Result;
                var countries = JsonConvert.DeserializeObject<CreateRsAgentResponse>(Data);
                countries.status_code = httpResponse.StatusCode;
                return countries;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<CartModel> GetCartList(string token, string user_id)
        {
            HttpResponseMessage http_response = new HttpResponseMessage();
            try
            {
                token = Preferences.Get("tokenIT", String.Empty);
                user_id = Preferences.Get("userIdIT", String.Empty);
                string Data = string.Empty;
                ////client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("JWT", token);
                http_response = await client.GetAsync($"{inventabBaseUrl}carts/my_cart/?user_id={user_id}");
                Data = http_response.Content.ReadAsStringAsync().Result;
                var oems = JsonConvert.DeserializeObject<CartModel>(Data);
                oems.status_code = http_response.StatusCode;
                return oems;//.results.FirstOrDefault(x => x.id == id);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<CreateRsAgentResponse> UpdateToCart(string token, int item_id, int item_quantity)
        {
            HttpResponseMessage httpResponse = new HttpResponseMessage();
            try
            {
                token = Preferences.Get("tokenIT", String.Empty);
                string Data = string.Empty;
                //client = new HttpClient();
                //var json = JsonConvert.SerializeObject(model);
                //var content = new StringContent(json, Encoding.UTF8, "application/json");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("JWT", token);
                httpResponse = await client.PutAsync($"{inventabBaseUrl}carts/update_quantity/{item_id}/{item_quantity}/", null);
                Data = httpResponse.Content.ReadAsStringAsync().Result;
                var countries = JsonConvert.DeserializeObject<CreateRsAgentResponse>(Data);
                countries.status_code = httpResponse.StatusCode;
                return countries;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<CreateRsAgentResponse> DeleteToCart(string token, int item_id)
        {
            HttpResponseMessage httpResponse = new HttpResponseMessage();
            try
            {
                token = Preferences.Get("tokenIT", String.Empty);
                string Data = string.Empty;
                //client = new HttpClient();
                //var json = JsonConvert.SerializeObject(model);
                //var content = new StringContent(json, Encoding.UTF8, "application/json");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("JWT", token);
                httpResponse = await client.DeleteAsync($"{inventabBaseUrl}carts/remove/{item_id}/");
                Data = httpResponse.Content.ReadAsStringAsync().Result;
                var countries = JsonConvert.DeserializeObject<CreateRsAgentResponse>(Data);
                countries.status_code = httpResponse.StatusCode;
                return countries;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

        #region Payment Method
        public async Task<GenerateOrderIdResponseModel> GenerateOrderId(GenerateOrderIdModel model)
        {
            HttpResponseMessage httpResponse = new HttpResponseMessage();
            try
            {
                string Data = string.Empty;
                ////client = new HttpClient();
                var json = JsonConvert.SerializeObject(model);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                //string auth = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes("rzp_test_LgkTD3Krt1IRVK" + ":" + "OvT1JcEL9QURL7W3Q3ybplcK"));

                // client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Authorization", "Basic " + auth);

                client.DefaultRequestHeaders.Authorization =
    new AuthenticationHeaderValue(
        "Basic", Convert.ToBase64String(
            System.Text.ASCIIEncoding.ASCII.GetBytes(
               $"rzp_live_r21sPXRDz2eJvu:nMOr1GuIgmyEmgK3xudsUA8k")));

                //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("JWT", token);
                string url = "https://api.razorpay.com/v1/orders";
                httpResponse = await client.PostAsync($"{url}", content);
                Data = httpResponse.Content.ReadAsStringAsync().Result;
                var generated_order = JsonConvert.DeserializeObject<GenerateOrderIdResponseModel>(Data);
                generated_order.status_code = httpResponse.StatusCode;
                return generated_order;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<PaymentServerResponseModel> RegisterOrderToServer(string token, PaymentServerRequestModel model)
        {
            HttpResponseMessage httpResponse = new HttpResponseMessage();
            try
            {
                token = Preferences.Get("tokenIT", String.Empty);
                string Data = string.Empty;
                //client = new HttpClient();
                var json = JsonConvert.SerializeObject(model);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("JWT", token);
                httpResponse = await client.PostAsync($"{inventabBaseUrl}carts/my_cart/checkout/save/", content);
                Data = httpResponse.Content.ReadAsStringAsync().Result;
                var res = JsonConvert.DeserializeObject<PaymentServerResponseModel>(Data);
                res.status_code = httpResponse.StatusCode;
                return res;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<bool> DeletePurchaseCart(string token)
        {
            HttpResponseMessage httpResponse = new HttpResponseMessage();
            try
            {
                token = Preferences.Get("tokenIT", String.Empty);
                string Data = string.Empty;


                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("JWT", token);
                httpResponse = await client.DeleteAsync($"{inventabBaseUrl}carts/my_cart/delete");
                Data = httpResponse.Content.ReadAsStringAsync().Result;
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion

        #region Subscription Method
        public async Task<SubscriprionPaymentServerResponseModel> RegisterSubscriptionToServer(string token, PurchasePackageModel model)
        {

            HttpResponseMessage httpResponse = new HttpResponseMessage();
            try
            {
                string Data = string.Empty;
                //client = new HttpClient();
                var json = JsonConvert.SerializeObject(model);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("JWT", token);
                httpResponse = await client.PostAsync($"{App.base_url}subscriptions/create/", content);
                Data = httpResponse.Content.ReadAsStringAsync().Result;
                var res = JsonConvert.DeserializeObject<SubscriprionPaymentServerResponseModel>(Data);
                res.Status_code = httpResponse.StatusCode;
                return res;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

        #region OTP
        public async Task<MobileNumberModel> ForgotPasswordMobile(string mobile_number)
        {
            #region Old Code
            HttpResponseMessage httpResponse = new HttpResponseMessage();
            MobileNumberModel mobileNumberModel = new MobileNumberModel();
            string Data = string.Empty;

            try
            {
                //NetworkConnection();
                //bool is_online = false;
                //if (!string.IsNullOrEmpty(Preferences.Get("token", null)))
                //{
                //    is_online = true;
                //}

                //{
                var network = await NetworkConnection();
                if (network)
                {
                    //var json = JsonConvert.SerializeObject(model);
                    string json = "{ \"mobile\":\"" + mobile_number + "\" }";
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    httpResponse = await client.PostAsync($"{App.base_url}users/password/forgot/mobile", content);
                    Data = httpResponse.Content.ReadAsStringAsync().Result;
                    mobileNumberModel = JsonConvert.DeserializeObject<MobileNumberModel>(Data);
                }
                else
                {
                    mobileNumberModel.error = "No internet connection";
                }

                mobileNumberModel.status_code = httpResponse.StatusCode;

                return mobileNumberModel;
            }
            catch (Exception ex)
            {
                mobileNumberModel.error = Data;
                mobileNumberModel.status_code = httpResponse.StatusCode;
                return mobileNumberModel;
            }
            #endregion
        }

        public async Task<VerifyOtpModel> VerifyOTP(string mobile_number, string otp)
        {
            HttpResponseMessage httpResponse = new HttpResponseMessage();
            VerifyOtpModel verifyOtpModel = new VerifyOtpModel();
            string Data = string.Empty;

            try
            {
                var network = await NetworkConnection();
                if (network)
                {
                    //var json = JsonConvert.SerializeObject(model);
                    //string json = "{ \"mobile\":\"" + mobile_number + "\" }";

                    string json = "{ \"mobile\":\"" + mobile_number + "\",\"otp\":\"" + otp + "\" }";
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    httpResponse = await client.PostAsync($"{App.base_url}users/password/verify/mobile", content);
                    Data = httpResponse.Content.ReadAsStringAsync().Result;
                    verifyOtpModel = JsonConvert.DeserializeObject<VerifyOtpModel>(Data);
                }
                else
                {
                    verifyOtpModel.error = "No internet connection";
                }

                verifyOtpModel.status_code = httpResponse.StatusCode;

                return verifyOtpModel;
            }
            catch (Exception ex)
            {
                verifyOtpModel.error = Data;
                verifyOtpModel.status_code = httpResponse.StatusCode;
                return verifyOtpModel;
            }
        }

        public async Task<VerifyOtpModel> ResetPassword(string reset_url, string new_password)
        {
            HttpResponseMessage httpResponse = new HttpResponseMessage();
            VerifyOtpModel verifyOtpModel = new VerifyOtpModel();
            string Data = string.Empty;

            try
            {
                var network = await NetworkConnection();
                if (network)
                {
                    string json = "{ \"password\":\"" + new_password + "\" }";
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    httpResponse = await client.PostAsync($"{reset_url}", content);
                    Data = httpResponse.Content.ReadAsStringAsync().Result;
                    verifyOtpModel = JsonConvert.DeserializeObject<VerifyOtpModel>(Data);
                }
                else
                {
                    verifyOtpModel.error = "No internet connection";
                }

                verifyOtpModel.status_code = httpResponse.StatusCode;

                return verifyOtpModel;
            }
            catch (Exception ex)
            {
                verifyOtpModel.error = Data;
                verifyOtpModel.status_code = httpResponse.StatusCode;
                return verifyOtpModel;
            }
        }

        public async Task<VerifyOtpModel> VerifyRegisteredMobileNumber(string mobile_number, string otp)
        {
            HttpResponseMessage httpResponse = new HttpResponseMessage();
            VerifyOtpModel verifyOtpModel = new VerifyOtpModel();
            string Data = string.Empty;

            try
            {
                var network = await NetworkConnection();
                if (network)
                {
                    string json = "{ \"mobile\":\"" + mobile_number + "\",\"otp\":\"" + otp + "\" }";
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    httpResponse = await client.PostAsync($"{App.base_url}users/account/verify/mobile", content);
                    Data = httpResponse.Content.ReadAsStringAsync().Result;
                    verifyOtpModel = JsonConvert.DeserializeObject<VerifyOtpModel>(Data);
                }
                else
                {
                    verifyOtpModel.error = "No internet connection";
                }

                verifyOtpModel.status_code = httpResponse.StatusCode;

                return verifyOtpModel;
            }
            catch (Exception ex)
            {
                verifyOtpModel.error = Data;
                verifyOtpModel.status_code = httpResponse.StatusCode;
                return verifyOtpModel;
            }
        }

        public async Task<VerifyOtpModel> UserChangePassword(string token, string current_password, string new_password)
        {
            HttpResponseMessage httpResponse = new HttpResponseMessage();
            VerifyOtpModel verifyOtpModel = new VerifyOtpModel();
            string Data = string.Empty;

            try
            {
                var network = await NetworkConnection();
                if (network)
                {
                    //var json = JsonConvert.SerializeObject(model);
                    //string json = "{ \"mobile\":\"" + mobile_number + "\" }";

                    string json = "{ \"current_password\":\"" + current_password + "\",\"password\":\"" + new_password + "\" }";
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("JWT", token);
                    httpResponse = await client.PostAsync($"{App.base_url}users/account/password", content);
                    Data = httpResponse.Content.ReadAsStringAsync().Result;
                    verifyOtpModel = JsonConvert.DeserializeObject<VerifyOtpModel>(Data);
                }
                else
                {
                    verifyOtpModel.error = "No internet connection";
                }

                verifyOtpModel.status_code = httpResponse.StatusCode;

                return verifyOtpModel;
            }
            catch (Exception ex)
            {
                verifyOtpModel.error = Data;
                verifyOtpModel.status_code = httpResponse.StatusCode;
                return verifyOtpModel;
            }
        }
        #endregion

        public string ReadTextFile(string FileName)
        {
            try
            {
                Stream stream = this.GetType().Assembly.GetManifestResourceStream($"WikitekMotorCycleMechanik.JsonFiles.{ FileName}.txt");
                string text = "";
                using (var reader = new System.IO.StreamReader(stream))
                {
                    text = reader.ReadToEnd();
                }
                return text;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        public async Task<bool> NetworkConnection()
        {
            bool is_network = false;
            App.is_update = false;
            var current = Connectivity.NetworkAccess;
            var profiles = Connectivity.ConnectionProfiles;
            if (current == NetworkAccess.Internet)
            {
                is_network = true;
                App.is_update = true;
            }

            return is_network;
        }

        public string ReadDtcList(string FileName)
        {
            try
            {
                string json = ReadTextFile(FileName);
                //var FaultList = JsonConvert.DeserializeObject<Model.Bs6FaulDes.Bs6FaulDesModel>(json);
                return json;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<byte[]> DownloadImageAsync(string imageUrl)
        {
            try
            {
                using (var httpResponse = await client.GetAsync(imageUrl))
                {
                    if (httpResponse.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        return await httpResponse.Content.ReadAsByteArrayAsync();
                    }
                    else
                    {
                        //Url is Invalid
                        return null;
                    }
                }
            }
            catch (Exception e)
            {
                //Handle Exception
                return null;
            }
        }

        #region NEW API
        public async Task<RegistrationResponseModel> UserRegistrationNew(MediaFile file, UserModel model)
        {
            bool resp = await UserRegistrationIT(file, model);
            if (resp)
            {
                HttpResponseMessage http_response = new HttpResponseMessage();
                RegistrationResponseModel registrationResponseModel = new RegistrationResponseModel();
                string Data = string.Empty;
                try
                {
                    model.user_profile_pic = null;
                    string json = JsonConvert.SerializeObject(model);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    //var content = new MultipartFormDataContent();

                    //StringContent first_name = new StringContent(model.first_name);
                    //StringContent last_name = new StringContent(model.last_name);
                    //StringContent email = new StringContent(model.email);
                    //StringContent mobile = new StringContent(model.mobile);
                    //StringContent password = new StringContent(model.password);
                    //StringContent device_type = new StringContent(model.device_type);
                    //StringContent mac_id = new StringContent(model.mac_id);
                    //StringContent role = new StringContent(model.role);
                    //StringContent pin_code = new StringContent(model.pin_code);
                    //StringContent coutry_id = new StringContent(model.country_name);

                    //content.Add(new StreamContent(file.GetStream()), "\"user_profile_pic\"", $"{file.Path}");

                    //content.Add(first_name, "first_name");
                    //content.Add(last_name, "last_name");
                    //content.Add(email, "email");
                    //content.Add(mobile, "mobile");
                    //content.Add(password, "password");
                    //content.Add(device_type, "device_type");
                    //content.Add(mac_id, "mac_id");
                    //content.Add(role, "role");
                    //content.Add(pin_code, "pincode");
                    //content.Add(coutry_id, "country");

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var url = $"{App.base_url}users/create/user/";
                    http_response = await client.PostAsync(url, content);


                    Data = http_response.Content.ReadAsStringAsync().Result;
                    if (http_response.StatusCode == System.Net.HttpStatusCode.OK || http_response.StatusCode == System.Net.HttpStatusCode.Created)
                    {
                        registrationResponseModel.userResponse = JsonConvert.DeserializeObject<UserResponse>(Data);
                    }
                    else
                    {
                        registrationResponseModel.registrationError = JsonConvert.DeserializeObject<RegistrationError>(Data);
                    }
                    registrationResponseModel.status_code = http_response.StatusCode;
                    return registrationResponseModel;
                }
                catch (Exception ex)
                {
                    //Debug.WriteLine("Error - inside get user regis api");
                    registrationResponseModel.null_error = Data;
                    return registrationResponseModel;
                } 
            }
            else
            {
                RegistrationResponseModel registrationResponseModel = new RegistrationResponseModel();
                registrationResponseModel.null_error = "Failed Inventab Registration";
                return registrationResponseModel;
            }
        }

        public async Task<bool> UserRegistrationIT(MediaFile file, UserModel model)
        {
            HttpResponseMessage http_response_inventab = new HttpResponseMessage();
            RegistrationResponseModel registrationResponseModel = new RegistrationResponseModel();
            string Data_inventab = string.Empty;
            try
            {
                model.user_profile_pic = null;
                model.org = "Wikitek Systems";
                model.role = "NORMALUSER";
                string json = JsonConvert.SerializeObject(model);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                //string js = JsonConvert.SerializeObject(model);
                //var content = new MultipartFormDataContent();

                // string segmentId = JsonConvert.SerializeObject(model.segment_id);

                //StringContent first_name = new StringContent(model.first_name);
                //StringContent last_name = new StringContent(model.last_name);
                //StringContent email = new StringContent(model.email);
                //StringContent mobile = new StringContent(model.mobile);
                //StringContent password = new StringContent(model.password);
                //StringContent device_type = new StringContent(model.device_type);
                //StringContent mac_id = new StringContent(model.mac_id);
                ////StringContent rs_agent_id = new StringContent(model.rs_agent_id);
                ////StringContent segment = new StringContent(model.segment);
                ////StringContent user_type = new StringContent(model.user_type);
                ////StringContent segment_id = new StringContent(segmentId);
                //StringContent role = new StringContent("NORMALUSER");
                //StringContent pin_code = new StringContent(model.pin_code);
                //StringContent coutry_id = new StringContent(model.country_name);
                //StringContent org = new StringContent("Wikitek Systems");
                //StringContent role1 = new StringContent("NORMALUSER");

                //content.Add(new StreamContent(file.GetStream()), "\"user_profile_pic\"", $"{file.Path}");

                //content.Add(first_name, "first_name");
                //content.Add(last_name, "last_name");
                //content.Add(email, "email");
                //content.Add(mobile, "mobile");
                //content.Add(password, "password");
                //content.Add(device_type, "device_type");
                //content.Add(mac_id, "mac_id");
                //content.Add(rs_agent_id, "rsagent_id");
                //content.Add(segment, "segment");
                //content.Add(segment_id, "segment_id");
                //content.Add(user_type, "user_type");
                //content.Add(role, "role");
                //content.Add(pin_code, "pincode");
                //content.Add(coutry_id, "country");
                //content.Add(org, "org");
                //var http//client = new System.Net.Http.HttpClient();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var urlIT = $"{inventabBaseUrl}users/create/user/";
                http_response_inventab = await client.PostAsync(urlIT, content);
                Data_inventab = http_response_inventab.Content.ReadAsStringAsync().Result;

                if (http_response_inventab.StatusCode == System.Net.HttpStatusCode.OK || http_response_inventab.StatusCode == System.Net.HttpStatusCode.Created)
                {
                    return true;
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Failed", http_response_inventab.StatusCode.ToString(), "Ok");
                    return false;
                }

                //Data = http_response.Content.ReadAsStringAsync().Result;
                //if (http_response.StatusCode == System.Net.HttpStatusCode.OK || http_response.StatusCode == System.Net.HttpStatusCode.Created)
                //{
                //    registrationResponseModel.userResponse = JsonConvert.DeserializeObject<UserResponse>(Data);
                //}
                //else
                //{
                //    registrationResponseModel.registrationError = JsonConvert.DeserializeObject<RegistrationError>(Data);
                //}
                //registrationResponseModel.status_code = http_response.StatusCode;
                //return registrationResponseModel;
            }
            catch (Exception ex)
            {
                return false;
                //Debug.WriteLine("Error - inside get user regis api");
                //registrationResponseModel.null_error = Data;
                //return registrationResponseModel;
            }
        }


        public async Task<LoginResponse> UserLoginNew(LoginModel model)
        {
            #region Old Code
            HttpResponseMessage httpResponse = new HttpResponseMessage();
            HttpResponseMessage httpResponseIT = new HttpResponseMessage();
            LoginResponse loginResponse = new LoginResponse();
            string Data = string.Empty;
            string DataIT = string.Empty;


            try
            {
                //var is_network = NetworkConnection();
                //bool is_online = false;
                //if(!string.IsNullOrEmpty(Preferences.Get("token", null)))
                //{
                //    is_online = true;
                //}
                ////if (is_network)
                //{
                var network = await NetworkConnection();
                var json = JsonConvert.SerializeObject(model);
                var jsonIT = "{ \"username\":\"" + model.email + "\",\"password\":\"" + model.password + "\"}";
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var contentIT = new StringContent(jsonIT, Encoding.UTF8, "application/json");
                httpResponse = await client.PostAsync($"{App.base_url}users/new/login/", content);
                httpResponseIT = await client.PostAsync($"{inventabBaseUrl}accounts/login", contentIT);

                Data = httpResponse.Content.ReadAsStringAsync().Result;
                DataIT = httpResponseIT.Content.ReadAsStringAsync().Result;
                if (httpResponse.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    loginResponse.success = false;
                    loginResponse.error = Data;
                    return loginResponse;
                }
                if (httpResponseIT.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    loginResponse.success = false;
                    loginResponse.error = httpResponseIT.StatusCode.ToString();
                    return loginResponse;
                }
                else
                {
                    InventabUser loginResponseIT = JsonConvert.DeserializeObject<InventabUser>(DataIT);
                    string token = loginResponseIT.data.auth_token.access;
                    string userIdIT = loginResponseIT.data.user_id;
                    Preferences.Set("tokenIT", token);
                    Preferences.Set("userIdIT", userIdIT);
                }

                loginResponse = JsonConvert.DeserializeObject<LoginResponse>(Data);

                loginResponse.picture_local = await DownloadImageAsync(loginResponse.picture);

                if (loginResponse.subscriptions != null && loginResponse.subscriptions.Any())
                {
                    foreach (var item2 in loginResponse.subscriptions?.ToList())
                    {
                        if (item2.oem != null && item2.oem.Any())
                        {
                            foreach (var item in item2.oem.ToList())
                            {
                                if (item.attachment != null)
                                {
                                    item.attachment.attachmentfile_local = await DownloadImageAsync(item.attachment.attachment);
                                }
                                else
                                {
                                    item.attachment = new AttachmentModel();
                                }
                            }
                        }
                    }
                }

                Data = JsonConvert.SerializeObject(loginResponse);

                Preferences.Set("LoginResponse", Data);

                loginResponse = JsonConvert.DeserializeObject<LoginResponse>(Data);
                App.user_first_name = loginResponse?.first_name;
                App.user_last_name = loginResponse?.last_name;
                await Application.Current.SavePropertiesAsync();
                App.user_id = loginResponse?.user_id;
                App.user_type = loginResponse?.user_type;
                App.country_id = loginResponse?.agent?.workshop?.country;

                App.is_update = true;
                Preferences.Set("IsUpdate", "true");
                if (loginResponse.subscriptions != null
                    && loginResponse.subscriptions.Any()
                    && loginResponse.dongles != null
                    && loginResponse.dongles.Any())
                {
                    App.is_update = false;
                    Preferences.Set("IsUpdate", "false");
                }
                loginResponse.success = true;
                return loginResponse;
            }
            catch (System.Net.Http.HttpRequestException ex)
            {
                Preferences.Set("LoginResponse", string.Empty);
                loginResponse.success = false;
                loginResponse.error = "Check internet connectivity";
                return loginResponse;
            }
            catch (Exception ex)
            {
                Preferences.Set("LoginResponse", string.Empty);
                loginResponse.success = false;
                loginResponse.error = ex.Message;
                return loginResponse;
            }
            #endregion
        }

        public async Task<SubcribtionModel> GetSubscription(string token, string part_no, string serial_no)
        {
            HttpResponseMessage http_response = new HttpResponseMessage();
            SubcribtionModel model = new SubcribtionModel();
            string Data = string.Empty;
            string url = invantab_url + $"api/v1/production/Batch/create/?part_no={part_no}&fg_serial_no={serial_no}";
            try
            {

                //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("JWT", token);
                http_response = await client.GetAsync(url);

                if (http_response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    model.status = false;
                    model.message = http_response.StatusCode.ToString();
                    return model;
                }

                Data = http_response.Content.ReadAsStringAsync().Result;
                model = JsonConvert.DeserializeObject<SubcribtionModel>(Data);
                model.status = true;
                return model;//.results.FirstOrDefault(x => x.id == id);
            }
            catch (Exception ex)
            {
                model.status = false;
                model.message = ex.Message;
                return model;
            }
        }

        public async Task<UserSubsLink> Subscription(string token, string user_id, string subscription_id)
        {
            HttpResponseMessage http_response = new HttpResponseMessage();
            UserSubsLink model = new UserSubsLink();
            string Data = string.Empty;
            string url = App.base_url + $"subscriptions/link/multiple/subscription";
            string json = "{ \"user_id\":\"" + user_id + "\",\"subscription_id\":\"" + subscription_id + "\" }";
            try
            {

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("JWT", token);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                http_response = await client.PostAsync(url, content);

                if (http_response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    model.success = false;
                    model.status = http_response.StatusCode.ToString();
                    return model;
                }

                Data = http_response.Content.ReadAsStringAsync().Result;
                model = JsonConvert.DeserializeObject<UserSubsLink>(Data);
                model.success = true;
                return model;//.results.FirstOrDefault(x => x.id == id);
            }
            catch (Exception ex)
            {
                model.success = false;
                model.status = ex.Message;
                return model;
            }
        }

        public async Task<GdModelGD> Get_gd(string token, string dtcPCode, int sub_model_id)
        {

            try
            {
                var isReachable = await CrossConnectivity.Current.IsRemoteReachable("https://www.google.com/");
                if (isReachable)
                {
                    //client = new HttpClient();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("JWT", token);

                    string url = $"{App.base_url}gdauthor/node-template/gd-by-year_id-dtc_id/?dtc_id={dtcPCode}&name={sub_model_id}";
                    var response = client.GetAsync(url).Result;
                    var Data = response.Content.ReadAsStringAsync().Result;
                    if (response.StatusCode != System.Net.HttpStatusCode.NotFound)
                    {
                        GdModelGD myDeserializedClass = JsonConvert.DeserializeObject<GdModelGD>(Data);
                        return myDeserializedClass;
                    }
                    else
                    {
                        return null;
                    }

                    //dynamic list = JsonConvert.DeserializeObject<List<GDData>>(Data);
                    //dynamic list = JsonConvert.DeserializeObject(Data);
                    //var json = JsonConvert.SerializeObject(list);

                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                string x = ex.Message;
                return null;
            }


            //try
            //{

            //    //client = new HttpClient();
            //    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("JWT", token);
            //    var response = client.GetAsync($"{BaseUrl}gdauthor/gd/gd-list/?dtc_id={pid_id}&name={model_id}").Result;
            //    var Data = response.Content.ReadAsStringAsync().Result;
            //    dynamic list = JsonConvert.DeserializeObject(Data);
            //    //var info = list["dtcs"];

            //    //var Data = ReadTextFile(filename);
            //    //var UserInfo = JsonConvert.DeserializeObject<List<JobCardModel>>(Data);
            //    //var list = UserInfo;//.Where(x => x.vehicle_model.parent.name.ToLower().Contains("MDE".ToLower())).ToList();
            //    return list;
            //}
            //catch (Exception ex)
            //{
            //    show_message(ex.StackTrace);
            //    string x = ex.Message;
            //    return null;
            //}
        }

        public async Task<SubcribtionModel> GetExpertList(string token)
        {
            HttpResponseMessage http_response = new HttpResponseMessage();
            SubcribtionModel model = new SubcribtionModel();
            string Data = string.Empty;
            string url = App.base_url + $"users/expert-user/";
            try
            {

                //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("JWT", token);
                http_response = await client.GetAsync(url);

                if (http_response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    model.status = false;
                    model.message = http_response.StatusCode.ToString();
                    return model;
                }

                Data = http_response.Content.ReadAsStringAsync().Result;
                model = JsonConvert.DeserializeObject<SubcribtionModel>(Data);
                model.status = true;
                return model;//.results.FirstOrDefault(x => x.id == id);
            }
            catch (Exception ex)
            {
                model.status = false;
                model.message = ex.Message;
                return model;
            }
        }

        public async Task<MibudUserModel> GetUserList(string token)
        {
            HttpResponseMessage http_response = new HttpResponseMessage();
            MibudUserModel model = new MibudUserModel();
            string Data = string.Empty;
            string url = App.base_url + $"users/get-mibud-user/";
            try
            {

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("JWT", token);
                http_response = await client.GetAsync(url);

                if (http_response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    model.status = false;
                    model.message = http_response.StatusCode.ToString();
                    return model;
                }

                Data = http_response.Content.ReadAsStringAsync().Result;
                model = JsonConvert.DeserializeObject<MibudUserModel>(Data);
                model.status = true;
                return model;//.results.FirstOrDefault(x => x.id == id);
            }
            catch (Exception ex)
            {
                model.status = false;
                model.message = ex.Message;
                return model;
            }
        }

        public async Task<VehicleListModel> GetVehicleList1(string token, string user_id)
        {
            HttpResponseMessage http_response = new HttpResponseMessage();
            VehicleListModel model = new VehicleListModel();
            string Data = string.Empty;
            string url = App.base_url + $"vehicles/list/?user={user_id}";
            try
            {

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("JWT", token);
                http_response = await client.GetAsync(url);

                if (http_response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    model.status = false;
                    model.message = http_response.StatusCode.ToString();
                    return model;
                }

                Data = http_response.Content.ReadAsStringAsync().Result;
                model = JsonConvert.DeserializeObject<VehicleListModel>(Data);
                model.status = true;
                return model;//.results.FirstOrDefault(x => x.id == id);
            }
            catch (Exception ex)
            {
                model.status = false;
                model.message = ex.Message;
                return model;
            }
        }

        public async Task<CreateJobcardResponse> CreateJobcard(string token, CreateJobcardModel model)
        {
            #region Old Code
            HttpResponseMessage httpResponse = new HttpResponseMessage();
            CreateJobcardResponse loginResponse = new CreateJobcardResponse();
            string Data = string.Empty;


            try
            {
                var json = JsonConvert.SerializeObject(model);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                httpResponse = await client.PostAsync($"{App.base_url}analyze/create-mechanik", content);

                if (httpResponse.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    loginResponse.success = false;
                    loginResponse.error = httpResponse.StatusCode.ToString();
                    return loginResponse;
                }

                Data = httpResponse.Content.ReadAsStringAsync().Result;
                loginResponse = JsonConvert.DeserializeObject<CreateJobcardResponse>(Data);
                loginResponse.success = true;
                return loginResponse;
            }
            catch (System.Net.Http.HttpRequestException ex)
            {
                loginResponse.success = false;
                loginResponse.error = "Check internet connectivity";
                return loginResponse;
            }
            catch (Exception ex)
            {
                loginResponse.success = false;
                loginResponse.error = ex.Message;
                return loginResponse;
            }
            #endregion
        }

        public async Task<JobcardModel> GetJobcardList(string token)
        {
            HttpResponseMessage http_response = new HttpResponseMessage();
            JobcardModel model = new JobcardModel();
            string Data = string.Empty;
            string url = App.base_url + $"analyze/jobcard-details";
            try
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("JWT", token);
                http_response = await client.GetAsync(url);

                if (http_response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    model.status = false;
                    model.message = http_response.StatusCode.ToString();
                    return model;
                }

                Data = http_response.Content.ReadAsStringAsync().Result;
                model = JsonConvert.DeserializeObject<JobcardModel>(Data);
                Debug.WriteLine(Data, "PrintJobcardJson");
                model.status = true;
                return model;//.results.FirstOrDefault(x => x.id == id);
            }
            catch (Exception ex)
            {
                model.status = false;
                model.message = ex.Message;
                return model;
            }
        }

        public async Task<JobcardModel> GetJobcardDetail(string token, string id)
        {
            HttpResponseMessage http_response = new HttpResponseMessage();
            JobcardModel model = new JobcardModel();
            string Data = string.Empty;
            string url = App.base_url + $"analyze/jobcard-details/?id={id}";
            try
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("JWT", token);
                http_response = await client.GetAsync(url);

                if (http_response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    model.status = false;
                    model.message = http_response.StatusCode.ToString();
                    return model;
                }

                Data = http_response.Content.ReadAsStringAsync().Result;
                Debug.WriteLine(Data, "Jobacad-detail");
                model = JsonConvert.DeserializeObject<JobcardModel>(Data);
                model.status = true;
                return model;//.results.FirstOrDefault(x => x.id == id);
            }
            catch (Exception ex)
            {
                model.status = false;
                model.message = ex.Message;
                return model;
            }
        }

        public async Task<VehicleListModel> GetVehicleDetail(string token, string reg_no)
        {
            HttpResponseMessage httpResponse = new HttpResponseMessage();
            VehicleListModel model = new VehicleListModel();
            //client = new HttpClient();
            string Data = string.Empty;
            string json = string.Empty;
            string url = string.Empty;

            try
            {

                //url = $"{base_url}symptom/symptom-list/?s_model={submodel_id}";
                url = $"{App.base_url}vehicles/list/?registration_id={reg_no}";
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("JWT", token);
                httpResponse = await client.GetAsync(url);

                if (httpResponse.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    model.status = false;
                    model.message = httpResponse.StatusCode.ToString();
                    return model;
                }

                Data = httpResponse.Content.ReadAsStringAsync().Result;
                Debug.WriteLine($"{Data}", "Relay Command Response");
                model = JsonConvert.DeserializeObject<VehicleListModel>(Data);
                model.message = httpResponse.StatusCode.ToString();
                model.status = true;
                return model;
            }
            catch (Exception ex)
            {
                model.status = false;
                //model.status = 1000;
                model.message = ex.Message;
                return model;
            }
        }

        public async Task<SymptomsModel> GetSymptoms(string token, int submodel_id)
        {
            HttpResponseMessage httpResponse = new HttpResponseMessage();
            SymptomsModel model = new SymptomsModel();
            //client = new HttpClient();
            string Data = string.Empty;
            string json = string.Empty;
            string url = string.Empty;

            try
            {

                //url = $"{base_url}symptom/symptom-list/?s_model={submodel_id}";
                url = $"{App.base_url}symptom/symptom-mapping/?s_model={submodel_id}";
                //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                httpResponse = await client.GetAsync(url);

                if (httpResponse.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    model.success = false;
                    model.status = httpResponse.StatusCode.ToString();
                    return model;
                }

                Data = httpResponse.Content.ReadAsStringAsync().Result;
                Debug.WriteLine($"{Data}", "Relay Command Response");
                model = JsonConvert.DeserializeObject<SymptomsModel>(Data);
                model.status = httpResponse.StatusCode.ToString();
                model.success = true;
                return model;
            }
            catch (Exception ex)
            {
                model.success = false;
                //model.status = 1000;
                model.status = ex.Message;
                return model;
            }
        }

        public async Task<AddSymptomResponse> AddSymptoms(string token, AddSymptom model)
        {
            #region Old Code
            HttpResponseMessage httpResponse = new HttpResponseMessage();
            AddSymptomResponse loginResponse = new AddSymptomResponse();
            string Data = string.Empty;


            try
            {
                var json = JsonConvert.SerializeObject(model);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                httpResponse = await client.PostAsync($"{App.base_url}analyze/create-symptom/", content);

                if (httpResponse.StatusCode != System.Net.HttpStatusCode.Created)
                {
                    loginResponse.success = false;
                    loginResponse.error = httpResponse.StatusCode.ToString();
                    return loginResponse;
                }

                Data = httpResponse.Content.ReadAsStringAsync().Result;
                loginResponse = JsonConvert.DeserializeObject<AddSymptomResponse>(Data);
                loginResponse.success = true;
                return loginResponse;
            }
            catch (System.Net.Http.HttpRequestException ex)
            {
                loginResponse.success = false;
                loginResponse.error = "Check internet connectivity";
                return loginResponse;
            }
            catch (Exception ex)
            {
                loginResponse.success = false;
                loginResponse.error = ex.Message;
                return loginResponse;
            }
            #endregion
        }

        public async Task<UpdateSymptomResponse> UpdateSymptoms(string token, UpdateSymptom model)
        {
            #region Old Code
            HttpResponseMessage httpResponse = new HttpResponseMessage();
            UpdateSymptomResponse response = new UpdateSymptomResponse();
            string Data = string.Empty;


            try
            {
                var json = JsonConvert.SerializeObject(model);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                httpResponse = await client.PostAsync($"{App.base_url}analyze/update-symptom", content);

                if (httpResponse.StatusCode != System.Net.HttpStatusCode.Created)
                {
                    response.success = false;
                    response.error = httpResponse.StatusCode.ToString();
                    return response;
                }

                Data = httpResponse.Content.ReadAsStringAsync().Result;
                response = JsonConvert.DeserializeObject<UpdateSymptomResponse>(Data);
                response.success = true;
                return response;
            }
            catch (System.Net.Http.HttpRequestException ex)
            {
                response.success = false;
                response.error = "Check internet connectivity";
                return response;
            }
            catch (Exception ex)
            {
                response.success = false;
                response.error = ex.Message;
                return response;
            }
            #endregion
        }

        public async Task<ServicesModel> GetServices(string token, int submodel_id)
        {
            HttpResponseMessage httpResponse = new HttpResponseMessage();
            ServicesModel model = new ServicesModel();
            //client = new HttpClient();
            string Data = string.Empty;
            string json = string.Empty;
            string url = string.Empty;

            try
            {
                //url = $"{base_url}pauthor/get-service-list/?id={id}&sub_model={submodel_id}";
                url = $"{App.base_url}pauthor/get-services-mapping/?s_model={submodel_id}";
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                httpResponse = await client.GetAsync(url);

                if (httpResponse.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    model.success = false;
                    model.status = httpResponse.StatusCode.ToString();
                    return model;
                }

                Data = httpResponse.Content.ReadAsStringAsync().Result;
                Debug.WriteLine($"{Data}", "Relay Command Response");
                model = JsonConvert.DeserializeObject<ServicesModel>(Data);
                model.status = httpResponse.StatusCode.ToString();
                model.success = true;
                return model;
            }
            catch (Exception ex)
            {
                model.success = false;
                //model.status = 1000;
                model.status = ex.Message;
                return model;
            }
        }

        public async Task<AddServiceResponse> AddServices(string token, AddService model)
        {
            #region Old Code
            HttpResponseMessage httpResponse = new HttpResponseMessage();
            AddServiceResponse loginResponse = new AddServiceResponse();
            string Data = string.Empty;


            try
            {
                var json = JsonConvert.SerializeObject(model);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                httpResponse = await client.PostAsync($"{App.base_url}analyze/create-service/", content);

                if (httpResponse.StatusCode != System.Net.HttpStatusCode.Created)
                {
                    loginResponse.success = false;
                    loginResponse.error = httpResponse.StatusCode.ToString();
                    return loginResponse;
                }

                Data = httpResponse.Content.ReadAsStringAsync().Result;
                loginResponse = JsonConvert.DeserializeObject<AddServiceResponse>(Data);
                loginResponse.success = true;
                return loginResponse;
            }
            catch (System.Net.Http.HttpRequestException ex)
            {
                loginResponse.success = false;
                loginResponse.error = "Check internet connectivity";
                return loginResponse;
            }
            catch (Exception ex)
            {
                loginResponse.success = false;
                loginResponse.error = ex.Message;
                return loginResponse;
            }
            #endregion
        }

        public async Task<UpdateServicesResponse> UpdateServices(string token, UpdateServices model)
        {
            #region Old Code
            HttpResponseMessage httpResponse = new HttpResponseMessage();
            UpdateServicesResponse response = new UpdateServicesResponse();
            string Data = string.Empty;


            try
            {
                var json = JsonConvert.SerializeObject(model);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                httpResponse = await client.PostAsync($"{App.base_url}analyze/update-service", content);

                if (httpResponse.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    response.success = false;
                    response.error = httpResponse.StatusCode.ToString();
                    return response;
                }

                Data = httpResponse.Content.ReadAsStringAsync().Result;
                response = JsonConvert.DeserializeObject<UpdateServicesResponse>(Data);
                response.success = true;
                return response;
            }
            catch (System.Net.Http.HttpRequestException ex)
            {
                response.success = false;
                response.error = "Check internet connectivity";
                return response;
            }
            catch (Exception ex)
            {
                response.success = false;
                response.error = ex.Message;
                return response;
            }
            #endregion
        }

        public async Task<SparePartModel> GetSpareParts(string token, int submodel_id)
        {
            HttpResponseMessage httpResponse = new HttpResponseMessage();
            SparePartModel model = new SparePartModel();
            //client = new HttpClient();
            string Data = string.Empty;
            string json = string.Empty;
            string url = string.Empty;

            try
            {
                //url = $"{base_url}pauthor/get-service-list/?id={id}&sub_model={submodel_id}";
                url = $"{App.base_url}pauthor/get-sparepart-mapping/?s_model={submodel_id}";
                //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                httpResponse = await client.GetAsync(url);

                if (httpResponse.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    model.success = false;
                    model.status = httpResponse.StatusCode.ToString();
                    return model;
                }

                Data = httpResponse.Content.ReadAsStringAsync().Result;
                Debug.WriteLine($"{Data}", "Relay Command Response");
                model = JsonConvert.DeserializeObject<SparePartModel>(Data);
                model.status = httpResponse.StatusCode.ToString();
                model.success = true;
                return model;
            }
            catch (Exception ex)
            {
                model.success = false;
                //model.status = 1000;
                model.status = ex.Message;
                return model;
            }
        }

        public async Task<AddSparePartResponse> AddSparePart(string token, AddSparePart model)
        {
            #region Old Code
            HttpResponseMessage httpResponse = new HttpResponseMessage();
            AddSparePartResponse loginResponse = new AddSparePartResponse();
            string Data = string.Empty;


            try
            {
                var json = JsonConvert.SerializeObject(model);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                httpResponse = await client.PostAsync($"{App.base_url}analyze/create-sparepart/", content);

                if (httpResponse.StatusCode != System.Net.HttpStatusCode.Created)
                {
                    loginResponse.success = false;
                    loginResponse.error = httpResponse.StatusCode.ToString();
                    return loginResponse;
                }

                Data = httpResponse.Content.ReadAsStringAsync().Result;
                loginResponse = JsonConvert.DeserializeObject<AddSparePartResponse>(Data);
                loginResponse.success = true;
                return loginResponse;
            }
            catch (System.Net.Http.HttpRequestException ex)
            {
                loginResponse.success = false;
                loginResponse.error = "Check internet connectivity";
                return loginResponse;
            }
            catch (Exception ex)
            {
                loginResponse.success = false;
                loginResponse.error = ex.Message;
                return loginResponse;
            }
            #endregion
        }

        public async Task<UpdateSparePartResponse> UpdateSparePart(string token, UpdateSparePart model)
        {
            #region Old Code
            HttpResponseMessage httpResponse = new HttpResponseMessage();
            UpdateSparePartResponse response = new UpdateSparePartResponse();
            string Data = string.Empty;


            try
            {
                var json = JsonConvert.SerializeObject(model);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                httpResponse = await client.PostAsync($"{App.base_url}analyze/update-sparepart", content);

                if (httpResponse.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    response.success = false;
                    response.error = httpResponse.StatusCode.ToString();
                    return response;
                }

                Data = httpResponse.Content.ReadAsStringAsync().Result;
                response = JsonConvert.DeserializeObject<UpdateSparePartResponse>(Data);
                response.success = true;
                return response;
            }
            catch (System.Net.Http.HttpRequestException ex)
            {
                response.success = false;
                response.error = "Check internet connectivity";
                return response;
            }
            catch (Exception ex)
            {
                response.success = false;
                response.error = ex.Message;
                return response;
            }
            #endregion
        }

        public async Task<VehicleModel> GetVehicleEcu(string token, int submodel_id)
        {
            HttpResponseMessage httpResponse = new HttpResponseMessage();
            VehicleModel model = new VehicleModel();
            //client = new HttpClient();
            string Data = string.Empty;
            //string json = string.Empty;
            string url = string.Empty;

            try
            {
                //url = $"{base_url}pauthor/get-service-list/?id={id}&sub_model={submodel_id}";
                url = $"{App.base_url}models/models-details/?sub_models={submodel_id}";
                //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                httpResponse = await client.GetAsync(url);

                if (httpResponse.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    model.success = false;
                    model.error = httpResponse.StatusCode.ToString();
                    return model;
                }

                Data = httpResponse.Content.ReadAsStringAsync().Result;
                Debug.WriteLine($"{Data}", "Relay Command Response");
                model = JsonConvert.DeserializeObject<VehicleModel>(Data);
                model.error = httpResponse.StatusCode.ToString();
                model.success = true;
                return model;
            }
            catch (Exception ex)
            {
                model.success = false;
                //model.status = 1000;
                model.error = ex.Message;
                return model;
            }
        }

        public async Task<VehicleModel> QuoteforPickupDrop(string token, string jobcard_id)
        {
            HttpResponseMessage httpResponse = new HttpResponseMessage();
            VehicleModel model = new VehicleModel();
            //client = new HttpClient();
            string Data = string.Empty;
            string json = string.Empty;
            string url = string.Empty;

            try
            {
                //url = $"{base_url}pauthor/get-service-list/?id={id}&sub_model={submodel_id}";
                url = $"{App.base_url}analyze/update-jcstatus";
                //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                json = "{ \"id\":\"" + jobcard_id + "\" }";//JsonConvert.SerializeObject(model);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                httpResponse = await client.PostAsync(url, content);

                if (httpResponse.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    model.success = false;
                    model.error = httpResponse.StatusCode.ToString();
                    return model;
                }

                Data = httpResponse.Content.ReadAsStringAsync().Result;
                Debug.WriteLine($"{Data}", "Relay Command Response");
                model = JsonConvert.DeserializeObject<VehicleModel>(Data);
                model.error = httpResponse.StatusCode.ToString();
                model.success = true;
                return model;
            }
            catch (Exception ex)
            {
                model.success = false;
                //model.status = 1000;
                model.error = ex.Message;
                return model;
            }
        }

        public async Task<TechnicianUserModel> GetTechnician(string token, string user_role)
        {
            HttpResponseMessage httpResponse = new HttpResponseMessage();
            TechnicianUserModel model = new TechnicianUserModel();
            //client = new HttpClient();
            string Data = string.Empty;
            string json = string.Empty;
            string url = string.Empty;

            try
            {
                url = $"{App.base_url}users/get-role-user/?role={user_role}";
                //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                httpResponse = await client.GetAsync(url);

                if (httpResponse.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    model.success = false;
                    model.status = httpResponse.StatusCode.ToString();
                    return model;
                }

                Data = httpResponse.Content.ReadAsStringAsync().Result;
                Debug.WriteLine($"{Data}", "Relay Command Response");
                model = JsonConvert.DeserializeObject<TechnicianUserModel>(Data);
                model.status = httpResponse.StatusCode.ToString();
                model.success = true;
                return model;
            }
            catch (Exception ex)
            {
                model.success = false;
                //model.status = 1000;
                model.status = ex.Message;
                return model;
            }
        }

        public async Task<AddPickupModelResponse> AssignTechnician(string token, AddPickupDropModel model)
        {
            #region Old Code
            HttpResponseMessage httpResponse = new HttpResponseMessage();
            AddPickupModelResponse response = new AddPickupModelResponse();
            string Data = string.Empty;


            try
            {
                var json = JsonConvert.SerializeObject(model);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                httpResponse = await client.PostAsync($"{App.base_url}analyze/update-pickupdrop", content);

                if (httpResponse.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    response.success = false;
                    response.status = httpResponse.StatusCode.ToString();
                    return response;
                }

                Data = httpResponse.Content.ReadAsStringAsync().Result;
                response = JsonConvert.DeserializeObject<AddPickupModelResponse>(Data);
                response.success = true;
                return response;
            }
            catch (System.Net.Http.HttpRequestException ex)
            {
                response.success = false;
                response.status = "Check internet connectivity";
                return response;
            }
            catch (Exception ex)
            {
                response.success = false;
                response.status = ex.Message;
                return response;
            }
            #endregion
        }

        public async Task<AddPickupModelResponse> TechnicianPickupStatus(string token, string id, string status)
        {
            #region Old Code
            HttpResponseMessage httpResponse = new HttpResponseMessage();
            AddPickupModelResponse response = new AddPickupModelResponse();
            string Data = string.Empty;


            try
            {
                var json = "{ \"id\":\"" + id + "\",\"status\":\"" + status + "\" }"; //JsonConvert.SerializeObject(model);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                httpResponse = await client.PostAsync($"{App.base_url}analyze/pickup-started", content);

                if (httpResponse.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    response.success = false;
                    response.status = httpResponse.StatusCode.ToString();
                    return response;
                }

                Data = httpResponse.Content.ReadAsStringAsync().Result;
                response = JsonConvert.DeserializeObject<AddPickupModelResponse>(Data);
                response.success = true;
                return response;
            }
            catch (System.Net.Http.HttpRequestException ex)
            {
                response.success = false;
                response.status = "Check internet connectivity";
                return response;
            }
            catch (Exception ex)
            {
                response.success = false;
                response.status = ex.Message;
                return response;
            }
            #endregion
        }

        public async Task<AddPickupModelResponse> CreateEntryExitCheck(string token, AddEntryExitCheckModel model)
        {
            #region Old Code
            HttpResponseMessage httpResponse = new HttpResponseMessage();
            AddPickupModelResponse response = new AddPickupModelResponse();
            string Data = string.Empty;


            try
            {
                string json = JsonConvert.SerializeObject(model);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                httpResponse = await client.PostAsync($"{App.base_url}analyze/create-entryexit/", content);

                if (httpResponse.StatusCode != System.Net.HttpStatusCode.Created)
                {
                    response.success = false;
                    response.status = httpResponse.StatusCode.ToString();
                    return response;
                }

                Data = httpResponse.Content.ReadAsStringAsync().Result;
                response = JsonConvert.DeserializeObject<AddPickupModelResponse>(Data);
                response.success = true;
                return response;
            }
            catch (System.Net.Http.HttpRequestException ex)
            {
                response.success = false;
                response.status = "Check internet connectivity";
                return response;
            }
            catch (Exception ex)
            {
                response.success = false;
                response.status = ex.Message;
                return response;
            }
            #endregion
        }

        public async Task<AddPickupModelResponse> JC_PickToEntry(string token, string id)
        {
            #region Old Code
            HttpResponseMessage httpResponse = new HttpResponseMessage();
            AddPickupModelResponse response = new AddPickupModelResponse();
            string Data = string.Empty;


            try
            {
                string json = "{ \"id\":\"" + id + "\" }"; //JsonConvert.SerializeObject(model);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                httpResponse = await client.PostAsync($"{App.base_url}analyze/pickup-reachedworkshop", content);

                if (httpResponse.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    response.success = false;
                    response.status = httpResponse.StatusCode.ToString();
                    return response;
                }

                Data = httpResponse.Content.ReadAsStringAsync().Result;
                response = JsonConvert.DeserializeObject<AddPickupModelResponse>(Data);
                response.success = true;
                return response;
            }
            catch (System.Net.Http.HttpRequestException ex)
            {
                response.success = false;
                response.status = "Check internet connectivity";
                return response;
            }
            catch (Exception ex)
            {
                response.success = false;
                response.status = ex.Message;
                return response;
            }
            #endregion
        }

        public async Task<ServiceAdvisorModel> GetServiceAdvisor(string token, string role)
        {
            HttpResponseMessage httpResponse = new HttpResponseMessage();
            ServiceAdvisorModel model = new ServiceAdvisorModel();
            //client = new HttpClient();
            string Data = string.Empty;
            string json = string.Empty;
            string url = string.Empty;

            try
            {

                //url = $"{base_url}symptom/symptom-list/?s_model={submodel_id}";
                url = $"{App.base_url}users/get-role-user/?role={role}";
                //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                httpResponse = await client.GetAsync(url);

                if (httpResponse.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    model.success = false;
                    model.status = httpResponse.StatusCode.ToString();
                    return model;
                }

                Data = httpResponse.Content.ReadAsStringAsync().Result;
                Debug.WriteLine($"{Data}", "Relay Command Response");
                model = JsonConvert.DeserializeObject<ServiceAdvisorModel>(Data);
                //model.status = httpResponse.StatusCode.ToString();
                model.success = true;
                return model;
            }
            catch (Exception ex)
            {
                model.success = false;
                //model.status = 1000;
                model.status = ex.Message;
                return model;
            }
        }

        public async Task<AddPickupModelResponse> AssignServiceAdvisor(string token, AddPickupDropModel model)
        {
            #region Old Code
            HttpResponseMessage httpResponse = new HttpResponseMessage();
            AddPickupModelResponse response = new AddPickupModelResponse();
            string Data = string.Empty;


            try
            {
                var json = JsonConvert.SerializeObject(model);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                httpResponse = await client.PostAsync($"{App.base_url}analyze/update-pickupdrop", content);

                if (httpResponse.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    response.success = false;
                    response.status = httpResponse.StatusCode.ToString();
                    return response;
                }

                Data = httpResponse.Content.ReadAsStringAsync().Result;
                response = JsonConvert.DeserializeObject<AddPickupModelResponse>(Data);
                response.success = true;
                return response;
            }
            catch (System.Net.Http.HttpRequestException ex)
            {
                response.success = false;
                response.status = "Check internet connectivity";
                return response;
            }
            catch (Exception ex)
            {
                response.success = false;
                response.status = ex.Message;
                return response;
            }
            #endregion
        }
        #endregion


        #region [Collaborate]
        public async Task<RegistrationResponseModel> CreateCollaborate(MediaFile img_file, MediaFile vid_file, CreateCollaborateModel model)
        {
            HttpResponseMessage http_response = new HttpResponseMessage();
            RegistrationResponseModel registrationResponseModel = new RegistrationResponseModel();
            string Data = string.Empty;
            string url = $"http://143.110.242.17/tweet/";
            try
            {
                var content = new MultipartFormDataContent();
                content.Add(new StreamContent(img_file.GetStream()), "\"image\"", $"{img_file.Path}");
                content.Add(new StreamContent(vid_file.GetStream()), "\"video\"", $"{vid_file.Path}");
                content.Add(new StringContent(model.user), "user");
                content.Add(new StringContent(model.Pid_tag), "Pid_tag");
                content.Add(new StringContent(model.Tweet_tag), "Tweet_tag");
                content.Add(new StringContent(model.hashtags), "hashtags");
                content.Add(new StringContent(model.parent), "parent");
                content.Add(new StringContent(model.reply_to), "reply_to");
                content.Add(new StringContent(model.timestamp), "timestamp"); ;

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                http_response = await client.PostAsync(url, content);

                Data = http_response.Content.ReadAsStringAsync().Result;
                if (http_response.StatusCode == System.Net.HttpStatusCode.OK || http_response.StatusCode == System.Net.HttpStatusCode.Created)
                {
                    registrationResponseModel.userResponse = JsonConvert.DeserializeObject<UserResponse>(Data);
                }
                else
                {
                    registrationResponseModel.registrationError = JsonConvert.DeserializeObject<RegistrationError>(Data);
                }
                registrationResponseModel.status_code = http_response.StatusCode;
                return registrationResponseModel;
            }
            catch (Exception ex)
            {
                //Debug.WriteLine("Error - inside get user regis api");
                registrationResponseModel.null_error = Data;
                return registrationResponseModel;
            }
        }

        public async Task<JobcardModel> GetCollaborate(string token, string dtc_code)
        {
            HttpResponseMessage http_response = new HttpResponseMessage();
            JobcardModel model = new JobcardModel();
            string Data = string.Empty;
            string url = $"http://143.110.242.17/pid_tag/?{dtc_code}";
            try
            {
                //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("JWT", token);
                http_response = await client.GetAsync(url);

                if (http_response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    model.status = false;
                    model.message = http_response.StatusCode.ToString();
                    return model;
                }

                Data = http_response.Content.ReadAsStringAsync().Result;
                model = JsonConvert.DeserializeObject<JobcardModel>(Data);
                model.status = true;
                return model;//.results.FirstOrDefault(x => x.id == id);
            }
            catch (Exception ex)
            {
                model.status = false;
                model.message = ex.Message;
                return model;
            }
        }
        #endregion

        #region PartsInformation
        public async Task<List<PartsTypeList>> GetPartType()
        {
            HttpResponseMessage httpResponse = new HttpResponseMessage();
            try
            {
                string token = Preferences.Get("tokenIT", String.Empty);
                string Data = string.Empty;
                var json = "{\"market_place\":\"wikitek1\"}";
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("JWT", token);
                httpResponse = await client.PostAsync($"{inventabBaseUrl}parts/get/parttype",content);
                Data = httpResponse.Content.ReadAsStringAsync().Result;
                var response = JsonConvert.DeserializeObject<List<string>>(Data);

                List<PartsTypeList> list = new List<PartsTypeList>();
                foreach (var item in response)
                {
                    list.Add(new PartsTypeList
                    {
                        name = item
                    });
                }
                //return response.results;
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public async Task<List<PartsCategoryList>> GetPartCategory()
        {
            HttpResponseMessage httpResponse = new HttpResponseMessage();
            try
            {
                string token = Preferences.Get("tokenIT", String.Empty);
                string Data = string.Empty;

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("JWT", token);
                httpResponse = await client.GetAsync($"{inventabBaseUrl}parts/get/partsubcategory/?market_place=wikitek1");
                Data = httpResponse.Content.ReadAsStringAsync().Result;
                var response = JsonConvert.DeserializeObject<PartsCategory>(Data);
                return response.results;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<List<MetaTagList>> GetMetatags()
        {
            HttpResponseMessage httpResponse = new HttpResponseMessage();
            try
            {
                string token = Preferences.Get("tokenIT", String.Empty);
                string Data = string.Empty;
                var json = "{\"market_place\":\"wikitek1\"}"; 
                
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("JWT", token);
                httpResponse = await client.PostAsync($"{inventabBaseUrl}parts/fetch/meta/tag/",content);
                Data = httpResponse.Content.ReadAsStringAsync().Result;
                var response = JsonConvert.DeserializeObject<List<string>>(Data);

                List<MetaTagList> list = new List<MetaTagList>();
                foreach (var item in response)
                {
                    list.Add(new MetaTagList
                    {
                        name = item
                    });
                }
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<HotPartsModel> GetHotPartList()
        {
            HttpResponseMessage httpResponse = new HttpResponseMessage();
            try
            {
                string token = Preferences.Get("tokenIT", String.Empty);
                string Data = string.Empty;

                //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("JWT", token);
                httpResponse = await client.GetAsync($"{inventabBaseUrl}pipo/marketplace/part/?marketplace_name=wikitek1");
                Data = httpResponse.Content.ReadAsStringAsync().Result;
                var response = JsonConvert.DeserializeObject<HotPartsModel>(Data);
                return response;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<MyOrdersModel> GetMyOrdersData()
        {
            HttpResponseMessage httpResponse = new HttpResponseMessage();
            try
            {
                string token = Preferences.Get("tokenIT", String.Empty);
                string user_id = Preferences.Get("userIdIT", String.Empty);
                string Data = string.Empty;
                MyOrdersModel response = new MyOrdersModel();

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("JWT", token);
                httpResponse = await client.GetAsync($"{inventabBaseUrl}carts/my_orders/?user_id={user_id}");

                if (httpResponse.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {  
                    response.status_code = httpResponse.StatusCode;
                }
                else
                {
                    Data = httpResponse.Content.ReadAsStringAsync().Result;
                    response = JsonConvert.DeserializeObject<MyOrdersModel>(Data);
                }
                    
                return response;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<InvoiceModel> GetInvoiceData(string order_id)
        {
            HttpResponseMessage httpResponse = new HttpResponseMessage();
            try
            {
                string token = Preferences.Get("tokenIT", String.Empty);
                string Data = string.Empty;

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("JWT", token);
                httpResponse = await client.GetAsync($"{inventabBaseUrl}invoices/fetch/all/invoices/?order_id={order_id}");
                Data = httpResponse.Content.ReadAsStringAsync().Result;
                var response = JsonConvert.DeserializeObject<InvoiceModel>(Data);
                return response;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion
    }
}
