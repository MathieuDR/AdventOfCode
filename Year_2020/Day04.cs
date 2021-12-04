using System.Text.RegularExpressions;

namespace AdventOfCode.Year_2020;

public class Day04 : BaseDay {
    private readonly Passport[] _passports;

    public Day04() {
        _passports = File.ReadAllText(InputFilePath).Split(Environment.NewLine + Environment.NewLine).Select(x => new Passport(x)).ToArray();
    }

    public override ValueTask<string> Solve_1() {
        return new ValueTask<string>(
            $"There are {_passports.Count(x => x.IsValidSimple())} valid passports");
    }

    public override ValueTask<string> Solve_2() {
        return new ValueTask<string>(
            $"There are {_passports.Count(x => x.IsValid())} valid passports");
    }

    public class Passport {
        public Passport(string record) {
            var lines = record.Split("\r\n");

            foreach (var line in lines) {
                var fields = line.Split(" ");
                foreach (var field in fields) {
                    var info = field.Split(":");

                    switch (info[0]) {
                        case "byr":
                            BirthYear = info[1];
                            break;
                        case "iyr":
                            IssueYear = info[1];
                            break;
                        case "eyr":
                            ExpirationYear = info[1];
                            break;
                        case "hgt":
                            Height = info[1];
                            break;
                        case "hcl":
                            HairColor = info[1];
                            break;
                        case "ecl":
                            EyeColor = info[1];
                            break;
                        case "pid":
                            PassportId = info[1];
                            break;
                        case "cid":
                            CountryId = info[1];
                            break;
                    }
                }
            }
        }

        public string BirthYear { get; set; }
        public string IssueYear { get; set; }
        public string ExpirationYear { get; set; }
        public string Height { get; set; }
        public string HairColor { get; set; }
        public string EyeColor { get; set; }
        public string PassportId { get; set; }
        public string CountryId { get; set; }

        public static bool NumberBetween(string number, int min, int max) {
            if (string.IsNullOrEmpty(number) || !int.TryParse(number, out var @int)) {
                return false;
            }

            return @int >= min && @int <= max;
        }

        private static readonly Regex HeightRegex = new("(\\d+)(cm|in)", RegexOptions.Compiled);
        private static readonly Regex HairRegex = new("#[0-9a-f]{6}", RegexOptions.Compiled);
        private static readonly Regex EyeRegex = new("(amb|blu|brn|gry|grn|hzl|oth)", RegexOptions.Compiled);
        private static readonly Regex PidRegex = new("^\\d{9}$", RegexOptions.Compiled);

        public static bool ValidateHeight(string height) {
            if (string.IsNullOrEmpty(height)) {
                return false;
            }

            var matches = HeightRegex.Matches(height);
            if (matches.Count != 1) {
                return false;
            }

            if (!int.TryParse(matches[0].Groups[1].Value, out var heightInt)) {
                return false;
            }

            return matches[0].Groups[2].Value switch {
                "cm" => NumberBetween(matches[0].Groups[1].Value, 150, 193),
                "in" => NumberBetween(matches[0].Groups[1].Value, 59, 76),
                _ => false
            };
        }

        public static bool ValidateHair(string hairColor) {
            if (string.IsNullOrEmpty(hairColor)) {
                return false;
            }

            return HairRegex.IsMatch(hairColor);
        }

        public static bool ValidatePid(string pid) {
            if (string.IsNullOrEmpty(pid)) {
                return false;
            }

            return PidRegex.IsMatch(pid);
        }

        public static bool ValidateEyes(string eyes) {
            if (string.IsNullOrEmpty(eyes)) {
                return false;
            }

            return EyeRegex.IsMatch(eyes);
        }

        public bool IsValidSimple() {
            return
                !string.IsNullOrEmpty(BirthYear) &&
                !string.IsNullOrEmpty(IssueYear) &&
                !string.IsNullOrEmpty(ExpirationYear) &&
                !string.IsNullOrEmpty(Height) &&
                !string.IsNullOrEmpty(HairColor) &&
                !string.IsNullOrEmpty(EyeColor) &&
                !string.IsNullOrEmpty(PassportId);
        }

        public bool IsValid() {
            return
                NumberBetween(BirthYear, 1920, 2002) &&
                NumberBetween(IssueYear, 2010, 2020) &&
                NumberBetween(ExpirationYear, 2020, 2030) &&
                ValidateHeight(Height) &&
                ValidateHair(HairColor) &&
                ValidateEyes(EyeColor) &&
                ValidatePid(PassportId);
        }
    }
}
