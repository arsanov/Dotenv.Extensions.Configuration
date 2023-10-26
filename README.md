# Dotenv.Extensions.Configuration
Configuration provider extensions to use dotenv file as configuration source


To add dotenv file to the application configuration write

```csharp
using Dotenv.Extensions.Configuration;

...


builder.AddDotenvFile(pathToDotenvFile);
```
