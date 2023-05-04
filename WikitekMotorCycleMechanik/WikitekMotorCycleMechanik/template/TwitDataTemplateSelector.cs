using System;
using System.Collections.Generic;
using System.Text;
using WikitekMotorCycleMechanik.Models;
using Xamarin.Forms;

namespace WikitekMotorCycleMechanik.template
{
    public class TwitDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate VideoTemplate { get; set; }

        public DataTemplate ImageTemplate { get; set; }

        public DataTemplate TextTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (item == null)
                return null;


            if (((CollaborateModel)item).type == "video")
                return VideoTemplate;

            if (((CollaborateModel)item).type == "image")
                return ImageTemplate;

            if (((CollaborateModel)item).type == "text")
                return TextTemplate;

            return null;
        }
    }
}