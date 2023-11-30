// See https://aka.ms/new-console-template for more information

using PolkadotNET.RPC.Namespaces;
using PolkadotNET.RPC.Services.ChainHead;
using PolkadotNET.RPC.Services.ChainHead.Parameters;
using PolkadotNET.Smoldot.Smoldot;

try
{
    var cts = new CancellationTokenSource();

    var smoldot = new Smoldot("./smoldot_light_wasm.wasm");
    var runTask = smoldot.Run(cts.Token);

    var chain = smoldot.AddChain(File.ReadAllText("./polkadot.json"));
    var rpcClient = new SmoldotJsonRpcClient(chain);

    var rpc = new ChainHeadService(rpcClient);

    var lastFinalized = string.Empty;

    rpc.OnInitialized += (id, initialized) => Console.WriteLine("Follow initialized!");
    // rpc.OnBestBlockChanged +=
    //     (subscription, changed) => Console.WriteLine($"Best Block Changed: {changed.BestBlockHash}");
    rpc.OnFinalized += (subscription, finalized) =>
    {
        foreach (var finalizedFinalizedBlockHash in finalized.FinalizedBlockHashes)
        {
            _ = Task.Run(async () =>
            {
                try
                {
                    var header = await rpc.Header(new HeaderParameters(subscription, finalizedFinalizedBlockHash));
                    if (header != null)
                    {
                        Console.WriteLine(
                            "-----------------------------------------------------------------------------");
                        Console.WriteLine($"Number: {header.Number}");
                        Console.WriteLine($"Parent: {header.ParentHash}");
                        Console.WriteLine($"State Root: {header.StateRoot}");
                        Console.WriteLine($"Extrinsics Root: {header.ExtrinsicsRoot}");
                        Console.WriteLine(
                            "-----------------------------------------------------------------------------");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            });

            lastFinalized = finalizedFinalizedBlockHash;
        }
    };
    rpc.OnStop += (id, stop) => Console.WriteLine("oh no.. we stopped?");

    Console.ReadKey();
    var sid = await rpc.FollowSubscription(new FollowParameters(false));
    Console.WriteLine(sid);
    await runTask;
    Console.ReadKey();
    cts.Cancel();
    Console.WriteLine("Bye!");
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}