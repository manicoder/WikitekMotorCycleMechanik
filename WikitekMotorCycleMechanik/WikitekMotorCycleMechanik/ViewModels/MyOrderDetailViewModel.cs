using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using WikitekMotorCycleMechanik.Models;
using WikitekMotorCycleMechanik.View.GdSection;
using Xamarin.Forms;

namespace WikitekMotorCycleMechanik.ViewModels
{
    public class MyOrderDetailViewModel : BaseViewModel
    {
        readonly Page page;
        public MyOrderDetailViewModel(MyOrderListModel selectedOrder, Page page)
        {
            this.page = page;
            this.orderDetail = selectedOrder;
            SelectedPartNo = orderDetail.part_number;
            serialPartList = orderDetail.serialized_parts;

            DropDownSelectionCommand = new Command(async (obj) =>
            {
                var selectedSerialNo = obj as SerializedPart;
                selectedSerialNo.isExpanded = !selectedSerialNo.isExpanded;
                selectedSerialNo.ic_updown = selectedSerialNo.isExpanded ? "ic_sort_up.png" : "ic_sort_down.png";
            });
            QRImageCommand = new Command(async (obj) =>
            {
                var selectedSerialNo = obj as SerializedPart;
                GdImageGD gdImage = new GdImageGD();
                gdImage.gd_image = selectedSerialNo.imageUrl;
                //await this.page.Navigation.PushAsync(new ImageZoomingPage(gdImage, null));
            });
        }

        private string _SelectedPartNo;
        public string SelectedPartNo
        {
            get => _SelectedPartNo;
            set
            {
                _SelectedPartNo = value;
                OnPropertyChanged("SelectedPartNo");
            }
        }

        private MyOrderListModel _orderDetail;
        public MyOrderListModel orderDetail
        {
            get => _orderDetail;
            set
            {
                _orderDetail = value;
                OnPropertyChanged("orderDetail");
            }
        }

        private List<SerializedPart> _serialPartList;
        public List<SerializedPart> serialPartList
        {
            get => _serialPartList;
            set
            {
                _serialPartList = value;
                OnPropertyChanged("serialPartList");
            }
        }

        public ICommand DropDownSelectionCommand { get; set; }
        public ICommand QRImageCommand { get; set; }
    }
}
