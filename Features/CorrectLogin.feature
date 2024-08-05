Feature: CorrectLogin

@smoke
Scenario: Correct Login
	Given Navigate to saucedemo page
	When Login with standard_user user
	Then Validate the inventory is loaded