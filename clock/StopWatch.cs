using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace clock
{
    class StopWatch : Timer
    {
        private static List<MainWindow> windows = new List<MainWindow>();
        private static StopWatch instance;

        private Thread timerThread;
        private static Stopwatch stopwatch;

        public StopWatch()
        {
        }

        public static StopWatch GetInstance(List<MainWindow> windows)
        {
            if (instance == null)
            {
                instance = new StopWatch();
            }
            StopWatch.windows = windows;
            stopwatch = new Stopwatch();
            return instance;
        }

        public void Start()
        {
            foreach (MainWindow w in windows)
            {
                UiUtil.SetEnableColor(w.txtTime);
//                w.Closing += (_, __) => Pause();
            }
            timerThread = new Thread(() =>
            {
                Update();

            });
            stopwatch.Start();
            timerThread.Start();
        }

        public void Pause()
        {
            foreach (MainWindow w in windows)
            {
                UiUtil.SetDisableColor(w.txtTime);
            }
            stopwatch.Stop();
            timerThread.Abort();
        }

        public void Clear()
        {
        }

        private void Update()
        {
            while (true)
            {
                TimeSpan elapsed = stopwatch.Elapsed;
                int mm = elapsed.Minutes;
                int ss = elapsed.Seconds;

                foreach (MainWindow w in windows)
                {
                    w.Dispatcher.Invoke((Action)(() =>
                    {

                        w.txtTime.Content = String.Format("{0:0}:{1:00}", mm, ss);
                    }
                    ));
                }

                Thread.Sleep(200);
            }
        }


        public void Increment(string time) { }

    }
}
