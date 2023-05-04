using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Xamarin.Forms;

namespace WikitekMotorCycleMechanik.Behaviors
{
    public class PasswordValidationBehavior : Behavior<Entry>
    {
        const string passwordRegex = @"^(?=.*[a-z])(?=.*\d).{8,}$";
        //public static readonly BindableProperty MaxLengthProperty = BindableProperty.Create("MaxLength", typeof(int), typeof(PasswordValidationBehavior), 0);

        //public int MaxLen
        //{
        //    get { return (int)GetValue(MaxLengthProperty); }
        //    set { SetValue(MaxLengthProperty, value); }
        //}

        protected override void OnAttachedTo(Entry bindable)
        {
            bindable.TextChanged += HandleTextChanged;
            base.OnAttachedTo(bindable);
        }

        void HandleTextChanged(object sender, TextChangedEventArgs e)
        {
            // Password Validation 
            bool IsValid = false;
            IsValid = (Regex.IsMatch(e.NewTextValue, passwordRegex));
           
            if (((Entry)sender).ClassId == "Login")
            {
                ((Entry)sender).TextColor = IsValid ? Color.White : Color.Red;
            }
            else
            {
                ((Entry)sender).TextColor = IsValid ? (Color)Application.Current.Resources["text_color"] : Color.Red;
            }

            ((Entry)sender).Text= e.NewTextValue.Replace(" ", "");

            // Max Lenght
            //if (e.NewTextValue.Length >= MaxLen)
            //    ((Entry)sender).Text = e.NewTextValue.Substring(0, MaxLen);
        }

        protected override void OnDetachingFrom(Entry bindable)
        {
            bindable.TextChanged -= HandleTextChanged;
            base.OnDetachingFrom(bindable);
        }
    }
}