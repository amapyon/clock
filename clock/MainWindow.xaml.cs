using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace clock
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        private static Timer timer;
        private static List<MainWindow> windows = new List<MainWindow>();

        public MainWindow()
        {
            InitializeComponent();
            windows.Add(this);
            this.Title = String.Format("[{0}]", windows.Count);
        }


        private void btnClock_Click(object sender, RoutedEventArgs e)
        {
            timer.Pause();
            timer = Clock.GetInstance(windows);
            timer.Start();
        }

        private void btnStopWatch_Click(object sender, RoutedEventArgs e)
        {
            timer.Pause();
            timer = StopWatch.GetInstance(windows);
            timer.Start();
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            timer.Pause();
            timer.Start();
        }

        private void btnPause_Click(object sender, RoutedEventArgs e)
        {
            timer.Pause();
        }

        private void btnCountDown_Click(object sender, RoutedEventArgs e)
        {
            timer.Pause();
            Button b = (Button)sender;
            timer = CountDownTimer.GetInstance(windows, (string)(b.Content));
            timer.Start();
        }

        private void sldFontSize_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            try
            {
//                this.txtDate.Content = e.NewValue.ToString();
                this.txtTime.FontSize = (int)e.NewValue;
            }
            catch (NullReferenceException exception)
            {
                Console.WriteLine(exception.ToString());
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Event: Window_Loaded");
            this.sldFontSize.Value = this.txtTime.FontSize;
            if (windows.Count < 2) {
                timer = Clock.GetInstance(windows);
                timer.Start();
            }
        }

        private void btnCountDown_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            Button btn = (Button)sender;
            InputBox ib = new InputBox();
            ib.txtValue.Text = (string)btn.Content;
            ib.Top = e.GetPosition(this).X;
            ib.Left = e.GetPosition(this).Y;
            ib.ShowDialog();
            btn.Content = ib.txtValue.Text;
        }

        private void txtMessage_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            try
            {
                txtMessage.FontSize = e.NewSize.Height * 0.75;
            }
            catch (NullReferenceException exception)
            {
                Console.WriteLine(exception.ToString());
            }
        }

        private void mitmDuplicate_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Event: mitmDuplicate_Click");
            MainWindow w = new MainWindow();
            w.Show();
        }

        private void mitmFormat_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Event: mitmFormat_Click");
            timer.ChangeFormat();
        }

        private void mitmIncrement_Click(object sender, RoutedEventArgs e)
        {
            MenuItem item = (MenuItem)sender;
            timer.Increment(item.Header.ToString());
        }

        private void txtMessage_TextChanged(object sender, TextChangedEventArgs e)
        {
            foreach (MainWindow w in windows)
            {
                w.txtMessage.Text = this.txtMessage.Text;
            }
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            Console.WriteLine("StateChanged:" + sender.GetType());
            if (((MainWindow)sender).WindowState == WindowState.Minimized)
            {
                foreach (MainWindow w in windows)
                {
                    if (w.WindowState != WindowState.Minimized)
                    {
                        w.WindowState = WindowState.Minimized;
                    }
                }
            }
        }

    }
}
