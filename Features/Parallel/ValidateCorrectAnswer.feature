Feature: Validate Correct Answer is True

@ParallelTests
Scenario: Validate existence of correct_answer as true
Given Make a GET Request to "https://opentdb.com/api.php?amount=10&category=9&difficulty=easy&type=boolean"
Then validate that at least one question has correct_answer: True