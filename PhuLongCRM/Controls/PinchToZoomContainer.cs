using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace PhuLongCRM.Controls
{
    public class PinchToZoomContainer : ContentView
    {
        double currentScale = 1;
        double startScale = 1;
        private const double MIN_SCALE = 1;
        private const double MAX_SCALE = 4;
        private const double OVERSHOOT = 0.05;
        double xOffset = 0;
        double yOffset = 0;

        double tmpX = 0;
        double tmpY = 0;

        private PinchGestureRecognizer pinch;
        private PanGestureRecognizer pan;
        private TapGestureRecognizer tap;

        public bool isZooming { get; set; }

        public PinchToZoomContainer ()
        {
            pinch = new PinchGestureRecognizer();
            pinch.PinchUpdated += OnPinchUpdated;
            GestureRecognizers.Add(pinch);

            pan = new PanGestureRecognizer();
            pan.PanUpdated += OnPanUpdated;
            GestureRecognizers.Add(pan);

            tap = new TapGestureRecognizer { NumberOfTapsRequired = 2 };
            tap.Tapped += OnTapped;
            GestureRecognizers.Add(tap);

            //Content.Scale = MIN_SCALE;
            //TranslationX = TranslationY = 0;
            //Content.AnchorX = Content.AnchorY = 0;
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            this.resetZoom();
        }
        protected override SizeRequest OnMeasure(double widthConstraint, double heightConstraint)
        {
            this.resetZoom();

            return base.OnMeasure(widthConstraint, heightConstraint);
        }
        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            this.resetZoom();
        }

        private void OnTapped(object sender, EventArgs e)
        {
            if (Content.Scale > MIN_SCALE)
            {
                this.Content.ScaleTo(MIN_SCALE, 250, Easing.CubicInOut);
                this.Content.TranslateTo(0, 0, 250, Easing.CubicInOut);
                currentScale = MIN_SCALE;
                xOffset = 0;
                yOffset = 0;
                isZooming = false;
            }
            else
            {
                Content.AnchorX = Content.AnchorY = 0.5; //TODO tapped position
                currentScale = MAX_SCALE;
                xOffset = 0;
                yOffset = 0;
                this.Content.ScaleTo(MAX_SCALE, 250, Easing.CubicInOut);
                this.Content.TranslateTo(xOffset, yOffset, 250, Easing.CubicInOut);
                isZooming = true;
            }
            this.SendZoomUpdated();
        }

        private void OnPanUpdated(object sender, PanUpdatedEventArgs e)
        {
            if (Content.Scale > MIN_SCALE)
            {
                isZooming = true;
                switch (e.StatusType)
                {
                    case GestureStatus.Started:
                        tmpX = Content.AnchorX;
                        tmpY = Content.AnchorY;
                        break;
                    case GestureStatus.Running:
                        Content.TranslateTo((xOffset + e.TotalX).Clamp(-(Content.Width * (currentScale - 1))/2, (Content.Width * (currentScale - 1)) / 2), (yOffset + e.TotalY).Clamp(-(Content.Height * (currentScale - 1))/2, (Content.Height * (currentScale - 1)) / 2), 50, Easing.Linear);
                        break;
                    case GestureStatus.Completed:
                        xOffset = Content.TranslationX;
                        yOffset = Content.TranslationY;
                        break;
                }
            }
            else
            {
                isZooming = false;
            }

            this.SendZoomUpdated();
        }

        void OnPinchUpdated (object sender, PinchGestureUpdatedEventArgs e)
        {
            if (e.Status == GestureStatus.Started) {
                // Store the current scale factor applied to the wrapped user interface element,
                // and zero the components for the center point of the translate transform.
                startScale = Content.Scale;
                Content.AnchorX = 0.5;
                Content.AnchorY = 0.5;

                //Content.TranslateTo(xOffset, yOffset);
            }
            if (e.Status == GestureStatus.Running) {
                // Calculate the scale factor to be applied.
                currentScale += (e.Scale - 1) * startScale;
                currentScale = currentScale.Clamp(MIN_SCALE,MAX_SCALE);

                // The ScaleOrigin is in relative coordinates to the wrapped user interface element,
                // so get the X pixel coordinate.
                double renderedX = xOffset;
                double deltaX = renderedX / Width;
                double deltaWidth = Width / (Content.Width * startScale);
                double originX = (e.ScaleOrigin.X - deltaX) * deltaWidth;

                // The ScaleOrigin is in relative coordinates to the wrapped user interface element,
                // so get the Y pixel coordinate.
                double renderedY = yOffset;
                double deltaY = renderedY / Height;
                double deltaHeight = Height / (Content.Height * startScale);
                double originY = (e.ScaleOrigin.Y - deltaY) * deltaHeight;

                // Calculate the transformed element pixel coordinates.
                double targetX = xOffset - (originX/2 * Content.Width) * (currentScale - startScale);
                double targetY = yOffset - (originY/2 * Content.Height) * (currentScale - startScale);

                // Apply translation based on the change in origin.
                Content.TranslationX = targetX.Clamp(-(Content.Width * (currentScale - 1)) / 2, (Content.Width * (currentScale - 1)) / 2);
                Content.TranslationY = targetY.Clamp(-(Content.Height * (currentScale - 1)) / 2, (Content.Height * (currentScale - 1)) / 2);
                //Content.TranslateTo(targetX.Clamp(-Content.Width * (currentScale - 1), 0), targetY.Clamp(-Content.Height * (currentScale - 1), 0), 250, Easing.CubicInOut);

                // Apply scale factor
                Content.Scale = currentScale;
            }
            if (e.Status == GestureStatus.Completed) {
                // Store the translation delta's of the wrapped user interface element.
                xOffset = Content.TranslationX;
                yOffset = Content.TranslationY;
            }


        }

        private T Clamp<T>(T value, T minimum, T maximum) where T : IComparable
        {
            this.SendZoomUpdated();
            if (value.CompareTo(minimum) < 0)
                return minimum;
            else if (value.CompareTo(maximum) > 0)
                return maximum;
            else
                return value;
        }

        public event EventHandler OnZoomUpdated;
        public void SendZoomUpdated()
        {
            EventHandler eventHandler = this.OnZoomUpdated;
            eventHandler?.Invoke((object)this, EventArgs.Empty);
        }

        public void resetZoom() {
            Content.Scale = MIN_SCALE;
            Content.TranslationX = Content.TranslationY = 0;
            Content.AnchorX = Content.AnchorY = 0.5;
            this.AnchorX = this.AnchorY = 0.5;

            isZooming = false;
            this.SendZoomUpdated();
        }

    }
}
