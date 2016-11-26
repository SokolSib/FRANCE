namespace TicketWindow.DAL.Additional
{
    public enum Privelege
    {
        /// <summary>
        /// все можно.
        /// </summary>
        All,

        /// <summary>
        /// Разрешение создавать роль.
        /// </summary>
        RedactRole,

        /// <summary>
        /// Разрешение создавать пользователя.
        /// </summary>
        RedactUser,

        /// <summary>
        /// Разрешение редактировать кнопки.
        /// </summary>
        RedactButton,

        /// <summary>
        /// Разрешение редактировать НДС.
        /// </summary>
        RedactTva,

        /// <summary>
        /// Разрешение редактировать группы и подгруппы продуктов.
        /// </summary>
        RedactGroupsProduct,

        /// <summary>
        /// Разрешение редактировать синхронизацию
        /// </summary>
        RedactSyncSettings,

        /// <summary>
        /// Разрешение удалять продукт из чека
        /// </summary>
        DeleteProductFromCheck
    }
}
