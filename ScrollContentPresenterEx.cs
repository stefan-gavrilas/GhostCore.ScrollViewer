using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Input;

namespace GhostCore.ScrollViewer
{
    public class ScrollContentPresenterEx : ContentControl
    {
        #region Events

        public event EventHandler<ScrollViewerViewChangedEventArgs> ViewChanged;
        private void OnViewChanged()
        {
            if (ViewChanged == null)
                return;

            ViewChanged(this, new ScrollViewerViewChangedEventArgs());
        }

        #endregion

        #region Dependency Properties

        public static readonly DependencyProperty ZoomFactorProperty =
            DependencyProperty.Register("ZoomFactor", typeof(float), typeof(ScrollContentPresenterEx), new PropertyMetadata(1F, (d, e) => (d as ScrollContentPresenterEx).OnZoomFactorChanged(e)));
        public static readonly DependencyProperty MinZoomFactorProperty =
            DependencyProperty.Register("MinZoomFactor", typeof(float), typeof(ScrollContentPresenterEx), new PropertyMetadata(0.45F));
        public static readonly DependencyProperty MaxZoomFactorProperty =
            DependencyProperty.Register("MaxZoomFactor", typeof(float), typeof(ScrollContentPresenterEx), new PropertyMetadata(1F));
        public static readonly DependencyProperty HorizontalOffsetProperty =
            DependencyProperty.Register("HorizontalOffset", typeof(int), typeof(ScrollContentPresenterEx), new PropertyMetadata(0, (d, e) => (d as ScrollContentPresenterEx).OnHorizontalOffsetChanged(e)));
        public static readonly DependencyProperty VerticalOffsetProperty =
            DependencyProperty.Register("VerticalOffset", typeof(int), typeof(ScrollContentPresenterEx), new PropertyMetadata(0, (d, e) => (d as ScrollContentPresenterEx).OnVerticalOffsetChanged(e)));
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
        public static readonly DependencyProperty ZoomModeProperty = 
            DependencyProperty.Register("ZoomMode", typeof(ZoomMode), typeof(ScrollViewerEx), new PropertyMetadata(ZoomMode.Disabled));
        
        #endregion

        #region Dependency Property Changed

        private void OnZoomFactorChanged(DependencyPropertyChangedEventArgs e)
        {
            if (ZoomMode == ZoomMode.Disabled)
                return;

            var newVal = (float)e.NewValue;

            _visual.Scale = new Vector3(newVal, newVal, 1);

            ScrollableWidth = (int)(_childSize.Width * newVal);
            ScrollableHeight = (int)(_childSize.Height * newVal);

            OnViewChanged();
        }

        private void OnHorizontalOffsetChanged(DependencyPropertyChangedEventArgs e)
        {
            var scaledWidth = _childSize.Width * ZoomFactor;

            var x = (int)e.NewValue;

            var vox = x * (ActualWidth - scaledWidth) / scaledWidth;

            _visual.Offset = new Vector3((float)vox, _visual.Offset.Y, 0);

            OnViewChanged();
        }

        private void OnVerticalOffsetChanged(DependencyPropertyChangedEventArgs e)
        {
            var scaledHeight = _childSize.Height * ZoomFactor;

            var y = (int)e.NewValue;

            var voy = y * (ActualHeight - scaledHeight) / scaledHeight;

            _visual.Offset = new Vector3(_visual.Offset.X, (float)voy, 0);

            OnViewChanged();
        }

        #endregion

        #region Fields

        private Size _childSize;
        private Visual _visual;
        private Point _initScalePoint;

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
        public ZoomMode ZoomMode
        {
            get { return (ZoomMode)GetValue(ZoomModeProperty); }
            set { SetValue(ZoomModeProperty, value); }
        }

        #endregion

        #region Constructors and Initialization

        public ScrollContentPresenterEx()
        {
            ManipulationMode = ManipulationModes.All;
            ManipulationStarted += ScrollContentPresenterEx_ManipulationStarted;
            ManipulationDelta += ScrollContentPresenterEx_ManipulationDelta;
            PointerWheelChanged += ScrollContentPresenterEx_PointerWheelChanged;
            Unloaded += ScrollContentPresenterEx_Unloaded;
        }

        #endregion

        #region Overrides

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _visual = ElementCompositionPreview.GetElementVisual(Content as UIElement);
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            if (!(Content is UIElement child))
                return base.MeasureOverride(availableSize);

            child.Measure(new Size(OverrideMaximumMeasureWidth, OverrideMaximumMeasureHeight));

            if (child.DesiredSize.Width == 0 && child.DesiredSize.Height == 0 && child is FrameworkElement fe)
            {
                _childSize = new Size(fe.ActualWidth, fe.ActualHeight);
            }
            else
            {
                _childSize = child.DesiredSize;
            }

            ScrollableWidth = (int)_childSize.Width;
            ScrollableHeight = (int)_childSize.Height;

            HorizontalOffset = 0;
            VerticalOffset = 0;

            return availableSize;
        }

        #endregion

        #region API

        public void ChangeView(double? horizontalOffset, double? verticalOffset, float? zoomFactor)
        {
            if (horizontalOffset != null)
                HorizontalOffset = (int)horizontalOffset.Value;

            if (verticalOffset != null)
                VerticalOffset = (int)verticalOffset.Value;

            if (zoomFactor != null)
                ZoomFactor = zoomFactor.Value;
        }

        #endregion

        #region Event handlers

        private void ScrollContentPresenterEx_ManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
            _initScalePoint = e.Position;
        }

        private void ScrollContentPresenterEx_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            float newScale = _visual.Scale.X * e.Delta.Scale;
            newScale = ClampScale(newScale);

            float newX = _visual.Offset.X + (float)e.Delta.Translation.X;
            float newY = _visual.Offset.Y + (float)e.Delta.Translation.Y;

            var newPosition = ClampPosition(newX, newY);

            _visual.Offset = newPosition;

            var scaledWidth = _childSize.Width * ZoomFactor;
            var scaledHeight = _childSize.Height * ZoomFactor;

            var x = -_visual.Offset.X - (-_visual.Offset.X / (ActualWidth - scaledWidth)) * ActualWidth;
            var y = -_visual.Offset.Y - (-_visual.Offset.Y / (ActualHeight - scaledHeight)) * ActualHeight;

            HorizontalOffset = (int)x;
            VerticalOffset = (int)y;

            var scaleChange = newScale - _visual.Scale.X;

            var xTrans = -(_initScalePoint.X * scaleChange);
            var yTrans = -(_initScalePoint.Y * scaleChange);

            ZoomFactor = newScale;

            _visual.Offset += new Vector3((float)xTrans, (float)yTrans, 0);
        }

        private void ScrollContentPresenterEx_PointerWheelChanged(object sender, PointerRoutedEventArgs e)
        {
            var pt = e.GetCurrentPoint(this);
            _initScalePoint = pt.Position;

            var delta = pt.Properties.MouseWheelDelta;

            switch (ScrollWheelBehaviour)
            {
                case ScrollWheelBehaviour.VerticalPan:

                    if (VerticalOffset <= 0 && delta >= 0)
                        return;

                    if (VerticalOffset >= ScrollableHeight && delta <= 0)
                        return;

                    VerticalOffset -= delta;
                    break;
                case ScrollWheelBehaviour.HorizontalPan:
                    if (HorizontalOffset <= 0 && delta >= 0)
                        return;

                    if (HorizontalOffset >= ScrollableWidth && delta <= 0)
                        return;

                    HorizontalOffset -= delta;
                    break;
                case ScrollWheelBehaviour.Zoom:
                    var scale = 0F;

                    if (delta <= 0)
                        scale = 0.925F;
                    else
                        scale = 1 / 0.925F;

                    var newScale = _visual.Scale.X * scale;
                    newScale = ClampScale(newScale);

                    var scaleChange = newScale - _visual.Scale.X;

                    var xTrans = -(_initScalePoint.X * scaleChange);
                    var yTrans = -(_initScalePoint.Y * scaleChange);

                    ZoomFactor = newScale;

                    var offset = new Vector3((float)xTrans, (float)yTrans, 0);

                    float newX = _visual.Offset.X + offset.X;
                    float newY = _visual.Offset.Y + offset.Y;

                    _visual.Offset = ClampPosition(newX, newY);
                    break;
            }
        }

        private void ScrollContentPresenterEx_Unloaded(object sender, RoutedEventArgs e)
        {
            Unloaded -= ScrollContentPresenterEx_Unloaded;
            ManipulationStarted -= ScrollContentPresenterEx_ManipulationStarted;
            ManipulationDelta -= ScrollContentPresenterEx_ManipulationDelta;
            PointerWheelChanged -= ScrollContentPresenterEx_PointerWheelChanged;
        }

        #endregion

        #region Math

        private Vector3 ClampPosition(float newX, float newY)
        {
            // default position is (0,0)

            // clamp top-left
            if (newX > 0)
                newX = 0;

            if (newY > 0)
                newY = 0;

            var scaledWidth = _childSize.Width * ZoomFactor;
            var scaledHeight = _childSize.Height * ZoomFactor;

            // clamp bottom-right
            if (newX < ActualWidth - scaledWidth)
                newX = (float)ActualWidth - (float)scaledWidth;

            if (newY < ActualHeight - scaledHeight)
                newY = (float)ActualHeight - (float)scaledHeight;

            return new Vector3(newX, newY, 0);
        }

        private float ClampScale(float newScale)
        {
            if (newScale > MaxZoomFactor)
                newScale = MaxZoomFactor;

            if (newScale < MinZoomFactor)
                newScale = MinZoomFactor;

            return newScale;
        }

        #endregion
    }
}
