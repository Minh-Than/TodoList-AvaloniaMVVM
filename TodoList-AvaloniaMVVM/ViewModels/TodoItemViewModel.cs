using CommunityToolkit.Mvvm.ComponentModel;
using TodoList_AvaloniaMVVM.Models;

namespace TodoList_AvaloniaMVVM.ViewModels;
    
public partial class TodoItemViewModel : ViewModelBase
{
    [ObservableProperty] private bool _isChecked;
    [ObservableProperty] private string? _content;
    [ObservableProperty] private bool _isEditMode;
    [ObservableProperty] private string? _editItemContent;
    [ObservableProperty] private string _editSave = "Edit";

    public void ToggleEdit()
    {
        IsEditMode = !IsEditMode;
        EditSave = IsEditMode ? "Save" : "Edit";
    }

    public TodoItemViewModel()
    {
    }

    public TodoItemViewModel(TodoItem item)
    {
        IsChecked = item.IsChecked;
        Content = item.Content;
    }

    public TodoItem GetTodoItem()
    {
        return new TodoItem()
        {
            IsChecked = this.IsChecked,
            Content = this.Content
        };
    }
}