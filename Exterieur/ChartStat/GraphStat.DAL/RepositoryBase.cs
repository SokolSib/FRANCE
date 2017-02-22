using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using Dapper;

namespace ChartStat.Model
{
    /// <summary>
    /// Базовый класс для репозитория.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class RepositoryBase<T>
    {
        /// <summary>
        /// Объект соединения с БД.
        /// </summary>
        protected readonly IDbConnection Connection;

        /// <summary>
        /// Репозиторий по выборке данных из БД.
        /// </summary>
        /// <param name="connection">Объект соединения с БД.</param>
        internal RepositoryBase(IDbConnection connection)
        {
            Connection = connection;
        }

        /// <summary>
        /// Выбор данных.
        /// </summary>
        /// <returns>Данные.</returns>
        public virtual ICollection<T> GetAllEntities()
        {
            using (Connection)
                return Connection.Query<T>(GetCommandText()).ToArray();
        }

        /// <summary>
        /// Текст SQL зарпоса.
        /// </summary>
        /// <returns>SQL запрос.</returns>
        protected abstract string GetCommandText();
    }
}
