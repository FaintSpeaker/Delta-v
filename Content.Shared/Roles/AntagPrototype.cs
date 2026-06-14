using Content.Shared._DV.Preferences;  // DeltaV - Add Profile Faction
using Content.Shared.Preferences;  // DeltaV - Add Profile Faction
using Content.Shared.Guidebook;
using Content.Shared.StatusIcon;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared.Roles;

/// <summary>
///     Describes information for a single antag.
/// </summary>
[Prototype]
public sealed partial class AntagPrototype : IPrototype
{
    // The name to group all antagonists under. Equivalent to DepartmentPrototype IDs.
    public static readonly string GroupName = "Antagonist";

    // The colour to group all antagonists using. Equivalent to DepartmentPrototype Color fields.
    public static readonly Color GroupColor = Color.Red;

    [ViewVariables]
    [IdDataField]
    public string ID { get; private set; } = default!;

    /// <summary>
    ///     The name of this antag as displayed to players.
    /// </summary>
    [DataField("name")]
    public string Name { get; private set; } = "";

    /// <summary>
    ///     The antag's objective, shown in a tooltip in the antag preference menu or as a ghost role description.
    /// </summary>
    [DataField("objective", required: true)]
    public string Objective { get; private set; } = "";

    /// <summary>
    ///     Whether or not the antag role is one of the bad guys.
    /// </summary>
    [DataField("antagonist")]
    public bool Antagonist { get; private set; }

    /// <summary>
    ///     Whether or not the player can set the antag role in antag preferences.
    /// </summary>
    /// <remarks>
    ///     DeltaV - This is also used to filter roles in the Jobs tab for <see cref="PrimaryRole"/> roles, and <see cref="PlayerPreferences.EnabledAntags"/> for <see cref="PlayerPreference"/> roles.
    /// </remarks>
    [DataField("setPreference")]
    public bool SetPreference { get; private set; }
    
    // DeltaV - Begin Additions (Profile Factions)
    /// <summary>
    ///     When true, this role will only appear in the Roles/Jobs tab instead of the Antagonists tab.  
    /// </summary>
    [DataField]
    public bool PrimaryRole { get; set; }
    
    /// <summary>
    ///     Which profile factions should this job show in preferences for?
    /// </summary>
    [DataField]
    public HashSet<CharacterProfileFaction> VisibleProfileFactions { get; set; } = [ CharacterProfileFaction.Crew ];
    // DeltaV - End Additions (Profile Factions)

    // DeltaV - Begin Additions (Separate Roundstart Antags from Profiles)
    /// <summary>
    ///     When true, this role will appear in <see cref="PlayerPreferences.EnabledAntags"/> instead of <see cref="HumanoidCharacterProfile.AntagPreferences"/>.
    /// </summary>
    [DataField]
    public bool PlayerPreference { get; set; } = true;
    
    /// <summary>
    ///     The <see cref="JobIconPrototype"/> to display for this antagonist.
    /// </summary>
    [DataField]
    public ProtoId<JobIconPrototype> Icon { get; private set; } = "JobIconUnknown";
    // DeltaV - End Additions (Separate Roundstart Antags from Profiles)

    /// <summary>
    ///     Requirements that must be met to opt in to this antag role.
    /// </summary>
    [DataField, Access(typeof(SharedRoleSystem), Other = AccessPermissions.None)]
    public HashSet<JobRequirement>? Requirements;

    /// <summary>
    /// Optional list of guides associated with this antag. If the guides are opened, the first entry in this list
    /// will be used to select the currently selected guidebook.
    /// </summary>
    [DataField]
    public List<ProtoId<GuideEntryPrototype>>? Guides;
}
