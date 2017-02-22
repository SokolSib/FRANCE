using System;
using Dapper;
using TicketWindow.DAL.Models;
using TicketWindow.Global;

namespace TicketWindow.DAL.Repositories
{
    /// <summary>
    ///     Xml Db. пустота в режиме локальной работы
    /// </summary>
    public class RepositoryTes
    {
        private static readonly ConnectionFactory ConnectionFactory = new ConnectionFactory(Config.ConnectionString);

        public static int MaxId(int type)
        {
            if (SyncData.IsConnect)
            {
                const string query = "SELECT MAX(Id) FROM TES WHERE Type = @type";

                using (var connection = ConnectionFactory.CreateConnection())
                    return (int) connection.ExecuteScalar(query, new {type});
            }

            throw new NotImplementedException();
        }

        public static int AddToDb(Tes tes)
        {
            if (SyncData.IsConnect)
            {
                using (var connection = ConnectionFactory.CreateConnection())
                {
                    connection.Open();
                    var trans = connection.BeginTransaction();

                    var result = connection.Execute(QueryTes, tes, trans);
                    if (result < 0)
                    {
                        trans.Rollback();
                        return result;
                    }

                    result = connection.Execute(QueryTesReglement, tes.TesReglaments, trans);
                    if (result < 0)
                    {
                        trans.Rollback();
                        return result;
                    }

                    result = connection.Execute(QueryTesProduct, tes.TesProducts, trans);
                    if (result < 0)
                        trans.Rollback();

                    trans.Commit();
                    return result;
                }
            }

            return 0;
        }

        #region sqripts

        private const string QueryTes = @"INSERT TES (
CustomerId,
Id,
Type,
DateTime,
Payement,
Livraison,
a_CodeFournisseur,
a_Sex,
a_Name,
a_Surname,
a_NameCompany,
a_SIRET,
a_FRTVA,
a_OfficeAddress,
a_OfficeZipCode,
a_OfficeCity,
a_Telephone,
a_Mail,
v_NameCompany,
v_CP,
v_Ville,
v_Adresse,
v_Phone,
v_Mail,
v_SIRET,
v_FRTVA,
v_CodeNAF,
v_Fax,
Montant,
Description,
Nclient)
     VALUES (
@CustomerId,
@Id,
@Type,
@DateTime,
@Payement,
@Livraison,
@ACodeFournisseur,
@ASex,
@AName,
@ASurname,
@ANameCompany,
@ASiret,
@AFrtva,
@AOfficeAddress,
@AOfficeZipCode,
@AOfficeCity,
@ATelephone,
@AMail,
@VNameCompany,
@VCp,
@VVille,
@VAdresse,
@VPhone,
@VMail,
@VSiret,
@VFrtva,
@VCodeNaf,
@VFax,
@Montant,
@Description,
@Nclient) ";

        private const string QueryTesReglement = @"INSERT INTO TESreglement (
CustomerId,
DateTime,
Caisse,
TypePay,
Montant,
TESCustomerId) 
    VALUES(
@CustomerId,
GETDATE(),
@Caisse,
@TypePay,
@Montant,
@TesCustomerId) ";

        private const string QueryTesProduct = @"INSERT INTO TESproducts (
TypeID,
[Date],
CustomerIdProduct,
NameProduct,
CodeBar,
Balance,
PrixHT,
QTY,
TVA,
TotalHT,
Description,
ProductsWeb,
SubGroup,
[Group],
TESCustomerId,
ConditionAchat)
     VALUES (
@TypeId,
@Date,
@CustomerIdProduct,
@NameProduct,
@CodeBar,
@Balance,
@PrixHt,
@Qty,
@Tva,
@TotalHt,
@Description,
@ProductsWeb,
@SubGroup,
@Group,
@TesCustomerId,
@ConditionAchat) ";

        #endregion
    }
}