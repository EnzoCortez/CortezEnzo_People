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

        public MainPageViewModel(string dbPath)
        {
            _personRepository = new PersonRepository(dbPath);

            AddPersonCommand = new Command(OnAddPerson);
            GetPeopleCommand = new Command(OnGetPeople);
            DeletePersonCommand = new Command<Person>(OnDeletePerson);

            OnGetPeople(); // Load initial data
        }

        public MainPageViewModel() : this("default_db_path")
        {
        }

        private string _newPersonName;
        public string NewPersonName
        {
            get => _newPersonName;
            set
            {
                _newPersonName = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NewPersonName)));
            }
        }

        public string StatusMessage { get; set; }

        private void OnAddPerson()
        {
            if (string.IsNullOrEmpty(NewPersonName))
            {
                StatusMessage = "El nombre no puede estar vacío.";
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(StatusMessage)));
                return;
            }

            _personRepository.AddNewPerson(NewPersonName);
            StatusMessage = _personRepository.StatusMessage;
            OnGetPeople();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(StatusMessage)));
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

        private async void OnDeletePerson(Person person)
        {
            if (person != null)
            {
                // Eliminar de la base de datos
                _personRepository.DeletePerson(person.Id);

                // Eliminar del ObservableCollection
                People.Remove(person);

                // Mostrar alerta
                await App.Current.MainPage.DisplayAlert("Alerta", "Enzo Cortez eliminó un registro.", "OK");
            }
        }

    }
}
