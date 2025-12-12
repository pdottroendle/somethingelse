
namespace RecipeManager.Models;

public class Recipe
{
    public string Id { get; set; } = string.Empty; // normalized key
    public string Title { get; set; } = string.Empty;
    public HashSet<string> Ingredients { get; } = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
    public List<string> Steps { get; } = new List<string>();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
