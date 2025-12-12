
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using RecipeManager.Models;

namespace RecipeManager.Services;

public class RecipeService
{
    private readonly Dictionary<string, Recipe> _recipes = new(StringComparer.OrdinalIgnoreCase);
    private readonly HashSet<string> _ingredients = new(StringComparer.OrdinalIgnoreCase);
    private readonly List<string> _recentIds = new();

    public int Count => _recipes.Count;
    public int UniqueIngredientCount => _ingredients.Count;

    public Recipe? AddRecipe(string title, IEnumerable<string> ingredients, IEnumerable<string> steps)
    {
        var id = NormalizeKey(title);
        if (string.IsNullOrWhiteSpace(id) || _recipes.ContainsKey(id)) return null;
        var recipe = new Recipe { Id = id, Title = NormalizeDisplay(title) };
        foreach (var ing in ingredients)
        {
            var norm = NormalizeDisplay(ing);
            if (string.IsNullOrWhiteSpace(norm)) continue;
            recipe.Ingredients.Add(norm);
            _ingredients.Add(norm);
        }
        foreach (var step in steps)
        {
            var norm = NormalizeDisplay(step);
            if (!string.IsNullOrWhiteSpace(norm)) recipe.Steps.Add(norm);
        }
        _recipes[id] = recipe;
        AddRecent(id);
        return recipe;
    }

    public IEnumerable<Recipe> ListRecipesAlphabetical()
        => _recipes.Values.OrderBy(r => r.Title, StringComparer.OrdinalIgnoreCase);

    public IEnumerable<Recipe> ListRecent()
    {
        foreach (var id in _recentIds)
        {
            if (_recipes.TryGetValue(id, out var r)) yield return r;
        }
    }

    public Recipe? FindByTitle(string title)
        => _recipes.TryGetValue(NormalizeKey(title), out var r) ? r : null;

    public IEnumerable<Recipe> FindByIngredient(string ingredient)
    {
        var norm = NormalizeDisplay(ingredient);
        foreach (var r in _recipes.Values)
            if (r.Ingredients.Contains(norm)) yield return r;
    }

    public bool RenameRecipe(string oldTitle, string newTitle)
    {
        var oldId = NormalizeKey(oldTitle);
        if (!_recipes.TryGetValue(oldId, out var r)) return false;
        var newId = NormalizeKey(newTitle);
        if (string.IsNullOrWhiteSpace(newId) || _recipes.ContainsKey(newId)) return false;
        _recipes.Remove(oldId);
        r.Id = newId;
        r.Title = NormalizeDisplay(newTitle);
        _recipes[newId] = r;
        AddRecent(newId);
        for (int i = 0; i < _recentIds.Count; i++) if (_recentIds[i] == oldId) _recentIds[i] = newId;
        return true;
    }

    public void ReplaceIngredients(string title, IEnumerable<string> ingredients)
    {
        var r = FindByTitle(title);
        if (r is null) return;
        r.Ingredients.Clear();
        foreach (var ing in ingredients)
        {
            var norm = NormalizeDisplay(ing);
            if (!string.IsNullOrWhiteSpace(norm)) r.Ingredients.Add(norm);
        }
        foreach (var ing in r.Ingredients) _ingredients.Add(ing);
        AddRecent(r.Id);
    }

    public void ReplaceSteps(string title, IEnumerable<string> steps)
    {
        var r = FindByTitle(title);
        if (r is null) return;
        r.Steps.Clear();
        foreach (var s in steps)
        {
            var norm = NormalizeDisplay(s);
            if (!string.IsNullOrWhiteSpace(norm)) r.Steps.Add(norm);
        }
        AddRecent(r.Id);
    }

    public bool DeleteRecipe(string title)
    {
        var id = NormalizeKey(title);
        var ok = _recipes.Remove(id);
        if (ok)
        {
            _recentIds.RemoveAll(x => x == id);
        }
        return ok;
    }

    private void AddRecent(string id)
    {
        _recentIds.RemoveAll(x => x == id);
        _recentIds.Insert(0, id);
        if (_recentIds.Count > 10) _recentIds.RemoveAt(_recentIds.Count - 1);
    }

    private static string NormalizeKey(string input)
    {
        if (string.IsNullOrWhiteSpace(input)) return string.Empty;
        var trimmed = input.Trim();
        var collapsed = Regex.Replace(trimmed, @"\s+", " ");
        return collapsed.ToLowerInvariant();
    }

    private static string NormalizeDisplay(string input)
    {
        if (string.IsNullOrWhiteSpace(input)) return string.Empty;
        var trimmed = input.Trim();
        return Regex.Replace(trimmed, @"\s+", " ");
    }
}
