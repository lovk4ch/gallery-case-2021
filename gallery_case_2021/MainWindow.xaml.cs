using System;
using System.Windows;
using System.Windows.Controls;

namespace gallery_case_2021
{
    public partial class MainWindow : Window
    {
        public const string ERROR_DATE_EXCESS = "Дата начала задана позже даты окончания рекламной кампании. Выберите другой интервал.";
        public const string ERROR_DATE_NOT_SELECT = "Выберите корректную дату начала и окончания показов";
        public const string ADV_WAS_PLANNED = "Your ad will be launched soon and everyone will know about your product!";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void CheckClick(object sender, RoutedEventArgs e)
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

            float k = 0;

            player1_data.SetData(dateTimeStart.SelectedDate, dateTimeEnd.SelectedDate, out float k1);
            k += k1;
            player2_data.SetData(dateTimeStart.SelectedDate, dateTimeEnd.SelectedDate, out float k2);
            k += k2;
            k /= 2f;

            idleCoefficient.Content = (1 - k) * 100 + "%";

            int views = (int)Math.Floor(72 * k);
            viewsPerHourLabel.Content = views;

            userViewsSlider.IsEnabled = true;
            userViewsSlider.Value = views;
            
            MainButton.IsEnabled = true;
        }

        private void MainClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(ADV_WAS_PLANNED);
        }

        private void SliderValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (userViewsLabel != null)
                userViewsLabel.Content = (int)userViewsSlider.Value;
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