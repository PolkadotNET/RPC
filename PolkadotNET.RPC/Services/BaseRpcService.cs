using PolkadotNET.RPC.Namespaces;

namespace PolkadotNET.RPC.Services;

public abstract class BaseRpcService
{
    protected readonly IJsonRpcClient RpcClient;

    protected BaseRpcService(IJsonRpcClient rpcClient)
    {
        RpcClient = rpcClient;
    }
}