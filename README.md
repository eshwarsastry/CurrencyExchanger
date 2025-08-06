## CurrencyExchanger

## CurrencyExchanger is a .NET 8.0 project that provides functionality for currency conversion.

# Packages required
- Microsoft.Extensions.DependencyInjection
- Microsoft.Extensions.Http
- xunit 

# Design Pricliples implemented
- Dependency Injection
- Onion architecture- Segregation of data fetching and business logic
- Unit testing

# Setup the project by following these steps:

1. Clone the repository:
   ```bash
   git clone
	```
2. Store the "FASTFOREX_API_KEY" in the environment variables:
   ```cmd
   set FASTFOREX_API_KEY="your_api_key_here"
   ```
	This API key is required for accessing the currency conversion service. If the key is not set, then the application is going to run using the 
	default exchange rates. 
	If the API is not set using the command, use the GUI Settings to set the API Key.
	
This may require you to restart the Visual Studio application at times after setting the environment variable.

3. Run the project.
   You can run the project using Visual Studio or the .NET CLI:
   ```bash
   dotnet run
   ```
