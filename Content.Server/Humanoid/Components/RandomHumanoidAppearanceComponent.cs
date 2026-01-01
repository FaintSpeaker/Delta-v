using Content.Shared.Humanoid;
using Robust.Shared.Enums;

namespace Content.Server.CharacterAppearance.Components;

[RegisterComponent]
public sealed partial class RandomHumanoidAppearanceComponent : Component
{
    [DataField("randomizeName")] public bool RandomizeName = true;

    // Overrides //

    /// <summary>
    /// After randomizing, sets the character's sex to this, if applicable.
    /// </summary>
    /// <remarks>
    ///     Delta-V - Support random appearances for spawned characters
    /// </remarks>
    [DataField] public Sex? Sex = null;
    /// <summary>
    /// After randomizing, sets the character's age to this, if applicable.
    /// </summary>
    /// <remarks>
    ///     Delta-V - Support random appearances for spawned characters
    /// </remarks>
    [DataField] public int? Age = null;
    /// <summary>
    /// After randomizing, sets the character's gender to this, if applicable.
    /// </summary>
    /// <remarks>
    ///     Delta-V - Support random appearances for spawned characters
    /// </remarks>
    [DataField] public Gender? Gender = null;

    /// <summary>
    /// After randomizing, sets the character's hair to this, if applicable.
    /// </summary>
    [DataField] public string? Hair = null;
    /// <summary>
    /// After randomizing, sets the character's hair color to this, if applicable.
    /// </summary>
    /// <remarks>
    ///     Delta-V - Support random appearances for spawned characters
    /// </remarks>
    [DataField] public Color? HairColor = null;
    /// <summary>
    /// After randomizing, sets the character's facial hair to this, if applicable.
    /// </summary>
    /// <remarks>
    ///     Delta-V - Support random appearances for spawned characters
    /// </remarks>
    [DataField] public string? FacialHair = null;
    /// <summary>
    /// After randomizing, sets the character's eye color to this, if applicable.
    /// </summary>
    /// <remarks>
    ///     Delta-V - Support random appearances for spawned characters
    /// </remarks>
    [DataField] public Color? EyeColor = null;
    /// <summary>
    /// After randomizing, sets the character's skin color to this, if applicable.
    /// </summary>
    /// <remarks>
    ///     Delta-V - Support random appearances for spawned characters
    /// </remarks>
    [DataField] public Color? SkinColor = null;
    /// <summary>
    /// If this dict is populated, all randomized markings will be replaced with these markings.
    /// If the list is empty the color of that marking will be randomized.
    /// </summary>
    /// <example>
    /// May be defined in YML files:
    /// <code>
    /// markings:
    ///   ArachnidTorsoFiddleback: [ "#daf7da" ]
    /// </code>
    /// </example>
    /// <remarks>
    ///     Delta-V - Support random appearances for spawned characters
    /// </remarks>
    [DataField] public Dictionary<string, List<Color>>? Markings = null;
}
