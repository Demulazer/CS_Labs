using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Model;

public partial class LoginWindowViewModel : ObservableObject
{
    
    private readonly Action _navigateToRegister;
    private readonly Action _navigateToMain;

    public LoginWindowViewModel(Action navigateToRegister, Action navigateToMain)
    {

        _navigateToRegister = navigateToRegister;
        _navigateToMain = navigateToMain;

        LoginCommand = new AsyncRelayCommand(LoginAsync);
        NavigateToRegisterCommand = new RelayCommand(_navigateToRegister);
    }

    [ObservableProperty]
    private string _email;

    [ObservableProperty]
    private string _password;

    [ObservableProperty]
    private string _statusMessage;

    public IAsyncRelayCommand LoginCommand { get; }
    public RelayCommand NavigateToRegisterCommand { get; }

    private async Task LoginAsync()
    {
        StatusMessage = "Processing login...";
        // var (message, token, userId) = await _authService.LoginAsync(Email, Password);
        //  if (token != null && userId != null)
        // {
        //     UserService.DecodeJwtToken(token);
        //     _navigateToMain();
        // }
        // else
        // {
        //     StatusMessage = message;
        // }
        
    }
}