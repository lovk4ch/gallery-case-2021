using System.Windows;
using System.Linq;
using System.Collections.Generic;
using Parquet.Data;
using System.IO;
using System;
using Parquet;

namespace gallery_case_2021
{
    public partial class MainWindow : Window
    {
        public Crowd[] ReadParquetCrowdFile()
        {
            // open file stream
            using (Stream fileStream = File.OpenRead(AppDomain.CurrentDomain.BaseDirectory + "crowd/month=2020-11.parquet"))
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

                            DataColumn[] columns = dataFields.Select(groupReader.ReadColumn).ToArray();
                            
                            // .Data member contains a typed array of column data you can cast to the type of the column
                            DataColumn oidColumn = columns[0];
                            ulong[] oidColumns = (ulong[])oidColumn.Data;

                            DataColumn mac = columns[2];
                            string[] macs = (string[])mac.Data;

                            DataColumn addedOnTick = columns[6];
                            long?[] addedOnTicks = (long?[])addedOnTick.Data;

                            DataColumn oidFrame = columns[7];
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
            using (Stream fileStream = File.OpenRead(AppDomain.CurrentDomain.BaseDirectory + "player_log/month=2020-11.parquet"))
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

                            DataColumn[] columns = dataFields.Select(groupReader.ReadColumn).ToArray();

                            // .Data member contains a typed array of column data you can cast to the type of the column
                            DataColumn oidColumn = columns[0];
                            string[] oidColumns = (string[])oidColumn.Data;

                            DataColumn mediaItemName = columns[3];
                            string[] mediaItemNames = (string[])mediaItemName.Data;

                            DataColumn duration = columns[4];
                            decimal?[] durations = (decimal?[])duration.Data;

                            DataColumn dateTimeInTick = columns[12];
                            long[] dateTimesInTick = (long[])dateTimeInTick.Data;

                            DataColumn dateTimeOutTick = columns[12];
                            long[] dateTimesOutTick = (long[])dateTimeOutTick.Data;

                            for (int i = 0; i < playerLogs.Length; i++)
                            {
                                playerLogs[i] = new PlayerLog(oidColumns[i] != null ? ulong.Parse(oidColumns[i]) : 0, mediaItemNames[i], (int)durations[i], dateTimesInTick[i], dateTimesOutTick[i]);
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
            MessageBox.Show(ReadParquetPlayerLogFile()[1].mediaItemName);
        }
    }
}
