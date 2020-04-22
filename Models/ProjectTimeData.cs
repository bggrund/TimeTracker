using System;

namespace TimeTracker
{
    public class ProjectTimeData : IComparable<ProjectTimeData>
    {
        #region Public Properties
        public string DisplayName { get; set; }
        public string ChargeNumber { get; set; }
        public decimal MondayTime { get; set; }
        public decimal TuesdayTime { get; set; }
        public decimal WednesdayTime { get; set; }
        public decimal ThursdayTime { get; set; }
        public decimal FridayTime { get; set; }
        public decimal TotalLoggedTime { get; set; }
        public string MondayNotes { get; set; }
        public string TuesdayNotes { get; set; }
        public string WednesdayNotes { get; set; }
        public string ThursdayNotes { get; set; }
        public string FridayNotes { get; set; }


        #endregion

        #region Constructors

        public ProjectTimeData()
        {
            DisplayName = ChargeNumber = string.Empty;
            MondayTime = TuesdayTime = WednesdayTime = ThursdayTime = FridayTime = TotalLoggedTime = 0;
            MondayNotes = TuesdayNotes = WednesdayNotes = ThursdayNotes = FridayNotes = string.Empty;
        }
        public ProjectTimeData(ProjectTimeDataViewModel p)
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

        public int CompareTo(ProjectTimeData other)
        {
            return DisplayName.CompareTo(other.DisplayName);
        }

        #endregion
    }
}
