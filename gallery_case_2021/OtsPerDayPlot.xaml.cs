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

        public void SetData(DateTimeOffset daysMin, int daysInterval, int year, List<int> dayContacts)
        {
            ChartValues<int> values = new ChartValues<int>();
            foreach (int dayContact in dayContacts)
            {
                values.Add(dayContact);
            }

            SeriesCollection = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Контакты за день",
                    FontSize = 20,
                    Values = values
                }
            };

            //also adding values updates and animates the chart automatically
            // SeriesCollection[1].Values.Add(48d);

            Labels = new List<string>();
            for (int i = 0; i < daysInterval; i++)
            {
                DateTimeOffset date = daysMin.AddDays(i);
                Labels.Add(date.Day + "." + date.Month + "." + date.Year);
            }

            // Labels = new[] { "Maria", "Susan", "Charles", "Frida" };
            Formatter = value => value.ToString("N");

            DataContext = this;
        }

        public SeriesCollection SeriesCollection { get; set; }
        public List<string> Labels { get; set; }
        public Func<double, string> Formatter { get; set; }

    }
}