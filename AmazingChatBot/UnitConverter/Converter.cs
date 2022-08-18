using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AmazingChatBot.UnitConverter
{
    public static class Converter
    {
        /*==============================================================================
         * 
         *          BLESS AND PRAISE BEAUTIFUL BINGO FOR HER HELP WITH REGEX
         * 
         ==============================================================================*/

        private static readonly Regex _cleaningRegex = new Regex("[^a-z0-9,. '\"-]");
        private static readonly Regex _regex = new Regex("(?>\\G|(?<![a-z0-9]))(-?\\d+(?:\\.\\d+)?)\\s?(ft|in|c|f|mi(?:les?)?|m(?:eters?)?|k(?:ilo)?m(?:eters?)?|\"|\'|\"?)");
        private static readonly InputProcessor[] _processors = new[]
        {
            new InputProcessorMuhMuricanValues(
                new[] { "ft", "'" },
                (value) => { return Math.Round(value * (decimal)0.3048, 2); },
                (orgValue, newValue) => { return $"{orgValue} feet = {newValue}m"; },
                Program.UnitConverterFeetEnabled),
            new InputProcessor(
                new[] { "in", "\"" },
                (value) => { return Math.Round(value * (decimal)25.44, 2); },
                (orgValue, newValue) => { return $"{orgValue} inches = {newValue}mm"; },
                Program.UnitConverterInchesEnabled),
            new InputProcessor(
                new[] { "f" },
                (value) => { return Math.Round((value - 32) * 5 / 9, 2); },
                (orgValue, newValue) => { return $"{orgValue}°F = {newValue}°C"; },
                Program.UnitConverterFahrenheitEnabled),
            new InputProcessor(
                new[] { "c" },
                (value) => { return Math.Round(value * 9 / 5 + 32, 2); },
                (orgValue, newValue) => { return $"{orgValue}°C = {newValue}°F"; },
                Program.UnitConverterCelsiusEnabled),
            new InputProcessor(
                new[] { "mi", "miles" },
                (value) => { return Math.Round(value * (decimal)1.609344, 2); },
                (orgValue, newValue) => { return $"{orgValue} miles = {newValue}km"; },
                Program.UnitConverterMilesEnabled),
            new InputProcessor(
                new[] { "km", "kilometers" },
                (value) => { return Math.Round(value * (decimal)0.621371192, 2); },
                (orgValue, newValue) => { return $"{orgValue}km = {newValue} miles"; },
                Program.UnitConverterKilometersEnabled),
            new InputProcessor(
                new[] { "m", "meters" },
                (value) => { return Math.Round(value * (decimal)3.2808399, 2); },
                (orgValue, newValue) => { return $"{orgValue}m = {newValue}ft"; },
                Program.UnitConverterMetersEnabled)
        };

        public static string Process(string userInput)
        {
            List<string> response = new List<string>();

            userInput = _cleaningRegex.Replace(userInput, "");

            MatchCollection matches = _regex.Matches(userInput);

            for (int i = 0; i < matches.Count;)
            {
                int ind = i;
                Match match = matches[i];
                Match match2 = ind + 1 < matches.Count ? matches[i + 1] : null;
                bool increased = false;
                foreach (var processor in _processors)
                {
                    var (message, index) = processor.ProcessString(match, match2);
                    if (!string.IsNullOrWhiteSpace(message))
                    {
                        response.Add(message);
                        i += index;
                        increased = true;
                        break;
                    }
                }
                if (!increased)
                    i++;
            }

            string responseText = "";
            if (response.Count > 0)
            {
                responseText = "Conversion: ";
                if (response.Count > 1)
                {
                    responseText += response[0];
                    for (int i = 1; i < response.Count; i++)
                    {
                        if (i + 1 < response.Count)
                            responseText += $", {response[i]}";
                        else
                            responseText += $" and {response[i]}.";
                    }
                }
                else
                    responseText += $"{response[0]}.";
            }
            return responseText;
        }
    }
}