using System;
using System.Configuration;

namespace TicketWindow.Global
{
    /// <summary>
    ///     Конфигурация.
    /// </summary>
    public static class Config
    {
        /// <summary>
        ///     Исполоьзование серверно БД (иначе локальная).
        /// </summary>
        public static bool IsUseServer
        {
            get { return Convert.ToBoolean(ConfigurationManager.AppSettings["IsUseServer"]); }
        }

        /// <summary>
        ///     Ид магазина.
        /// </summary>
        public static Guid IdEstablishment
        {
            get { return new Guid(ConfigurationManager.AppSettings["IdEstablishment"]); }
        }

        /// <summary>
        ///     Ид пользователя.
        /// </summary>
        public static Guid CustomerId
        {
            get { return new Guid(ConfigurationManager.AppSettings["CustumerId"]); }
        }

        /// <summary>
        ///     Ид оптового магазина.
        /// </summary>
        public static Guid IdEstablishmentGros
        {
            get { return new Guid(ConfigurationManager.AppSettings["IdEstablishmentGros"]); }
        }

        /// <summary>
        ///     Название кассы.
        /// </summary>
        public static string NameTicket
        {
            get { return ConfigurationManager.AppSettings["NameTicket"]; }
        }

        /// <summary>
        ///     Пользователь.
        /// </summary>
        public static string User
        {
            get { return ConfigurationManager.AppSettings["User"]; }
        }

        /// <summary>
        ///     Название.
        /// </summary>
        public static string Name
        {
            get { return ConfigurationManager.AppSettings["Name"]; }
            set
            {
                var config = ConfigurationManager.OpenExeConfiguration(AppPath + "TicketWindow.exe");
                config.AppSettings.Settings["Name"].Value = value;
                config.Save(ConfigurationSaveMode.Modified);
            }
        }

        /// <summary>
        ///     Название.
        /// </summary>
        public static string Language
        {
            get { return ConfigurationManager.AppSettings["Language"]; }
            set
            {
                var config = ConfigurationManager.OpenExeConfiguration(AppPath + "TicketWindow.exe");
                config.AppSettings.Settings["Language"].Value = value;
                config.Save(ConfigurationSaveMode.Modified);
            }
        }

        /// <summary>
        ///     Строка подключения.
        /// </summary>
        public static string ConnectionString
        {
            get { return ConfigurationManager.ConnectionStrings["db"].ConnectionString; }
        }

        /// <summary>
        ///     Смещение от мирового времени.
        /// </summary>
        public static short Utc
        {
            get { return Convert.ToInt16(ConfigurationManager.AppSettings["Utc"]); }
        }

        /// <summary>
        ///     Номер чека.
        /// </summary>
        public static int NumberTicket
        {
            get { return Convert.ToInt32(ConfigurationManager.AppSettings["NumberTicket"]); }
        }

        /// <summary>
        ///     Папка приложения.
        /// </summary>
        public static string AppPath
        {
            get { return AppDomain.CurrentDomain.BaseDirectory; }
        }

        /// <summary>
        ///     Разрешение на изменение решетки программы.
        /// </summary>
        public static bool GridModif
        {
            get { return bool.Parse(ConfigurationManager.AppSettings["GridModif"]); }
        }

        /// <summary>
        ///     Синхронизация при запуске программы.
        /// </summary>
        public static bool FromLoadSyncAll
        {
            get { return bool.Parse(ConfigurationManager.AppSettings["FromLoadSyncAll"]); }
            set
            {
                var config = ConfigurationManager.OpenExeConfiguration(AppPath + "TicketWindow.exe");
                config.AppSettings.Settings["FromLoadSyncAll"].Value = value.ToString();
                config.Save(ConfigurationSaveMode.Modified);
            }
        }

        /// <summary>
        ///     Функции кассы отключены если true.
        /// </summary>
        public static bool Bureau
        {
            get { return bool.Parse(ConfigurationManager.AppSettings["Bureau"]); }
        }

        public static string DateFormat
        {
            get { return "dd/MM/yyyy HH:mm:ss"; }
        }
    }
}