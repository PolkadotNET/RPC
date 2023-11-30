using PolkadotNET.RPC.Services.ChainHead.Parameters;
using PolkadotNET.RPC.Services.ChainHead.Results;
using PolkadotNET.RPC.Structs;

namespace PolkadotNET.RPC.Services.ChainHead;

public interface IChainHeadService
{
    public delegate void Initialized(string followSubscription, Event.Initialized initialized);
    public delegate void NewBlock(string followSubscription, Event.NewBlock newBlock);
    public delegate void BestBlockChanged(string followSubscription, Event.BestBlockChanged bestBlockChanged);
    public delegate void Finalized(string followSubscription, Event.Finalized finalized);
    public delegate void OperationBodyDone(string followSubscription, Event.OperationBodyDone operationBodyDone);
    public delegate void OperationCallDone(string followSubscription, Event.OperationCallDone operationCallDone);
    public delegate void OperationStorageItems(string followSubscription, Event.OperationStorageItems operationStorageItems);
    public delegate void OperationWaitingForContinue(string followSubscription, Event.OperationWaitingForContinue operationWaitingForContinue);
    public delegate void OperationStorageDone(string followSubscription, Event.OperationStorageDone operationStorageDone);
    public delegate void OperationInaccessible(string followSubscription, Event.OperationInaccessible operationInaccessible);
    public delegate void OperationError(string followSubscription, Event.OperationError operationError);
    public delegate void Stop(string followSubscription, Event.Stop stop);

    public event Initialized? OnInitialized;
    public event NewBlock? OnNewBlock;
    public event BestBlockChanged? OnBestBlockChanged;
    public event Finalized? OnFinalized;
    public event OperationBodyDone? OnOperationBodyDone;
    public event OperationCallDone? OnOperationCallDone;
    public event OperationStorageItems? OnOperationStorageItems;
    public event OperationWaitingForContinue? OnOperationWaitingForContinue;
    public event OperationStorageDone? OnOperationStorageDone;
    public event OperationInaccessible? OnOperationInaccessible;
    public event OperationError? OnOperationError;
    public event Stop? OnStop;
    
    
    /// <summary>
    /// https://paritytech.github.io/json-rpc-interface-spec/api/chainHead_unstable_body.html
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public Task<BodyResult> Body(BodyParameters parameters);
    
    /// <summary>
    /// https://paritytech.github.io/json-rpc-interface-spec/api/chainHead_unstable_call.html
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public Task<CallResult> Call(CallParameters parameters);
    
    /// <summary>
    /// https://paritytech.github.io/json-rpc-interface-spec/api/chainHead_unstable_continue.html
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public Task Continue(ContinueParameters parameters);
    
    /// <summary>
    /// https://paritytech.github.io/json-rpc-interface-spec/api/chainHead_unstable_follow.html
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public Task<string> FollowSubscription(FollowParameters parameters);
    
    /// <summary>
    /// https://paritytech.github.io/json-rpc-interface-spec/api/chainHead_unstable_header.html
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public Task<BlockHeader?> Header(HeaderParameters parameters);
    
    /// <summary>
    /// https://paritytech.github.io/json-rpc-interface-spec/api/chainHead_unstable_stopOperation.html
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public Task StopOperation(StopOperationParameters parameters);
    
    /// <summary>
    /// https://paritytech.github.io/json-rpc-interface-spec/api/chainHead_unstable_storage.html
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public Task<StorageResult> Storage(StorageParameters parameters);
    
    /// <summary>
    /// https://paritytech.github.io/json-rpc-interface-spec/api/chainHead_unstable_unfollow.html
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public Task Unfollow(UnfollowOperationParameters parameters);
    
    /// <summary>
    /// https://paritytech.github.io/json-rpc-interface-spec/api/chainHead_unstable_unpin.html
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public Task Unpin(UnpinOperationParameters parameters);
}