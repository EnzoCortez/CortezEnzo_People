using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using People.Models;

namespace People.ViewModels
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<Person> People { get; set; } = new ObservableCollection<Person>();

        public ICommand AddPersonCommand { get; }
        public ICommand GetPeopleCommand { get; }
        public ICommand DeletePersonCommand { get; }

        private readonly PersonRepository _personRepository;

        public string StatusMessage { get; set; }

        public MainPageViewModel(string dbPath)
        {
            _personRepository = new PersonRepository(dbPath);

            AddPersonCommand = new Command<string>(OnAddPerson);
            GetPeopleCommand = new Command(OnGetPeople);
            DeletePersonCommand = new Command<Person>(OnDeletePerson);

            OnGetPeople(); // Load initial data
        }

        private void OnAddPerson(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                StatusMessage = "Name cannot be empty.";
                return;
            }

            _personRepository.AddNewPerson(name);
            StatusMessage = _personRepository.StatusMessage;

            OnGetPeople();
        }

        private void OnGetPeople()
        {
            People.Clear();
            var peopleFromDb = _personRepository.GetAllPeople();
            foreach (var person in peopleFromDb)
            {
                People.Add(person);
            }
        }

        private void OnDeletePerson(Person person)
        {
            if (person != null)
            {
                People.Remove(person);
                App.Current.MainPage.DisplayAlert("Alerta", $"{person.Name} acaba de eliminar un registro.", "OK");
            }
        }
    }

}
