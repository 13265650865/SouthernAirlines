using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.NET.Core;
public class HttpExpand
{
    private readonly HttpClient _httpClient;
    public HttpExpand(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    public async Task<string> GetStringAsync(string url)
    {
        try
        {
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            string jsonResponse = await response.Content.ReadAsStringAsync();

            return jsonResponse;
        }
        catch (Exception ex)
        {
            return string.Empty;
        }
    }
}
