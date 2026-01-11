using PalatulCopiilor3.Mobile.Services;
using System.Runtime.CompilerServices;

namespace PalatulCopiilor3.Mobile;

public partial class LoginPage : ContentPage
{
    private readonly ApiClient _api;

    public LoginPage(ApiClient api)
    {
        InitializeComponent();
        _api = api;

        // Optional: completează automat dacă vrei pentru test
        // EmailEntry.Text = "test@test.com";
        // PasswordEntry.Text = "123456";
    }

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        var email = EmailEntry.Text?.Trim();
        var pass = PasswordEntry.Text;

        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(pass))
        {
            await DisplayAlert("Eroare", "Completează email și parola.", "OK");
            return;
        }

        try
        {
            Loading.IsVisible = true;
            Loading.IsRunning = true;

            var token = await _api.LoginAsync(email, pass);

            if (token == null)
            {
                await DisplayAlert("Eroare", "Email sau parolă greșite.", "OK");
                return;
            }

            await SecureStorage.SetAsync("auth_token", token);

            // Mergem la pagina rezervărilor (o facem imediat după)
            await Shell.Current.GoToAsync("//reservations");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Eroare", ex.Message, "OK");
        }
        finally
        {
            Loading.IsRunning = false;
            Loading.IsVisible = false;
        }
    }
}
