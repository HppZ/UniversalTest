using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using UniversalTest.Annotations;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace UniversalTest.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BlankPage26 : Page
    {
        private PClass vm;
        public BlankPage26()
        {
            this.InitializeComponent();
            vm = new PClass()
            {
                MyProperty = "hello"
            }; ;
            this.DataContext = vm;

            Tapped += BlankPage26_Tapped;
        }

        private void BlankPage26_Tapped(object sender, TappedRoutedEventArgs e)
        {
            try
            {
                Task.Run((() =>
                {
                    vm.MyProperty = vm.MyProperty + " world";
                }));
            }
            catch (Exception exception)
            {
                vm.MyProperty = "error: " + exception.Message;
            }

        }
    }


    class PClass : INotifyPropertyChanged
    {
        private string myVar;
        public string MyProperty
        {
            get { return myVar; }
            set
            {
                myVar = value;
                OnPropertyChanged();
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
