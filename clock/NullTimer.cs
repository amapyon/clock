using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clock
{
    class NullTimer : Timer
    {
        public void Start() { }
        public void Pause() { }
        public void Clear() { }
        public void Increment(string time) { }
        public void AddWindow(MainWindow mainWindow) { }

    }
}
