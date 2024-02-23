## Task for a .NET developer candidate

Build a console app isun.exe which will show and save weather forecasts retrieved from API.
Weather forecasts should be periodically fetched every 15 seconds then printed and saved.
Console app should accept cities parameter, --cities city1, city2, ..., cityN.

### Example:

```
isun.exe --cities Vilnius, Kaunas, Klaipėda
```

Please take into account that incorrect data might be entered, so the application should have
proper error handling.

- API specification: https://weather-api.isun.ch
- Username ""\*\*\*", password "\*\*\*".
- Feel free to use any library or nuget package

### Application should be architected to be able to adapt to the upcoming requirements:

1. App could become a web or desktop app
2. API could change
3. Persistent data storage could change

### Non functional requirements:

1. Use latest .NET
2. Implement logging
3. Write automatic tests (do not need to have 100% coverage, just cover what you think is
   important)
4. Write clean, high quality code.

### Usage guide

This is a standalone console application which can be built using the existing `SelfContainedWin64Profile.pubxml` publish profile.

Before using it make sure to update appsettings.json and provide weather api credentials

```
"WeatherForecastClientOptions": {
  "Username": "",
  "Password": "",
  "ApiUrl": "https://weather-api.isun.ch/api/"
},
```

Later on you can use the created standalone console application like so:

```
isun.exe --cities Vilnius,Kaunas,Klaipėda
```
