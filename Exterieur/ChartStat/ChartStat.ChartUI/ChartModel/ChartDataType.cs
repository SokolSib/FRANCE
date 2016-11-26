using System;

namespace ChartStat.ChartUI.ChartModel
{
    /// <summary>
    /// Тип данных для таблицы и графика.
    /// </summary>
    public class ChartDataType
    {
        /// <summary>
        /// Дата и время.
        /// </summary>
        public DateTime DateTime { get; set; }

        /// <summary>
        /// Название.
        /// </summary>
        public string DateName { get; set; }

        /// <summary>
        /// Название.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Штрих код.
        /// </summary>
        public string CodeBar { get; set; }

        /// <summary>
        /// Цена.
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Количество.
        /// </summary>
        public decimal Count { get; set; }
    }
}
