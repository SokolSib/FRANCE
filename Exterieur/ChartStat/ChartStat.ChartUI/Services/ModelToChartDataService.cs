using System;
using System.Collections.Generic;
using System.Linq;
using ChartStat.ChartUI.ChartModel;
using ChartStat.ChartUI.Enums;
using ChartStat.Model.Models;

namespace ChartStat.ChartUI.Services
{
    public class ModelToChartDataService
    {
        public static ChartDataType[] GetChartData(StatSalesType[] modelData, StatTypeEnum statType, ViewTypeEnum viewType, ChartTypeEnum chartType, FilterTypeEnum filterType,
            bool isCount, bool isUseCodeInName)
        {
            switch (statType)
            {
                case StatTypeEnum.StatSales:
                    return GetViewData(modelData.Select(row => new ChartDataType
                                                               {
                                                                   DateTime = row.Date,
                                                                   DateName = row.Date.ToString(),
                                                                   Name = isUseCodeInName ? string.Format("{0} ({1})", row.Name, row.Number) : row.FullName,
                                                                   CodeBar = isCount ? row.Qty.ToString() : string.Format("{0} €", GetTotal(row)),
                                                                   Price = GetTotal(row),
                                                                   Count = isCount ? row.Qty : GetTotal(row)
                                                               }), viewType, chartType, filterType);
                case StatTypeEnum.StatSalesWithReceipts:
                    return GetViewData(modelData.Select(row => new ChartDataType
                                                               {
                                                                   DateTime = row.Date,
                                                                   DateName = row.Date.ToString(),
                                                                   Name = isUseCodeInName ? string.Format("{0} ({1})", row.Name, row.Number) : row.FullName,
                                                                   CodeBar = isCount ? row.Qty.ToString() : string.Format("{0} €", GetPrix(row)),
                                                                   Price = GetPrix(row),
                                                                   Count = isCount ? row.Qty : GetPrix(row)
                                                               }), viewType, chartType, filterType);
            }

            return null;
        }

        /*         
                        Ринат

                        row.Prix - цена покупки без НДС то есть = 100%

                        row.QTY - кол-во купленного товара

                        row.Total - в данном случае это Итог с НДС - Скидка

                        row.TVA - % НДС

                        Значит для расчета выгоды, действуем так


                        Для расчета Total без НДС = row.Total / (100 + row.TVA) * 100 - в данном примере с Айраном, наш НДС 5.5%  из этого следует

                        = row.Total / (100 + 5.5 ) * 100  - Итог без НДС и без Скидки то етсь искоммай сумма

                        теперь осталось убрать цену покупки не забывая про кол-во


                        row.Prix * row.QTY = сумма без НДС потраченная на покупку данного товара

                              Math.Round(row.Total - ((row.PriceHt/(row.Tva + 100)*row.Tva)*row.Qty) - row.Prix, 2)),
                      Price = Math.Round(row.Total - ((row.PriceHt/(row.Tva + 100)*row.Tva)*row.Qty) - row.Prix, 2),
                      */

        private static decimal GetPrix(StatSalesType row)
        {
            var prix = row.Total/(100 + row.Tva)*100 - (row.Prix*row.Qty);
            //var prix = row.PriceHt * row.Qty - (row.Prix * row.Qty);
            return Math.Round(prix);
        }

        private static decimal GetTotal(StatSalesType row)
        {
            return row.Total;
            //return row.PriceHt*row.Qty;
        }

        private static ChartDataType[] GetViewData(IEnumerable<ChartDataType> datas, ViewTypeEnum viewType, ChartTypeEnum chartType, FilterTypeEnum filterType)
        {
            switch (chartType)
            {
                case ChartTypeEnum.HistogramChart:
                case ChartTypeEnum.ColumnarChart:
                case ChartTypeEnum.LineChart:
                    return GetChartedData(datas, viewType, filterType != FilterTypeEnum.ProductOrBarcode);
                case ChartTypeEnum.ThreeDimensionalChart:
                    return filterType == FilterTypeEnum.ProductOrBarcode ? GetChartedData(datas, viewType, false, true) : GetChartedData(datas, viewType, true);
                default:
                    return datas.OrderBy(d => d.DateTime).ToArray();
            }
        }

        private static ChartDataType[] GetChartedData(IEnumerable<ChartDataType> datas, ViewTypeEnum viewType, bool isUseName, bool isUseDateNameInChart = false)
        {
            var dic = new Dictionary<string, ChartDataType>();
            foreach (var data in datas)
            {
                DateTime date;
                string dateName;
                switch (viewType)
                {
                    case ViewTypeEnum.Hours:
                        date = new DateTime(data.DateTime.Year, data.DateTime.Month, data.DateTime.Day, data.DateTime.Hour, 0, 0);
                        dateName = string.Format("{0}.{1:00}.{2:00} {3}h", data.DateTime.Year, data.DateTime.Month, data.DateTime.Day, data.DateTime.Hour);
                        break;
                    case ViewTypeEnum.Days:
                        date = new DateTime(data.DateTime.Year, data.DateTime.Month, data.DateTime.Day);
                        dateName = string.Format("{0}.{1:00}.{2:00}", data.DateTime.Year, data.DateTime.Month, data.DateTime.Day);
                        break;
                    case ViewTypeEnum.Weeks:
                        date = new DateTime(data.DateTime.Year, data.DateTime.Month, data.DateTime.Day / 7 + 1);
                        dateName = string.Format("{0}.{1:00}.{2}w", data.DateTime.Year, data.DateTime.Month, data.DateTime.Day / 7 + 1);
                        break;
                    case ViewTypeEnum.Months:
                        date = new DateTime(data.DateTime.Year, data.DateTime.Month, 1);
                        dateName = string.Format("{0}.{1:00}", data.DateTime.Year, data.DateTime.Month);
                        break;
                    case ViewTypeEnum.Years:
                        date = new DateTime(data.DateTime.Year, 1, 1);
                        dateName = string.Format("{0}", data.DateTime.Year);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("viewType", viewType, null);
                }
                var key = data.Name + dateName;
                if (dic.ContainsKey(key))
                {
                    dic[key].DateTime = date;
                    dic[key].Name = isUseDateNameInChart ? dateName : data.Name;
                    dic[key].Price += data.Price;
                    dic[key].Count += data.Count;
                    dic[key].DateName = dateName;
                    dic[key].CodeBar = data.CodeBar;
                }
                else
                {
                    dic[key] = new ChartDataType
                    {
                        DateTime = date,
                        Name = isUseDateNameInChart ? dateName : data.Name,
                        Price = data.Price,
                        Count = data.Count,
                        DateName = dateName,
                        CodeBar = data.CodeBar
                    };
                }
            }
            return dic.Select(pair => pair.Value).OrderBy(d => d.DateTime).ToArray();
        }
    }
}