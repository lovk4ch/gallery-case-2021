using System.Windows;
using System.Linq;
using System.Collections.Generic;
using Parquet.Data;
using System.IO;
using System;
using Parquet;
using System.Data;

namespace gallery_case_2021
{
    public partial class MainWindow : Window
    {
        private const string pathPrefix = "..//..//data/";
        public Crowd[] ReadParquetCrowdFile()
        {
            // open file stream
            using (Stream fileStream = File.OpenRead(AppDomain.CurrentDomain.BaseDirectory + pathPrefix + "crowd/month=2020-11.parquet"))
            {
                // open parquet file reader
                using (var parquetReader = new ParquetReader(fileStream))
                {
                    // get file schema (available straight after opening parquet reader)
                    // however, get only data fields as only they contain data values
                    DataField[] dataFields = parquetReader.Schema.GetDataFields();

                    // enumerate through row groups in this file
                    // for (int i = 0; i < parquetReader.RowGroupCount; i++)
                    {
                        // create row group reader
                        using (ParquetRowGroupReader groupReader = parquetReader.OpenRowGroupReader(0))
                        {
                            // read all columns inside each row group (you have an option to read only
                            // required columns if you need to.
                            Crowd[] crowds = new Crowd[groupReader.RowCount];

                            Parquet.Data.DataColumn[] columns = dataFields.Select(groupReader.ReadColumn).ToArray();

                            // .Data member contains a typed array of column data you can cast to the type of the column
                            Parquet.Data.DataColumn oidColumn = columns[0];
                            ulong[] oidColumns = (ulong[])oidColumn.Data;

                            Parquet.Data.DataColumn mac = columns[2];
                            string[] macs = (string[])mac.Data;

                            Parquet.Data.DataColumn addedOnTick = columns[6];
                            long?[] addedOnTicks = (long?[])addedOnTick.Data;

                            Parquet.Data.DataColumn oidFrame = columns[7];
                            string[] oidFrames = (string[])oidFrame.Data;

                            for (int i = 0; i < crowds.Length; i++)
                            {
                                crowds[i] = new Crowd(oidColumns[i], macs[i], addedOnTicks[i], oidFrames[i] != null ? ulong.Parse(oidFrames[i]) : 0);
                            }
                            return crowds;
                        }
                    }
                }
            }
        }

        public PlayerLog[] ReadParquetPlayerLogFile()
        {
            // open file stream
            using (Stream fileStream = File.OpenRead(AppDomain.CurrentDomain.BaseDirectory + pathPrefix + "player_log/month=2020-11.parquet"))
            {
                // open parquet file reader
                using (var parquetReader = new ParquetReader(fileStream))
                {
                    // get file schema (available straight after opening parquet reader)
                    // however, get only data fields as only they contain data values
                    DataField[] dataFields = parquetReader.Schema.GetDataFields();

                    // enumerate through row groups in this file
                    // for (int i = 0; i < parquetReader.RowGroupCount; i++)
                    {
                        // create row group reader
                        using (ParquetRowGroupReader groupReader = parquetReader.OpenRowGroupReader(0))
                        {
                            // read all columns inside each row group (you have an option to read only
                            // required columns if you need to.
                            PlayerLog[] playerLogs = new PlayerLog[groupReader.RowCount];

                            Parquet.Data.DataColumn[] columns = dataFields.Select(groupReader.ReadColumn).ToArray();

                            // .Data member contains a typed array of column data you can cast to the type of the column
                            Parquet.Data.DataColumn oidColumn = columns[0];
                            string[] oidColumns = (string[])oidColumn.Data;

                            Parquet.Data.DataColumn billingDate = columns[2];
                            DateTimeOffset[] billingDates = (DateTimeOffset[])billingDate.Data;

                            Parquet.Data.DataColumn mediaItemName = columns[3];
                            string[] mediaItemNames = (string[])mediaItemName.Data;

                            Parquet.Data.DataColumn duration = columns[4];
                            decimal?[] durations = (decimal?[])duration.Data;

                            Parquet.Data.DataColumn dateTimeInTick = columns[12];
                            long[] dateTimesInTick = (long[])dateTimeInTick.Data;

                            Parquet.Data.DataColumn dateTimeOutTick = columns[12];
                            long[] dateTimesOutTick = (long[])dateTimeOutTick.Data;

                            for (int i = 0; i < playerLogs.Length; i++)
                            {
                                playerLogs[i] = new PlayerLog(oidColumns[i] != null ? ulong.Parse(oidColumns[i]) : 0, billingDates[i], mediaItemNames[i], (int)durations[i], dateTimesInTick[i], dateTimesOutTick[i]);
                            }
                            return playerLogs;
                        }
                    }
                }
            }
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainClick(object sender, RoutedEventArgs e)
        {
            MainButton.Content = "Ok!";
            List<int> a = new List<int>();

            PlayerLog[] playerLogs = ReadParquetPlayerLogFile();
            playerLogDataGrid.Columns.Clear();
            playerLogDataGrid.ItemsSource = playerLogs;

            Crowd[] crowds = ReadParquetCrowdFile();
            crowdDataGrid.Columns.Clear();
            crowdDataGrid.ItemsSource = crowds;

            var results = playerLogs.Join(crowds,
              pl => pl.OidColumn,
              cr => cr.OidFrame,
              (playerLog, crowd) => new
              {
                  playerLog.BillingDate,
                  playerLog.MediaItemName,
                  playerLog.DateTimeInTick,
                  playerLog.DateTimeOutTick,
                  crowd.Mac,
                  crowd.AddedOnTick
              }
            ).ToList();

            int totalContacts = results.Count;
            DateTimeOffset daysMin = results.Min(x => x.BillingDate);
            DateTimeOffset daysMax = results.Max(x => x.BillingDate);
            int daysInterval = (daysMax - daysMin).Days + 1;

            List<int> dayContacts = new List<int>();
            string dayContactsData = string.Empty;
            for (int i = 0; i < daysInterval; i++)
            {
                int currentDayOTS = results.Where(
                    x => x.BillingDate.Ticks > daysMin.AddDays(i - 1).Ticks
                    && x.BillingDate.Ticks < daysMin.AddDays(i + 1).Ticks
                ).ToList().Count;
                dayContacts.Add(currentDayOTS);
                listBoxPerDayOTS.Items.Add(i + 1 + ".10.2021: " + currentDayOTS);
            }

            labelContacts.Content = totalContacts;
            labelInterval.Content = daysInterval + " дней";
            labelAverageOTS.Content = totalContacts / daysInterval;
        }
    }
}
