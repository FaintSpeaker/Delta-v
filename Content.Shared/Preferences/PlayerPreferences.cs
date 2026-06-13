using System.Linq; // DeltaV
using Content.Shared._DV.Species; // DeltaV - Hidden species
using Content.Shared.Construction.Prototypes;
using Content.Shared.Roles; // DeltaV - Job Priorities are in PlayerPreferences now
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;
using Robust.Shared.Utility;

namespace Content.Shared.Preferences
{
    /// <summary>
    ///     Contains all player characters and the index of the currently selected character.
    ///     Serialized both over the network and to disk.
    /// </summary>
    [Serializable]
    [NetSerializable]
    // DeltaV - Begin Changes (Convert to record for easier syntax, use primary constructor)
    // I have adjusted PlayerPreferences to be a record class to allow for using the `with` syntax to clean up some other files.
    public sealed record PlayerPreferences( 
        Dictionary<int, ICharacterProfile> Characters,
        int SelectedCharacterIndex,
        Color AdminOOCColor,
        List<ProtoId<ConstructionPrototype>> ConstructionFavorites,
        Dictionary<ProtoId<JobPrototype>, JobPriority> JobPriorities,
        HashSet<ProtoId<AntagPrototype>> EnabledAntags)
    // DeltaV - End Changes (Convert to record for easier syntax)
    {

        public ICharacterProfile GetProfile(int index)
        {
            return Characters[index]; // DeltaV - Characters is now a property
        }

        // DeltaV - Begin Additions (Add method to get active/usable job prototype ids)
        /// <summary>
        ///     Retrieve the filtered set of Job Prototype Ids which are preferred by at least one character.  
        /// </summary>
        /// <returns></returns>
        public HashSet<ProtoId<JobPrototype>> GetActiveJobs()
        {
            HashSet<ProtoId<JobPrototype>> jobs = [];

            foreach (var characterProfile in Characters.Values)
            {
                if(characterProfile is not HumanoidCharacterProfile humanoidProfile) 
                    continue;

                jobs.UnionWith(humanoidProfile.JobPriorities.Where(kvp => kvp.Value != JobPriority.Never).Select(kvp => kvp.Key));
            }

            return jobs;
        }
        // DeltaV - End Additions (Add method to get active/usable job prototype ids)

        // DeltaV - Begin Additions (Add method to get characters that have a preference for a job prototype id)
        /// <summary>
        ///     Retrieve the characters which are marked as preferring a particular job.
        /// </summary>
        /// <param name="job"></param>
        /// <returns></returns>
        public IEnumerable<ICharacterProfile> GetCharactersForJob(ProtoId<JobPrototype> job)
        {
            foreach (var profile in Characters.Values)
            {
                if(profile is not HumanoidCharacterProfile humanoidProfile) 
                    continue;

                if(humanoidProfile.JobPriorities.TryGetValue(job, out var priority) && priority != JobPriority.Never)
                    yield return profile;
            }
        }
        // DeltaV - End Additions (Add method to get characters that have a preference for a job prototype id)
        
        // DeltaV - Begin Additions (Add method to get characters that have a preference for an antag prototype id)
        /// <summary>
        ///     Retrieve the characters which are marked as preferring a particular antag. 
        /// </summary>
        /// <param name="antag"></param>
        /// <returns></returns>
        public IEnumerable<ICharacterProfile> GetCharactersForAntag(ProtoId<AntagPrototype> antag)
        {
            foreach (var profile in Characters.Values)
            {
                if(profile is not HumanoidCharacterProfile humanoidProfile) 
                    continue;

                if(humanoidProfile.AntagPreferences.Contains(antag))
                    yield return profile;
            }
        }
        // DeltaV - End Additions (Add method to get characters that have a preference for an antag prototype id)

        /// <summary>
        ///     The currently selected character.
        /// </summary>
        public ICharacterProfile SelectedCharacter
        { // Start DeltaV - Prevent spawning as hidden speceis (At all costs)
            get
            {
                // Firstly, check if we CAN use the selected character.
                if (Characters.ContainsKey(SelectedCharacterIndex)) // If we've selected a character
                {
                    // Throughout this, we use this If(Valid)return pattern rather than the inverse if(Invalid)continue
                    // Because the conditions in which it's valid are more seperate. This makes it slightly more readable.
                    if (Characters[SelectedCharacterIndex] is not HumanoidCharacterProfile humanoidProfile)
                        return Characters[SelectedCharacterIndex]; // If it's a non-humanoid, return it.
                    if (!SpeciesHiderSystem.IsHidden(humanoidProfile.Species))
                        return humanoidProfile; // Otherwise, return it if it's not hidden
                }
                // Otherwise, return the first valid character we can find.
                foreach (var (_index, profile) in Characters)
                {
                    if (profile is not HumanoidCharacterProfile nextHumanoidProfile)
                        return profile; // If it's a non-humanoid, return it.
                    if (!SpeciesHiderSystem.IsHidden(nextHumanoidProfile.Species))
                        return profile; // If it's not a hidden species, return it.
                }
                // If we can't find ANY valid character, make a new one.
                return HumanoidCharacterProfile.Random();
            }
        } // End DeltaV

        public Color AdminOOCColor { get; set; }

        /// <summary>
        ///    List of favorite items in the construction menu.
        /// </summary>
        public List<ProtoId<ConstructionPrototype>> ConstructionFavorites { get; set; } = [];

        public int IndexOfCharacter(ICharacterProfile profile)
        {
            return Characters.FirstOrNull(p => p.Value == profile)?.Key ?? -1;
        }

        public bool TryIndexOfCharacter(ICharacterProfile profile, out int index)
        {
            return (index = IndexOfCharacter(profile)) != -1;
        }
    }
}
