namespace Build.Darwin

/// <summary>
/// Set of goodies for common tasks.
/// </summary>
module internal Goodies =
    /// <summary>
    /// Converts a given sequence of strings into a CMake list (a string with elements separated by <c>;</c>).
    /// </summary>
    /// <param name="items">A sequence of strings to be converted into a CMake list.</param>
    let toCMakeList (items: seq<string>) : string = items |> String.concat ";"
