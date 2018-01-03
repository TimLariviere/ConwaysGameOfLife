using System;
using System.Diagnostics;
using System.Timers;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace CSharp
{
    public partial class MainWindow : Window
    {
        private const int GameTick = 1000;

        private static Brush LiveCellBrush = new SolidColorBrush(Colors.Black);
        private static Brush DeadCellBrush = new SolidColorBrush(Colors.White);

        private Game _game;
        private Timer _timer;

        public MainWindow()
        {
            InitializeComponent();
            _game = new Game();
            _timer = new Timer(GameTick);
            _timer.Elapsed += OnTimerElapsed;

            MapControl.Init(Game.MapWidth, Game.MapHeight);
        }

        private void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            var sw = new Stopwatch();

            sw.Start();
            _game.PlayStep();
            sw.Stop();
            Debug.WriteLine($"PlayStep : {sw.ElapsedMilliseconds}ms");

            sw.Restart();
            MapControl.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
            {
                MapControl.Draw(_game.Map);
            }));
            sw.Stop();
            Debug.WriteLine($"DrawMap : {sw.ElapsedMilliseconds}ms");
        }

        private void OnStartButtonClicked(object sender, RoutedEventArgs e)
        {
            if (!_timer.Enabled)
            {
                MapControl.Draw(_game.Map);
                StartButton.Content = "Pause";
                _timer.Start();
            }
            else
            {
                StartButton.Content = "Start";
                _timer.Stop();
            }
        }

        private void OnRestartButtonClicked(object sender, RoutedEventArgs e)
        {
            _timer.Stop();            
            _game = new Game();
            _timer.Start();
        }
    }
}
