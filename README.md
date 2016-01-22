# BunnyWay.Metrics
BunnyWay.Metrics is a high performance .NET metrics tracker library that automatically sends data directly into InfluxDB 0.9.x.

It requires minimum configuration and is super easy to get started.
```
<appSettings>
    <add key="BunnyMetrics.InfluxDB.ServerUrl" value="http://127.0.0.1/" />
    <add key="BunnyMetrics.InfluxDB.DatabaseName" value="xxx" />
    <add key="BunnyMetrics.InfluxDB.Username" value="xxx" />
    <add key="BunnyMetrics.InfluxDB.Password" value="xxx" />
</appSettings>
```
