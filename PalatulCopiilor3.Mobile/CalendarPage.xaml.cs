using PalatulCopiilor3.Mobile.Services;
using System.Collections.ObjectModel;
using Microsoft.Maui.Storage;


namespace PalatulCopiilor3.Mobile;

public partial class CalendarPage : ContentPage
{
    private readonly ApiClient _api;

    private List<ReservationDto> _all = new();
    private readonly ObservableCollection<ReservationVm> _day = new();

    public CalendarPage(ApiClient api)
    {
        InitializeComponent();
        _api = api;
        DayReservations.ItemsSource = _day;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        DayPicker.Date = DateTime.Today;

        // aici aduci rezervările din API
        var token = await SecureStorage.GetAsync("auth_token");

        if (string.IsNullOrWhiteSpace(token))
        {
            await DisplayAlert("Eroare", "Nu ești autentificat. Te rog loghează-te din nou.", "OK");
            await Shell.Current.GoToAsync("//login");
            return;
        }

        _all = await _api.GetMyReservationsAsync(token);


        ShowDay(DayPicker.Date);
    }

    private void OnDateSelected(object sender, DateChangedEventArgs e)
    {
        ShowDay(e.NewDate);
    }

    private void ShowDay(DateTime date)
    {
        _day.Clear();

        var items = _all
            .Where(r => r.StartAt.Date == date.Date)
            .OrderBy(r => r.StartAt)
            .Select(r => new ReservationVm(r))
            .ToList();

        foreach (var i in items)
            _day.Add(i);
    }

}

public class ReservationVm
{
    public string ActivityName { get; set; }
    public string IntervalText { get; set; }
    public string DetailsText { get; set; }

    public ReservationVm(PalatulCopiilor3.Mobile.Services.ReservationDto r)
    {
        ActivityName = r.ActivityTitle;
        IntervalText = $"{r.StartAt:HH:mm}";
        DetailsText = $"{r.DepartmentName ?? ""} {(!string.IsNullOrWhiteSpace(r.TeacherName) ? "• " + r.TeacherName : "")}".Trim();
    }
}

