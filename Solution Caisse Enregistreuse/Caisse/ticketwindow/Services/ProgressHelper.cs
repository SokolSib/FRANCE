using System;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using TicketWindow.Winows;

namespace TicketWindow.Services
{
    /// <summary>
    ///     Для управления прогресс баром.
    /// </summary>
    public class ProgressHelper
    {
        private static readonly ProgressHelper instance = new ProgressHelper();
        private static ProgressWindow _window;
        private int _count;
        private Dispatcher _dispatcher;
        private string _name;

        /// Защищенный конструктор нужен, чтобы предотвратить создание экземпляра класса Singleton
        protected ProgressHelper()
        {
        }

        public static ProgressHelper Instance
        {
            get { return instance; }
        }

        /// <summary>
        ///     Показывать вечный цикл или нет.
        /// </summary>
        public bool IsIndeterminate { get; set; }

        /// <summary>
        ///     Запуск окна прогресса.
        /// </summary>
        /// <param name="count">Максимальное значения прогресса.</param>
        /// <param name="text">Комментарий прогресса.</param>
        public void Start(int count, string text = null)
        {
            Run(count, text);
        }

        /// <summary>
        ///     Остановка окна прогресса.
        /// </summary>
        public void Stop()
        {
            try
            {
                _dispatcher.Invoke(new Action(() => { _window.Close(); }));
            }
            catch
            {
                // ignored
            }
        }

        /// <summary>
        ///     Установка значения прогресса.
        /// </summary>
        /// <param name="value">Значение прогресса.</param>
        /// <param name="text">Комментарий прогресса.</param>
        public void SetValue(int value, string text = null)
        {
            try
            {
                _dispatcher.Invoke(new Action(() =>
                                              {
                                                  _window.Current = value;
                                                  _window.Description = text;
                                              }));
            }
            catch
            {
                // ignored
            }
        }

        private void Run(int count, string name = null)
        {
            _count = count;
            _name = name;

            if (_dispatcher == null)
            {
                var thread = new Thread(ThreadStartFunc);
                thread.SetApartmentState(ApartmentState.STA);
                thread.IsBackground = true;
                thread.Start();
            }
            else _dispatcher.Invoke(new Action(() => { StartProgress(_count, _name); }));
        }

        private void ThreadStartFunc()
        {
            StartProgress(_count, _name);
            _dispatcher = Dispatcher.CurrentDispatcher;
            Dispatcher.Run();
        }

        private void StartProgress(int count, string name = null)
        {
            _window = new ProgressWindow(count, name);

            if (IsIndeterminate)
            {
                _window.BoxText.Visibility = Visibility.Collapsed;
                _window.BoxProgress.IsIndeterminate = IsIndeterminate;
            }

            _window.Show();
        }
    }
}