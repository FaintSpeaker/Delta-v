using Robust.Shared.Serialization;
namespace Content.Shared._DV.Preferences;

[Serializable, NetSerializable]
public enum CharacterProfileFaction : byte
{
    Crew = 0,
    Antagonist = 1
}
