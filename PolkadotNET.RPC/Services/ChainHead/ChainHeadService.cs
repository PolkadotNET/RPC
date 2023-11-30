using PolkadotNET.RPC.Namespaces;
using PolkadotNET.RPC.Services.ChainHead.Parameters;
using PolkadotNET.RPC.Services.ChainHead.Results;
using PolkadotNET.RPC.Services.Rpc;
using PolkadotNET.RPC.Structs;
using ServiceStack;
using ServiceStack.Text;

namespace PolkadotNET.RPC.Services.ChainHead;

public class ChainHeadService : BaseRpcService, IChainHeadService
{
    private const string Namespace = "chainHead";
    private const string Version = "unstable";
    private const string Prefix = $"{Namespace}_{Version}";

    public event IChainHeadService.Initialized? OnInitialized;
    public event IChainHeadService.NewBlock? OnNewBlock;
    public event IChainHeadService.BestBlockChanged? OnBestBlockChanged;
    public event IChainHeadService.Finalized? OnFinalized;
    public event IChainHeadService.OperationBodyDone? OnOperationBodyDone;
    public event IChainHeadService.OperationCallDone? OnOperationCallDone;
    public event IChainHeadService.OperationStorageItems? OnOperationStorageItems;
    public event IChainHeadService.OperationWaitingForContinue? OnOperationWaitingForContinue;
    public event IChainHeadService.OperationStorageDone? OnOperationStorageDone;
    public event IChainHeadService.OperationInaccessible? OnOperationInaccessible;
    public event IChainHeadService.OperationError? OnOperationError;
    public event IChainHeadService.Stop? OnStop;

    public ChainHeadService(IJsonRpcClient rpcClient) : base(rpcClient)
    {
        RpcClient.OnNotification += OnRpcNotification;
    }

    private void OnRpcNotification(SubscriptionMessage message)
    {
        if (message.Method != "chainHead_unstable_followEvent")
            return;
        
        // ToJson -> FromJson not so cool. How can be convert a JsonObject to T with consideration of DataContract properties?
        var receivedEvent = message.Params.Result.ToJson().FromJson<Event>();

        switch (receivedEvent.Result)
        {
            case Event.EventType.Initialized:
                OnInitialized?.Invoke(message.Params.Subscription, (Event.Initialized)receivedEvent);
                break;
            case Event.EventType.NewBlock:
                OnNewBlock?.Invoke(message.Params.Subscription, (Event.NewBlock)receivedEvent);
                break;
            case Event.EventType.BestBlockChanged:
                OnBestBlockChanged?.Invoke(message.Params.Subscription, (Event.BestBlockChanged)receivedEvent);
                break;
            case Event.EventType.Finalized:
                OnFinalized?.Invoke(message.Params.Subscription, (Event.Finalized)receivedEvent);
                break;
            case Event.EventType.OperationBodyDone:
                OnOperationBodyDone?.Invoke(message.Params.Subscription, (Event.OperationBodyDone)receivedEvent);
                break;
            case Event.EventType.OperationCallDone:
                OnOperationCallDone?.Invoke(message.Params.Subscription, (Event.OperationCallDone)receivedEvent);
                break;
            case Event.EventType.OperationStorageItems:
                OnOperationStorageItems?.Invoke(message.Params.Subscription, (Event.OperationStorageItems)receivedEvent);
                break;
            case Event.EventType.OperationWaitingForContinue:
                OnOperationWaitingForContinue?.Invoke(message.Params.Subscription, (Event.OperationWaitingForContinue)receivedEvent);
                break;
            case Event.EventType.OperationStorageDone:
                OnOperationStorageDone?.Invoke(message.Params.Subscription, (Event.OperationStorageDone)receivedEvent);
                break;
            case Event.EventType.OperationInaccessible:
                OnOperationInaccessible?.Invoke(message.Params.Subscription, (Event.OperationInaccessible)receivedEvent);
                break;
            case Event.EventType.OperationError:
                OnOperationError?.Invoke(message.Params.Subscription, (Event.OperationError)receivedEvent);
                break;
            case Event.EventType.Stop:
                OnStop?.Invoke(message.Params.Subscription, (Event.Stop)receivedEvent);
                break;
            default:
                throw new ArgumentOutOfRangeException(receivedEvent.Result.ToString());
        }
    }

    public Task<BodyResult> Body(BodyParameters parameters)
        => RpcClient.SendAsync<BodyParameters, BodyResult>($"{Prefix}_body", parameters);

    public Task<CallResult> Call(CallParameters parameters)
        => RpcClient.SendAsync<CallParameters, CallResult>($"{Prefix}_call", parameters);

    public Task Continue(ContinueParameters parameters)
        => RpcClient.SendAsync<ContinueParameters, object>($"{Prefix}_continue", parameters);

    public Task<string> FollowSubscription(FollowParameters parameters)
        => RpcClient.SendAsync<FollowParameters, string>($"{Prefix}_follow", parameters);

    public async Task<BlockHeader?> Header(HeaderParameters parameters)
    {
        var response = await RpcClient.SendAsync<HeaderParameters, string?>($"{Prefix}_header", parameters);
        if (string.IsNullOrEmpty(response))
            return null;

        using var ms = new MemoryStream(Convert.FromHexString(response[2..]));
        using var br = new BinaryReader(ms);

        return br.ReadBlockHeader();
    }

    public Task StopOperation(StopOperationParameters parameters)
        => RpcClient.SendAsync<StopOperationParameters, object>($"{Prefix}_stopOperation", parameters);

    public Task<StorageResult> Storage(StorageParameters parameters)
        => RpcClient.SendAsync<StorageParameters, StorageResult>($"{Prefix}_storage", parameters);

    public Task Unfollow(UnfollowOperationParameters parameters)
        => RpcClient.SendAsync<UnfollowOperationParameters, object>($"{Prefix}_unfollow", parameters);

    public Task Unpin(UnpinOperationParameters parameters)
        => RpcClient.SendAsync<UnpinOperationParameters, object>($"{Prefix}_unpin", parameters);
}