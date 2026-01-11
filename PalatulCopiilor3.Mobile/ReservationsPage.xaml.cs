using PalatulCopiilor3.Mobile.Services;
using System.Runtime.CompilerServices;

namespace PalatulCopiilor3.Mobile;

public partial class ReservationsPage : ContentPage
{
    private readonly ApiClient _api;

    public ReservationsPage(ApiClient api)
    {
        InitializeComponent();
        _api = api;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadReservationsAsync();
    }

    private async void OnRefreshClicked(object sender, EventArgs e)
    {
        await LoadReservationsAsync();
    }

    private async Task LoadReservationsAsync()
    {
        try
        {
            Loading.IsVisible = true;
            Loading.IsRunning = true;

            var token = await SecureStorage.GetAsync("auth_token");
            if (string.IsNullOrWhiteSpace(token))
            {
                await DisplayAlert("Info", "Nu ești logată. Te rog autentifică-te.", "OK");
                await Shell.Current.GoToAsync("//login");
                return;
            }

            var items = await _api.GetMyReservationsAsync(token);
            ReservationsList.ItemsSource = items;
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
