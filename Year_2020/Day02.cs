using System.Text.RegularExpressions;

namespace AdventOfCode.Year_2020;

/// <summary>
///     Day 2 from year 2020
/// </summary>
public class Day02 : BaseDay {
    private PasswordRecord[] passwords;

    public Day02() {
        passwords = File.ReadAllLines(InputFilePath).Select(x => new PasswordRecord(x)).ToArray();
    }

    public override ValueTask<string> Solve_1() {
        return new ValueTask<string>(
            $"RentalSled, Valid passwords: {passwords.Count(x => x.IsValidForSledRental)}, invalid: {passwords.Count(x => !x.IsValidForSledRental)}");
    }

    public override ValueTask<string> Solve_2() {
        return new ValueTask<string>(
            $"Toboggan, Valid passwords: {passwords.Count(x => x.IsValidForToboggan)}, invalid: {passwords.Count(x => !x.IsValidForToboggan)}");
    }

    public class PasswordRecord {
        private static readonly Regex regex = new(@"^(\d+)-(\d+) (\w): (\w+)$", RegexOptions.Compiled);
        private static Dictionary<char, Regex> _regexes = new();

        public PasswordRecord(string record) {
            var regexResult = regex.Matches(record);
            Password = regexResult[0].Groups[4].Value;
            Minimum = int.Parse(regexResult[0].Groups[1].Value);
            Maximum = int.Parse(regexResult[0].Groups[2].Value);
            RequiredLetter = regexResult[0].Groups[3].Value[0];
            
            ValidateSledRentalPassword();
            ValidateTobogganPassword();
        }

        private bool? _isValidForSledRental;
        public bool IsValidForSledRental => _isValidForSledRental ??= ValidateSledRentalPassword();
        
        private bool? _isValidForToboggan;
        public bool IsValidForToboggan => _isValidForToboggan ??= ValidateTobogganPassword();
        public string Password { get; set; }
        public char RequiredLetter { get; set; }
        public int Minimum { get; set; }
        public int Maximum { get; set; }

        private bool ValidateSledRentalPassword() {
            var regex = GetRegexFromDictionary(RequiredLetter);
            var amountOfMatches = Password.Count(x => regex.IsMatch(x.ToString()));
            
            if(amountOfMatches >= Minimum && amountOfMatches <= Maximum) {
                return true;
            }

            return false;
        }
        
        private bool ValidateTobogganPassword() {
            char? firstLetter = null, lastLetter = null;
            
            if (Password.Length + 1 >= Minimum ) {
                firstLetter = Password[Minimum - 1];
            }
            
            if (Password.Length + 1 >= Maximum ) {
                lastLetter = Password[Maximum - 1];
            }
            
            if(!firstLetter.HasValue) {
                return false;
            }
            
            if((firstLetter.Value == RequiredLetter && (lastLetter.HasValue && lastLetter.Value != RequiredLetter || !lastLetter.HasValue)) || (lastLetter == RequiredLetter && firstLetter != RequiredLetter)) {
                return true;
            }
            
            return false;
        }

        private Regex GetRegexFromDictionary(char requiredLetter) {
            if(!_regexes.TryGetValue(requiredLetter, out Regex result)) {
                result = new(requiredLetter.ToString(), RegexOptions.Compiled);
                _regexes.Add(requiredLetter, result);
            }

            return result;
        }
    }
}
