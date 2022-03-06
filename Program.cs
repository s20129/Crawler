// See https://aka.ms/new-console-template for more information
using System;
using System.Text.RegularExpressions;

namespace Crawler 
{
    class Crawler
    {
        public async static Task Main(string[] args)
        {
            if (args.Length != 1) 
            {
                throw new ArgumentNullException("Program wymaga przekazania jedengo parametru");
            }
            var websiteURL = args[0];
            if (!IsValidURL(websiteURL)) 
            {
                throw new ArgumentException("Przekazany parametr nie jest poprawnym adresem URL");
            }
            
            var httpClient = new HttpClient();
            try {
                var response = await httpClient.GetAsync(websiteURL);
                var content = await response.Content.ReadAsStringAsync();
                DisplayEmails(content);
            } catch {
                Console.WriteLine("Błąd w czasie pobierania strony");
            }
            httpClient.Dispose();
        }

        private static bool IsValidURL(string URL)
        {
            string Pattern = @"^(?:http(s)?:\/\/)?[\w.-]+(?:\.[\w\.-]+)+[\w\-\._~:/?#[\]@!\$&'\(\)\*\+,;=.]+$";
            Regex Rgx = new Regex(Pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            return Rgx.IsMatch(URL);
        }

        private static void DisplayEmails(string content)
        {
            var pattern = @"\b[\w\.-]+@[\w\.-]+\.\w{2,4}\b";

            MatchCollection matches = Regex.Matches(content, pattern);
            if (matches.Count == 0) 
            {
                Console.WriteLine("Nie znaleziono adresów email");
            }

            var uniqueMatches = matches
                .OfType<Match>()
                .Select(m => m.Value)
                .Distinct();

            uniqueMatches.ToList().ForEach(Console.WriteLine);
        }
    }
}