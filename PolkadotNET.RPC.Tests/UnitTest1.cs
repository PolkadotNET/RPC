using NSubstitute;
using PolkadotNET.RPC.Namespaces;
using PolkadotNET.RPC.Services.ChainHead;
using PolkadotNET.RPC.Services.ChainHead.Parameters;
using PolkadotNET.RPC.Types;
using PolkadotNET.SCALE;
using PolkadotNET.SCALE.Types;

namespace PolkadotNET.RPC.Tests;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    private const string Namespace = "chainHead";
    private const string Version = "unstable";
    private const string Prefix = $"{Namespace}_{Version}";

    [Test]
    public async Task BlockHeader()
    {
        const string scaleHex =
            "0x4b45af2f246cb157b7043cb06be549bd9fc28d3ac13f8d0fb89f6a8aa72f60a59aa75b040491ae415c13f12a5f49ff3dadea2f1954d2f3da398c4d4dcff7326a019f7f85eae48d2833b3b32a8ab05d75a1e33b9bf7c1e56f510776a92d717f5350991158080642414245b50101a80000002afee4100000000010178689521522ac32046ac2f60eade224d5f0dc24d6194f626b53c8dbd1187ebbdaf47418077d6ac0cee6020f0ae4e44e0c9bf6d5525db0d67446a6ca40480557b39cd5a1a91a78e249aafd7548f507ca5244442bd92a5f6782c583d8e6500c054241424501012641590a1a41eddab78e28eb3da8dcb266e2baf0e09e86b53f78adf2b4d1be4667dee4c5d085f07d07347ba9f1e02d9ceb9ee26d15c623d7a8250addec37db89";

        var client = Substitute.For<IJsonRpcClient>();
        client.SendAsync<HeaderParameters, string?>($"{Prefix}_header", Arg.Any<HeaderParameters>())!
            .Returns(Task.Run(() => scaleHex));

        var rpc = new ChainHeadService(client);

        var blockHeaderResponse = await rpc.Header(new HeaderParameters("", string.Empty));

        Assert.Multiple(() =>
        {
            Assert.That(blockHeaderResponse!.ParentHash,
                Is.EqualTo(new Hash("4B45AF2F246CB157B7043CB06BE549BD9FC28D3AC13F8D0FB89F6A8AA72F60A5")));
            Assert.That(blockHeaderResponse!.StateRoot,
                Is.EqualTo(new Hash("0491AE415C13F12A5F49FF3DADEA2F1954D2F3DA398C4D4DCFF7326A019F7F85")));
            Assert.That(blockHeaderResponse!.ExtrinsicsRoot,
                Is.EqualTo(new Hash("EAE48D2833B3B32A8AB05D75A1E33B9BF7C1E56F510776A92D717F5350991158")));
            Assert.That(blockHeaderResponse!.Number, Is.EqualTo(18278886));
        });
    }
}