using PolkadotNET.RPC.Namespaces;

namespace PolkadotNET.RPC.Services.Rpc;

class RpcService : BaseRpcService, IRpcService
{
    public RpcService(SmoldotJsonRpcClient rpcClient) : base(rpcClient)
    {
    }

    public Task<MethodsResponse> Methods()
        => RpcClient.SendAsync<MethodsResponse>("rpc_methods");
}