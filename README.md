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
SparkVariableResult temperatureResult = client.GetVariable("temperature");
SparkFunctionResult brewResult = client.ExecuteFunction("brew","202","230");
```

**Convenience Methods**
```csharp
decimal temperature = client.GetVariableReturnValue<decimal>("temperature");
int returnValue = client.ExecuteFunctionReturnValue("brew","202","230");
```

Compiling & Getting Tests to Run
---------

1. Copy app.config.sample in the Tests project to app.config.
2. Update the access token and device id appsettings
3. Flash TestFirmware.cpp from the tests project to your device.
4. Build, run tests
