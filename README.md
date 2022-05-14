# The API integration coding test for .NET

# Description
CompanyName has a Public API available at https://www.metaweather.com/api/ that you will use to get weather information for a specified city, 
including the current weather state (snow, heavy rain, heavy cloud, clear, etc.)

As an example, https://www.metaweather.com/api/location/44418/ returns a 5 day forecast for London. Note that to get the weather information for a location you need
to use a woeid (Where On Earth ID). There is a separate endpoint to search for locations https://www.metaweather.com/api/location/search/?query=london

The task is to create an application that accepts the name of a city as parameter. The application should then display the following current weather information:
- current weather state (snow, heavy rain, heavy cloud, clear, etc.)
- the current temperature
- the current wind direction
- the current wind speed
- humidity
- air pressure

# Platform choice
You can create the application as either a command line application, web application or mobile application in any of the following platforms:
- .NET (full framework or core), PHP, Ruby, Python or JavaScript for web application
- .NET (full framework or core), Ruby or Python for command line applications
- iOS, Android or Windows Mobile for mobile applications

Think about the type of work you would like to do at CompanyName and *choose an appropriate application type and platform*.

# Task requirements
Feel free to spend as much or as little time on the exercise as you like as long as the following requirements have been met:
- please complete the user story bellow
- your code should compile and run in one step
- feel free to use whatever framework / library / package you like
- you *must* include tests
- please avoid including artifacts	from your local build (Such as NuGet packages or the bin/obj folders) in your final ZIP file

# User story
As a *user running the application* I can *view the current weather information in a user submitted city name*, so that I know if I should bring an umbrella when leaving the house.

If you have chosen a native mobile application platform please also include the following:
As a user running the application I can view a current weather information logo (sun, clouds).
As a user running the application I can use the GPS to find my current city so I don't need to type it in.

# Acceptance criteria
- for the known city name, results are returned
- The current weather state, current temperature, the current wind direction, the current wind speed, the current humidity and the current air pressure are shown.