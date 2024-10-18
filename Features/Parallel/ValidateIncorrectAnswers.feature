Feature: Validate Questions with Three Incorrect Answers

@ParallelTests
  Scenario: Validate that all questions have 3 incorrect answers
    Given I make a GET request
    Then I validate that all questions have 3 incorrect answers

@ParallelTests
  Scenario: Extract and print the correct answer for a specific question
    Given I make a GET request
    Then I extract and print the correct answer for the question "Which car manufacturer won the 2017 24 Hours of Le Mans?"