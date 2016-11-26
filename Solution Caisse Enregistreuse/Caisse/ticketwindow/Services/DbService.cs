using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using TicketWindow.Global;

namespace TicketWindow.Services
{
    public static class DbService
    {
        /// <summary>
        ///     Открывает соединение.
        /// </summary>
        /// <returns>Успешно или нет.</returns>
        public static bool Connect()
        {
            var connection = new SqlConnection(Config.ConnectionString);

            try
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();
                return true;
            }
            catch (SqlException odbcEx)
            {
                if (connection.State != ConnectionState.Closed)
                    connection.Close();

                FunctionsService.ShowMessageTimeList(odbcEx.Message);

                return false;
            }
        }

        public static List<object[]> QueryResonse(string cmd)
        {
            var result = new List<object[]>();

            if (SyncData.IsConnect)
            {
                try
                {
                    using (var connection = new SqlConnection(Config.ConnectionString))
                    {
                        var command = new SqlCommand(cmd, connection);
                        connection.Open();

                        var dataReader = command.ExecuteReader();

                        while (dataReader.Read())
                        {
                            var o = new object[dataReader.FieldCount];
                            dataReader.GetValues(o);
                            result.Add(o);
                        }
                        connection.Close();
                    }
                }
                catch
                {
                    LogService.LogText(TraceLevel.Error, Config.ConnectionString + " error ");
                    LogService.SqlLog(TraceLevel.Error, "\"" + cmd + "\"");
                }

                var r = string.Empty;

                if (result.Count < 100)
                    r = result.Aggregate(r, (current1, e) => e.Aggregate(current1, (current, n) => current + ("<" + n + ">")));
                else
                    r += "Record > 100";

                GlobalVar.Messages.Add(cmd + r);
            }
            else
            {
                LogService.LogText(TraceLevel.Error, Config.ConnectionString + " not connect ");
                LogService.SqlLog(TraceLevel.Error, "\"" + cmd + "\"");
            }
            return result;
        }
    }
}