using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Threading;

namespace clock
{
    class CountDownTimer : Timer
    {
        private static List<MainWindow> windows = new List<MainWindow>();
        private static CountDownTimer instance;

        private Thread timerThread;
        private static int limitSec;
        private static Stopwatch stopwatch;

        private enum ViewMode { S, MMSS };
        private static ViewMode viewMode = ViewMode.MMSS;

        private CountDownTimer() { }

        public static CountDownTimer GetInstance(List<MainWindow> windows, string time)
        {
            if (instance == null)
            {
                instance = new CountDownTimer();
            }
            CountDownTimer.windows = windows;
            limitSec = ParseTime(time);
//          w.txtMessage.Text = limit.ToString();
            stopwatch = new Stopwatch();
            return instance;
        }

        private static int ParseTime(string time)
        {
            float number = float.Parse(time.Substring(0, time.Length - 1));
            string unit = time.Substring(time.Length - 1);

            switch (unit)
            {
                case "秒":
                    return (int)number;
                case "分":
                    return (int)(number * 60);
                default:
                    return 0;
            }
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

        private void Update()
        {
            while (true)
            {
                TimeSpan elapsed = stopwatch.Elapsed;
                int remainSec = limitSec - (int)elapsed.TotalSeconds;
                int mm = remainSec / 60;
                int ss = remainSec % 60;

                foreach (MainWindow w in windows)
                {
                    w.Dispatcher.Invoke((Action)(() =>
                    {
                        if (viewMode == ViewMode.S)
                        {
                            if (remainSec < 0)
                            {
                                w.txtTime.Content = String.Format("{0:0}", 0);
                                UiUtil.SetDisableColor(w.txtTime);
                            }
                            else
                            {
                                w.txtTime.Content = String.Format("{0:0}", remainSec);
                            }
                        }
                        else
                        {
                            if (remainSec < 0)
                            {
                                w.txtTime.Content = String.Format("{0:0}:{1:00}", 0, 0);
                                UiUtil.SetDisableColor(w.txtTime);
                            }
                            else
                            {
                                w.txtTime.Content = String.Format("{0:0}:{1:00}", mm, ss);
                            }
                        }

                    }
                    ));
                }

                if (remainSec < 0)
                {
                    timerThread.Abort();
                    return;
                }
                Thread.Sleep(200);

            }

        }

        protected bool isThrugh()
        {
            return false;
        }


        public void Clear() { }

        public void Increment(string time)
        {
            int sec = 0;
            MatchCollection mc = Regex.Matches(time, @"(?<minuts>^\d+)(?<unit>分|秒)(?<inc>延長|短縮)");
            if (mc.Count == 1)
            {
                sec = int.Parse(mc[0].Groups["minuts"].Value);
                if (mc[0].Groups["unit"].Value == "分")
                {
                    sec *= 60;
                }

                if (mc[0].Groups["inc"].Value == "短縮")
                {
                    sec *= -1;
                }
                Console.WriteLine(sec);
            }

            limitSec += sec;
        }

        public void ChangeFormat()
        {
            if (viewMode == ViewMode.MMSS)
            {
                viewMode = ViewMode.S;
            }
            else
            {
                viewMode = ViewMode.MMSS;
            }
        }

    }
}
