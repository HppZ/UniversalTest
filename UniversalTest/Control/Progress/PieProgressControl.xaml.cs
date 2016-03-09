using System;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace UniversalTest.Control.Progress
{
    public sealed partial class PieProgressControl : UserControl
    {
        #region Ctor
        public PieProgressControl()
        {
            this.InitializeComponent();
            this.SizeChanged += PieProgressControl_SizeChanged;
        }
        #endregion

        #region private
        
        /// <summary>
        /// 大小变化
        /// </summary>
        private void PieProgressControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ProgressPie.Width = this.ActualHeight - 6;
            ProgressPie.Height = ProgressPie.Width;
            var h = (ProgressPie.Height) * 1.0 / 2;
            PiePath.Center = new Point(h, h);
            PiePath.Radius = h;
        }
        #endregion
    }
}
