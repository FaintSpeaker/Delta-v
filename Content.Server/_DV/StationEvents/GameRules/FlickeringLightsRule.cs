using Content.Server._DV.StationEvents.Components;
using Content.Server.Light.EntitySystems;
using Content.Server.StationEvents.Components;
using Content.Server.StationEvents.Events;
using Content.Shared.GameTicking.Components;
using Content.Shared.Light.Components;
using Robust.Shared.Random;
namespace Content.Server._DV.StationEvents.Events;

/// <summary>
/// This event flickers a portion of the lights on the station while the game rule is running.
/// </summary>
public sealed class FlickeringLightsRule : StationEventSystem<FlickeringLightsRuleComponent>
{
    [Dependency] private readonly EntityLookupSystem _entityLookup = default!;
    [Dependency] private readonly IRobustRandom _random = default!;
    [Dependency] private readonly PoweredLightSystem _lightSystem = default!;

    private EntityQuery<PoweredLightComponent> _lightQuery;

    public override void Initialize()
    {
        base.Initialize();

        _lightQuery = GetEntityQuery<PoweredLightComponent>();
    }

    protected override void Started(EntityUid uid, FlickeringLightsRuleComponent component, GameRuleComponent gameRule, GameRuleStartedEvent args)
    {
        base.Started(uid, component, gameRule, args);

        if (!TryComp<StationEventComponent>(uid, out var stationEvent) || stationEvent is not { EndTime: var endTime })
        {
            // Something is wrong with this station event, so bail out just to be safe.
            return;
        }

        // Whatever starts the rule may have set the station before starting the game rule
        // If that's the case, we shouldn't randomly choose a station.
        if (!component.AffectedStation.HasValue)
        {
            if (!TryGetRandomStation(out var chosenStation))
            {
                Log.Error($"{ToPrettyString(uid):rule} failed to find a station!");
                return;
            }

            component.AffectedStation = chosenStation.Value;
        }

        var grid = StationSystem.GetLargestGrid(component.AffectedStation.Value);

        if (!grid.HasValue)
        {
            Log.Error($"{ToPrettyString(uid):rule} picked station {component.AffectedStation.Value} which had no largest grid!");
            ForceEndSelf(uid, gameRule);
            return;
        }

        var mapId = Transform(grid.Value).MapID;

        var lightLookup = new HashSet<Entity<PoweredLightComponent>>();
        _entityLookup.GetEntitiesOnMap(mapId, lightLookup);

        foreach (var light in lightLookup)
        {
            if(Transform(light).MapID != mapId)
                continue;

            if (!_random.Prob(component.LightFlickerChance))
                continue;

            var blinking = EnsureComp<BlinkingPoweredLightComponent>(light.Owner);

            // Skip always blinking lights
            if (blinking.StopBlinkingTime is null)
                continue;

            // Skip lights that should blink for longer
            if (blinking.StopBlinkingTime > endTime)
                continue;

            blinking.StopBlinkingTime = endTime;
            Dirty(light, blinking);
        }
    }
}
