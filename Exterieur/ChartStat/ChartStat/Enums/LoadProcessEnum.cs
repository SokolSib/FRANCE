namespace ChartStat.Enums
{
    /// <summary>
    /// Шаг процесса обработки данных.
    /// </summary>
    public enum LoadProcessEnum
    {
        /// <summary>
        /// Нет данных.
        /// </summary>
        None,

        /// <summary>
        /// Ошибка.
        /// </summary>
        Fail,

        /// <summary>
        /// Успешно.
        /// </summary>
        Success,

        /// <summary>
        /// В процессе.
        /// </summary>
        InProcess
    }
}
