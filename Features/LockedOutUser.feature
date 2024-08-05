Feature: LockedOutUser

@regression
Scenario: Locked out user Login
	Given Navigate to saucedemo page
	When Login with locked_out_user user
	Then Validate locked out error message