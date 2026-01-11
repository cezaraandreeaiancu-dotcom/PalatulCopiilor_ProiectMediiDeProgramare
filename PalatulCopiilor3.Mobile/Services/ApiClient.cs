using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace PalatulCopiilor3.Mobile.Services
{
    public class ApiClient
    {
        private readonly HttpClient _http;

        public ApiClient(HttpClient http)
        {
            _http = http;
        }

        // LOGIN -> primim JWT
        public async Task<string?> LoginAsync(string email, string password)
        {
            var response = await _http.PostAsJsonAsync("/api/auth/login", new
            {
                Email = email,
                Password = password
            });

            if (!response.IsSuccessStatusCode)
                return null;

            var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
            return result?.Token;
        }

        // REZERVĂRILE MELE (cu Bearer token)
        public async Task<List<ReservationDto>> GetMyReservationsAsync(string token)
        {
            _http.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var items = await _http.GetFromJsonAsync<List<ReservationDto>>(
                "/api/reservations/my");

            return items ?? new List<ReservationDto>();
        }
    }

    // =====================
    // DTO-uri (aceleași ca în API)
    // =====================
    public class LoginResponse
    {
        public string Token { get; set; } = "";
        public DateTime ExpiresAtUtc { get; set; }
    }

    public class ReservationDto
    {
        public int Id { get; set; }
        public DateTime ReservedAt { get; set; }
        public int ActivityId { get; set; }
        public string ActivityTitle { get; set; } = "";
        public DateTime StartAt { get; set; }
        public string? DepartmentName { get; set; }
        public string? TeacherName { get; set; }
    }
}
