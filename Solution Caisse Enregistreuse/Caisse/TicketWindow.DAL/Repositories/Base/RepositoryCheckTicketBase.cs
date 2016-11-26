using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using TicketWindow.DAL.Models.Base;
using TicketWindow.Global;

namespace TicketWindow.DAL.Repositories.Base
{
    public abstract class RepositoryCheckTicketBase<T> where T : CheckTicketBase
    {
        protected static readonly ConnectionFactory ConnectionFactory = new ConnectionFactory(Config.ConnectionString);

        protected static List<T> GetFromDbByCloseTicketIdBase(Guid customerId, string query)
        {
            using (var connection = ConnectionFactory.CreateConnection())
                return connection.Query<T>(query, new {customerId}).ToList();
        }
    }
}