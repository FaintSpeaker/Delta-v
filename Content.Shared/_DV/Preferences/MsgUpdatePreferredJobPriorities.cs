using System.IO;
using Content.Shared.Preferences;
using Content.Shared.Roles;
using Lidgren.Network;
using Robust.Shared.Network;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared._DV.Preferences;

public sealed class MsgUpdatePreferredJobPriorities : NetMessage
{
    public override MsgGroups MsgGroup => MsgGroups.Command;

    public Dictionary<ProtoId<JobPrototype>, JobPriority> Priorities = default!;
    
    public override void ReadFromBuffer(NetIncomingMessage buffer, IRobustSerializer serializer)
    {
        var length = buffer.ReadVariableInt32();
        using var stream = new MemoryStream(length);

        buffer.ReadAlignedMemory(stream, length);
        Priorities = serializer.Deserialize<Dictionary<ProtoId<JobPrototype>, JobPriority>>(stream);
    }

    public override void WriteToBuffer(NetOutgoingMessage buffer, IRobustSerializer serializer)
    {
        using var stream = new MemoryStream();
        
        serializer.Serialize(stream, Priorities);
        buffer.WriteVariableInt32((int) stream.Length);
        stream.TryGetBuffer(out var segment);
        buffer.Write(segment);
    }
}