using Acr.UserDialogs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WikitekMotorCycleMechanik.Models;
using WikitekMotorCycleMechanik.Services;
using WikitekMotorCycleMechanik.StaticInfo;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace WikitekMotorCycleMechanik.ViewModels
{
    public class VehicleModelsViewModel : ViewModelBase
    {
        #region prop
        ApiServices apiServices;
        LoginResponse user_data;
        GetModel getModel;
        #endregion

        #region ctro
        public VehicleModelsViewModel(Page page, LoginResponse user_data, Oem oem, ObservableCollection<VehicleModelResult> model_list) : base(page)
        {
            try
            {
                apiServices = new ApiServices();
                this.user_data = user_data;
                selected_oem = oem;
                this.static_model_list = this.model_list = new ObservableCollection<VehicleModelResult>(model_list.Where(x => x.oem.id == oem.id));


                //var groupedCustomerList = (model_list.Where(x => x.oem.id == oem.id)).GroupBy(u => u.name).Select(grp => grp.ToList()).ToList();

                //var results = persons.GroupBy(p => p.PersonId,p => p.car,(key, g) => new { PersonId = key, Cars = g.ToList() });

                empty_view_detail = new ErrorModel();
            }
            catch (Exception ex)
            {
            }
        }
        #endregion

        #region Properties
        private Oem _selected_oem;
        public Oem selected_oem
        {
            get => _selected_oem;
            set
            {
                _selected_oem = value;
                OnPropertyChanged("selected_oem");
            }
        }

        private ObservableCollection<VehicleModelResult> _model_list;
        public ObservableCollection<VehicleModelResult> model_list
        {
            get => _model_list;
            set
            {
                _model_list = value;
                OnPropertyChanged("model_list");
            }
        }

        private ObservableCollection<VehicleModelResult> _static_model_list;
        public ObservableCollection<VehicleModelResult> static_model_list
        {
            get => _static_model_list;
            set
            {
                _static_model_list = value;
                OnPropertyChanged("static_model_list");
            }
        }

        private VehicleModelResult _selected_model;
        public VehicleModelResult selected_model
        {
            get => _selected_model;
            set
            {
                _selected_model = value;
                OnPropertyChanged("selected_model");
            }
        }

        //private ObservableCollection<VehicleSubModel> _selected_sub_model_list;
        //public ObservableCollection<VehicleSubModel> selected_sub_model_list
        //{
        //    get => _selected_sub_model_list;
        //    set
        //    {
        //        _selected_sub_model_list = value;
        //        OnPropertyChanged("selected_sub_model_list");
        //    }
        //}

        private ObservableCollection<SubmodelList> _sub_model_list;
        public ObservableCollection<SubmodelList> sub_model_list
        {
            get => _sub_model_list;
            set
            {
                _sub_model_list = value;
                OnPropertyChanged("sub_model_list");
            }
        }

        private SubmodelList _selected_submodel;
        public SubmodelList selected_submodel
        {
            get => _selected_submodel;
            set
            {
                _selected_submodel = value;
                OnPropertyChanged("selected_submodel");
            }
        }

        private ObservableCollection<VehicleSubModel> _model_year_list;
        public ObservableCollection<VehicleSubModel> model_year_list
        {
            get => _model_year_list;
            set
            {
                _model_year_list = value;
                OnPropertyChanged("model_year_list");
            }
        }

        private VehicleSubModel _selected_model_year;
        public VehicleSubModel selected_model_year
        {
            get => _selected_model_year;
            set
            {
                _selected_model_year = value;
                OnPropertyChanged("selected_model_year");
            }
        }

        private ErrorModel _empty_view_detail;
        public ErrorModel empty_view_detail
        {
            get => _empty_view_detail;
            set
            {
                _empty_view_detail = value;
                OnPropertyChanged("empty_view_detail");
            }
        }

        private string _txtSearchModel;
        public string txtSearchModel
        {
            get => _txtSearchModel;
            set
            {
                _txtSearchModel = value;
                OnPropertyChanged("txtSearchModel");
                try
                {
                    //var Data = static_model_list;
                    if (!string.IsNullOrEmpty(_txtSearchModel))
                    {
                        if (static_model_list != null && static_model_list.Any())
                        {
                            model_list = new ObservableCollection<VehicleModelResult>(static_model_list.Where(x => x.name.ToLower().Contains(_txtSearchModel.ToLower())).ToList());

                            if (model_list.Count < 1)
                            {
                                empty_view_detail.is_visible = false;
                                empty_view_detail.error_message = "Model does not exist";
                            }
                        }

                        //List<VehicleModelResult> VR = new List<VehicleModelResult>();
                        //if (model_list.Count == 0)
                        //{
                        //    model_list = Data;
                        //}

                        //foreach (var item in static_model_list)
                        //{
                        //    if (item.name.ToLower().Contains(_txtSearchModel.ToLower()))
                        //    {
                        //        VR.Add(new VehicleModelResult
                        //        {
                        //            name = item.name,
                        //            id = item.id,
                        //            model_file = item.model_file,
                        //            sub_models = item.sub_models,
                        //            model_file_lacal = item.model_file_lacal,
                        //            model_year = item.model_year,
                        //            oem = item.oem,
                        //            parent = item.parent
                        //        });
                        //    }
                        //    else
                        //    {
                        //        empty_view_detail.is_visible = false;
                        //        empty_view_detail.error_message = "This model does not exist";
                        //    }
                        //}

                        //model_list = new ObservableCollection<VehicleModelResult>(VR);
                    }
                    else
                    {
                        model_list = static_model_list;
                    }
                }
                catch (Exception ex)
                {
                }
            }
        }

        private string _change_gui_button = "ic_grid_list.png";
        public string change_gui_button
        {
            get => _change_gui_button;
            set
            {
                _change_gui_button = value;
                OnPropertyChanged("change_gui_button");
            }
        }

        private bool _bulleted_list_visible;
        public bool bulleted_list_visible
        {
            get => _bulleted_list_visible;
            set
            {
                _bulleted_list_visible = value;
                OnPropertyChanged("bulleted_list_visible");
            }
        }

        private bool _grid_list_visible = false;
        public bool grid_list_visible
        {
            get => _grid_list_visible;
            set
            {
                _grid_list_visible = value;
                OnPropertyChanged("grid_list_visible");
            }
        }

        private bool _submodel_view_visible = false;
        public bool submodel_view_visible
        {
            get => _submodel_view_visible;
            set
            {
                _submodel_view_visible = value;
                OnPropertyChanged("submodel_view_visible");
            }
        }

        private bool _modelyear_view_visible = false;
        public bool modelyear_view_visible
        {
            get => _modelyear_view_visible;
            set
            {
                _modelyear_view_visible = value;
                OnPropertyChanged("modelyear_view_visible");
            }
        }

        private bool _submitbutton_view_visible = false;
        public bool submitbutton_view_visible
        {
            get => _submitbutton_view_visible;
            set
            {
                _submitbutton_view_visible = value;
                OnPropertyChanged("submitbutton_view_visible");
            }
        }
        #endregion

        #region Methods
        public void CancelSelectedModel()
        {
            CancelSelectedModel();
        }
        #endregion

        #region ICommands
        public ICommand BackCommand => new Command(async (obj) =>
        {
            await page.Navigation.PopAsync();
        });

        public ICommand ChangeGuiCommand => new Command(async (obj) =>
        {
            try
            {
                if (bulleted_list_visible)
                {
                    bulleted_list_visible = false;
                    grid_list_visible = true;
                    change_gui_button = "ic_bulleted_list.png";
                    Preferences.Set("list_gui", "tabel_list");
                }
                else
                {
                    bulleted_list_visible = true;
                    grid_list_visible = false;
                    change_gui_button = "ic_grid_list.png";
                    Preferences.Set("list_gui", "bulleted_list");
                }
            }
            catch (Exception ex)
            {
            }
        });

        public ICommand ItemSelectionCommand => new Command((obj) =>
        {
            try
            {
                selected_model = (VehicleModelResult)obj;
                sub_model_list = new ObservableCollection<SubmodelList>();
                App.model_id = selected_model.id;
                //selected_sub_model_list = new ObservableCollection<VehicleSubModel>(selected_model.sub_models);
                var res = selected_model.sub_models.GroupBy(p => p.name, (key, g) => new { PersonId = key, model_year_list = g.ToList() }).ToList();

                foreach (var item in res)
                {
                    sub_model_list.Add(
                        new SubmodelList
                        {
                            submodel = item.PersonId,
                            model_year_list = item.model_year_list
                        });
                }

                var json = JsonConvert.SerializeObject(res);
                submodel_view_visible = true;
            }
            catch (Exception ex)
            {
            }
        });

        public ICommand SelectSubModelCheckBoxCommand => new Command(async (obj) =>
        {
            try
            {
                var _selected_model = (SubmodelList)obj;
                if (sub_model_list != null && sub_model_list.Any())
                {
                    foreach (var sub_model in sub_model_list)
                    {
                        if (sub_model == _selected_model)
                        {
                            _selected_model.selected_sub_model = true;
                        }
                        else
                        {
                            sub_model.selected_sub_model = false;
                        }
                    }
                    return;
                }
            }
            catch (Exception ex)
            {
            }
        });

        public ICommand SelectSubmodelCommand => new Command(async (obj) =>
        {
            try
            {
                selected_submodel = sub_model_list.FirstOrDefault(x => x.selected_sub_model);

                if (selected_submodel == null)
                {
                    await page.DisplayAlert("Alert", "Please select a model.", "OK");
                    return;
                }
                else
                {
                    model_year_list = new ObservableCollection<VehicleSubModel>(selected_submodel.model_year_list.ToList());
                    //foreach (var sub_model in sub_model_list.ToList())
                    //{
                    //    if (selected_submodel != sub_model)
                    //    {
                    //        sub_model_list.Remove(sub_model);
                    //    }
                    //}
                    submodel_view_visible = false;
                    modelyear_view_visible = true;
                }
            }
            catch (Exception ex)
            {
            }
        });

        public ICommand SelectModelYearCommand => new Command(async (obj) =>
        {
            try
            {
                selected_model_year = model_year_list.FirstOrDefault(x => x.selected_sub_model);

                if (selected_model_year == null)
                {
                    await page.DisplayAlert("Alert", "Please select a model year.", "OK");
                    return;
                }
                else
                {
                    modelyear_view_visible = false;
                    submitbutton_view_visible = true;
                }
            }
            catch (Exception ex)
            {
            }
        });

        public ICommand SubmitCommand => new Command(async (obj) =>
        {
            try
            {
                StaticMethods.ecu_info = new List<EcuDataSet>();

                if (selected_model_year.ecus != null && selected_model_year.ecus.Any())
                {
                    App.submodel_id = selected_model_year.id;
                    StaticMethods.ecu_info.Clear();
                    foreach (var item in selected_model_year.ecus)
                    {
                      
                        StaticMethods.ecu_info.Add(
                            new EcuDataSet
                            {
                                ecu_name = item.name,
                                clear_dtc_index = item.clear_dtc_fn_index,
                                dtc_dataset_id = item.datasets.FirstOrDefault(),
                                pid_dataset_id = item.pid_datasets.FirstOrDefault(),
                                read_dtc_index = item.read_dtc_fn_index,
                                read_data_fn_index = item.read_data_fn_index,
                                tx_header = item.tx_header,
                                rx_header = item.rx_header,
                                protocol = item.protocol,
                                is_padding = item.padding
                            });
                    }
                    await page.Navigation.PushAsync(new Views.Connections.ConnectionPage(user_data, selected_model, selected_model_year, selected_oem));
                }
                else
                {
                    await page.DisplayAlert("Alert", "ECU not found for this submodel.", "OK");
                    return;
                }

            }
            catch (Exception ex)
            {
            }
        });

        public ICommand CancelCommand => new Command(async (obj) =>
        {
            submodel_view_visible = false;
            modelyear_view_visible = false;
            submitbutton_view_visible = false;


            foreach (SubmodelList submodel in sub_model_list.ToList())
            {
                submodel.selected_sub_model = false;
            }

            selected_submodel = new SubmodelList();
            selected_model = new VehicleModelResult();
        });
        #endregion
    }
}
