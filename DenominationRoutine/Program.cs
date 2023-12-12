// See https://aka.ms/new-console-template for more information

List<int> denominations = new List<int>() { 10, 50, 100 };

Console.WriteLine("ATM Started!!!");
RunATM();

void RunATM()
{
    // List of payout amounts
    List<int> payouts = new List<int> { 30, 50, 60, 80, 140, 230, 370, 610, 980 };

    // Calculate combinations for each payout
    foreach (int payout in payouts)
    {
        Console.WriteLine($"Payout amount: {payout}");

        // Call function to find combinations
        List<List<int>> combinations = CalculateCombinations(payout, 0, new List<int>());

        // Print all possible combinations
        foreach (List<int> combination in combinations)
        {
            string text = string.Empty;
            var list = combination
                          .GroupBy(l => l)
                          .Select(g => new
                          {
                              Value = g.Key,
                              Count = g.Select(l => l).Count()
                          }).ToList();

            foreach (var item in list)
            {
                text = text.Length > 0 ? 
                        text + $" + {item.Count} x {item.Value} EUR" : 
                        $"{item.Count} x {item.Value} EUR";
            }

            Console.WriteLine(text);      
        }

        Console.WriteLine("End");
        Console.WriteLine("  ");
    }
}

List<List<int>> CalculateCombinations(int remainingAmount, int index, List<int> combination)
{
    List<List<int>> combinations = new List<List<int>>();

    if (remainingAmount == 0)
    {
        combinations.Add(new List<int>(combination));
        return combinations;
    }

    for (int i = index; i < denominations.Count; i++)
    {
        int currentDenomination = denominations[i];

        if (remainingAmount >= currentDenomination)
        {
            // Add current denomination to the combination
            combination.Add(currentDenomination);

            // Recursively find combinations for remaining amount
            combinations.AddRange(CalculateCombinations(remainingAmount - currentDenomination, i, combination));

            // Remove current denomination from the combination backtracking
            combination.RemoveAt(combination.Count - 1);
        }
    }

    return combinations;
}