using System;
using WikitekMotorCycleMechanik.CustomControls;
using WikitekMotorCycleMechanik.Droid.Renders;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomEditor), typeof(CustomEditorRenderer))]
namespace WikitekMotorCycleMechanik.Droid.Renders
{
    [Obsolete]
    public class CustomEditorRenderer : EditorRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            base.OnElementChanged(e);

            if (Control == null || e.NewElement == null) return;

            Control.SetBackgroundDrawable(null);
        }
    }
}