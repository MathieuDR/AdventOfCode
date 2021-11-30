namespace AdventOfCode.Helpers; 

public static class NumberExtensions {
    public static IEnumerable<IEnumerable<int>> GetNumbersThatSumTo(this int[] numbers, int amountOfNumbers, int amountToSumTo, bool breakOnFirtMatch = true) {
        var result = new List<int[]>();
        var set = new HashSet<HashSet<int>>();
        var breakPoint = amountOfNumbers - 1;
        
        foreach (var current in numbers) {
            if(current> amountToSumTo) {
                continue;
            }

            var candidates = set.Where(x => x.Count <= breakPoint && x.Sum() + current <= amountToSumTo).ToHashSet();
            foreach (var candidate in candidates) {
                if (candidate.Count == breakPoint && candidate.Sum() + current == amountToSumTo) {
                    result.Add(candidate.Concat(new[] { current }).ToArray());
                    if (breakOnFirtMatch) {
                        return result;
                    }
                }
                else if(candidate.Count != breakPoint){
                    var newSet = new HashSet<int>(candidate) { current };
                    set.Add(newSet);
                }
                    
            }

            // Add current to set
            set.Add(new HashSet<int>() { current });
        }
        
        return result;
    }
}
