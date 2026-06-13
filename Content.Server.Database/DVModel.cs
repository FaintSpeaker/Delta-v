// File to store DeltaV-specific database models

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Content.Server.Database;

// ReSharper disable once InconsistentNaming
public static class DVModel
{
    /// <summary>
    /// Stores tip dismissal preferences for a player, tracking which tips they've marked
    /// as "Don't show again".
    /// </summary>
    [Table("dv_seen_tips")]
    [Index(nameof(PlayerUserId), nameof(TipProtoId), IsUnique = true)]
    public class SeenTip
    {
        public int Id { get; set; }

        /// <summary>
        /// The player's user ID (GUID). References the Player table.
        /// </summary>
        public Guid PlayerUserId { get; set; }

        /// <summary>
        /// The prototype ID of the tip that was dismissed.
        /// </summary>
        [MaxLength(64)]
        public string TipProtoId { get; set; } = string.Empty;

        /// <summary>
        /// When the tip was dismissed.
        /// </summary>
        public DateTime DismissedAt { get; set; }
    }

    /// <summary>
    /// Stores job priorities for player preferences
    /// </summary>
    [Table("dv_player_jobs")]
    [Index(nameof(PreferenceId))]
    [Index(nameof(PreferenceId), nameof(JobName), IsUnique = true)]
    public class PlayerJob
    {
        public int Id { get; set; }
        
        [ForeignKey(nameof(PreferenceId))]
        public Preference Preference { get; set; } = null!;
        public int PreferenceId { get; set; }

        public string JobName { get; set; } = null!;
        public DbJobPriority Priority { get; set; }
    }
    
    /// <summary>
    /// Stores which antags are enabled as part of a player's preferences.
    /// </summary>
    /// <remarks>
    ///     This is only for certain antagonists, usually round-start antagonists such as Xenoborgs or Nukies.
    /// </remarks>
    [Table("dv_player_antags")]
    [Index(nameof(PreferenceId), nameof(AntagName), IsUnique = true)]
    public class PlayerAntag
    {
        public int Id { get; set; }
        
        [ForeignKey(nameof(PreferenceId))]
        public Preference Preference { get; set; } = null!;
        public int PreferenceId { get; set; }

        public string AntagName { get; set; } = null!;
    }

    /// <summary>
    /// This should match CharacterProfileFaction
    /// </summary>
    public enum DbCharacterProfileFaction : byte
    {
        Crew = 0,
        Antagonist = 1
    }
}
