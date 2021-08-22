using System;
using System.Windows;
using System.Windows.Controls;

namespace gallery_case_2021
{
    public partial class MainWindow : Window
    {
        public const string ERROR_DATE_EXCESS = "Дата начала задана позже даты окончания рекламной кампании. Выберите другой интервал.";
        public const string ERROR_DATE_NOT_SELECT = "Выберите корректную дату начала и окончания показов";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainClick(object sender, RoutedEventArgs e)
        {
            if (dateTimeStart.SelectedDate == null || dateTimeEnd.SelectedDate == null)
            {
                MessageBox.Show(ERROR_DATE_NOT_SELECT);
                return;
            }
            if (dateTimeStart.SelectedDate > dateTimeEnd.SelectedDate)
            {
                MessageBox.Show(ERROR_DATE_EXCESS);
                return;
            }

            player1_data.SetData(dateTimeStart.SelectedDate, dateTimeEnd.SelectedDate);
            player2_data.SetData(dateTimeStart.SelectedDate, dateTimeEnd.SelectedDate);
        }

        private void StartDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime selectedDate = (DateTime)((DatePicker)sender).SelectedDate;
            DateTime maxDate = new DateTime(selectedDate.Year, selectedDate.Month, DateTime.DaysInMonth(selectedDate.Year, selectedDate.Month));

            if (dateTimeEnd.SelectedDate > maxDate)
                dateTimeEnd.SelectedDate = maxDate;

            dateTimeEnd.DisplayDateEnd = maxDate;
        }
    }
}