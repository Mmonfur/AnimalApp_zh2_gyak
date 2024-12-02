using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AnimalApp;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        BindingContext = new MainViewModel();
    }
}

public class MainViewModel : INotifyPropertyChanged
{
    // Tulajdonságok
    public ObservableCollection<Animal> Animals { get; set; } = new ObservableCollection<Animal>();
    public string NewAnimalName { get; set; }
    public string NewAnimalSpecies { get; set; }
    public string NewAnimalAge { get; set; }
    public string ACountResult { get; set; }

    // Command-ek, ezeknél csak getter
    public Command AddAnimalCommand { get; }
    public Command CountAnimalsStartingWithACommand { get; }

    public MainViewModel()
    {
        AddAnimalCommand = new Command(AddAnimal);
        CountAnimalsStartingWithACommand = new Command(async () => await CountAnimalsStartingWithAAsync());
    }

    private void AddAnimal()
    {
        if (string.IsNullOrWhiteSpace(NewAnimalName) || string.IsNullOrWhiteSpace(NewAnimalSpecies) || !int.TryParse(NewAnimalAge, out int age))
        {
            return; // Helyes adat ellenőrzés
        }

        Animals.Add(new Animal
        {
            Name = NewAnimalName,
            Species = NewAnimalSpecies,
            Age = age
        });

        // Mezők törlése
        NewAnimalName = string.Empty;
        NewAnimalSpecies = string.Empty;
        NewAnimalAge = string.Empty;
        OnPropertyChanged(nameof(NewAnimalName));
        OnPropertyChanged(nameof(NewAnimalSpecies));
        OnPropertyChanged(nameof(NewAnimalAge));
    }

    private async Task CountAnimalsStartingWithAAsync()
    {
        // Aszinkron számolás LINQ használatával
        await Task.Run(() =>
        {
            var count = Animals.Count(a => a.Name.StartsWith("A", StringComparison.OrdinalIgnoreCase));
            ACountResult = $"A-betűs állatok száma: {count}";
            OnPropertyChanged(nameof(ACountResult));
        });
    }

    // INotifyPropertyChanged implementáció
    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

