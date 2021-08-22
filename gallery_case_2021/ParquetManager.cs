using Parquet;
using Parquet.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace gallery_case_2021
{
    class ParquetManager
    {
        private static string path = AppDomain.CurrentDomain.BaseDirectory + "..//..//data/{0}/player={1}/month={2}-{3}.parquet";
        private const int maxRowsCount = 1000000;

        private static PlayerLog[] ReadParquetPlayerLogFile(int playerId, int year, int month)
        {
            try
            {
                // open file stream
                using (Stream fileStream = File.OpenRead(string.Format(path, "player_log", playerId, year, month)))
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
                                PlayerLog[] playerLogs = new PlayerLog[Math.Min(groupReader.RowCount, maxRowsCount)];

                                DataColumn[] columns = dataFields.Select(groupReader.ReadColumn).ToArray();

                                // .Data member contains a typed array of column data you can cast to the type of the column
                                DataColumn oidColumn = columns[0];
                                string[] oidColumns = (string[])oidColumn.Data;

                                DataColumn billingDate = columns[2];
                                DateTimeOffset[] billingDates = (DateTimeOffset[])billingDate.Data;

                                DataColumn mediaItemName = columns[3];
                                string[] mediaItemNames = (string[])mediaItemName.Data;

                                DataColumn duration = columns[4];
                                decimal?[] durations = (decimal?[])duration.Data;

                                for (int i = 0; i < playerLogs.Length; i++)
                                {
                                    playerLogs[i] = new PlayerLog(oidColumns[i] != null ? ulong.Parse(oidColumns[i]) : 0, billingDates[i], mediaItemNames[i], (int)durations[i]);
                                }
                                fileStream.Dispose();
                                return playerLogs;
                            }
                        }
                    }
                }
            }
            catch (FileNotFoundException)
            {
                return null;
            }
        }
        private static Crowd[] ReadParquetCrowdFile(int playerId, int year, int month)
        {
            try
            {
                // open file stream
                using (Stream fileStream = File.OpenRead(string.Format(path, "crowd", playerId, year, month)))
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
                                Crowd[] crowds = new Crowd[Math.Min(groupReader.RowCount, maxRowsCount)];

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
            catch (FileNotFoundException)
            {
                return null;
            }
        }

        public static float GetMonthlyIdle(int month)
        {
            List<float> idles = new List<float>()
            {
                0.4f, // January
                0.5f, // February
                0.7f, // March
                0.7f, // April
                0.6f, // May
                0.7f, // June
                0.5f, // July
                0.6f, // August
                0.7f, // September
                0.7f, // October
                0.7f, // November
                0.7f  // December
            };
            return idles[month - 1];
        }
        public static List<PlayerData> GetPlayerData(int playerId, DateTime startDate, DateTime endDate)
        {
            int year = startDate.Year - 1;
            int month = startDate.Month;
            int startDay = startDate.Day;
            int endDay = endDate.Day;

            Crowd[] crowds = ReadParquetCrowdFile(playerId, year, month);
            PlayerLog[] playerLogs = ReadParquetPlayerLogFile(playerId, year, month);

            if (crowds == null || playerLogs == null)
                return null;

            List<PlayerData> playerData = playerLogs.Join(crowds,
              pl => pl.OidColumn,
              cr => cr.OidFrame,
              (playerLog, crowd) => new PlayerData(
                  playerLog.BillingDate,
                  playerLog.MediaItemName,
                  playerLog.Duration,
                  crowd.Mac,
                  crowd.AddedOnTick
              )
            )
            .Where(x => x.BillingDate.Day >= startDay && x.BillingDate.Day <= endDay)
            .ToList();
            return playerData;
        }
    }
}