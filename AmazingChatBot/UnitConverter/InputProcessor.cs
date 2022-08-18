using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace AmazingChatBot.UnitConverter
{
    public class InputProcessor
    {
        protected readonly bool _enabled;
        protected readonly string[] _units;
        protected readonly Func<decimal, decimal> _math;
        protected readonly Func<decimal, decimal, string> _response;

        public InputProcessor(string[] units, Func<decimal, decimal> mathProcessor, Func<decimal, decimal, string> responseProcessor, bool enabled)
        {
            _units = units;
            _math = mathProcessor;
            _response = responseProcessor;
            _enabled = enabled;
        }

        public virtual (string message, int index) ProcessString(Match input, Match nextInput = null)
        {
            if (!_enabled)
                return ("", 1);

            decimal? value = null;
            try { value = decimal.Parse(input.Groups[1].Value.Replace(',', '.')); }
            catch { value = null; }
            if (value == null) return ("", 1);

            string unit = input.Groups[2].Value;
            if (_units.Any(x => x == unit))
                return (_response(value.Value, _math(value.Value)), 1);

            return ("", 1);
        }
    }
}
