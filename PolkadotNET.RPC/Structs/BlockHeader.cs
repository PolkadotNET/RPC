using PolkadotNET.RPC.Types;
using PolkadotNET.SCALE;

namespace PolkadotNET.RPC.Structs;

public record BlockHeader(Hash ParentHash, ulong Number, Hash StateRoot, Hash ExtrinsicsRoot);

public static class BinaryReaderExtensions
{
    public static BlockHeader ReadBlockHeader(this BinaryReader reader)
    {
        // parenthash
        var parentHash = reader.ReadBytes(32);

        // number (u256)
        var blockNumber = reader.ReadCompactUInt64();

        // stateRoot
        var stateRoot = reader.ReadBytes(32);

        // extrinstics Root
        var extrinsicsRoot = reader.ReadBytes(32);

        return new BlockHeader(
            new Hash(parentHash),
            blockNumber,
            new Hash(stateRoot),
            new Hash(extrinsicsRoot)
        );
    }
}