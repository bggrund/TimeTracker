using Microsoft.Win32;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using Microsoft.Office.Interop;
using System.Runtime.InteropServices;
using System.Xml;

namespace TimeTracker
{
    // TODO:    Add item notification
    //          Let user choose account data file path when the one in ProjectTimeDataFilePathFilePath isn't found
    [AddINotifyPropertyChangedInterface]
    public class ProjectTimeDataCollectionViewModel
    {

        #region Private Members

        //private string searchText, placeholderText = "Search here...";
        //private bool searchWebsite, searchEmail, searchUsername, searchPasswordHint;
        //private BackgroundWorker bwUpdateFilter = new BackgroundWorker();
        //private int delayFilterUpdate = 0;
        private decimal incrementValue = 1;

        private string mProjectTimeDataFilePath;
        private string mTxtProjectTimeDataFilePath;
        private string mTxtProjectTimeDataDailyReportFilePath;

        #endregion

        #region Static Members

        public static readonly string DefaultProjectTimeDataPath = AppDomain.CurrentDomain.BaseDirectory + "ProjectTimeData.xml";
        public static readonly string DefaultTimesheetsDir = AppDomain.CurrentDomain.BaseDirectory;
        /// <summary>
        /// File path of the file that stores the path of the account data file
        /// </summary>
        public static readonly string ProgramDataPath = AppDomain.CurrentDomain.BaseDirectory + "FilePathData.xml";

        #endregion

        #region Public Properties

        public string ProjectTimeDataPath { get { return mProjectTimeDataFilePath; } set { mProjectTimeDataFilePath = value; mTxtProjectTimeDataFilePath = value.Replace(".xml", ".txt"); mTxtProjectTimeDataDailyReportFilePath = value.Replace(".xml", "_DailyReport.txt"); } }
        public string TimesheetsDir { get; set; }

        public string Name { get; set; }
        public string ID { get; set; }

        public bool ItemAdded { get; set; }
        private bool mSavingTimesheet = false;
        public bool SavingTimesheet { get { return mSavingTimesheet; } set { mSavingTimesheet = value; CommandManager.InvalidateRequerySuggested(); } }

        public decimal MondayTotal { get; set; }
        public decimal TuesdayTotal { get; set; }
        public decimal WednesdayTotal { get; set; }
        public decimal ThursdayTotal { get; set; }
        public decimal FridayTotal { get; set; }

        // Display charged time for each day - used in daily total tooltips
        public string MondayDisplay { get { string lines = ""; foreach (ProjectTimeDataViewModel item in Items) { if (item.MondayTime > 0) lines += item.DisplayName + ": " + item.MondayTime + Environment.NewLine; } return lines.TrimEnd(Environment.NewLine.ToCharArray()); } }
        public string TuesdayDisplay { get { string lines = ""; foreach (ProjectTimeDataViewModel item in Items) { if (item.TuesdayTime > 0) lines += item.DisplayName + ": " + item.TuesdayTime + Environment.NewLine; } return lines.TrimEnd(Environment.NewLine.ToCharArray()); } }
        public string WednesdayDisplay { get { string lines = ""; foreach (ProjectTimeDataViewModel item in Items) { if (item.WednesdayTime > 0) lines += item.DisplayName + ": " + item.WednesdayTime + Environment.NewLine; } return lines.TrimEnd(Environment.NewLine.ToCharArray()); } }
        public string ThursdayDisplay { get { string lines = ""; foreach (ProjectTimeDataViewModel item in Items) { if (item.ThursdayTime > 0) lines += item.DisplayName + ": " + item.ThursdayTime + Environment.NewLine; } return lines.TrimEnd(Environment.NewLine.ToCharArray()); } }
        public string FridayDisplay { get { string lines = ""; foreach (ProjectTimeDataViewModel item in Items) { if (item.FridayTime > 0) lines += item.DisplayName + ": " + item.FridayTime + Environment.NewLine; } return lines.TrimEnd(Environment.NewLine.ToCharArray()); } }

        /// <summary>
        /// List of project time data items
        /// </summary>
        public ObservableCollection<ProjectTimeDataViewModel> Items { get; set; }

        /// <summary>
        /// Collection view for sorting and filtering <see cref="Items"/>
        /// </summary>
        public ICollectionView CollectionView { get; set; }

        /// <summary>
        /// Command that removes account data item from list
        /// </summary>
        public ICommand RemoveItemCommand { get; set; }

        /// <summary>
        /// Command that resets all time data in all projects
        /// </summary>
        public ICommand ResetAllCommand { get; set; }

        /// <summary>
        /// Command that calls <see cref="ResetAllAndLogTime(object)"/>
        /// </summary>
        public ICommand ResetAllAndLogTimeCommand { get; set; }

        /// <summary>
        /// Command that calls <see cref="ChooseProjectTimeDataFile"/>
        /// </summary>
        public ICommand ChooseProjectTimeDataFileCommand { get; set; }

        /// <summary>
        /// Command that calls <see cref="ChooseTimesheetsDir"/>
        /// </summary>
        public ICommand ChooseTimesheetsDirCommand { get; set; }

        /// <summary>
        /// Command that calls <see cref="OpenProjectTimeDataFileLocation"/>
        /// </summary>
        public ICommand OpenProjectTimeDataFileLocationCommand { get; set; }

        /// <summary>
        /// Command that calls <see cref="SaveTimesheet(object)"/>
        /// </summary>
        public ICommand SaveTimesheetCommand { get; set; }
        public ICommand IncrementMondayCommand { get; set; }
        public ICommand DecrementMondayCommand { get; set; }
        public ICommand IncrementTuesdayCommand { get; set; }
        public ICommand DecrementTuesdayCommand { get; set; }
        public ICommand IncrementWednesdayCommand { get; set; }
        public ICommand DecrementWednesdayCommand { get; set; }
        public ICommand IncrementThursdayCommand { get; set; }
        public ICommand DecrementThursdayCommand { get; set; }
        public ICommand IncrementFridayCommand { get; set; }
        public ICommand DecrementFridayCommand { get; set; }

        /*
        public string SearchText
          {
              get
              {
                  return searchText;
              }
              set
              {
                  searchText = value;
                  //updateFilterDelayed();
              }
          }

          public bool SearchWebsite
          {
              get
              {
                  return searchWebsite;
              }
              set
              {
                  searchWebsite = value;
                  //updateFilterDelayed();
              }
          }

          public bool SearchEmail
          {
              get
              {
                  return searchEmail;
              }
              set
              {
                  searchEmail = value;
                  //updateFilterDelayed();
              }
          }

          public bool SearchUsername
          {
              get
              {
                  return searchUsername;
              }
              set
              {
                  searchUsername = value;
                  //updateFilterDelayed();
              }
          }

          public bool SearchPasswordHint
          {
              get
              {
                  return searchPasswordHint;
              }
              set
              {
                  searchPasswordHint = value;
                  //updateFilterDelayed();
              }
          }
          */
        public int RemoveItemDelayMs { get; set; } = 0;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public ProjectTimeDataCollectionViewModel()
        {
            // Initialize account data file path
            InitializeProgramData();

            // Initialize account data items
            InitializeProjectTimeDataItems();

            // Initialze commands
            RemoveItemCommand = new RelayCommand(RemoveItem);
            ResetAllCommand = new RelayCommand(ResetAll);
            ResetAllAndLogTimeCommand = new RelayCommand(ResetAllAndLogTime);
            IncrementMondayCommand = new RelayCommand(IncrementMonday);
            IncrementTuesdayCommand = new RelayCommand(IncrementTuesday);
            IncrementWednesdayCommand = new RelayCommand(IncrementWednesday);
            IncrementThursdayCommand = new RelayCommand(IncrementThursday);
            IncrementFridayCommand = new RelayCommand(IncrementFriday);
            DecrementMondayCommand = new DecrementMondayCommand(DecrementMonday);
            DecrementTuesdayCommand = new DecrementTuesdayCommand(DecrementTuesday);
            DecrementWednesdayCommand = new DecrementWednesdayCommand(DecrementWednesday);
            DecrementThursdayCommand = new DecrementThursdayCommand(DecrementThursday);
            DecrementFridayCommand = new DecrementFridayCommand(DecrementFriday);
            ChooseProjectTimeDataFileCommand = new RelayCommand(ChooseProjectTimeDataFile);
            ChooseTimesheetsDirCommand = new RelayCommand(ChooseTimesheetsDir);
            OpenProjectTimeDataFileLocationCommand = new RelayCommand(OpenProjectTimeDataFileLocation);
            SaveTimesheetCommand = new SaveTimesheetCommand(SaveTimesheetAsync);

            // Initialize Properties
            MondayTotal = TuesdayTotal = WednesdayTotal = ThursdayTotal = FridayTotal = 0;

            foreach (ProjectTimeDataViewModel item in Items)
            {
                MondayTotal += item.MondayTime;
                TuesdayTotal += item.TuesdayTime;
                WednesdayTotal += item.WednesdayTime;
                ThursdayTotal += item.ThursdayTime;
                FridayTotal += item.FridayTime;
            }
            //SearchWebsite = SearchEmail = SearchUsername = SearchPasswordHint = true;

            // Initialize background worker for filtering
            //bwUpdateFilter.DoWork += BwUpdateFilter_DoWork;
            // bwUpdateFilter.RunWorkerCompleted += BwUpdateFilter_RunWorkerCompleted;
        }

        #endregion

        #region Commands and Helpers

        /// <summary>
        /// Removes account data item from <see cref="Items"/>
        /// </summary>
        /// <param name="item"><see cref="ProjectTimeDataViewModel"/> to remove</param>
        private async void RemoveItem(object item)
        {
            await Task.Delay(RemoveItemDelayMs);

            ProjectTimeDataViewModel p = item as ProjectTimeDataViewModel;

            MondayTotal -= p.MondayTime;
            TuesdayTotal -= p.TuesdayTime;
            WednesdayTotal -= p.WednesdayTime;
            ThursdayTotal -= p.ThursdayTime;
            FridayTotal -= p.FridayTime;

            Items.Remove(item as ProjectTimeDataViewModel);

            SaveProjectTimeData();
        }

        /// <summary>
        /// Adds account data item to <see cref="Items"/>
        /// </summary>
        /// <param name="item"><see cref="ProjectTimeDataViewModel"/> to add</param>
        public void AddItem(string displayName, string chargeNumber)
        {
            ItemAdded = false;

            try
            {
                ProjectTimeDataViewModel projectTimeData = new ProjectTimeDataViewModel() { DisplayName = displayName, ChargeNumber = chargeNumber };
                Items.Add(projectTimeData);

                SaveProjectTimeData();

                ItemAdded = true;
            }
            catch { }
        }

        /// <summary>
        /// Resets all time data in all projects
        /// </summary>
        /// <param name="item"><see cref="ProjectTimeDataViewModel"/> to add</param>
        private void ResetAll(object o)
        {
            foreach (ProjectTimeDataViewModel item in Items)
            {
                item.ClearTime();
            }
            SaveProjectTimeData();

            MondayTotal = TuesdayTotal = WednesdayTotal = ThursdayTotal = FridayTotal = 0;
        }

        /// <summary>
        /// Resets all time data in all projects and logs time
        /// </summary>
        /// <param name="item"><see cref="ProjectTimeDataViewModel"/> to add</param>
        private void ResetAllAndLogTime(object o)
        {
            foreach (ProjectTimeDataViewModel item in Items)
            {
                item.ClearAndLogTime();
            }
            SaveProjectTimeData();

            MondayTotal = TuesdayTotal = WednesdayTotal = ThursdayTotal = FridayTotal = 0;
        }

        public void IncrementMonday(object item)
        {
            ProjectTimeDataViewModel projectTimeData = item as ProjectTimeDataViewModel;
            projectTimeData.MondayTime += incrementValue;
            SaveProjectTimeData();

            MondayTotal += incrementValue;
        }
        public void DecrementMonday(object item)
        {
            ProjectTimeDataViewModel projectTimeData = item as ProjectTimeDataViewModel;
            projectTimeData.MondayTime -= incrementValue;
            SaveProjectTimeData();

            MondayTotal -= incrementValue;
        }
        public void IncrementTuesday(object item)
        {
            ProjectTimeDataViewModel projectTimeData = item as ProjectTimeDataViewModel;
            projectTimeData.TuesdayTime += incrementValue;
            SaveProjectTimeData();

            TuesdayTotal += incrementValue;
        }
        public void DecrementTuesday(object item)
        {
            ProjectTimeDataViewModel projectTimeData = item as ProjectTimeDataViewModel;
            projectTimeData.TuesdayTime -= incrementValue;
            SaveProjectTimeData();

            TuesdayTotal -= incrementValue;
        }
        public void IncrementWednesday(object item)
        {
            ProjectTimeDataViewModel projectTimeData = item as ProjectTimeDataViewModel;
            projectTimeData.WednesdayTime += incrementValue;
            SaveProjectTimeData();

            WednesdayTotal += incrementValue;
        }
        public void DecrementWednesday(object item)
        {
            ProjectTimeDataViewModel projectTimeData = item as ProjectTimeDataViewModel;
            projectTimeData.WednesdayTime -= incrementValue;
            SaveProjectTimeData();

            WednesdayTotal -= incrementValue;
        }
        public void IncrementThursday(object item)
        {
            ProjectTimeDataViewModel projectTimeData = item as ProjectTimeDataViewModel;
            projectTimeData.ThursdayTime += incrementValue;
            SaveProjectTimeData();


            ThursdayTotal += incrementValue;
        }
        public void DecrementThursday(object item)
        {
            ProjectTimeDataViewModel projectTimeData = item as ProjectTimeDataViewModel;
            projectTimeData.ThursdayTime -= incrementValue;
            SaveProjectTimeData();

            ThursdayTotal -= incrementValue;
        }
        public void IncrementFriday(object item)
        {
            ProjectTimeDataViewModel projectTimeData = item as ProjectTimeDataViewModel;
            projectTimeData.FridayTime += incrementValue;
            SaveProjectTimeData();

            FridayTotal += incrementValue;
        }
        public void DecrementFriday(object item)
        {
            ProjectTimeDataViewModel projectTimeData = item as ProjectTimeDataViewModel;
            projectTimeData.FridayTime -= incrementValue;
            SaveProjectTimeData();

            FridayTotal -= incrementValue;
        }

        /// <summary>
        /// Saves account data as sorted by <see cref="CollectionView"/>
        /// </summary>
        private void SaveProjectTimeData()
        {
            List<ProjectTimeData> sortedProjectTimeData = new List<ProjectTimeData>(Items.Select(i => new ProjectTimeData(i)));
            sortedProjectTimeData.Sort();

            ProjectTimeDataCollection.SaveProjectTimeData(ProjectTimeDataPath, sortedProjectTimeData);

            List<string> lines = new List<string>();

            foreach (ProjectTimeData data in sortedProjectTimeData)
            {
                lines.Add("--" + data.DisplayName + "--");
                lines.Add("M: " + data.MondayTime + " - " + data.MondayNotes);
                lines.Add("T: " + data.TuesdayTime + " - " + data.TuesdayNotes);
                lines.Add("W: " + data.WednesdayTime + " - " + data.WednesdayNotes);
                lines.Add("R: " + data.ThursdayTime + " - " + data.ThursdayNotes);
                lines.Add("F: " + data.FridayTime + " - " + data.FridayNotes);
                lines.Add("Total logged time: " + data.TotalLoggedTime);
                lines.Add("");
            }

            lines = new List<string>();

            lines.Add("==Monday==");
            foreach (ProjectTimeDataViewModel item in Items)
            {
                if (item.MondayTime > 0)
                {
                    lines.Add(item.MondayTime + " - " + item.DisplayName + " - " + item.MondayNotes);
                }
            }
            lines.Add("");
            lines.Add("==Tuesday==");
            foreach (ProjectTimeDataViewModel item in Items)
            {
                if (item.TuesdayTime > 0)
                {
                    lines.Add(item.TuesdayTime + " - " + item.DisplayName + " - " + item.TuesdayNotes);
                }
            }
            lines.Add("");
            lines.Add("==Wednesday==");
            foreach (ProjectTimeDataViewModel item in Items)
            {
                if (item.WednesdayTime > 0)
                {
                    lines.Add(item.WednesdayTime + " - " + item.DisplayName + " - " + item.WednesdayNotes);
                }
            }
            lines.Add("");
            lines.Add("==Thursday==");
            foreach (ProjectTimeDataViewModel item in Items)
            {
                if (item.ThursdayTime > 0)
                {
                    lines.Add(item.ThursdayTime + " - " + item.DisplayName + " - " + item.ThursdayNotes);
                }
            }
            lines.Add("");
            lines.Add("==Friday==");
            foreach (ProjectTimeDataViewModel item in Items)
            {
                if (item.FridayTime > 0)
                {
                    lines.Add(item.FridayTime + " - " + item.DisplayName + " - " + item.FridayNotes);
                }
            }

            File.WriteAllLines(mTxtProjectTimeDataDailyReportFilePath, lines);
            /*
            for(int i = 0; i < lines.Count; i++)
            {
                lines[i] = "<p>" + lines[i] + "</p>";
            }

            File.WriteAllText(mHtmlAccountDataFilePath, "<html><body>");
            File.AppendAllLines(mHtmlAccountDataFilePath, lines);
            File.AppendAllText(mHtmlAccountDataFilePath, "</body></html>");
            */
        }

        private void SaveProgramData()
        {
            ProjectTimeDataCollection.SaveProgramData(ProgramDataPath, new ProgramData(ProjectTimeDataPath, TimesheetsDir, Name, ID));
        }

        public void SaveAllData()
        {
            SaveProjectTimeData();
            SaveProgramData();
        }

        /// <summary>
        /// Presents file dialog for user to choose the account data file from. If the user selects a valid file, this method updates <see cref="ProjectTimeDataPath"/> with selected file, writes the selected file's path to <see cref="ProgramDataPath"/>, and reinitializes the <see cref="Items"/> collection and its <see cref="CollectionView"/>.
        /// </summary>
        /// <param name="parameter">null parameter</param>
        private void ChooseProjectTimeDataFile(object parameter)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.CheckFileExists = true;
            ofd.AddExtension = true;
            ofd.DefaultExt = ".xml";
            ofd.FileName = "ProjectTimeData.xml";
            ofd.Filter = "Xml Files (*.xml)|*.xml";
            ofd.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
            ofd.Multiselect = false;

            if (ofd.ShowDialog() == true)
            {
                ProjectTimeDataPath = ofd.FileName;

                SaveProgramData();

                InitializeProjectTimeDataItems();
            }
        }

        /// <summary>
        /// Presents file dialog for user to choose the path where timesheets are stored. If the user selects a valid path, this method updates <see cref="TimesheetsDir"/> with selected path.
        /// </summary>
        /// <param name="parameter">null parameter</param>
        private void ChooseTimesheetsDir(object parameter)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.CheckFileExists = false;
            //ofd.AddExtension = true;
            //ofd.DefaultExt = ".xml";
            ofd.FileName = "Folder Selection.";
            ofd.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
            ofd.Multiselect = false;

            if (ofd.ShowDialog() == true)
            {
                TimesheetsDir = new FileInfo(ofd.FileName).DirectoryName;

                SaveProgramData();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameter">null parameter</param>
        private void OpenProjectTimeDataFileLocation(object parameter)
        {
            Process.Start((new FileInfo(ProjectTimeDataPath)).Directory.FullName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameter">null parameter</param>
        private async void SaveTimesheetAsync(object parameter)
        {
            SavingTimesheet = true;
            await Task.Run(() => SaveTimesheet());
            SavingTimesheet = false;
        }

        private void SaveTimesheet()
        {
            DateTime sunday = DateTime.Now.AddDays(7 - (int)DateTime.Now.DayOfWeek - (DateTime.Now.DayOfWeek < DayOfWeek.Tuesday ? 7 : 0));
            string xlFilename = TimesheetsDir + "\\Time Entry " + Name[0] + Name.Substring(Name.LastIndexOf(' ') + 1) + "_" + sunday.Year + sunday.Month.ToString("D2") + sunday.Day.ToString("D2") + ".xlsx";

            try
            {
                File.WriteAllBytes(xlFilename, Properties.Resources.Template);
            }
            catch
            {
                return;
            }

            //excel COM object data
            Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(xlFilename, ReadOnly: false, Editable: true);
            Microsoft.Office.Interop.Excel._Worksheet xlWorksheet = xlWorkbook.Sheets[1];
            Microsoft.Office.Interop.Excel.Range xlRange = xlWorksheet.UsedRange;

            int currRow = 6;
            int projCol = 5;
            int monCol = 10;
            int tueCol = 11;
            int wedCol = 12;
            int thuCol = 13;
            int friCol = 14;
            try
            {
                xlWorksheet.Cells[2, 16].Value = sunday.Date.ToShortDateString();
                xlWorksheet.Cells[4, 5].Value = Name;
                xlWorksheet.Cells[4, 10].Value = ID;

                foreach (ProjectTimeDataViewModel p in CollectionView)
                {
                    if (p.TotalTime == 0)
                    {
                        continue;
                    }
                    xlWorksheet.Cells[currRow, projCol].Value = p.ChargeNumber;
                    xlWorksheet.Cells[currRow, monCol].Value = p.MondayTime;
                    xlWorksheet.Cells[currRow, tueCol].Value = p.TuesdayTime;
                    xlWorksheet.Cells[currRow, wedCol].Value = p.WednesdayTime;
                    xlWorksheet.Cells[currRow, thuCol].Value = p.ThursdayTime;
                    xlWorksheet.Cells[currRow, friCol].Value = p.FridayTime;

                    currRow++;
                }
            }
            catch { }

            //cleanup
            GC.Collect();
            GC.WaitForPendingFinalizers();

            //release com objects to fully kill excel process from running in the background
            Marshal.ReleaseComObject(xlRange);
            Marshal.ReleaseComObject(xlWorksheet);
            //close and release
            xlWorkbook.Save();
            xlWorkbook.Close(false);
            Marshal.ReleaseComObject(xlWorkbook);
            //quit and release
            xlApp.Quit();
            Marshal.ReleaseComObject(xlApp);
        }

        /// <summary>
        /// Attempts to read program data from <see cref="ProgramDataPath"/> and sets file paths to base directory if the path specified is invalid
        /// </summary>
        private void InitializeProgramData()
        {
            // Create the file if it doesn't exist
            if (!File.Exists(ProgramDataPath))
            {
                ProjectTimeDataCollection.SaveProgramData(ProgramDataPath, new ProgramData());
            }

            ProgramData programData = ProjectTimeDataCollection.GetProgramData(ProgramDataPath);

            // Verify the read-in file's directory exists and that the file is an xml file, otherwise set path to default
            try
            {
                ProjectTimeDataPath = programData.ProjectTimeDataPath;
                FileInfo f = new FileInfo(ProjectTimeDataPath);
                if (!Directory.Exists(f.Directory.FullName))
                {
                    throw new Exception();
                }
                if (string.Compare(f.Extension, ".xml", true) != 0)
                {
                    throw new Exception();
                }
            }
            // Exception caught means read-in file's directory does not exist or that the file is not an xml file, so set to default
            catch (Exception ex)
            {
                ProjectTimeDataPath = DefaultProjectTimeDataPath;
            }

            // Verify the read-in directory exists, otherwise set path to default
            try
            {
                TimesheetsDir = programData.TimesheetsDir;
                if (!Directory.Exists(TimesheetsDir))
                {
                    throw new Exception();
                }
            }
            // Exception caught means read-in directory does not exist, so set to default
            catch (Exception ex)
            {
                TimesheetsDir = DefaultTimesheetsDir;
            }

            Name = programData.Name;
            ID = programData.ID;
        }

        /// <summary>
        /// Reads items from <see cref="ProjectTimeDataPath"/> and initializes the <see cref="Items"/> collection and its <see cref="CollectionView"/>
        /// </summary>
        private void InitializeProjectTimeDataItems()
        {
            // Initialize items
            Items = new ObservableCollection<ProjectTimeDataViewModel>(ProjectTimeDataCollection.GetProjectTimeData(ProjectTimeDataPath).Select(item => new ProjectTimeDataViewModel(item)));

            // Initialize collection view
            CollectionView = CollectionViewSource.GetDefaultView(Items);
            CollectionView.SortDescriptions.Add(new SortDescription("ChargeNumber", ListSortDirection.Ascending));
            //CollectionView.Filter = Filter;
        }

        #endregion

        #region CollectionView Filtering
        /*
        private bool Filter(object obj)
        {
            if (string.IsNullOrWhiteSpace(SearchText) || SearchText == placeholderText)
            {
                return true;
            }

            ProjectTimeDataViewModel item = (ProjectTimeDataViewModel)obj;
            if (SearchWebsite && item.Website.IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return true;
            }
            else if (SearchEmail && item.Email.IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return true;
            }
            else if (SearchUsername && item.Username.IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return true;
            }
            else if (SearchPasswordHint && item.PasswordHint.IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return true;
            }
            return false;
        }

        private void BwUpdateFilter_DoWork(object sender, DoWorkEventArgs e)
        {
            Interlocked.Exchange(ref delayFilterUpdate, 0);
            Thread.Sleep(500);

            if (delayFilterUpdate == 1)
            {
                return;
            }

            Application.Current.Dispatcher.Invoke(updateFilter);
        }

        private void BwUpdateFilter_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (delayFilterUpdate == 1)
            {
                bwUpdateFilter.RunWorkerAsync();
            }
        }

        private void updateFilter()
        {
            using (CollectionView.DeferRefresh())
            {
                CollectionView.Filter = Filter;
            }
        }

        private void updateFilterDelayed()
        {
            Interlocked.Exchange(ref delayFilterUpdate, 1);
            if (!bwUpdateFilter.IsBusy)
            {
                bwUpdateFilter.RunWorkerAsync();
            }
        }
        */
        #endregion
    }
}
