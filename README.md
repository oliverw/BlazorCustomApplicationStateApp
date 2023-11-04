# BlazorCustomApplicationStateApp
Demonstrates a not-so-pretty workaround for https://github.com/dotnet/aspnetcore/issues/51584

# Why
For some odd reason `PersistentComponentState` is only available on initial page load but not when navigating using enhanced navigation. This will cause nasty flickering issues when the client renders the page a second time.

# How it works
Component renders its state into a `script` tag as base64 encoded JSON during pre-render, and retrieves it during client-side hydration if `PersistentComponentState` is not available.
