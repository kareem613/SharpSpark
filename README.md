SharpSpark
==========

A .NET client for the SparkCore Cloud API


Usage
-----
```csharp
SparkClient client = new SparkClient("[access token]", "[device id]");
```

**Device Info**
```csharp
SparkDevice deviceInfo = client.GetDevice();
```

**Variables and Functions**
```csharp
SparkResult temperatureResult = client.GetVariable("temperature");
SparkReult brewResult = client.ExecuteFunction("brew","202","230");
```

**Convenience Methods**
```csharp
decimal temperature = client.GetVariableReturnValue<decimal>("temperature");
int returnValue = client.ExecuteFunctionReturnValue("brew","202","230");
```