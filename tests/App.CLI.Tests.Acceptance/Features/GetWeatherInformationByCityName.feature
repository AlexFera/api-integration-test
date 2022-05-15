Feature: Get weather information by city name

As a user
I want to get the weather information for the specified city name

Scenario: User should get the weather information by city name
	Given the city name Madrid
    And the following locations returned by that city
    | Title  | LocationType | WhereOnEarthID |
    | Madrid | City         | 766273         |
    And the following current weather information for that city
        | WeatherStateName | WindDirection | Temperature | AirPressure | Humidity |
        | Light Rain       | ESE           | 17.815      | 1016        | 68       |
        | Light Rain       | SW            | 20.25000    | 1015        | 63       |
        | Heavy Cloud      | SSE           | 24.13       | 1019        | 48       |
	When a user gets the weather information for that city
	Then weather information is returned

Scenario: City name is invalid
	Given the city name 666
     And the following locations returned by that city
    | Title  | LocationType | WhereOnEarthID |
    | Madrid | City         | 766273         |
    And the following current weather information for that city
        | WeatherStateName | WindDirection | Temperature | AirPressure | Humidity |
        | Light Rain       | ESE           | 17.815      | 1016        | 68       |
        | Light Rain       | SW            | 20.25000    | 1015        | 63       |
        | Heavy Cloud      | SSE           | 24.13       | 1019        | 48       |
	When a user gets the weather information for that city
	Then the error Please enter a valid city name is returned
