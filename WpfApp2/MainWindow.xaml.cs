using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
class Program
{
    static async Task Main(string[] args)
    {
        string baseCurrency = "USD"; // Базовая валюта
        string targetCurrency = "EUR"; // Целевая валюта
        decimal amount = 100; // Сумма для конвертации
        using (HttpClient client = new HttpClient())
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync($"https://api.exchangerate-api.com/v4/latest/{baseCurrency}");
                response.EnsureSuccessStatusCode(); // Гарантирует, что ответ успешный
                string responseBody = await response.Content.ReadAsStringAsync();
                // Преобразуем ответ в объект
                ExchangeRates exchangeRates = Newtonsoft.Json.JsonConvert.DeserializeObject<ExchangeRates>(responseBody);
                // Получаем курс для целевой валюты
                decimal exchangeRate = exchangeRates.Rates.First(r => r.Key == targetCurrency).Value;
                // Вычисляем конвертированную сумму
                decimal convertedAmount = amount * exchangeRate;
                Console.WriteLine($"{amount} {baseCurrency} = {convertedAmount} {targetCurrency}");
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Ошибка: {e.Message}");
            }
        }
    }
    // Класс для десериализации JSON-ответа
    public class ExchangeRates
    {
        public string Base { get; set; }
        public string Date { get; set; }
        public Dictionary<string, decimal> Rates { get; set; }
    }
}