using System.Runtime.Serialization;

namespace PolkadotNET.RPC.Services.ChainHead;


// todo: Type -> Enum and serialize
[DataContract]
public record Item([property: DataMember(Name = "key")] string Key, [property: DataMember(Name = "type")] string Type);