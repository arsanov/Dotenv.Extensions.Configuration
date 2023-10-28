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
