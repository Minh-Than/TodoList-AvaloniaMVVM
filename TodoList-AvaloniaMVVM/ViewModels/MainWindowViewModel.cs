using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace TodoList_AvaloniaMVVM.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    /// <summary>
    /// Gets a collection of <see cref="TodoItems"/> which allows adding and removing items
    /// </summary>
    public ObservableCollection<TodoItemViewModel> TodoItems { get; } = new ObservableCollection<TodoItemViewModel>();

    /// <summary>
    /// Gets or set the content for new Items to add. If this string is not empty, the AddItemCommand will be enabled automatically
    /// </summary>
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(AddItemCommand))]
    private string? _newItemContent;

    /// <summary>
    /// Returns if a new Item can be added. We require to have the NewItem some Text
    /// </summary>
    private bool CanAddItem() => !string.IsNullOrWhiteSpace(NewItemContent);

    /// <summary>
    /// This command is used to add a new Item to the List
    /// </summary>
    [RelayCommand (CanExecute = nameof(CanAddItem))]
    private void AddItem()
    {
        TodoItems.Add(new TodoItemViewModel { Content = NewItemContent });
        NewItemContent = null;
    }

    /// <summary>
    /// Removes the given Item from the list
    /// </summary>
    /// <param name="item">the item to remove</param>
    [RelayCommand]
    private void RemoveItem(TodoItemViewModel item)
    {
        TodoItems.Remove(item);
    }
    
    /// <summary>
    /// Update the content in a given Item from the list
    /// </summary>
    /// <param name="item">the item to update</param>
    [RelayCommand]
    private void EditItem(TodoItemViewModel item)
    {
        item.ToggleEdit();
        if (item.IsEditMode)
        {
            item.EditItemContent = item.Content;
            return;
        }
        

        if (!string.IsNullOrWhiteSpace(item.EditItemContent))
        {
            item.Content = item.EditItemContent;
            item.EditItemContent = null;
        }
    }
}