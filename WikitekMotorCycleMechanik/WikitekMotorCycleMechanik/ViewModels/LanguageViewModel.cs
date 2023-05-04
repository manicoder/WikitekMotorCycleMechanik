using WikitekMotorCycleMechanik.Models;
using WikitekMotorCycleMechanik.Services;
using WikitekMotorCycleMechanik.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace WikitekMotorCycleMechanik.ViewModels
{
    public class LanguageViewModel : BaseViewModel
    {
        ApiServices services;
        public LanguageViewModel()
        {
            services = new ApiServices();
            LaguageList = new ObservableCollection<LanguageModel>
            {
                new LanguageModel
                {
                    id = 1,
                    is_checked = false,
                    Language = "English",
                },
                new LanguageModel
                {
                    id = 2,
                    is_checked = false,
                    Language = "हिन्दी",
                },
                new LanguageModel
                {
                    id = 3,
                    is_checked = false,
                    Language = "ಕನ್ನಡ",
                },
                new LanguageModel
                {
                    id = 4,
                    is_checked = false,
                    Language = "മലയാളം",
                },
                new LanguageModel
                {
                    id = 5,
                    is_checked = false,
                    Language = "తెలుగు",
                },
                new LanguageModel
                {
                    id = 6,
                    is_checked = false,
                    Language = "বাংলা",
                },
                new LanguageModel
                {
                    id = 7,
                    is_checked = false,
                    Language = "தமிழ்",
                },
                //new LanguageModel
                //{
                //    id = 8,
                //    is_checked = false,
                //    Language = "മലയാളം",
                //},
                //new LanguageModel
                //{
                //    id = 9,
                //    is_checked = false,
                //    Language = "தமிழ்",
                //},
                //new LanguageModel
                //{
                //    id = 10,
                //    is_checked = false,
                //    Language = "ಕನ್ನಡ",
                //}
            };
        }

        private ObservableCollection<LanguageModel> laguageList;
        public ObservableCollection<LanguageModel> LaguageList
        {
            get => laguageList;
            set
            {
                laguageList = value;
                OnPropertyChanged("LaguageList");
            }
        }
    }
}
