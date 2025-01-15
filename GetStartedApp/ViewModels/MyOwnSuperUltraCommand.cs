using System;
using System.Windows.Input;

namespace GetStartedApp.ViewModels;

public class MyOwnSuperUltraCommand : ICommand
{
    private Func<object, bool> _canExecuteAction;
    private Action<object>? _executeAction;

    public MyOwnSuperUltraCommand(Func<object, bool> canExecuteAction, Action<object>? executeAction = null)
    {
        _canExecuteAction = canExecuteAction;
        _executeAction = executeAction;
    }

    public bool CanExecute(object parameter)
    {
        return _canExecuteAction == null || _canExecuteAction(parameter);
    }

    public void Execute(object parameter)
    {
        _executeAction(parameter);
    }
    

    public event EventHandler? CanExecuteChanged;
}
