using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;


namespace GhostCore.ScrollViewer
{
    public class ScrollViewerEx : ContentControl
    {
        #region Dependency Properties

        public static readonly DependencyProperty ZoomFactorProperty =
            DependencyProperty.Register("ZoomFactor", typeof(float), typeof(ScrollContentPresenterEx), new PropertyMetadata(1F));
        public static readonly DependencyProperty MinZoomFactorProperty =
            DependencyProperty.Register("MinZoomFactor", typeof(float), typeof(ScrollContentPresenterEx), new PropertyMetadata(0.45F));
        public static readonly DependencyProperty MaxZoomFactorProperty =
            DependencyProperty.Register("MaxZoomFactor", typeof(float), typeof(ScrollContentPresenterEx), new PropertyMetadata(1F));
        public static readonly DependencyProperty HorizontalOffsetProperty =
            DependencyProperty.Register("HorizontalOffset", typeof(int), typeof(ScrollContentPresenterEx), new PropertyMetadata(0));
        public static readonly DependencyProperty VerticalOffsetProperty =
            DependencyProperty.Register("VerticalOffset", typeof(int), typeof(ScrollContentPresenterEx), new PropertyMetadata(0));
        public static readonly DependencyProperty ScrollableWidthProperty = 
            DependencyProperty.Register("ScrollableWidth", typeof(int), typeof(ScrollContentPresenterEx), new PropertyMetadata(0));
        public static readonly DependencyProperty ScrollableHeightProperty =
            DependencyProperty.Register("ScrollableHeight", typeof(int), typeof(ScrollContentPresenterEx), new PropertyMetadata(0));
        public static readonly DependencyProperty ScrollWheelBehaviourProperty =
            DependencyProperty.Register("ScrollWheelBehaviour", typeof(ScrollWheelBehaviour), typeof(ScrollContentPresenterEx), new PropertyMetadata(ScrollWheelBehaviour.VerticalPan));
        public static readonly DependencyProperty OverrideMaximumMeasureWidthProperty =
            DependencyProperty.Register("OverrideMaximumMeasureWidth", typeof(double), typeof(ScrollViewerEx), new PropertyMetadata(double.PositiveInfinity));
        public static readonly DependencyProperty OverrideMaximumMeasureHeightProperty =
            DependencyProperty.Register("OverrideMaximumMeasureHeight", typeof(double), typeof(ScrollViewerEx), new PropertyMetadata(double.PositiveInfinity));
        public static readonly DependencyProperty HorizontalScrollBarStyleProperty =
            DependencyProperty.Register("HorizontalScrollBarStyle", typeof(Style), typeof(ScrollViewerEx), new PropertyMetadata(null));
        public static readonly DependencyProperty VerticalScrollBarStyleProperty =
            DependencyProperty.Register("VerticalScrollBarStyle", typeof(Style), typeof(ScrollViewerEx), new PropertyMetadata(null));

        #endregion

        #region Properties

        public float ZoomFactor
        {
            get { return (float)GetValue(ZoomFactorProperty); }
            set { SetValue(ZoomFactorProperty, value); }
        }
        public float MinZoomFactor
        {
            get { return (float)GetValue(MinZoomFactorProperty); }
            set { SetValue(MinZoomFactorProperty, value); }
        }
        public float MaxZoomFactor
        {
            get { return (float)GetValue(MaxZoomFactorProperty); }
            set { SetValue(MaxZoomFactorProperty, value); }
        }
        public int HorizontalOffset
        {
            get { return (int)GetValue(HorizontalOffsetProperty); }
            set { SetValue(HorizontalOffsetProperty, value); }
        }
        public int VerticalOffset
        {
            get { return (int)GetValue(VerticalOffsetProperty); }
            set { SetValue(VerticalOffsetProperty, value); }
        }
        public int ScrollableWidth
        {
            get { return (int)GetValue(ScrollableWidthProperty); }
            set { SetValue(ScrollableWidthProperty, value); }
        }
        public int ScrollableHeight
        {
            get { return (int)GetValue(ScrollableHeightProperty); }
            set { SetValue(ScrollableHeightProperty, value); }
        }
        public ScrollWheelBehaviour ScrollWheelBehaviour
        {
            get { return (ScrollWheelBehaviour)GetValue(ScrollWheelBehaviourProperty); }
            set { SetValue(ScrollWheelBehaviourProperty, value); }
        }
        public double OverrideMaximumMeasureWidth
        {
            get { return (double)GetValue(OverrideMaximumMeasureWidthProperty); }
            set { SetValue(OverrideMaximumMeasureWidthProperty, value); }
        }
        public double OverrideMaximumMeasureHeight
        {
            get { return (double)GetValue(OverrideMaximumMeasureHeightProperty); }
            set { SetValue(OverrideMaximumMeasureHeightProperty, value); }
        }
        public Style HorizontalScrollBarStyle
        {
            get { return (Style)GetValue(HorizontalScrollBarStyleProperty); }
            set { SetValue(HorizontalScrollBarStyleProperty, value); }
        }
        public Style VerticalScrollBarStyle
        {
            get { return (Style)GetValue(VerticalScrollBarStyleProperty); }
            set { SetValue(VerticalScrollBarStyleProperty, value); }
        }

        #endregion

        #region Constructors and Initialization

        public ScrollViewerEx()
        {
            DefaultStyleKey = typeof(ScrollViewerEx);
        }

        #endregion
    }

    public enum ScrollWheelBehaviour
    {
        VerticalPan,
        HorizontalPan,
        Zoom
    }
}
