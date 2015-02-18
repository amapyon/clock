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
    class CountDownTimer : Timer
    {
        private static List<MainWindow> windows = new List<MainWindow>();
        private static CountDownTimer instance;

        private Thread timerThread;
        private static int limitSec;
        private static Stopwatch stopwatch;

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
            int number = int.Parse(time.Substring(0, time.Length - 1));
            string unit = time.Substring(time.Length - 1);

            switch (unit)
            {
                case "秒":
                    return number;
                case "分":
                    return number * 60;
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
/*
            Console.WriteLine(mc.Count);
            foreach (Match m in mc)
            {
                Console.WriteLine(m.Value);
                Console.WriteLine(m.Groups["minuts"].Value);
                Console.WriteLine(m.Groups["unit"].Value);
                Console.WriteLine(m.Groups["inc"].Value);
            }
*/
            limitSec += sec;
        }

    }
}
