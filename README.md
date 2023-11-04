# BlazorCustomApplicationStateApp
Demonstrates a not-so-pretty workaround for https://github.com/dotnet/aspnetcore/issues/51584

# Why
For some odd reason `PersistentComponentState` is only available on initial page load but not when navigating using enhanced navigation. This will cause nasty flickering issues when the client renders the page a second time.

# How it works
Component renders its state into a `script` tag as base64 encoded JSON during pre-render, and retrieves it during client-side hydration if `PersistentComponentState` is not available.

# Drawbacks
This is a Band-Aid at best and not a particularly good one:

- State is rendered twice on initial page load. One time by PersistentComponentState and another copy by the workaround. Depending on the size of your state, page sizes might increase significantly. Might be able to improve if able to tell a full page load from enhanced navigation load.
- Significant boiler plate in every component using this
