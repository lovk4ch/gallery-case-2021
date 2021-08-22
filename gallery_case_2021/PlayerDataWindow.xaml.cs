using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace gallery_case_2021
{
    public partial class PlayerDataWindow : UserControl
    {
        public int PlayerId
        {
            get { return (int)GetValue(PlayerIdProperty); }
            set { SetValue(PlayerIdProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PlayerId.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PlayerIdProperty =
            DependencyProperty.Register("PlayerId", typeof(int), typeof(PlayerDataWindow), new PropertyMetadata(0));

        public int GetWorkload()
        {
            return new Random().Next(44, 95);
        }

        public PlayerDataWindow()
        {
            InitializeComponent();
        }

        public void SetData(DateTime? startDate, DateTime? endDate, out float k)
        {
            var playerData = ParquetManager.GetPlayerData(PlayerId, (DateTime)startDate, (DateTime)endDate);
            if (playerData == null || playerData.Count == 0)
            {
                dataNotFoundLabel.Visibility = Visibility.Visible;
                playerPlot.Visibility = Visibility.Collapsed;
                workloadLabel.Content = GetWorkload() + "%";
                k = ParquetManager.GetMonthlyIdle(((DateTime)startDate).Month);
                return;
            }

            dataNotFoundLabel.Visibility = Visibility.Collapsed;
            playerPlot.Visibility = Visibility.Visible;

            playerDataGrid.Columns.Clear();
            playerDataGrid.ItemsSource = playerData;

            // total ODS count for time interval
            int totalContacts = playerData.Count;

            DateTimeOffset daysMin = playerData.Min(x => x.BillingDate);
            DateTimeOffset daysMax = playerData.Max(x => x.BillingDate);
            int daysInterval = (daysMax - daysMin).Days + 1;

            List<int> dayContacts = new List<int>();
            string dayContactsData = string.Empty;
            for (int i = 0; i < daysInterval; i++)
            {
                int currentDayOTS = playerData.Where(
                    x => x.BillingDate.Ticks > daysMin.AddDays(i - 1).Ticks
                    && x.BillingDate.Ticks < daysMin.AddDays(i + 1).Ticks
                ).ToList().Count;
                dayContacts.Add(currentDayOTS);
            }

            float avg = 0, max = 0;
            for (int i = 0; i < dayContacts.Count; i++)
            {
                if (max < dayContacts[i])
                    max = dayContacts[i];
                avg += dayContacts[i];
            }
            avg /= dayContacts.Count;
            k = avg / max;

            contactsLabel.Content = totalContacts;
            intervalLabel.Content = daysInterval + " дней";
            averageOTSLabel.Content = totalContacts / daysInterval;
            workloadLabel.Content = GetWorkload() + "%";

            playerPlot.SetData(daysMin, daysInterval, dayContacts);
        }
    }
}