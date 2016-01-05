using System;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Templated Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234235

namespace UniversalTest.Control.Listview
{
    public enum RefreshState
    {
        Pulling = 1,
        ReleasePulling = 2,
        RefreshDoing = 3,
        RefreshCompleted = 4,
    }

    public sealed class PullToFresh : ListBox
    {
        private bool isPulling = false;
        private Windows.UI.Xaml.Controls.ScrollViewer ElementScrollViewer;
        private UIElement ElementRelease;
        private TextBlock StateDescription;
        private TextBlock UpdateDescription;
        private double ManipulationStartedYOffset;
        private RefreshState CurrentState = RefreshState.Pulling;

        public PullToFresh()
        {
            this.DefaultStyleKey = typeof(PullToFresh);
        }

        /// <summary>
        /// Builds the visual tree for the <see cref="T:System.Windows.Controls.ListBox"/>
        /// control when a new template is applied.
        /// </summary>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (ElementScrollViewer != null)
            {
                //				ElementScrollViewer.MouseMove -= viewer_MouseMove;
                ElementScrollViewer.DragEnter -= ElementScrollViewer_DragEnter;
                //ElementScrollViewer.ManipulationStarted -= ElementScrollViewer_ManipulationStarted;
                //ElementScrollViewer.ManipulationCompleted -= viewer_ManipulationCompleted;
                ElementScrollViewer.RemoveHandler(ManipulationStartedEvent, new EventHandler<ManipulationStartedEventArgs>(this.ElementScrollViewer_ManipulationStarted));
                ElementScrollViewer.RemoveHandler(ManipulationCompletedEvent, new EventHandler<ManipulationCompletedEventArgs>(this.viewer_ManipulationCompleted));
            }
            ElementScrollViewer = GetTemplateChild("ScrollViewer") as Windows.UI.Xaml.Controls.ScrollViewer;
            if (ElementScrollViewer != null)
            {
                ElementScrollViewer.DragEnter += ElementScrollViewer_DragEnter;
                //ElementScrollViewer.MouseMove += viewer_MouseMove;
                //ElementScrollViewer.ManipulationStarted += ElementScrollViewer_ManipulationStarted;
                //ElementScrollViewer.ManipulationCompleted += viewer_ManipulationCompleted;
                ElementScrollViewer.AddHandler(ManipulationStartedEvent, new EventHandler<ManipulationStartedEventArgs>(this.ElementScrollViewer_ManipulationStarted), true);
                ElementScrollViewer.AddHandler(ManipulationCompletedEvent, new EventHandler<ManipulationCompletedEventArgs>(this.viewer_ManipulationCompleted), true);
            }
            ElementRelease = GetTemplateChild("ReleaseElement") as UIElement;

            StateDescription = GetTemplateChild("StateDescription") as TextBlock;
            UpdateDescription = GetTemplateChild("UpdateDescription") as TextBlock;

            ChangeVisualState(RefreshState.Pulling, false);
            //GoToState("Pulling", false);
        }



        public void ChangeVisualState(RefreshState state, bool useTransitions)
        {
            switch (state)
            {
                case RefreshState.Pulling:
                    {
                        CurrentState = RefreshState.Pulling;
                        StateDescription.Text = PullDownText;
                        GoToState("Pulling", useTransitions);
                    }
                    break;
                case RefreshState.ReleasePulling:
                    {
                        if (CurrentState == RefreshState.Pulling)
                        {
                            CurrentState = RefreshState.ReleasePulling;
                            StateDescription.Text = ReleaseText;
                            GoToState("ReleasePulling", useTransitions);
                        }
                    }
                    break;
                case RefreshState.RefreshDoing:
                    {
                        CurrentState = RefreshState.RefreshDoing;
                        StateDescription.Text = DoRefreshText;
                        GoToState("RefreshDoing", useTransitions);
                    }
                    break;
                case RefreshState.RefreshCompleted:
                    {
                        if (CurrentState == RefreshState.RefreshDoing)
                        {
                            CurrentState = RefreshState.RefreshCompleted;
                            StateDescription.Text = RefreshedText;
                            GoToState("RefreshCompleted", useTransitions);
                        }
                    }
                    break;

            }
        }

        private bool GoToState(string stateName, bool useTransitions)
        {
            return VisualStateManager.GoToState(this, stateName, useTransitions);
        }

        void ElementScrollViewer_DragEnter(object sender, DragEventArgs e)
        {
            //}
            //private void viewer_MouseMove(object sender, MouseEventArgs e)
            //{
            if (VerticalOffset == 0 && e != null)
            {
                //var p = this.TransformToVisual(ElementRelease).Transform(new Point(0, 0));
                //if (p.Y < -VerticalPullToRefreshDistance)
                double Translation_Y = e.GetPosition(ElementScrollViewer).Y;

                if (Translation_Y - ManipulationStartedYOffset < VerticalPullToRefreshDistance)
                {
                    if (!isPulling)
                    {
                        isPulling = true;

                        ChangeVisualState(RefreshState.Pulling, true);

                        if (EnteredPullRefreshThreshold != null)
                        {
                            EnteredPullRefreshThreshold(this, EventArgs.Empty);
                        }
                        //GoToState("Pulling", true);
                    }
                }
                else if (isPulling)
                {
                    isPulling = false;
                    if (LeftPullRefreshThreshold != null)
                    {
                        LeftPullRefreshThreshold(this, EventArgs.Empty);
                    }
                    ChangeVisualState(RefreshState.ReleasePulling, true);
                    //GoToState("ReleasePulling", true);
                }
            }
        }

        void ElementScrollViewer_ManipulationStarted(object sender, ManipulationStartedEventArgs e)
        {
            if (VerticalOffset == 0 && e != null && e.Position != null)
            {
                ManipulationStartedYOffset = e.Position.Y;
                ChangeVisualState(RefreshState.Pulling, true);
            }
        }

        private void viewer_ManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
        {
            if (VerticalOffset == 0 && CurrentState <= RefreshState.ReleasePulling)
            {
                //var p = this.TransformToVisual(ElementRelease).Transform(new Point(0, 0));
                //if (p.Y < -VerticalPullToRefreshDistance)
                double Translation_Y = e.Cumulative.Translation.Y;
                if (Translation_Y > VerticalPullToRefreshDistance)
                {
                    if (PullRefresh != null)
                    {
                        PullRefresh(this, EventArgs.Empty);
                        if (UpdateDescription.Visibility == Visibility.Collapsed)
                        {
                            UpdateDescription.Visibility = Visibility.Visible;
                        }
                        PullSubtext = DateTime.Now.ToString("MM-dd HH:mm:ss");
                    }
                    isPulling = false;
                    ChangeVisualState(RefreshState.ReleasePulling, true);
                    //ChangeVisualState(RefreshState.RefreshDoing, true);
                    //GoToState("Doing", true);
                }
            }
        }

        private double VerticalOffset
        {
            get
            {
                if (ElementScrollViewer == null) return double.NaN;
                return ElementScrollViewer.VerticalOffset;
            }
        }

        /// <summary>
        /// Distance in pixels to pull down the RefreshBox before a refresh will get initiated.
        /// </summary>
        public double VerticalPullToRefreshDistance
        {
            get { return (double)GetValue(VerticalPullToRefreshDistanceProperty); }
            set { SetValue(VerticalPullToRefreshDistanceProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="VerticalPullToRefreshDistance"/> property
        /// </summary>
        public static readonly DependencyProperty VerticalPullToRefreshDistanceProperty =
            DependencyProperty.Register("VerticalPullToRefreshDistance", typeof(double), typeof(PullToFresh), new PropertyMetadata(150d));

        /// <summary>
        /// Gets or sets the refresh text. Ie "Pull down to refresh".
        /// </summary>
        public string PullDownText
        {
            get { return (string)GetValue(PullDownTextProperty); }
            set { SetValue(PullDownTextProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="PullDownText"/> property
        /// </summary>
        public static readonly DependencyProperty PullDownTextProperty =
            DependencyProperty.Register("PullDownText", typeof(string), typeof(PullToFresh), new PropertyMetadata("下拉刷新"));

        /// <summary>
        /// Gets or sets the release text. Ie "Release to refresh".
        /// </summary>
        public string ReleaseText
        {
            get { return (string)GetValue(ReleaseTextProperty); }
            set { SetValue(ReleaseTextProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="ReleaseText"/> property
        /// </summary>
        public static readonly DependencyProperty ReleaseTextProperty =
            DependencyProperty.Register("ReleaseText", typeof(string), typeof(PullToFresh), new PropertyMetadata("释放立即刷新"));

        /// <summary>
        /// Gets or sets the do refresh text. Ie "refresh is doing".
        /// </summary>
        public string DoRefreshText
        {
            get { return (string)GetValue(DoRefreshTextProperty); }
            set { SetValue(DoRefreshTextProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="DoRefreshText"/> property
        /// </summary>
        public static readonly DependencyProperty DoRefreshTextProperty =
            DependencyProperty.Register("DoRefreshText", typeof(string), typeof(PullToFresh), new PropertyMetadata("刷新中..."));

        /// <summary>
        /// Gets or sets the refreshed text. Ie "Refresh Completed".
        /// </summary>
        public string RefreshedText
        {
            get { return (string)GetValue(DoRefreshTextProperty); }
            set { SetValue(RefreshedTextProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="DoRefreshText"/> property
        /// </summary>
        public static readonly DependencyProperty RefreshedTextProperty =
            DependencyProperty.Register("RefreshedText", typeof(string), typeof(PullToFresh), new PropertyMetadata(""));

        /// <summary>
        /// Sub text below Release/Refresh text. For example: Updated last: 12:34pm
        /// </summary>
        public string PullSubtext
        {
            get { return (string)GetValue(PullSubtextProperty); }
            set { SetValue(PullSubtextProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="PullSubtext"/> property
        /// </summary>
        public static readonly DependencyProperty PullSubtextProperty =
            DependencyProperty.Register("PullSubtext", typeof(string), typeof(PullToFresh), null);

        #region ListHeaderTemplate DependencyProperty

        public static readonly DependencyProperty ListHeaderTemplateProperty =
            DependencyProperty.Register("ListHeaderTemplate", typeof(DataTemplate), typeof(PullToFresh), new PropertyMetadata(default(DataTemplate)));

        public DataTemplate ListHeaderTemplate
        {
            get { return (DataTemplate)GetValue(ListHeaderTemplateProperty); }
            set { SetValue(ListHeaderTemplateProperty, value); }
        }

        #endregion

        /// <summary>
        /// Triggered when the user requested a refresh.
        /// </summary>
        public event EventHandler PullRefresh;
        /// <summary>
        /// 开始下拉
        /// </summary>
        public event EventHandler EnteredPullRefreshThreshold;
        /// <summary>
        /// 如果用户退出“刷新”区域不放屏幕
        /// </summary>
        public event EventHandler LeftPullRefreshThreshold;
    }


}
