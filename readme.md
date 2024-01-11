# Demonstrate bug in Blazor for badly formed circuit handlers

Demonstrate a bug in Blazor server side when the developer has added a
circuit handler whose dependencies cannot be resolved.

If you run this application, you will immediately see the following error pop up in the browser console window:

```
[2024-01-11T10:58:50.870Z] Information: Normalizing '_blazor' to 'https://localhost:7256/_blazor'.
blazor.server.js:1 [2024-01-11T10:58:50.914Z] Information: WebSocket connected to wss://localhost:7256/_blazor?id=paav-gIWuV5ANugJpf9Hog.
blazor.server.js:1 [2024-01-11T10:58:50.974Z] Error: The circuit failed to initialize.
log @ blazor.server.js:1
blazor.server.js:1 [2024-01-11T10:58:50.975Z] Information: Connection disconnected.
blazor.server.js:1 Uncaught (in promise) Error: Invocation canceled due to the underlying connection being closed.
    at Ft._connectionClosed (blazor.server.js:1:76250)
    at Ft.connection.onclose (blazor.server.js:1:67065)
    at Nt._stopConnection (blazor.server.js:1:62559)
    at transport.onclose (blazor.server.js:1:60297)
    at At._close (blazor.server.js:1:52510)
    at At.stop (blazor.server.js:1:52130)
    at Nt._stopInternal (blazor.server.js:1:55448)
    at async Nt.stop (blazor.server.js:1:55258)
    at async Ft.stop (blazor.server.js:1:69730)
```

No other information is included, even when 'DetailedErrors' is set for the circuit.

On the server side, no exceptions are hit, and with default log severity settings nothing is printed to the logs. There *is* the following log message:

```
dbug: Microsoft.AspNetCore.SignalR.Internal.DefaultHubDispatcher[1]
      Received hub invocation: InvocationMessage { InvocationId: "0", Target: "StartCircuit", Arguments: [ https://localhost:6567/, https://localhost:6567/, [{"type":"server","prerenderId":"8157031140834edfac788f982cdbe531","key":{"locationHash":"F0D08B7EDA2582111DC0201BF7AE993F27FDC7A8:0","formattedComponentKey":""},"sequence":0,"descriptor":"CfDJ8G+andg/AaBBgctMAO6zW/om7M7KV6ZrXMyVWbmAhQxVri2Yucv3nKR/ZxsRrHNsk1Yik9ntYN11aJXyyCygcsfBAj3CLppv/YtYYvV9Ujlo0BzE3rz84Hyq9ZjVWuQkNvmbNBURxgRQ33+Acjh1U2GXA1H2s0vX0V8xd1unD3w/dF0AFjr6V+u2oDnzVSr58A55Z16PQNLrsQmziBBdp4PCta+F6eJ8bLBEwzNdZW/qCOZUN9JJSuc7wFMXSbTcYkFUF4V4qhKTkjJaQZW5AwRcXhYHWR/IYzd4jnZQqKAFIYVtRFIxKW7iw1A/mdlBkfSlmgy7ORgi/s3uUTlUCPlYpMgR3hpOLlBhuuPuhaDsz6HxUfe5jLL4WT7Xyp5KH6h/PYnMG27QzuUZj5DMULF//dLLCDraeS4Ibphjb0bkYx1tfgQWtoSxkYAJ09O8qPuQNWS2iJme+6cM52jA6J046fKSg4hCcUtWDNBoPQ+8zBykgTsE20U9LBMert6uTuV9k54N+JYv2kfA9u3Tbzjlmq4C0N1i0sKZeG8bTeim","uniqueId":0},{"type":"server","prerenderId":"7a76de4f95974a34a2c1fae3e321d92f","key":{"locationHash":"D37BEBFB2244B82C3611A796B48C02D91C663DBC:0","formattedComponentKey":""},"sequence":1,"descriptor":"CfDJ8G+andg/AaBBgctMAO6zW/rCMOIlJsQK4o4OKo4jN299ZtcIuH5CO3jMDAmOCpI2W7j+EU5AkNSv/uCeYeqUc2odDnCVKh1Ho66sKPFmm+duOh25UPrfgaUC/25pYJeujZ/jxyyQv5rnEpdtMV/WhAUPMPjF5xklZKHaCVYuAI0/yeIkdIZCXUFfMpjxilpvYBTevT2RYeyX8yec9q8KGiFKBNO0eKjWmfm94/RwQbNLpdxKKtqcqVxS99bmcfmJVNRUOfnybciFwAcR/asoWmOD10IUYUW2lKvzyCDoNS8qxFYJ8Bnl9rCG5iARGHK/6TawRqkzH/qnOu0Idi5OhXIMWcW8wC7m1M2wlLkOpTxSJXa3mJHWq6J848nFtRrIcmpe5FT5nfYVOaRjDJt+MPuIRWYhE2iCuEIujDCNAZbYmOAG/5vHo0NZM12osjbdFGEUTmk8UNuJBqwtCYZ15CoW899xdUVjToqwF6QrXd6DRoIGRnBg2nl50f3sAcrDqYZSnir61kZE3SFsxaLz/D8=","uniqueId":1}],  ], StreamIds: [  ] }.
dbug: Microsoft.AspNetCore.Components.Server.Circuits.RemoteJSRuntime[1]
      Begin invoke JS interop '2': 'Blazor._internal.attachWebRendererInterop'
dbug: Microsoft.AspNetCore.Components.Server.ComponentHub[6]
      Circuit initialization failed
      System.InvalidOperationException: Unable to resolve service for type 'Indices.IndexEditor.Ui.Common.FromWpfMessageListener' while attempting to activate 'Indices.IndexEditor.Ui.Common.DefaultCircuitHandler'.
         at Microsoft.Extensions.DependencyInjection.ServiceLookup.CallSiteFactory.CreateArgumentCallSites(ServiceIdentifier serviceIdentifier, Type implementationType, CallSiteChain callSiteChain, ParameterInfo[] parameters, Boolean throwIfCallSiteNotFound)
         at Microsoft.Extensions.DependencyInjection.ServiceLookup.CallSiteFactory.CreateConstructorCallSite(ResultCache lifetime, ServiceIdentifier serviceIdentifier, Type implementationType, CallSiteChain callSiteChain)
         at Microsoft.Extensions.DependencyInjection.ServiceLookup.CallSiteFactory.TryCreateExact(ServiceDescriptor descriptor, ServiceIdentifier serviceIdentifier, CallSiteChain callSiteChain, Int32 slot)
         at Microsoft.Extensions.DependencyInjection.ServiceLookup.CallSiteFactory.TryCreateEnumerable(ServiceIdentifier serviceIdentifier, CallSiteChain callSiteChain)
         at Microsoft.Extensions.DependencyInjection.ServiceLookup.CallSiteFactory.CreateCallSite(ServiceIdentifier serviceIdentifier, CallSiteChain callSiteChain)
         at Microsoft.Extensions.DependencyInjection.ServiceLookup.CallSiteFactory.GetCallSite(ServiceIdentifier serviceIdentifier, CallSiteChain callSiteChain)
         at Microsoft.Extensions.DependencyInjection.ServiceProvider.CreateServiceAccessor(ServiceIdentifier serviceIdentifier)
         at System.Collections.Concurrent.ConcurrentDictionary`2.GetOrAdd(TKey key, Func`2 valueFactory)
         at Microsoft.Extensions.DependencyInjection.ServiceProvider.GetService(ServiceIdentifier serviceIdentifier, ServiceProviderEngineScope serviceProviderEngineScope)
         at Microsoft.Extensions.DependencyInjection.ServiceLookup.ServiceProviderEngineScope.GetService(Type serviceType)
         at Microsoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions.GetRequiredService(IServiceProvider provider, Type serviceType)
         at Microsoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions.GetRequiredService[T](IServiceProvider provider)
         at Microsoft.AspNetCore.Components.Server.Circuits.CircuitFactory.CreateCircuitHostAsync(IReadOnlyList`1 components, CircuitClientProxy client, String baseUri, String uri, ClaimsPrincipal user, IPersistentComponentStateStore store)         at Microsoft.AspNetCore.Components.Server.ComponentHub.StartCircuit(String baseUri, String uri, String serializedComponentRecords, String applicationState)
```

But it has severity of 'Debug' so is very easy to miss.
