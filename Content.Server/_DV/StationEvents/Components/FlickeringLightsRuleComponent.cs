namespace Content.Server._DV.StationEvents.Components;

/// <summary>
/// This is used to configure the flickering lights game rule.
/// </summary>
[RegisterComponent]
public sealed partial class FlickeringLightsRuleComponent : Component
{
    [DataField, ViewVariables(VVAccess.ReadOnly)]
    public EntityUid? AffectedStation;

    [DataField]
    public float LightFlickerChance = 0.25f;
}
