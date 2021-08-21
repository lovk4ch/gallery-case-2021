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
        public void ReadParquetFile()
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
                    for (int i = 0; i < parquetReader.RowGroupCount; i++)
                    {
                        // create row group reader
                        using (ParquetRowGroupReader groupReader = parquetReader.OpenRowGroupReader(i))
                        {
                            // read all columns inside each row group (you have an option to read only
                            // required columns if you need to.
                            DataColumn[] columns = dataFields.Select(groupReader.ReadColumn).ToArray();

                            // get first column, for instance
                            DataColumn firstColumn = columns[0];

                            // .Data member contains a typed array of column data you can cast to the type of the column
                            Array data = firstColumn.Data;
                            ulong[] OidColumns = (ulong[])data;
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
            MainButton.Content = "1111";
            List<int> a = new List<int>();
            ReadParquetFile();
        }
    }
}
