using Xamarin.Forms;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Threading.Tasks;

namespace CSharp
{
    public partial class CSharpPage : ContentPage
    {
        private const int RefreshTime = 100;

        private Game _game;
        private bool _timerRunning;
        private SKPaint _aliveCellPaint;
        private SKCanvasView _canvasView;

        public CSharpPage()
        {
            InitializeComponent();

            _game = new Game();
            _aliveCellPaint = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                Color = Color.Black.ToSKColor()
            };

            _canvasView = new SKCanvasView();
            _canvasView.PaintSurface += OnPaintSurface;
            CanvasRoot.Content = _canvasView;
        }

        private void OnPaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            var canvas = e.Surface.Canvas;
            var tileWidth = e.Info.Width / Game.MapWidth;
            var tileHeight = e.Info.Height / Game.MapHeight;

            canvas.Clear();

            for (int x = 0; x < Game.MapWidth; x++)
            {
                for (int y = 0; y < Game.MapHeight; y++)
                {
                    if (_game.Map[x, y])
                    {
                        canvas.DrawRect(new SKRect(x * tileWidth, y * tileHeight, (x+1) * tileWidth, (y+1) * tileHeight), _aliveCellPaint);
                    }
                }
            }
        }

        private void OnStartButtonClicked(object sender, EventArgs e)
        {
            if (!_timerRunning)
            {
                _timerRunning = true;
                StartButton.Text = "Pause";

                Device.StartTimer(TimeSpan.FromMilliseconds(RefreshTime), () => {
                    _game.PlayStep();
                    _canvasView.InvalidateSurface();
                    return _timerRunning;
                });
            }
            else
            {
                _timerRunning = false;
                StartButton.Text = "Continue";
            }
        }

        private async void OnRestartButtonClicked(object sender, System.EventArgs e)
        {
            _timerRunning = false;
            _game = new Game();
            _canvasView.InvalidateSurface();
            StartButton.Text = "Pause";

            await Task.Delay(RefreshTime);

            _timerRunning = true;

            Device.StartTimer(TimeSpan.FromMilliseconds(RefreshTime), () => {
                _game.PlayStep();
                _canvasView.InvalidateSurface();
                return _timerRunning;
            });
        }
    }
}