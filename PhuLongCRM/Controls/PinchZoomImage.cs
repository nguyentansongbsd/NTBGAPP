﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PhuLongCRM.Controls
{
    public class PinchZoomImage : ContentView
    {
        private double _currentScale = 1;
        private double _startScale = 1;
        private double _xOffset = 0;
        private double _yOffset = 0;
        private bool _secondDoubleTapp = false;
        public event EventHandler OnZoom;
        public event EventHandler OutZoom;

        public PinchZoomImage()
        {
            var pinchGesture = new PinchGestureRecognizer();
            pinchGesture.PinchUpdated += PinchUpdated;
            GestureRecognizers.Add(pinchGesture);

            var panGesture = new PanGestureRecognizer();
            panGesture.PanUpdated += OnPanUpdated;
            GestureRecognizers.Add(panGesture);

            var tapGesture = new TapGestureRecognizer { NumberOfTapsRequired = 2 };
            tapGesture.Tapped += DoubleTapped;
            GestureRecognizers.Add(tapGesture);
        }

        public void PinchUpdated(object sender, PinchGestureUpdatedEventArgs e)
        {
            switch (e.Status)
            {
                case GestureStatus.Started:
                    _startScale = Content.Scale;
                    Content.AnchorX = 0;
                    Content.AnchorY = 0;
                    SendZoomUpdated();
                    break;
                case GestureStatus.Running:
                    {
                        _currentScale += (e.Scale - 1) * _startScale;
                        _currentScale = Math.Max(1, _currentScale);

                        var renderedX = Content.X + _xOffset;
                        var deltaX = renderedX / Width;
                        var deltaWidth = Width / (Content.Width * _startScale);
                        var originX = (e.ScaleOrigin.X - deltaX) * deltaWidth;

                        var renderedY = Content.Y + _yOffset;
                        var deltaY = renderedY / Height;
                        var deltaHeight = Height / (Content.Height * _startScale);
                        var originY = (e.ScaleOrigin.Y - deltaY) * deltaHeight;

                        var targetX = _xOffset - (originX * Content.Width) * (_currentScale - _startScale);
                        var targetY = _yOffset - (originY * Content.Height) * (_currentScale - _startScale);

                        Content.TranslationX = Math.Min(0, Math.Max(targetX, -Content.Width * (_currentScale - 1)));
                        Content.TranslationY = Math.Min(0, Math.Max(targetY, -Content.Height * (_currentScale - 1)));

                        Content.Scale = _currentScale;
                        break;
                    }
                case GestureStatus.Completed:
                    _xOffset = Content.TranslationX;
                    _yOffset = Content.TranslationY;
                    if(_currentScale ==1)
                    {
                        SendZoomEnd();
                    }    
                    break;
            }
        }

        public void OnPanUpdated(object sender, PanUpdatedEventArgs e)
        {
            if (Content.Scale == 1)
            {
                SendZoomEnd();
                return;
            }

            switch (e.StatusType)
            {
                case GestureStatus.Running:

                    var newX = (e.TotalX * Scale) + _xOffset;
                    var newY = (e.TotalY * Scale) + _yOffset;

                    var width = (Content.Width * Content.Scale);
                    var height = (Content.Height * Content.Scale);

                    var canMoveX = width > Application.Current.MainPage.Width;
                    var canMoveY = height > Application.Current.MainPage.Height;

                    if (canMoveX)
                    {
                        var minX = (width - (Application.Current.MainPage.Width / 2)) * -1;
                        var maxX = Math.Min(Application.Current.MainPage.Width / 2, width / 2);

                        if (newX < minX)
                        {
                            newX = minX;
                        }

                        if (newX > maxX)
                        {
                            newX = maxX;
                        }
                    }
                    else
                    {
                        newX = 0;
                    }

                    if (canMoveY)
                    {
                        var minY = (height - (Application.Current.MainPage.Height / 2)) * -1;
                        var maxY = Math.Min(Application.Current.MainPage.Width / 2, height / 2);

                        if (newY < minY)
                        {
                            newY = minY;
                        }

                        if (newY > maxY)
                        {
                            newY = maxY;
                        }
                    }
                    else
                    {
                        newY = 0;
                    }

                    Content.TranslationX = newX;
                    Content.TranslationY = newY;
                    break;
                case GestureStatus.Completed:
                    _xOffset = Content.TranslationX;
                    _yOffset = Content.TranslationY;
                    SendZoomUpdated();
                    break;
                case GestureStatus.Started:
                    SendZoomUpdated();
                    break;
                case GestureStatus.Canceled:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public async void DoubleTapped(object sender, EventArgs e)
        {
            var multiplicator = Math.Pow(2, 1.0 / 10.0);
            _startScale = Content.Scale;
            Content.AnchorX = 0;
            Content.AnchorY = 0;

            for (var i = 0; i < 10; i++)
            {
                if (!_secondDoubleTapp) //if it's not the second double tapp we enlarge the scale
                {
                    _currentScale *= multiplicator;
                }
                else //if it's the second double tap we make the scale smaller again 
                {
                    _currentScale /= multiplicator;
                }

                var renderedX = Content.X + _xOffset;
                var deltaX = renderedX / Width;
                var deltaWidth = Width / (Content.Width * _startScale);
                var originX = (0.5 - deltaX) * deltaWidth;

                var renderedY = Content.Y + _yOffset;
                var deltaY = renderedY / Height;
                var deltaHeight = Height / (Content.Height * _startScale);
                var originY = (0.5 - deltaY) * deltaHeight;

                var targetX = _xOffset - (originX * Content.Width) * (_currentScale - _startScale);
                var targetY = _yOffset - (originY * Content.Height) * (_currentScale - _startScale);

                Content.TranslationX = Math.Min(0, Math.Max(targetX, -Content.Width * (_currentScale - 1)));
                Content.TranslationY = Math.Min(0, Math.Max(targetY, -Content.Height * (_currentScale - 1)));

                Content.Scale = _currentScale;
                await Task.Delay(10);
            }
            _secondDoubleTapp = !_secondDoubleTapp;
            _xOffset = Content.TranslationX;
            _yOffset = Content.TranslationY;
            SendZoomUpdated();
        }
        public void SendZoomUpdated()
        {
            EventHandler eventHandler = this.OnZoom;
            eventHandler?.Invoke((object)this, EventArgs.Empty);
        }

        public void SendZoomEnd()
        {
            EventHandler eventHandler = this.OutZoom;
            eventHandler?.Invoke((object)this, EventArgs.Empty);
        }
    }
}
