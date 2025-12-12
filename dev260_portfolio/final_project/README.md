Using the App (Quick Start)
Typical workflow:

Choose Add Recipe and provide a title, comma-separated ingredients, and steps.
Choose List Recipes to see what you've added.
Use Search by title or ingredient to find a specific recipe.
Pick Update or Delete to modify or remove.

Input tips:

Titles are case-insensitive (normalized). Empty titles are rejected.
Ingredients are trimmed and stored uniquely.
Steps are trimmed; empty steps are ignored.
The app handles invalid menu choices gracefully.
In Search, you can type 1 or title for title search, and 2 or ingredient for ingredient search.


Data Structures (Brief Summary)

Full rationale in DESIGN.md.

Data structures used:

Dictionary<string, Recipe> → Fast O(1) average title-based lookup and CRUD.
HashSet<string> → Global set of all ingredients to prevent duplicates and enable membership checks.
List<string> → Recent recipe IDs list for quick "Recent" view.

Optional extensions (future): SortedDictionary, Queue for workflows, JSON persistence.

Manual Testing Summary
Test scenarios:
Scenario 1: Add + List

Steps: Add two recipes; list all.
Expected: Both appear; alphabetical listing shows A→Z; recent includes the last added.

Scenario 2: Search by Title (case-insensitive)

Steps: Add "Buttercake"; search "buttercake".
Expected: Found.

Scenario 3: Search by Ingredient

Steps: Add recipe with ingredient "Milk"; search for "Milk".
Expected: Recipes containing Milk are listed.

Scenario 4: Update Steps

Steps: Update "Buttercake" steps to "Bake".
Expected: Steps replaced; listing reflects update.

Scenario 5: Delete

Steps: Delete a recipe by title.
Expected: Removed from dictionary, recent list updated, global ingredients remain for other recipes.


Known Limitations

No persistence yet (data cleared on exit).
Steps entry is simple (line-per-step); no multiline editing.
Ingredient search is exact-match; no synonyms.


Comparers & String Handling
Keys comparer: StringComparer.OrdinalIgnoreCase to avoid case-related duplicates.
Normalization: Trim whitespace; collapse multiple spaces; canonicalize casing for display.

Credits & AI Disclosure
Resources: .NET docs for collections and LINQ (general reference).
AI usage: Menu flow and scaffolding drafted with AI assistance, then reviewed and edited.

Challenges and Solutions
Biggest challenge: Designing update paths that rewrite dictionary keys safely.
Solution: Implemented RenameRecipe that checks collisions and moves the value under the new key.

Code Quality
Proud of: Clear separation of concerns (RecipeService, InputHelper).
Improve next: Persistence, ingredient synonyms, sorted views.

Real-World Applications
Relevance: Similar to inventory systems and library catalogs: key-based lookups, uniqueness, and structured records.
Learnings: Choosing Dictionary for primary index is natural; HashSet avoids duplicates; List fits for ordered views.

Submission Checklist

 Public GitHub repository link submitted
 README.md completed (this file)
 DESIGN.md completed
 Source code builds successfully
 (Optional) Demo video link


# Recipe Manager — Applied Data Structures
> A simple console app to manage recipes. Designed to demonstrate thoughtful use of multiple data structures in C#.

---

## What I Built (Overview)
**Problem this solves:**  
Home cooks and students often need a quick way to store, find, and edit recipes. This app provides a minimal console interface to add recipes, search by title or ingredient, list all recipes, update details, and delete entries.

**Core features:**
- Add recipe (title, ingredients, steps)
- List all recipes (alphabetical and recent)
- Search by title or ingredient
- Update existing recipe (title/ingredients/steps)
- Delete recipe
- Stats screen (counts, recent items)

---

## How to Run
**Requirements:**
- .NET SDK 8.0 or newer installed
- Windows/macOS/Linux

```bash
git clone <your-repo-url>
cd dev260_fall_2025/final_project
dotnet build
dotnet run



# Project Title

> One-sentence summary of what this app does and who it's for.

---

## What I Built (Overview)

**Problem this solves:**  
_Explain the real-world task your app supports and why it's useful (2–4 sentences)._

**Your Answer:**

**Core features:**  
_List the main features your application provides (Add, Search, List, Update, Delete, etc.)_

**Your Answer:**

-
-
-
-

## How to Run

**Requirements:**  
_List required .NET version, OS requirements, and any dependencies._

**Your Answer:**

```bash
git clone <your-repo-url>
cd <your-folder>
dotnet build
```

**Run:**  
_Provide the command to run your application._

**Your Answer:**

```bash
dotnet run
```

**Sample data (if applicable):**  
_Describe where sample data lives and how to load it (e.g., JSON file path, CSV import)._

**Your Answer:**

---

## Using the App (Quick Start)

**Typical workflow:**  
_Describe the typical user workflow in 2–4 steps._

**Your Answer:**

1.
2.
3.
4.

**Input tips:**  
_Explain case sensitivity, required fields, and how common errors are handled gracefully._

**Your Answer:**

---

## Data Structures (Brief Summary)

> Full rationale goes in **DESIGN.md**. Here, list only what you used and the feature it powers.

**Data structures used:**  
_List each data structure and briefly explain what feature it powers._

**Your Answer:**

- `Dictionary<...>` →
- `List<...>` →
- `HashSet<...>` →
- _(Add others: Queue, Stack, SortedDictionary, custom BST/Graph, etc.)_

---

## Manual Testing Summary

> No unit tests required. Show how you verified correctness with 3–5 test scenarios.

**Test scenarios:**  
_Describe each test scenario with steps and expected results._

**Your Answer:**

**Scenario 1: [Name]**

- Steps:
- Expected result:
- Actual result:

**Scenario 2: [Name]**

- Steps:
- Expected result:
- Actual result:

**Scenario 3: [Name]**

- Steps:
- Expected result:
- Actual result:

**Scenario 4: [Name] (optional)**

- Steps:
- Expected result:
- Actual result:

**Scenario 5: [Name] (optional)**

- Steps:
- Expected result:
- Actual result:

---

## Known Limitations

**Limitations and edge cases:**  
_Describe any edge cases not handled, performance caveats, or known issues._

**Your Answer:**

-
-

## Comparers & String Handling

**Keys comparer:**  
_Describe what string comparer you used (e.g., StringComparer.OrdinalIgnoreCase) and why._

**Your Answer:**

**Normalization:**  
_Explain how you normalize strings (trim whitespace, consistent casing, duplicate checks)._

**Your Answer:**

---

## Credits & AI Disclosure

**Resources:**  
_List any articles, documentation, or code snippets you referenced or adapted._

**Your Answer:**

-
- **AI usage (if any):**  
   _Describe what you asked AI tools, what code they influenced, and how you verified correctness._

  **Your Answer:**

  ***

## Challenges and Solutions

**Biggest challenge faced:**  
_Describe the most difficult part of the project - was it choosing the right data structures, implementing search functionality, handling edge cases, designing the user interface, or understanding a specific algorithm?_

**Your Answer:**

**How you solved it:**  
_Explain your solution approach and what helped you figure it out - research, consulting documentation, debugging with breakpoints, testing with simple examples, refactoring your design, etc._

**Your Answer:**

**Most confusing concept:**  
_What was hardest to understand about data structures, algorithm complexity, key comparers, normalization, or organizing your code architecture?_

**Your Answer:**

## Code Quality

**What you're most proud of in your implementation:**  
_Highlight the best aspect of your code - maybe your data structure choices, clean architecture, efficient algorithms, intuitive user interface, thorough error handling, or elegant solution to a complex problem._

**Your Answer:**

**What you would improve if you had more time:**  
_Identify areas for potential improvement - perhaps adding more features, optimizing performance, improving error handling, adding data persistence, refactoring for better maintainability, or enhancing the user experience._

**Your Answer:**

## Real-World Applications

**How this relates to real-world systems:**  
_Describe how your implementation connects to actual software systems - e.g., inventory management, customer databases, e-commerce platforms, social networks, task managers, or other applications in the industry._

**Your Answer:**

**What you learned about data structures and algorithms:**  
_What insights did you gain about choosing appropriate data structures, performance tradeoffs, Big-O complexity in practice, the importance of good key design, or how data structures enable specific features?_

**Your Answer:**

## Submission Checklist

- [ ] Public GitHub repository link submitted
- [ ] README.md completed (this file)
- [ ] DESIGN.md completed
- [ ] Source code included and builds successfully
- [ ] (Optional) Slide deck or 5–10 minute demo video link (unlisted)

**Demo Video Link (optional):**


test 

C:\Node\GitPort\dev260_portfolio\final_project (master)
λ dotnet build
Restore complete (0.3s)
  RecipeManager succeeded (1.3s) → bin\Debug\net8.0\RecipeManager.dll

Build succeeded in 2.0s

C:\Node\GitPort\dev260_portfolio\final_project (master)
λ dotnet run

=== Recipe Manager ===
1) Add Recipe
2) List Recipes
3) Search
4) Update Recipe
5) Delete Recipe
6) Stats
0) Quit
Choose an option: 1
Title: buttercake
Ingredients (comma-separated): milk,honey
Steps (semicolon-separated): bake, mix
Added recipe 'buttercake'.

=== Recipe Manager ===
1) Add Recipe
2) List Recipes
3) Search
4) Update Recipe
5) Delete Recipe
6) Stats
0) Quit
Choose an option: 1
Title: badbake
Ingredients (comma-separated): no
Steps (semicolon-separated): no
Added recipe 'badbake'.

=== Recipe Manager ===
1) Add Recipe
2) List Recipes
3) Search
4) Update Recipe
5) Delete Recipe
6) Stats
0) Quit
Choose an option: 1
Title: icecream
Ingredients (comma-separated): choco, milk
Steps (semicolon-separated): freeze, mix
Added recipe 'icecream'.

=== Recipe Manager ===
1) Add Recipe
2) List Recipes
3) Search
4) Update Recipe
5) Delete Recipe
6) Stats
0) Quit
Choose an option: 2

All Recipes (A-Z):
- badbake (ingredients: 1, steps: 1)
- buttercake (ingredients: 2, steps: 1)
- icecream (ingredients: 2, steps: 1)

Recent:
- icecream
- badbake
- buttercake

=== Recipe Manager ===
1) Add Recipe
2) List Recipes
3) Search
4) Update Recipe
5) Delete Recipe
6) Stats
0) Quit
Choose an option: 3

Search by: 1) Title  2) Ingredient
Choose (1/title or 2/ingredient): 2 milk  
Please enter one of: 1 (1/title/t), 2 (2/ingredient/ing/i)
Choose (1/title or 2/ingredient): 2/milk
Please enter one of: 1 (1/title/t), 2 (2/ingredient/ing/i)
Choose (1/title or 2/ingredient): 2/ingredient/milk
Please enter one of: 1 (1/title/t), 2 (2/ingredient/ing/i)
Choose (1/title or 2/ingredient): 2     <<<<<<<<<<<<<<<<<<<< only allows numerical selections
Ingredient: milk

[buttercake]
Ingredients: milk, honey
Steps:
  1. bake, mix

[icecream]
Ingredients: choco, milk
Steps:
  1. freeze, mix                       <<<<<<<<<<<<<<<<<<<<<<<<<  Search successfull

=== Recipe Manager ===
1) Add Recipe
2) List Recipes
3) Search
4) Update Recipe
5) Delete Recipe
6) Stats
0) Quit
Choose an option: 4
Existing title: badbake
Update: 1) Title  2) Ingredients  3) Steps
Choose: 2
New ingredients (comma-separated): yes
Ingredients updated.

=== Recipe Manager ===
1) Add Recipe
2) List Recipes
3) Search
4) Update Recipe
5) Delete Recipe
6) Stats
0) Quit
Choose an option: 2

All Recipes (A-Z):
- badbake (ingredients: 1, steps: 1)
- buttercake (ingredients: 2, steps: 1)
- icecream (ingredients: 2, steps: 1)

Recent:
- badbake
- icecream
- buttercake

=== Recipe Manager ===
1) Add Recipe
2) List Recipes
3) Search
4) Update Recipe
5) Delete Recipe
6) Stats
0) Quit
Choose an option: 3

Search by: 1) Title  2) Ingredient
Choose (1/title or 2/ingredient): 1
Title: badbake

[badbake]
Ingredients: yes
Steps:
  1. no

=== Recipe Manager ===
1) Add Recipe
2) List Recipes
3) Search
4) Update Recipe
5) Delete Recipe
6) Stats
0) Quit
Choose an option: 5
Title to delete: badbake
Deleted.

=== Recipe Manager ===
1) Add Recipe
2) List Recipes
3) Search
4) Update Recipe
5) Delete Recipe
6) Stats
0) Quit
Choose an option: 2

All Recipes (A-Z):
- buttercake (ingredients: 2, steps: 1)
- icecream (ingredients: 2, steps: 1)

Recent:
- icecream
- buttercake

=== Recipe Manager ===
1) Add Recipe
2) List Recipes
3) Search
4) Update Recipe
5) Delete Recipe
6) Stats
0) Quit
Choose an option: 6

=== Stats ===
Total recipes: 2
Unique ingredients: 5
Recent items:
- icecream
- buttercake     <<<<<<<<<< stat successfull

=== Recipe Manager ===
1) Add Recipe
2) List Recipes
3) Search
4) Update Recipe
5) Delete Recipe
6) Stats
0) Quit

the quit option 0 worked also

----------------------------------

FULL FUNCTIONAL TEST  : OK
