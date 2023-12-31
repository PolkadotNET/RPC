﻿using ServiceStack;
using ServiceStack.Text;

namespace PolkadotNET.RPC.Services.ChainHead.Results;

public abstract record StorageResult
{
    public static StorageResult ParseJson(string json)
    {
        var jsonObject = json.FromJson<JsonObject>();
        return jsonObject["result"] switch
        {
            "started" => new StartedResult(jsonObject["operationId"], int.Parse(jsonObject["discardedItems"])),
            "limitReached" => new LimitReachedResult(),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public enum ResultType
    {
        Started,
        LimitReached
    }
    
    public abstract ResultType Result { get; }

    public record LimitReachedResult : StorageResult
    {
        public override ResultType Result => ResultType.LimitReached;
    }

    public record StartedResult(string OperationId, int DiscardedItems) : StorageResult
    {
        public override ResultType Result => ResultType.Started;
    }
}