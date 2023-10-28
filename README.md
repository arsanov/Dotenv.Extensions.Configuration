# Dotenv.Extensions.Configuration

![Build status](https://github.com/arsanov/Dotenv.Extensions.Configuration/actions/workflows/build.yml/badge.svg)
![Nuget](https://img.shields.io/nuget/v/Dotenv.Extensions.Configuration)

Configuration provider extensions to use dotenv file as configuration source

To add dotenv file to the application configuration write

```csharp
using Dotenv.Extensions.Configuration;

...


builder.AddDotenvFile(pathToDotenvFile);
```

This package uses [DotNetEnv ](https://github.com/tonerdo/dotnet-env) to parse dotenv files, but provides different extension method and configuration provider in order to be consistent with existing methods.

File providers usually, like [AddJsonFile](https://github.com/dotnet/runtime/blob/main/src/libraries/Microsoft.Extensions.Configuration.Json/src/JsonConfigurationExtensions.cs#L23), [AddYamlFile](https://github.com/andrewlock/NetEscapades.Configuration), [AddIniFile](https://github.com/dotnet/runtime/blob/main/src/libraries/Microsoft.Extensions.Configuration.Ini/src/IniConfigurationExtensions.cs#L23), have `optional: false` parameter.
The default environment provider also has `prefix` parameter. Therefore `AddDotenvFile` has signature 
```csharp
(string path, bool optional = false, string prefix = String.Empty)
```

`reloadOnChange` is not supported yet, but will be implemented in future.


