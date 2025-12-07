using System.Net.Http.Json;
using ShiftsLogger.UI.Models;
using Spectre.Console;

namespace ShiftsLogger.UI.Services;

public class ShiftService
{
    private readonly HttpClient _httpClient;
    private readonly string _baseApiUrl = "http://localhost:5095";
    public ShiftService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(_baseApiUrl);
    }
    
    private async Task<T?> GetAsync<T>(string endpoint)
    {
        try
        {
            var response = await _httpClient.GetAsync(endpoint);
            if (!response.IsSuccessStatusCode) return default;
            return await response.Content.ReadFromJsonAsync<T>();
        }
        catch (Exception ex)
        {
            AnsiConsole.WriteException(ex);
            return default;
        }
    }

    public async Task<List<ShiftDto>> GetAllShiftsAsync()
    {
        var result = await GetAsync<List<ShiftDto>>("shift");
        return result ?? [];
    }

    public async Task<ShiftDto?> GetShiftByIdAsync(Guid id)
    {
        return await GetAsync<ShiftDto>($"shift/{id}");
    }

    public async Task<ShiftDto?> StartShiftAsync()
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("shift", new { });
            if (response.IsSuccessStatusCode) return await response.Content.ReadFromJsonAsync<ShiftDto>();
        }
        catch (Exception ex) {AnsiConsole.WriteException(ex);}

        return null;
    }

    public async Task StopShiftAsync(Guid id)
    {
        try
        {
            var response = await _httpClient.PutAsync($"shift/{id}/stop", null);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception(error);
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"ERROR: {ex.Message}");
        }
    }
    
    public async Task DeleteShiftAsync(Guid id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"shift/{id}");
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("An error occured while deleting ID (Maybe it does not exist).");
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    
}