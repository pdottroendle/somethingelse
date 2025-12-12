
namespace RecipeManager.Utils;

public static class InputHelper
{
    public static string ReadNonEmpty(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            var s = Console.ReadLine() ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(s)) return s.Trim();
            Console.WriteLine("Input cannot be empty. Please try again.");
        }
    }

    public static int ReadInt(string prompt, int min, int max)
    {
        while (true)
        {
            Console.Write(prompt);
            var s = Console.ReadLine();
            if (int.TryParse(s, out var n) && n >= min && n <= max) return n;
            Console.WriteLine($"Enter an integer between {min} and {max}.");
        }
    }


public static int ReadChoiceWithAliases(string prompt, (int number, string[] aliases)[] options)
{
    while (true)
    {
        Console.Write(prompt);
        var s = (Console.ReadLine() ?? string.Empty).Trim();

        // Try integer first
        if (int.TryParse(s, out var n))
        {
            foreach (var opt in options)
                if (n == opt.number) return n;
        }

        // Try alias match (case-insensitive)
        var sNorm = s.ToLowerInvariant();
        foreach (var opt in options)
        {
            if (opt.aliases.Any(a => a.Equals(sNorm, StringComparison.OrdinalIgnoreCase)))
                return opt.number;
        }

        // Build a nice error message
        var allowed = string.Join(", ",
            options.Select(o => $"{o.number} ({string.Join("/", o.aliases)})"));
        Console.WriteLine($"Please enter one of: {allowed}");
    }
}


}
