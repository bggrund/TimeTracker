using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Serialization;
using System.IO;
using System.ComponentModel;

namespace PasswordHints
{
   /// <summary>
   /// Interaction logic for MainWindow.xaml
   /// </summary>
   public partial class MainWindow : Window
   {
      private string placeholderText = "Search here...";
      private string accountDataFilePath = AppDomain.CurrentDomain.BaseDirectory + "AccountData.xml";
      //private string customDataPathFile = AppDomain.CurrentDomain.BaseDirectory + "Path.txt";
      private XmlSerializer serializer = new XmlSerializer(typeof(ObservableCollection<AccountData>));
      private CollectionViewSource accountCollectionSource = new CollectionViewSource();
      private ICollectionView source;
      private ObservableCollection<AccountData> accountDataList = new ObservableCollection<AccountData>();
      private AccountData newAccountData = new AccountData();

      public MainWindow()
      {
         InitializeComponent();

         searchBox.GotFocus += (s, e) => searchBox.Text = searchBox.Text == placeholderText ? "" : searchBox.Text;
         searchBox.LostFocus += (s, e) => searchBox.Text = string.IsNullOrWhiteSpace(searchBox.Text) ? placeholderText : searchBox.Text;

         newAccountBox.DataContext = newAccountData;

         if (!File.Exists(accountDataFilePath))
         {
            saveAccountData();
         }

         loadAccountData();

         source = CollectionViewSource.GetDefaultView(accountDataList);
         source.SortDescriptions.Add(new SortDescription("Website", ListSortDirection.Ascending));
         source.Filter = filter;
         credentialItemsControl.ItemsSource = source;

         searchBox.Focus();
      }

      private bool filter(object obj)
      {
         if (string.IsNullOrWhiteSpace(searchBox.Text) || searchBox.Text == placeholderText)
         {
            return true;
         }

         AccountData item = (AccountData)obj;
         if (cbWebsite.IsChecked == true && item.Website.IndexOf(searchBox.Text, StringComparison.OrdinalIgnoreCase) >= 0)
         {
            return true;
         }
         else if (cbEmail.IsChecked == true && item.Email.IndexOf(searchBox.Text, StringComparison.OrdinalIgnoreCase) >= 0)
         {
            return true;
         }
         else if (cbUsername.IsChecked == true && item.Username.IndexOf(searchBox.Text, StringComparison.OrdinalIgnoreCase) >= 0)
         {
            return true;
         }
         else if (cbPasswordHint.IsChecked == true && item.PasswordHint.IndexOf(searchBox.Text, StringComparison.OrdinalIgnoreCase) >= 0)
         {
            return true;
         }
         return false;
      }
      
      private bool keyStroke = false;
      private bool inAsync = false;
      private void SearchBox_KeyUp(object sender, KeyEventArgs e)
      {
         if (!searchBox.IsLoaded)
         {
            return;
         }

         keyStroke = true;
         if (!inAsync)
         {
            updateFilterAsync();
         }
      }
      
      private async Task updateFilterAsync()
      {
         inAsync = true;
         keyStroke = false;

         await Task.Delay(500);

         if (keyStroke)
         {
            inAsync = false;
            return;
         }

         updateFilter();
         inAsync = false;
      }

      private void updateFilter()
      {
         source.Filter = filter;
      }

      private void btnAdd_Click(object sender, RoutedEventArgs e)
      {
         accountDataList.Add(new AccountData(txtInputWebsite.Text, txtInputEmail.Text, txtInputUsername.Text, txtInputPasswordHint.Text));

         updateFilter();

         saveAccountData();
      }

      private void saveAccountData()
      {
         using (FileStream f = new FileStream(accountDataFilePath, FileMode.Create))
         {
            serializer.Serialize(f, accountDataList);
         }
      }

      private void loadAccountData()
      {
         using (FileStream f = new FileStream(accountDataFilePath, FileMode.Open))
         {
            accountDataList = (ObservableCollection<AccountData>)serializer.Deserialize(f);
         }
      }

      private void btnRemove_Click(object sender, RoutedEventArgs e)
      {
         accountDataList.Remove((AccountData)((FrameworkElement)sender).DataContext);

         updateFilter();

         saveAccountData();
      }

      private void searchField_CheckedChanged(object sender, RoutedEventArgs e)
      {
         if (!((FrameworkElement)sender).IsLoaded)
         {
            return;
         }
         updateFilter();
      }

      private void btnClearText_Click(object sender, RoutedEventArgs e)
      {
         searchBox.Text = string.Empty;
         updateFilter();
         searchBox.Focus();
      }
   }

   public class AccountData
   {
      public string Website { get; set; }
      public string Email { get; set; }
      public string Username { get; set; }
      public string PasswordHint { get; set; }

      public AccountData()
      {
         Website = Email = Username = PasswordHint = "";
      }

      public AccountData(string website, string email, string username, string passwordHint)
      {
         Website = website;
         Email = email;
         Username = username;
         PasswordHint = passwordHint;
      }
   }
}
