using System;
using System.Collections.Generic;
using ChartStat.ChartUI.Enums;
using ChartStat.ChartUI.Services;
using ChartStat.Model.Models;
using Microsoft.Reporting.WinForms;

namespace ChartStat.ChartUI.ChartControl
{
    public class ViewModel
    {
        private readonly ReportViewer _report;

        public ViewModel(ReportViewer report)
        {
            _report = report;
        }

        public bool LoadTableAndChart(StatSalesType[] modelData, StatTypeEnum statType, ChartTypeEnum chartType, ViewTypeEnum viewType, FilterTypeEnum filterType, bool isCount, bool isUseCodeInName)
        {
            try
            {
                var parameters = new List<ReportParameter>
                                 {
                                     new ReportParameter("ChartType", chartType.ToString()),
                                     new ReportParameter("Symbol", isCount?" ":"€")
                                 };

                var reportDataSource = new ReportDataSource
                                       {
                                           Name = "ChartDataSet",
                                           Value = ModelToChartDataService.GetChartData(modelData, statType, viewType, chartType, filterType, isCount, isUseCodeInName)
                                       };

                _report.Reset();
                _report.ProcessingMode = ProcessingMode.Local;
                _report.LocalReport.ReportEmbeddedResource = "ChartStat.ChartUI.Reports.ReportWithTable.rdlc";
                _report.LocalReport.SetParameters(parameters);
                _report.LocalReport.DataSources.Add(reportDataSource);
                _report.RefreshReport();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
