using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using TodoList_AvaloniaMVVM.Services;
using TodoList_AvaloniaMVVM.ViewModels;
using TodoList_AvaloniaMVVM.Views;

namespace TodoList_AvaloniaMVVM;

public partial class App : Application
{
    // This is a reference to our MainViewModel which we use to save the list on shutdown.
    // You can also use Dependency Injection in your App.
    private readonly MainWindowViewModel _mainWindowViewModel = new();
    
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override async void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = _mainWindowViewModel,
            };
            
            // Listen to the ShutdownRequested-event
            desktop.ShutdownRequested += DesktopOnShutdownRequested;

            await InitMainViewModelAsync();
        }

        base.OnFrameworkInitializationCompleted();
    }
    
    // We want to save our ToDoList before we actually shutdown the App. As File I/O is async,
    // we need to wait until file is closed before we can actually close this window
    private bool _canClose; // This flag is used to check if window is allowed to close
    private async void DesktopOnShutdownRequested(object? sender, ShutdownRequestedEventArgs e)
    {
        e.Cancel = !_canClose; // cancel closing event first time

        if (!_canClose)
        {
            // To save the items, we map them to the ToDoItem-Model which is better suited for I/O operations
            var itemsToSave = _mainWindowViewModel.TodoItems.Select(item => item.GetTodoItem());
            await ToDoListFileService.SaveToFileAsync(itemsToSave);

            // Set _canClose to true and Close this Window again
            _canClose = true;
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.Shutdown();
            }
        }
    }
    
    // Optional: Load data from disc
    private async Task InitMainViewModelAsync()
    {
        // get the items to load
        var itemsLoaded = await ToDoListFileService.LoadFromFileAsync();

        if (itemsLoaded is not null)
        {
            foreach (var item in itemsLoaded)
            {
                _mainWindowViewModel.TodoItems.Add(new TodoItemViewModel(item));
            }
        }
    }
}