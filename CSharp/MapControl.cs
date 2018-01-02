using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace CSharp
{
    public class MapControl : Canvas
    {
        private int _width = 1;
        private int _height = 1;
        private int _tileWidth;
        private int _tileHeight;
        private Brush _liveCellBrush;
        private Brush _deadCellBrush;

        public MapControl()
        {
            _liveCellBrush = new SolidColorBrush(Colors.Black);
            _deadCellBrush = new SolidColorBrush(Colors.White);
            SizeChanged += OnSizeChanged;
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            Init(_width, _height);
        }

        public void Init(int width, int height)
        {
            _width = width;
            _height = height;
            _tileWidth = (int)ActualWidth / width;
            _tileHeight = (int)ActualHeight / height;

            Children.Clear();

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    var rect = new Rectangle { Width = _tileWidth, Height = _tileHeight };
                    SetLeft(rect, x * _tileWidth);
                    SetTop(rect, y * _tileHeight);
                    Children.Add(rect);
                }
            }
        }

        public void Draw(bool[,] map)
        {
            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    var rect = Children[x * _height + y] as Rectangle;
                    rect.Fill = map[x, y] ? _liveCellBrush : _deadCellBrush;
                }
            }
        }
    }
}
