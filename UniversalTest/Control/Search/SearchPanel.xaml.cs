using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace UniversalTest.Control.Search
{
    public sealed partial class SearchPanel : UserControl
    {
        #region fields
        private const double _width1 = 250;
        private const double _width2 = 270;
        private Popup _popup;
        private Storyboard _entranceStoryboard;
        private Storyboard _fadeInStoryboard;
        private ContentType _contentType;
        private bool _isShowing;
        private Action _entranceStoryboardCompleteAction;
        #endregion

        public SearchPanel()
        {
            this.InitializeComponent();
            _popup = new Popup
            {
                Child = this
            };
            this.RenderTransform = new CompositeTransform();
        }

        #region public
        /// <summary>
        /// 显示
        /// </summary>
        /// <returns></returns>
        public void Show(Point finalPosition, bool isPreview)
        {
            _isShowing = true;

            this.Opacity = 1;
            if (isPreview)
            {
                this.Width = _width2;
                (this.RenderTransform as CompositeTransform).TranslateX = _width2;
                this.Height = Window.Current.Bounds.Height - finalPosition.Y;
                _contentType = ContentType.PreviewResult;
                CreateEntranceAnimation(true);
            }
            else
            {
                this.Width = _width1;
                (this.RenderTransform as CompositeTransform).TranslateX = 0;
                this.Height = 110;
                _contentType = ContentType.Result;
            }

            _popup.HorizontalOffset = finalPosition.X;
            _popup.VerticalOffset = finalPosition.Y;
            _popup.IsOpen = true;
        }

        /// <summary>
        /// 隐藏
        /// </summary>
        /// <returns></returns>
        public void Close()
        {
            _isShowing = false;

            if (_contentType == ContentType.PreviewResult)
            {
                _entranceStoryboardCompleteAction = HidePopup;
                CreateEntranceAnimation(false);
            }
            else
            {
                HidePopup();
            }
        }

        public void Swith()
        {
            if (_contentType == ContentType.Result) return;

            _contentType = ContentType.Result;
            CreateFadeOutAnimation();
        }

        public void SetPreviewSource(object source)
        {
            
        }

        public void SetSearchResult(object result)
        {
            
        }

        #endregion

        #region private

        private void CreateEntranceAnimation(bool isShow)
        {
            if (_entranceStoryboard == null)
            {
                _entranceStoryboard = new Storyboard();
                DoubleAnimation doubleAnimation = new DoubleAnimation()
                {
                    Duration = TimeSpan.FromMilliseconds(300),
                    EasingFunction = new QuarticEase()
                    {
                        EasingMode = EasingMode.EaseOut
                    }
                };

                Storyboard.SetTarget(doubleAnimation, this.RenderTransform);
                Storyboard.SetTargetProperty(doubleAnimation, "(CompositeTransform.TranslateX)");
                _entranceStoryboard.Children.Add(doubleAnimation);
                _entranceStoryboard.Completed += (e, e1) =>
                {
                    _entranceStoryboardCompleteAction?.Invoke();
                    _entranceStoryboardCompleteAction = null;
                };
            }

            (_entranceStoryboard.Children[0] as DoubleAnimation).To = isShow ? 0 : _width2;
            _entranceStoryboard.Begin();
        }


        private void CreateFadeOutAnimation(bool isShow = false)
        {
            if (_fadeInStoryboard == null)
            {
                _fadeInStoryboard = new Storyboard();
                DoubleAnimation doubleAnimation = new DoubleAnimation()
                {
                    Duration = TimeSpan.FromMilliseconds(300),
                    EasingFunction = new QuarticEase()
                    {
                        EasingMode = EasingMode.EaseOut
                    }
                };

                Storyboard.SetTarget(doubleAnimation, this);
                Storyboard.SetTargetProperty(doubleAnimation, "Opacity");
                _fadeInStoryboard.Children.Add(doubleAnimation);
            }

            (_fadeInStoryboard.Children[0] as DoubleAnimation).To = isShow ? 1 : 0;
            _fadeInStoryboard.Begin();
        }

        private void HidePopup()
        {
            if (!_isShowing)
            {
                _popup.IsOpen = false;
            }
        }
 
        #endregion
    }

    enum ContentType
    {
        None,
        PreviewResult,
        Result,
    }
}
