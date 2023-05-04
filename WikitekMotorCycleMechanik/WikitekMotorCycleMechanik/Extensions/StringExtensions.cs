using WikitekMotorCycleMechanik.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Text;
using Xamarin.Forms;

namespace WikitekMotorCycleMechanik.Extensions
{
    public static class StringExtensions
    {
        public static string OutPutValue { get; set; }

        public static string Translate(this string text)
        {
            if (text != null)
            {
                OutPutValue = null;
                string[] ReceivedString = text.Split(' ');
                foreach (var item in ReceivedString)
                {
                    var assembly = typeof(StringExtensions).GetTypeInfo().Assembly;
                    var assemblyName = assembly.GetName();
                    ResourceManager resourceManager = new ResourceManager($"{assemblyName.Name}.Resources", assembly);
                    CultureInfo.CurrentCulture.ClearCachedData();
                    //var lg = CultureInfo.CurrentCulture.TwoLetterISOLanguageName.Trim();
                    var SelectedLanguage = DependencyService.Get<ISaveLocalData>().GetData("LastSelectedLanguage");
                    if (string.IsNullOrEmpty(OutPutValue))
                    {
                        OutPutValue = resourceManager.GetString(item.ToLower(), new CultureInfo(SelectedLanguage, true));
                        if (OutPutValue == null)
                        {
                            OutPutValue = item.ToLower();
                        }
                    }
                    else
                    {
                        if (item != "")
                        {
                            var value = resourceManager.GetString(item.ToLower(), new CultureInfo(SelectedLanguage, true));
                            if (value == null)
                            {
                                OutPutValue = OutPutValue + " " + item.ToLower();
                                if (OutPutValue == null)
                                {
                                    OutPutValue = item.ToLower();
                                }
                            }
                            else
                            {
                                OutPutValue = OutPutValue + " " + value;
                                if (OutPutValue == null)
                                {
                                    OutPutValue = item.ToLower();
                                }
                            }
                        }
                    }
                }

                return OutPutValue;

                //var assembly = typeof(StringExtensions).GetTypeInfo().Assembly;
                //var assemblyName = assembly.GetName();
                //ResourceManager resourceManager = new ResourceManager($"{assemblyName.Name}.Resources", assembly);
                //var lg = CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
                //return resourceManager.GetString(text, new CultureInfo(lg));
            }

            return null;
        }
    }
}
