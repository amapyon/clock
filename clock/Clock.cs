using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace clock
{
    public delegate void Update();

    class Clock : Timer
    {
        private static List<MainWindow> windows;
        private Thread timerThread;

        private static Clock instance;

        private Clock()
        {
        }

        public static Clock GetInstance(List<MainWindow> windows)
        {
            if (instance == null)
            {
                instance = new Clock();
            }
            Clock.windows = windows;

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
            timerThread.Start();
        }

        public void Pause()
        {
            Console.WriteLine("Method: Clock Pause()");
            foreach (MainWindow w in windows)
            {
                UiUtil.SetDisableColor(w.txtTime);
            }
            timerThread.Abort();
        }

        private void Update()
        {
            while (true)
            {
                foreach (MainWindow w in windows)
                {
                    w.Dispatcher.Invoke((Action)(() =>
                    {
                        w.txtDate.Content = DateTime.Now.ToString("yyyy年M月d日(ddd)");
                        w.txtTime.Content = DateTime.Now.ToString("H:mm:ss");
                    }
                    ));
                }
                Thread.Sleep(200);
            }
        }

        public void Clear() { }
        public void Increment(string time) {}
        public void ChangeFormat() {}

    }
}
