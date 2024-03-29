﻿using Xamarin.Forms;

namespace WikitekMotorCycleMechanik.Interfaces
{
    public interface ILodingPageService
    {
        void InitLoadingPage(ContentPage loadingIndicatorPage = null);

        void ShowLoadingPage();

        void HideLoadingPage();

        void IsExpert(ContentPage loadingIndicatorPage = null);

        void IsExpertShow_Active();

        void IsExpertShow_NotActive();
    }
}
