using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media.Animation;
using System.Text.RegularExpressions;

namespace TimeTracker
{
    //TODO: add notification when added item using template from isostotxts
    // Split ProjectName into ProjNum and DisplayName
    // Fix lock on close
    // Fix date in timesheets

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        Config config;
        SavingTimesheet savingTimsheet;
        WindowViewModel context;

        #region Constructor
        public MainWindow()
        {
            InitializeComponent();
            context = new WindowViewModel();
            DataContext = context;
        }

        #endregion

        #region Events

        /*public void btnClearText_Click(object sender, RoutedEventArgs e)
          {
              SearchTermTextBox.Text = string.Empty;
          }*/

        public void CollapseButton_Click(object sender, RoutedEventArgs e)
        {
            Image content = (Image)(((Button)sender).Content);
            FrameworkElement element = (FrameworkElement)FindName(((Button)sender).Tag.ToString());
            if (content.Source == (BitmapImage)FindResource("DownTriangle"))
            {
                Storyboard sb = new Storyboard();

                DoubleAnimation anim = new DoubleAnimation(element.MinHeight, new Duration(new TimeSpan(0, 0, 0, 0, 700)));
                Storyboard.SetTargetProperty(anim, new PropertyPath("Height"));
                anim.DecelerationRatio = 0.8;

                sb.Children.Add(anim);

                sb.Begin(element);

                //NewItemGroupBox.Height = NewItemGroupBox.MinHeight;
                content.Source = (BitmapImage)FindResource("RightTriangle");
            }
            else
            {
                Storyboard sb = new Storyboard();

                DoubleAnimation anim = new DoubleAnimation(element.MaxHeight, new Duration(new TimeSpan(0, 0, 0, 0, 700)));
                Storyboard.SetTargetProperty(anim, new PropertyPath("Height"));
                anim.DecelerationRatio = 0.8;

                sb.Children.Add(anim);

                sb.Begin(element);

                //NewItemGroupBox.Height = NewItemGroupBox.MaxHeight;
                content.Source = (BitmapImage)FindResource("DownTriangle");
            }
        }

        private void SaveProjectTimeData(object sender, EventArgs e)
        {
            ((WindowViewModel)DataContext).ProjectTimeDataCollectionViewModel.SaveAllData();
        }

        private void RemoveItemButtonClick(object sender, RoutedEventArgs e)
        {
            int animationDurationMs = 000;

            // Set RemoveItemDelayMs in ProjectTimeDataCollectionViewModel so that it knows how long to delay before removing item from collection
            ((WindowViewModel)DataContext).ProjectTimeDataCollectionViewModel.RemoveItemDelayMs = animationDurationMs;

            Border border = (Border)((Grid)((FrameworkElement)sender).Parent).Parent;

            Storyboard sb = new Storyboard();

            DoubleAnimation anim = new DoubleAnimation(0, new Duration(new TimeSpan(0, 0, 0, 0, animationDurationMs)));
            Storyboard.SetTargetProperty(anim, new PropertyPath("Opacity"));
            anim.DecelerationRatio = 0.8;

            sb.Children.Add(anim);

            sb.Begin(border);
        }

        private void TextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = (new Regex("[^0-9.]+")).IsMatch(e.Text);
        }

        /*private void ResizeButton_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;

            if (((Image)btn.Content).Source == (BitmapImage)FindResource("Maximize"))
            {
                // Set main scroll viewer to collapsed
                MainScrollViewer.Visibility = Visibility.Collapsed;

                // Set maximized border to visible
                ProjectItem.Visibility = Visibility.Visible;

                // Set data context to button's item
                ProjectItem.DataContext = btn.DataContext;
            }

            if (((Image)btn.Content).Source == (BitmapImage)FindResource("Restore"))
            {
                // Set main scroll viewer to collapsed
                MainScrollViewer.Visibility = Visibility.Visible;

                // Set maximized border to visible
                ProjectItem.Visibility = Visibility.Collapsed;
            }

            ((WindowViewModel)DataContext).ProjectTimeDataCollectionViewModel.SaveProjectTimeData();
        }*/

        #endregion

        private void DayDisplay_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            ((Border)FindName("MondayDisplay")).GetBindingExpression(Border.ToolTipProperty).UpdateTarget();
            ((Border)FindName("TuesdayDisplay")).GetBindingExpression(Border.ToolTipProperty).UpdateTarget();
            ((Border)FindName("WednesdayDisplay")).GetBindingExpression(Border.ToolTipProperty).UpdateTarget();
            ((Border)FindName("ThursdayDisplay")).GetBindingExpression(Border.ToolTipProperty).UpdateTarget();
            ((Border)FindName("FridayDisplay")).GetBindingExpression(Border.ToolTipProperty).UpdateTarget();
        }

        private void Config_Click(object sender, RoutedEventArgs e)
        {
            if (!config.IsVisible)
            {
                config.Visibility = Visibility.Visible;
            }
        }

        private void mainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            config = new Config(this);
            config.DataContext = context;
            config.Visibility = Visibility.Collapsed;
            savingTimsheet = new SavingTimesheet(this);
            savingTimsheet.DataContext = context;
            //savingTimsheet.Visibility = Visibility.Collapsed;
        }
    }
}
