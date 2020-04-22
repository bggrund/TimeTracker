using PropertyChanged;
using System.Windows.Input;

namespace TimeTracker
{
    /// <summary>
    /// Window view model containing view models for contents
    /// </summary>
    [AddINotifyPropertyChangedInterface]
    public class WindowViewModel
    {
        public ProjectTimeDataCollectionViewModel ProjectTimeDataCollectionViewModel { get; set; }
        //public ProjectTimeDataViewModel NewProjectTimeDataViewModel { get; set; }
        //public ProjectTimeDataViewModel SelectedProjectTimeDataViewModel { get; set; }

        public string NewProjectDisplayName { get; set; }
        public string NewProjectChargeNumber { get; set; }
        public string[] AddItemCanExecuteParameter { get { return new string[] { NewProjectChargeNumber, NewProjectDisplayName }; } }

        public ICommand AddItemCommand { get; set; }
        
        public WindowViewModel()
        {
            ProjectTimeDataCollectionViewModel = new ProjectTimeDataCollectionViewModel();
            //NewProjectTimeDataViewModel = new ProjectTimeDataViewModel();
            //SelectedProjectTimeDataViewModel = new ProjectTimeDataViewModel();


            AddItemCommand = new AddItemCommand(AddItem);

         //might need to initialize NewProjectTimeDataViewModel values to 0 here
      }

      public void AddItem(object param)
      {
            ProjectTimeDataCollectionViewModel.AddItem(NewProjectDisplayName, NewProjectChargeNumber);
            NewProjectDisplayName = NewProjectChargeNumber = string.Empty;
      }
   }
}
