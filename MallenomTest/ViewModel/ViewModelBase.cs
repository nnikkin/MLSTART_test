using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace MallenomTest.ViewModel
{
    // Базовый класс для ViewModel
    public class ViewModelBase : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    };

}
