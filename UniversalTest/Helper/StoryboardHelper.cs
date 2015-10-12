using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Animation;

namespace UniversalTest.Helper
{
    /// <summary>
    /// 动画辅助
    /// </summary>
    public class StoryboardHelper
    {
        /// <summary>
        /// 创建动画
        /// </summary> 
        public static DoubleAnimation CreatAnimation(DependencyObject obj, string path, int duration, EasingFunctionBase easing = null)
        {
            DoubleAnimation da = new DoubleAnimation();
            da.EasingFunction = easing;
            da.Duration = TimeSpan.FromMilliseconds(duration);
            Storyboard.SetTarget(da, obj);
            Storyboard.SetTargetProperty(da, path);
            return da;
        }

        /// <summary>
        /// 创建动画
        /// </summary> 
        public static DoubleAnimation CreatAnimation(DependencyObject obj, Storyboard storyboard, string path, int duration, EasingFunctionBase easing = null)
        {
            DoubleAnimation da = new DoubleAnimation();
            da.EasingFunction = easing;
            da.Duration = TimeSpan.FromMilliseconds(duration);
            Storyboard.SetTarget(da, obj);
            Storyboard.SetTargetProperty(da, path);
            storyboard.Children.Add(da);
            return da;
        }

        /// <summary>
        /// 创建动画
        /// </summary> 
        public static DoubleAnimation CreatAnimation(DependencyObject obj, Storyboard storyboard, string path, int duration, double? toValue, EasingFunctionBase easing, bool isEnableDependency)
        {
            DoubleAnimation da = new DoubleAnimation();
            da.EasingFunction = easing;
            da.Duration = TimeSpan.FromMilliseconds(duration);
            da.To = toValue;
            da.EnableDependentAnimation = isEnableDependency;
            Storyboard.SetTarget(da, obj);
            Storyboard.SetTargetProperty(da, path);
            storyboard.Children.Add(da);
            return da;
        }

        /// <summary>
        /// 创建动画
        /// </summary> 
        public static DoubleAnimation CreatAnimation(DependencyObject obj, Storyboard storyboard, string path, int duration,int beginTime, double? toValue, EasingFunctionBase easing, bool isEnableDependency)
        {
            DoubleAnimation da = new DoubleAnimation();
            da.EasingFunction = easing;
            da.BeginTime = TimeSpan.FromMilliseconds(beginTime);
            da.Duration = TimeSpan.FromMilliseconds(duration);
            da.To = toValue;
            da.EnableDependentAnimation = isEnableDependency;
            Storyboard.SetTarget(da, obj);
            Storyboard.SetTargetProperty(da, path);
            storyboard.Children.Add(da);
            return da;
        }
    }
}
