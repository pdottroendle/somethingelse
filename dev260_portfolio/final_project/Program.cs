
using System;
using System.Linq;
using RecipeManager.Models;
using RecipeManager.Services;
using RecipeManager.Utils;

namespace RecipeManager;

public class Program
{
    private static RecipeService _service = new();

    public static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        RunMenu();
    }

    private static void RunMenu()
    {
        while (true)
        {
            Console.WriteLine("\n=== Recipe Manager ===");
            Console.WriteLine("1) Add Recipe");
            Console.WriteLine("2) List Recipes");
            Console.WriteLine("3) Search");
            Console.WriteLine("4) Update Recipe");
            Console.WriteLine("5) Delete Recipe");
            Console.WriteLine("6) Stats");
            Console.WriteLine("0) Quit");
            var choice = InputHelper.ReadInt("Choose an option: ", min:0, max:6);
            switch (choice)
            {
                case 1: AddRecipeFlow(); break;
                case 2: ListFlow(); break;
                case 3: SearchFlow(); break;
                case 4: UpdateFlow(); break;
                case 5: DeleteFlow(); break;
                case 6: StatsFlow(); break;
                case 0: Console.WriteLine("Goodbye!"); return;
            }
        }
    }

    private static void AddRecipeFlow()
    {
        var title = InputHelper.ReadNonEmpty("Title: ");
        var ingredientsCsv = InputHelper.ReadNonEmpty("Ingredients (comma-separated): ");
        var stepsInput = InputHelper.ReadNonEmpty("Steps (semicolon-separated): ");

        var ingredients = ingredientsCsv
            .Split(',')
            .Select(s => s.Trim())
            .Where(s => !string.IsNullOrWhiteSpace(s));

        var steps = stepsInput
            .Split(';')
            .Select(s => s.Trim())
            .Where(s => !string.IsNullOrWhiteSpace(s));

        var created = _service.AddRecipe(title, ingredients, steps);
        Console.WriteLine(created is not null
            ? $"Added recipe '{created.Title}'."
            : "A recipe with that title already exists.");
    }

    private static void ListFlow()
    {
        Console.WriteLine("\nAll Recipes (A-Z):");
        foreach (var r in _service.ListRecipesAlphabetical())
            Console.WriteLine($"- {r.Title} (ingredients: {r.Ingredients.Count}, steps: {r.Steps.Count})");

        Console.WriteLine("\nRecent:");
        foreach (var r in _service.ListRecent())
            Console.WriteLine($"- {r.Title}");
    }

    private static void SearchFlow()
    {
        Console.WriteLine("\nSearch by: 1) Title  2) Ingredient");
        var c = InputHelper.ReadChoiceWithAliases(
            "Choose (1/title or 2/ingredient): ",
            new (int, string[])[]
            {
                (1, new[]{ "1", "title", "t" }),
                (2, new[]{ "2", "ingredient", "ing", "i" })
            }
        );

        if (c == 1)
        {
            var title = InputHelper.ReadNonEmpty("Title: ");
            var r = _service.FindByTitle(title);
            if (r is null) Console.WriteLine("Not found.");
            else PrintRecipe(r);
        }
        else
        {
            var ing = InputHelper.ReadNonEmpty("Ingredient: ");
            var found = _service.FindByIngredient(ing).ToList();
            if (found.Count == 0) Console.WriteLine("No recipes contain that ingredient.");
            else foreach (var r in found) PrintRecipe(r);
        }
    }

    private static void UpdateFlow()
    {
        var title = InputHelper.ReadNonEmpty("Existing title: ");
        var recipe = _service.FindByTitle(title);
        if (recipe is null) { Console.WriteLine("Not found."); return; }

        Console.WriteLine("Update: 1) Title  2) Ingredients  3) Steps");
        var c = InputHelper.ReadInt("Choose: ", 1, 3);
        switch (c)
        {
            case 1:
                var newTitle = InputHelper.ReadNonEmpty("New title: ");
                Console.WriteLine(_service.RenameRecipe(title, newTitle)
                    ? "Title updated."
                    : "Update failed (collision?).");
                break;
            case 2:
                var ingredientsCsv = InputHelper.ReadNonEmpty("New ingredients (comma-separated): ");
                var ingredients = ingredientsCsv
                    .Split(',')
                    .Select(s => s.Trim())
                    .Where(s => !string.IsNullOrWhiteSpace(s));
                _service.ReplaceIngredients(title, ingredients);
                Console.WriteLine("Ingredients updated.");
                break;
            case 3:
                var stepsInput = InputHelper.ReadNonEmpty("New steps (semicolon-separated): ");
                var steps = stepsInput
                    .Split(';')
                    .Select(s => s.Trim())
                    .Where(s => !string.IsNullOrWhiteSpace(s));
                _service.ReplaceSteps(title, steps);
                Console.WriteLine("Steps updated.");
                break;
        }
    }

    private static void DeleteFlow()
    {
        var title = InputHelper.ReadNonEmpty("Title to delete: ");
        Console.WriteLine(_service.DeleteRecipe(title) ? "Deleted." : "Not found.");
    }

    private static void StatsFlow()
    {
        Console.WriteLine("\n=== Stats ===");
        Console.WriteLine($"Total recipes: {_service.Count}");
        Console.WriteLine($"Unique ingredients: {_service.UniqueIngredientCount}");
        Console.WriteLine("Recent items:");
        foreach (var r in _service.ListRecent())
            Console.WriteLine("- " + r.Title);
    }

    private static void PrintRecipe(Recipe r)
    {
        Console.WriteLine($"\n[{r.Title}]\nIngredients: {string.Join(", ", r.Ingredients)}\nSteps:");
        int i = 1; foreach (var s in r.Steps) Console.WriteLine($"  {i++}. {s}");
    }
};