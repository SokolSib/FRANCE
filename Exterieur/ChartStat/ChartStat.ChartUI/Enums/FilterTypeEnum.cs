namespace ChartStat.ChartUI.Enums
{
    /// <summary>
    /// Тип фильтра данных.
    /// </summary>
    public enum FilterTypeEnum
    {
        /// <summary>
        /// Группа или подгруппа.
        /// </summary>
        GroupOrSubgroup,

        /// <summary>
        /// Продукт или штрихкод.
        /// </summary>
        ProductOrBarcode,

        /// <summary>
        /// Несколько продуктов.
        /// </summary>
        AnyProducts,

        /// <summary>
        /// Несколько подгрупп.
        /// </summary>
        AnySubgroups
    }
}
