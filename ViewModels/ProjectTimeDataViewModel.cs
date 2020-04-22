using PropertyChanged;
using System;
using System.ComponentModel;
using System.Windows.Input;

namespace TimeTracker
{
    [AddINotifyPropertyChangedInterface]
    public class ProjectTimeDataViewModel
    {
        private decimal mondayTime, tuesdayTime, wednesdayTime, thursdayTime, fridayTime;

        #region Public Properties
        public string DisplayName { get; set; }
        public string ChargeNumber { get; set; }
        public decimal MondayTime
        {
            get { return mondayTime; }
            set { mondayTime = value; CommandManager.InvalidateRequerySuggested(); }
        }
        public decimal TuesdayTime
        {
            get { return tuesdayTime; }
            set { tuesdayTime = value; CommandManager.InvalidateRequerySuggested(); }
        }
        public decimal WednesdayTime
        {
            get { return wednesdayTime; }
            set { wednesdayTime = value; CommandManager.InvalidateRequerySuggested(); }
        }
        public decimal ThursdayTime
        {
            get { return thursdayTime; }
            set { thursdayTime = value; CommandManager.InvalidateRequerySuggested(); }
        }
        public decimal FridayTime
        {
            get { return fridayTime; }
            set { fridayTime = value; CommandManager.InvalidateRequerySuggested(); }
        }
        public decimal TotalLoggedTime { get; set; }
        public string MondayNotes { get; set; }
        public string TuesdayNotes { get; set; }
        public string WednesdayNotes { get; set; }
        public string ThursdayNotes { get; set; }
        public string FridayNotes { get; set; }
        public decimal TotalTime
        {
            get { return mondayTime + tuesdayTime + wednesdayTime + thursdayTime + fridayTime; }
        }
        #endregion

        public ProjectTimeDataViewModel()
        {
        }

        public ProjectTimeDataViewModel(ProjectTimeData p)
        {
            DisplayName = p.DisplayName;
            ChargeNumber = p.ChargeNumber;
            MondayTime = p.MondayTime;
            TuesdayTime = p.TuesdayTime;
            WednesdayTime = p.WednesdayTime;
            ThursdayTime = p.ThursdayTime;
            FridayTime = p.FridayTime;
            TotalLoggedTime = p.TotalLoggedTime;
            MondayNotes = p.MondayNotes;
            TuesdayNotes = p.TuesdayNotes;
            WednesdayNotes = p.WednesdayNotes;
            ThursdayNotes = p.ThursdayNotes;
            FridayNotes = p.FridayNotes;
        }

        internal void ClearTime()
        {
            MondayTime = TuesdayTime = WednesdayTime = ThursdayTime = FridayTime = 0;
            MondayNotes = TuesdayNotes = WednesdayNotes = ThursdayNotes = FridayNotes = string.Empty;
        }

        public void ClearAndLogTime()
        {
            TotalLoggedTime += MondayTime + TuesdayTime + WednesdayTime + ThursdayTime + FridayTime;
            ClearTime();
        }
    }
}
