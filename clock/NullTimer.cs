namespace clock
{
    class NullTimer : Timer
    {
        public void Start() { }
        public void Pause() { }
        public void Clear() { }
        public void Increment(string time) { }
        public void ChangeFormat() { }
        public void AddWindow(MainWindow mainWindow) { }

    }
}
