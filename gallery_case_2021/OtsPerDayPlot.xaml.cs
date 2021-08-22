using System;
using System.Collections.Generic;
using System.Windows.Controls;
using LiveCharts;
using LiveCharts.Wpf;

namespace gallery_case_2021
{
    public partial class OtsPerDayPlot : UserControl
    {
        public OtsPerDayPlot()
        {
            InitializeComponent();
        }

        public void SetData(DateTimeOffset daysMin, int daysInterval, List<int> dayContacts)
        {
            ChartValues<int> values = new ChartValues<int>();
            foreach (int dayContact in dayContacts)
            {
                values.Add(dayContact);
            }

            SeriesCollection?.Clear();
            SeriesCollection = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Контакты за день",
                    FontSize = 20,
                    Values = values
                }
            };

            Labels = new List<string>();
            for (int i = 0; i < daysInterval; i++)
            {
                DateTimeOffset date = daysMin.AddDays(i);
                Labels.Add(date.Day + "." + date.Month + "." + date.Year);
            }

            Formatter = value => value.ToString("N");
            DataContext = this;
        }

        public SeriesCollection SeriesCollection { get; set; }
        public List<string> Labels { get; set; }
        public Func<double, string> Formatter { get; set; }

    }
}