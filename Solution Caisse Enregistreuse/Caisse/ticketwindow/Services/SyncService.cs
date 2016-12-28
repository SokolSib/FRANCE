using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using TicketWindow.Class;
using TicketWindow.Classes;
using TicketWindow.DAL.Additional;
using TicketWindow.DAL.Models;
using TicketWindow.DAL.Repositories;
using TicketWindow.Global;
using TicketWindow.Properties;
using TicketWindow.Winows.OtherWindows.Setting;

namespace TicketWindow.Services
{
    public static class SyncService
    {
        public static void SetDefoultTypesPays()
        {
            var payCash = new TypePay(1, true, Resources.TypePayCash, "Cash", 450600, true, true, true);
            var payBankCards = new TypePay(2, true, Resources.TypePayBankCards, "BankCards", 450601, false, true, false);
            var payBankChecks = new TypePay(3, true, Resources.TypePayBankChecks, "BankChecks", 450602, false, true, false);
            var payResto = new TypePay(4, true, Resources.TypePayResto, "Resto", 450603, false, true, false);
            var payBondAchat = new TypePay(5, true, Resources.TypePayBondAchat, "BondAchat", 450604, false, true, false);

            var payCashRep = RepositoryTypePay.TypePays.FirstOrDefault(tp => tp.NameCourt == payCash.NameCourt);
            var payBankCardsRep = RepositoryTypePay.TypePays.FirstOrDefault(tp => tp.NameCourt == payBankCards.NameCourt);
            var payBankChecksRep = RepositoryTypePay.TypePays.FirstOrDefault(tp => tp.NameCourt == payBankChecks.NameCourt);
            var payRestoRep = RepositoryTypePay.TypePays.FirstOrDefault(tp => tp.NameCourt == payResto.NameCourt);
            var payBondAchatRep = RepositoryTypePay.TypePays.FirstOrDefault(tp => tp.NameCourt == payBondAchat.NameCourt);

            if (payCashRep == null) RepositoryTypePay.TypePays.Add(payCash);
            else payCashRep.Name = payCash.Name;

            if (payBankCardsRep == null) RepositoryTypePay.TypePays.Add(payBankCards);
            else payBankCardsRep.Name = payBankCards.Name;

            if (payBankChecksRep == null) RepositoryTypePay.TypePays.Add(payBankChecks);
            else payBankChecksRep.Name = payBankChecks.Name;

            if (payRestoRep == null) RepositoryTypePay.TypePays.Add(payResto);
            else payRestoRep.Name = payResto.Name;

            if (payBondAchatRep == null) RepositoryTypePay.TypePays.Add(payBondAchat);
            else payBondAchatRep.Name = payBondAchat.Name;
        }

        public static void SyncAll(Dispatcher dispatcher)
        {
            var progressCount = Config.IsUseServer ? 7 + RepositorySyncIsLoading.CheckedCount() : 7;

            ProgressHelper.Instance.Start(progressCount, Resources.LabelDataLoading);

            var progressValue = 0;
            ProgressHelper.Instance.SetValue(progressValue++, Resources.LabelConnectionTest);
            SyncData.SetConnect(Config.IsUseServer && DbService.Connect());

            ProgressHelper.Instance.SetValue(progressValue++, Resources.MenuUsers);
            RepositoryAccountUser.Set();

            ProgressHelper.Instance.SetValue(progressValue++, Resources.LabelTypesPays);
            RepositoryTypePay.Sync();
            SetDefoultTypesPays();

            ProgressHelper.Instance.SetValue(progressValue++, Resources.LabelCurrency);
            RepositoryCurrency.Sync();
            if (RepositoryCurrency.Currencys.Count == 0)
            {
                LogService.LogText(TraceLevel.Error, "Currencus count is 0");
                GlobalVar.IsOpen = false;
            }

            ProgressHelper.Instance.SetValue(progressValue++, Resources.LabelCassieData);
            RepositoryGeneral.Set();
            RepositoryOpenTicketWindow.Sync();
            RepositoryCloseTicketG.Sync();
            RepositorySyncIsLoading.LoadFile();
            RepositoryEstablishment.Sync();
            if (RepositoryEstablishment.Establishment != null)
                LogService.EstablishmentInfo = "Ville = " + RepositoryEstablishment.Establishment.Ville + "\r\n" +
                                               "Adress = " + RepositoryEstablishment.Establishment.Adress + "\r\n" +
                                               "Name = " + RepositoryEstablishment.Establishment.Name + "\r\n" +
                                               "Type = " + RepositoryEstablishment.Establishment.Type + "\r\n" +
                                               "Mail = " + RepositoryEstablishment.Establishment.Mail + "\r\n" +
                                               "Phone = " + RepositoryEstablishment.Establishment.Phone + "\r\n" +
                                               "Fax = " + RepositoryEstablishment.Establishment.Fax + "\r\n" +
                                               "CodeNaf = " + RepositoryEstablishment.Establishment.CodeNaf + "\r\n" +
                                               "Cp = " + RepositoryEstablishment.Establishment.Cp + "\r\n" +
                                               "Ntva = " + RepositoryEstablishment.Establishment.Ntva + "\r\n" +
                                               "Siret = " + RepositoryEstablishment.Establishment.Siret;
            
            ProgressHelper.Instance.SetValue(progressValue++, Resources.LabelProducts);
            RepositoryProduct.Set();

            if (Config.IsUseServer)
            {
                if (RepositorySyncIsLoading.IsLoading(SyncEnum.PayProduct))
                {
                    ProgressHelper.Instance.SetValue(progressValue++, Resources.LabelPayProduct);
                    RepositoryPayProduct.Sync();
                }

                if (RepositorySyncIsLoading.IsLoading(SyncEnum.CloseTicket))
                {
                    ProgressHelper.Instance.SetValue(progressValue++, Resources.LabelCloseTicket);
                    RepositoryCloseTicket.Sync();
                }

                if (RepositorySyncIsLoading.IsLoading(SyncEnum.PayProductTmp))
                {
                    ProgressHelper.Instance.SetValue(progressValue++, Resources.LabelPayProductTmp);
                    RepositoryPayProductTmp.Sync();
                }

                if (RepositorySyncIsLoading.IsLoading(SyncEnum.CheckTicketTmp))
                {
                    ProgressHelper.Instance.SetValue(progressValue++, Resources.LabelCheckTicketTmp);
                    RepositoryCheckTicketTmp.Sync();
                }

                if (RepositorySyncIsLoading.IsLoading(SyncEnum.CheckTicket))
                {
                    ProgressHelper.Instance.SetValue(progressValue++, Resources.LabelCheckTicket);
                    RepositoryCheckTicket.Sync();
                }

                if (RepositorySyncIsLoading.IsLoading(SyncEnum.ClientInfo))
                {
                    ProgressHelper.Instance.SetValue(progressValue++, Resources.LabelClientInfo);
                    RepositoryClientInfo.Sync();
                }

                if (RepositorySyncIsLoading.IsLoading(SyncEnum.Pro))
                {
                    ProgressHelper.Instance.SetValue(progressValue++, Resources.LabelProviders);
                    RepositoryPro.Sync();
                }

                if (RepositorySyncIsLoading.IsLoading(SyncEnum.DiscountCard))
                {
                    ProgressHelper.Instance.SetValue(progressValue++, Resources.LabelDiscountCard);
                    RepositoryDiscountCard.Sync();
                }

                if (RepositorySyncIsLoading.IsLoading(SyncEnum.DevisWeb))
                {
                    ProgressHelper.Instance.SetValue(progressValue++, Resources.LabelDevisWeb);
                    RepositoryDevisWeb.Sync();
                }

                if (RepositorySyncIsLoading.IsLoading(SyncEnum.XmlFile))
                {
                    ProgressHelper.Instance.SetValue(progressValue++, Resources.LabelXmlFile);
                    RepositoryXmlFile.SetAllFromDb();
                }
            }

            ProgressHelper.Instance.SetValue(progressValue, Resources.LabelDataLoading);
            ClassGridGroup.Initialize();
            ClassGridProduct.Initialize();
            ClassGridStatistiqueRegionEtPays.Initialize();
            ProgressHelper.Instance.Stop();

            if (SyncData.IsConnect)
                if (ClassDataTimeSrv.GetDateTimeFromSrv())
                {
                    var text = Resources.LabelServerTime + " : " + ClassDataTimeSrv.DateTimeFromSrv + Environment.NewLine +
                               Resources.LabelCashboxTime + " : " + DateTime.Now + Environment.NewLine;

                    var window = new WDateTimeSrv(text);
                    window.ShowDialog();
                }

            if (!Config.Bureau)
            {
                var generalEstablishment =
                    RepositoryGeneral.Generals.Find(l => l.EstablishmentCustomerId == Config.IdEstablishment);

                GlobalVar.TicketWindowG = generalEstablishment.TicketWindowGeneral; //SQL.OCC.cassieInf.idTicketWindowG;
                var openTicketWindow =
                    RepositoryOpenTicketWindow.OpenTicketWindows.FirstOrDefault(l => l.CustomerId == Config.CustomerId);
                if (openTicketWindow != null)
                {
                    GlobalVar.TicketWindow = openTicketWindow.IdTicketWindow;
                    GlobalVar.IsOpen = openTicketWindow.IsOpen;
                    if (!GlobalVar.IsOpen) openTicketWindow = null;
                }
                else GlobalVar.IsOpen = false;

                // окно закрытия кассы
                if (generalEstablishment.Date.Date != DateTime.Now.Date
                    && generalEstablishment.TicketWindowGeneral != Guid.Empty
                )
                {
                    var errorlist = Resources.LabelNow + " : " + DateTime.Now.ToLongDateString() + "  " +
                                    DateTime.Now.ToLongTimeString() + Environment.NewLine +
                                    "--------------------------------" + Environment.NewLine + Environment.NewLine;

                    errorlist += " " + Resources.LabelOpenTotalTW + " : " +
                                 RepositoryGeneral.Generals.First().Date.ToLongDateString() + Environment.NewLine;
                    errorlist += Resources.LabelOpenLocal + " : ";
                    errorlist += openTicketWindow?.DateOpen.ToLongDateString() ?? string.Empty + Environment.NewLine;
                    
                    var tickedWindowId = GlobalVar.TicketWindow != Guid.Empty ? GlobalVar.TicketWindow.ToString() : string.Empty;
                    
                    var window = new WCloseTicketWindow(errorlist)
                                 {
                                     BtnCloseLocal = {IsEnabled = tickedWindowId != string.Empty}
                                 };
                    window.ShowDialog();
                    RepositoryGeneral.Set();
                    RepositoryOpenTicketWindow.Sync();
                }

                if (!GlobalVar.IsBreak)
                {
                    // окно открытия кассы
                    if (!RepositoryGeneral.IsOpen)

                        if (GlobalVar.TicketWindowG == Guid.Empty)
                        {
                            var status = Environment.NewLine + "--------------------------------" + Environment.NewLine +
                                         Resources.LabelCashBox + " : " + Config.NameTicket + Environment.NewLine +
                                         Resources.LabelPost + " : " + Config.NumberTicket + Environment.NewLine +
                                         Resources.LabelOpenedBy + " : " + Config.User + Environment.NewLine +
                                         Environment.NewLine +
                                         "--------------------------------" + Environment.NewLine + Environment.NewLine +
                                         Resources.LabelTotalOpeningKey + " : " + GlobalVar.TicketWindowG +
                                         Environment.NewLine + Environment.NewLine +
                                         Resources.LabelLocalOpeningKey + " : " + GlobalVar.TicketWindow +
                                         Environment.NewLine;

                            var window = new WOpenTicletG(status);
                            window.ShowDialog();
                        }

                    if (GlobalVar.TicketWindow == Guid.Empty)
                    {
                        var status = Environment.NewLine + "--------------------------------" + Environment.NewLine +
                                     Resources.LabelCashBox + " : " + Config.NameTicket + Environment.NewLine +
                                     Resources.LabelPost + " : " + Config.NumberTicket + Environment.NewLine +
                                     Resources.LabelOpenedBy + " : " + Config.User + Environment.NewLine +
                                     Environment.NewLine +
                                     "--------------------------------" + Environment.NewLine +
                                     Resources.LabelTotalOpeningKey + " : " + GlobalVar.TicketWindowG +
                                     Environment.NewLine +
                                     Resources.LabelLocalOpeningKey + " : " + GlobalVar.TicketWindow +
                                     Environment.NewLine;
                        var window = new WOpenTicket(status);
                        window.ShowDialog();
                    }
                }
            }
            
            DotLiquidService.SetPath(0);
            DotLiquidService.SetPath(1);
            DotLiquidService.SetPath(2);

            RepositoryActionHashBox.Sync();
            CassieService.LoadProductCheckFromFile();
            RepositoryCheck.OpenTicket();
        }

        public static int InsDevis(DevisIdType di)
        {
            if (RepositoryDevisId.Add(di) == 1)
            {
                var id = RepositoryDevisId.GetMaxId();
                ClassProMode.Ndevis = id;

                var count = RepositoryDevisWeb.AddRange(di.DivisWebs);
                if (count != di.DivisWebs.Count)
                    LogService.Log(TraceLevel.Error, 903);

                return di.DivisWebs.Count + 1;
            }
            return 0;
        }

        public static void MegaSync()
        {
            // check tables
            var syncTask = Task<bool>.Factory.StartNew(
                () =>
                {
                    RepositoryActionHashBox.Sync();
                    //RepositoryCheck
                    RepositoryCheckTicket.Sync();
                    RepositoryCheckTicketTmp.Sync();
                    RepositoryPro.Sync();
                    RepositoryClientInfo.Sync();
                    RepositoryCloseTicket.Sync();
                    RepositoryCloseTicketCheckDiscount.Sync();
                    RepositoryCloseTicketG.Sync();
                    RepositoryCloseTicketTmp.Sync();
                    RepositoryCountry.Sync();
                    RepositoryCurrency.Sync();
                    //RepositoryCurrencyRelations
                    RepositoryDevisId.Sync();
                    RepositoryDevisWeb.Sync();
                    //RepositoryDiscount
                    RepositoryDiscountCard.Sync();
                    RepositoryEstablishment.Sync();
                    RepositoryGeneral.Sync();
                    RepositoryGroupProduct.Sync();
                    //RepositoryHistoryChangeProduct
                    RepositoryInfoClientsDiscountsType.Sync();
                    RepositoryLastUpdate.Sync();
                    RepositoryOpenTicketWindow.Sync();
                    RepositoryPayProduct.Sync();
                    RepositoryPayProductTmp.Sync();
                    //RepositoryPriceGros
                    RepositoryProduct.Sync();
                    RepositoryProductBc.Sync();
                    RepositoryStatNation.Sync();
                    RepositoryStatNationPopup.Sync();
                    RepositoryStatPlaceArrond.Sync();
                    //RepositoryStockLogs
                    RepositoryStockReal.Sync();
                    RepositorySyncPlus.Sync();
                    RepositorySyncPlusProduct.Sync();
                    //RepositoryTes
                    RepositoryTva.Sync();
                    RepositoryTypePay.Sync();
                    //RepositoryXmlFile
                    return true;
                });
            syncTask.ContinueWith(
                s => { });
        }
    }
}