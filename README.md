# PayApp
## How to run the Application

#### Application Run command 
 
 > **PayApp.Main.Console.exe** *[rates_file_path]* *[paypackets_file_path]*
 
 > **Example :** PayApp.Main.Console.exe Data\rates.csv Data\testdata.csv
 
 > **NOTE :** I have provided both rates.csv and testdata.csv in the Data Folder 
		  within the PayApp.Main.Console

#### To run the application please follow these steps:

	1. Download the solution as zip and unzip to desired location
	
	2. Open solution in Visual Studio 2015 and un-check any untrusted message that 
	   pop up
	
	3. Build the solution to restore all the nuget packages  
	
	4. Test Runner and test cases, 
		1. If Test Explorer is not open, click on Test -> Windows -> Test Explorer
		2. Click on the run all tests link
		3. All the tests should pass
	
	5. Running from Visual Studio, right click on PayApp.Main.Console project 
		1. Click on Properties
		1. In the new window, go to Debug Tab
		2. Find Command Line Arguments box
		3. Paste this "Data\rates.csv Data\testdata.csv" without quotes
		4. Click the Start to debug and see the application run
	
	6 Running in Command Prompt 
		1. Go to location of PayApp.Main.Console.exe
		2. Execute command PayApp.Main.Console.exe [rates_file_path] [paypackets_file_path]

## Assumptions 
	1. The CSV file data does not required sorting, filtering and does not have duplicates
	
	2. The data within the file will be in the correct structure and completeness
	
	3. All months within the CSV file as between July 2012 and June 2013, I have altered 
	   the CSV to include the year for the monthly date range
	
	4. Pay period will always be monthly and across one month, we currently don't have a scenario 
	   where a pay period could span two months or alternatively a fortnight
	
	5. Customers don't have unique identifiers so duplicate entries will treated as unique 
	   customers
	
## Decisions

	1. The console application is built as a test harness and the end user will need to provide 
	   two CSV files as command line arguments for rates and pay packets respectively
	   
	2. The output will be printed to console instead of written out to file 
	   
	3. Input files will exclude the header definition row or line as not necessary for mapping.
	   
	4. CSV for pay packets now will have the year incorporated in the date, this will be used 
	   to ensure rates within the date range are used to calculate the tax 
	   
	5. Any input lines on which have unforeseen errors are caught and logged. The application will
	   not stop running and continue to next line in the CSV for processing.
	
	6. Excluded the use of persistence (EF and SQL) to simplify the solution especially with 
	   regards to tax rates these will be stored in memory
	
	7. Given customers don't have unique identifiers any duplicate entries will treated as unique 
	   customers and processed independently
	
	8. Based on point 3 and 4, we have factored out properties such as id's and navigation 
	   properties within our domain models 
	
	9. Each Customer has one Pay Period(Monthly)
		- Realistically a customer can have more than one Pay Period and should be represented by
		  by a collection
		- Limitations due to no unique customer id in place, we will currently have a one to one 
		  relationship
	
	10. Each PayPeriod has a Salary Package that holds AnnualGrossIncome and SuperAnnuationRate
	
	11.	Solution structure is as follows:
		 - PayApp.Core - has core models, extensions and validators
		 - PayApp.Core.Presentation - has viewmodels and extension 
		 - PayApp.Data - basically in memory Rates DB and accessor methods, it can be extended to 
						 hold EF configuration, Repositories/DBContext
		 - PayApp.Service - has application services and business logic
		 - PayApp.Main.Console - this is the start up project and test harness
		 - PayApp.Test - has test to verify the layers built excluding the main project
	
	12. Key Technologies:
		 - C#
		 - Unity
		 - Fluent-Validation
		 - Xunit v2
		 - AutoFixture with xUnit.net v2 data theories
		 - AutoFixture with Auto Mocking using Moq
		 