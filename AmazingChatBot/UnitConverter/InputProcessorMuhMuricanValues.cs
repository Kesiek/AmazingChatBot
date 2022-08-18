using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace AmazingChatBot.UnitConverter
{
    public class InputProcessorMuhMuricanValues : InputProcessor
    {
        public InputProcessorMuhMuricanValues(string[] units, Func<decimal, decimal> mathProcessor, Func<decimal, decimal, string> responseProcessor)
            : base(units, mathProcessor, responseProcessor)
        {
        }

        public override (string message, int index) ProcessString(Match input, Match nextInput = null)
        {
            decimal? value = null;
            try { value = decimal.Parse(input.Groups[1].Value.Replace(',', '.')); }
            catch { value = null; }
            if (value == null) return ("", 1);

            string unit = input.Groups[2].Value;
            if (_units.Any(x => x == unit))
            {
                if ((unit == "ft" || unit == "'") && nextInput != null)
                {
                    decimal? value2 = null;
                    try { value2 = decimal.Parse(nextInput.Groups[1].Value.Replace(',', '.')); }
                    catch { value2 = null; }
                    if (value2 != null)
                    {
                        string unit2 = nextInput.Groups[2].Value;
                        if (unit2 == "in" || unit2 == "\"" || unit2 == "")
                        {
                            decimal feetToMm = value.Value * (decimal)304.8;
                            decimal inchToMm = value2.Value * (decimal)25.44;
                            decimal rounded = Math.Round((feetToMm + inchToMm) / 1000, 2);
                            string fixedUnit = nextInput.Groups[2].Value == "" ? "in" : nextInput.Groups[2].Value;
                            return ($"{input} {nextInput.Groups[1]}{fixedUnit} = {rounded}m", 2);
                        }
                        else
                            return (_response(value.Value, _math(value.Value)), 1);
                    }
                    else
                        return (_response(value.Value, _math(value.Value)), 1);
                }
                else
                    return (_response(value.Value, _math(value.Value)), 1);
            }
            return ("", 1);
        }
    }
}