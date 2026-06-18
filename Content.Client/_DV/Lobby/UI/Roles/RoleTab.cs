using Content.Client.Players.PlayTimeTracking;
using Content.Shared.Guidebook;
using Content.Shared.Preferences;
using Content.Shared.Preferences.Loadouts;
using Content.Shared.StatusIcon;
using Robust.Client.GameObjects;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using Robust.Shared.Prototypes;
namespace Content.Client._DV.Lobby.UI.Roles;

public abstract class RoleTab<TCategory, TPrototype> : BoxContainer where TPrototype : class, IPrototype where TCategory : class, IPrototype
{
    public HumanoidCharacterProfile? Profile { get; set; }

    [Dependency] protected readonly IPrototypeManager PrototypeManager = default!;
    [Dependency] protected readonly JobRequirementsManager RequirementsManager = default!;
    [Dependency] protected readonly IEntityManager EntityManager = default!;
    protected readonly SpriteSystem SpriteSystem = default!;
    protected abstract Control CategoryContainer { get; }

    protected readonly Dictionary<ProtoId<TPrototype>, List<RoleListEntry>> Entries = [];
    protected readonly Dictionary<string, RoleCategory> Categories = [];

    public event Action<List<ProtoId<GuideEntryPrototype>>>? GuidebooksOpened;
    public event Action<RoleToggledEventArgs>? RoleToggled;
    public event Action<LoadoutOpeningEventArgs>? LoadoutOpened;

    protected RoleTab()
    {
        IoCManager.InjectDependencies(this);

        SpriteSystem = EntityManager.System<SpriteSystem>();

        PrototypeManager.PrototypesReloaded += OnProtoReload;
        RequirementsManager.Updated += OnRequirementsManagerUpdated;
    }

    public void Refresh()
    {
        Populate();
        UpdateSelectedRoles();
    }

    public void Clear()
    {
        foreach (var categoryContainer in Categories.Values)
        {
            categoryContainer.RemoveAllChildren();
        }
        CategoryContainer.RemoveAllChildren();

        Entries.Clear();
        Categories.Clear();
    }

    /// <summary>
    ///     Retrieve the role prototypes for the specified category prototype.
    /// </summary>
    /// <param name="category"></param>
    /// <returns></returns>
    protected abstract IEnumerable<TPrototype> GetRolePrototypes(TCategory category);
    
    /// <summary>
    ///     Retrieve the category prototypes.
    /// </summary>
    /// <returns></returns>
    protected abstract IEnumerable<TCategory> GetCategoryPrototypes();
    
    /// <summary>
    ///     Create a <see cref="RoleListEntry"/> for the specified role prototype.
    /// </summary>
    /// <param name="prototype"></param>
    /// <returns></returns>
    protected abstract RoleListEntry CreateEntry(TPrototype prototype);
    
    /// <summary>
    ///     Create a <see cref="RoleCategory"/> for the specified category prototype. 
    /// </summary>
    /// <param name="category"></param>
    /// <returns></returns>
    protected abstract RoleCategory CreateCategory(TCategory category);
    
    /// <summary>
    ///     Update the selected role entries from the current state.
    /// </summary>
    protected abstract void UpdateSelectedRoles();
    
    protected virtual void OnGuidebooksOpened(List<ProtoId<GuideEntryPrototype>>? guidebooks)
    {
        if (guidebooks is null || guidebooks.Count == 0) return;
        
        GuidebooksOpened?.Invoke(guidebooks);
    }
    
    protected virtual void OnRoleToggled(RoleToggledEventArgs args)
    {
        RoleToggled?.Invoke(args);
    }
    
    protected virtual void OnLoadoutOpened(LoadoutOpeningEventArgs obj)
    {
        LoadoutOpened?.Invoke(obj);
    }

    private void Populate()
    {
        Clear();

        foreach (var category in GetCategoryPrototypes())
        {
            var categoryContainer = CreateCategory(category);
            categoryContainer.Visible = false; // Categories are hidden until something is actually added.
            Categories[category.ID] = categoryContainer;
            CategoryContainer.AddChild(categoryContainer);

            foreach (var prototype in GetRolePrototypes(category))
            {
                var entry = CreateEntry(prototype);
                categoryContainer.Visible |= entry.Visible;
                
                if (!Entries.ContainsKey(prototype.ID))
                {
                    Entries.Add(prototype.ID, []);
                }
                Entries[prototype.ID].Add(entry);
                
                // Use AddItem here because AddChild doesn't respect XamlChildren.
                categoryContainer.AddItem(entry);
            }
        }
    }

    private void OnProtoReload(PrototypesReloadedEventArgs args)
    {
        if (Disposed) return;

        if (!args.WasModified<TPrototype>()
            && !args.WasModified<TCategory>()
            && !args.WasModified<JobIconPrototype>())
            return;

        Populate();
    }

    private void OnRequirementsManagerUpdated()
    {
        Populate();
    }

    public record LoadoutOpeningEventArgs(TPrototype Prototype, RoleLoadoutPrototype Loadout);

    public record RoleToggledEventArgs(TPrototype Prototype, bool Active);
}