using Content.Shared.Roles;
using Robust.Shared.Prototypes;

namespace Content.Shared._DV.Roles;

/// <summary>
///     Prototype for grouping <see cref="AntagPrototype"/>s into categories.
/// </summary>
[Prototype]
public sealed partial class AntagCategoryPrototype : IPrototype
{
    /// <inheritdoc/>
    [IdDataField]
    public string ID { get; private set; } = string.Empty;

    /// <summary>
    /// The name LocId of the category name that will be displayed in the various menus.
    /// </summary>
    [DataField(required: true)]
    public LocId Name = string.Empty;

    /// <summary>
    /// A description LocId to display in the character menu as an explanation of the category's content.
    /// </summary>
    [DataField(required: true)]
    public LocId Description = string.Empty;
    
    /// <summary>
    ///     The <see cref="AntagPrototype"/>s inside this category.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public List<ProtoId<AntagPrototype>> Roles = new();

    /// <summary>
    /// Categories with a higher weight sorted before other departments in UI.
    /// </summary>
    [DataField]
    public int Weight { get; private set; }

    /// <summary>
    /// Toggles the display of the category in the preference menus and the character editor.
    /// </summary>
    [DataField]
    public bool EditorHidden;
    
    /// <summary>
    ///     
    /// </summary>
    [DataField]
    public Color Color = Color.White;
}


/// <summary>
/// Sorts <see cref="AntagCategoryPrototype"/> appropriately for display in the UI,
/// respecting their <see cref="AntagCategoryPrototype.Weight"/>.
/// </summary>
public sealed class AntagCategoryUIComparer : IComparer<AntagCategoryPrototype>
{
    public static readonly AntagCategoryUIComparer Instance = new();

    public int Compare(AntagCategoryPrototype? x, AntagCategoryPrototype? y)
    {
        if (ReferenceEquals(x, y))
            return 0;

        if (ReferenceEquals(null, y))
            return 1;

        if (ReferenceEquals(null, x))
            return -1;

        var cmp = -x.Weight.CompareTo(y.Weight);
        return cmp != 0 ? cmp : string.Compare(x.ID, y.ID, StringComparison.Ordinal);
    }
}
