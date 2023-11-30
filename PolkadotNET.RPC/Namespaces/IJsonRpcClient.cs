namespace PolkadotNET.RPC.Namespaces;

public interface IJsonRpcClient
{
    delegate void Notification(SubscriptionMessage subscriptionMessage);

    event Notification? OnNotification;
    Task<TResponse> SendAsync<TResponse>(string method);
    Task<TResponse> SendAsync<TRequest, TResponse>(string method, TRequest payload);
}