using FluentAssertions;
using Newtonsoft.Json.Linq;
using Reqnroll;
[assembly: Parallelizable(ParallelScope.Fixtures)]

namespace PlaywrightTest.Steps
{
    [Binding]
    internal class AnswersStepDefinitions
    {
        private JArray questions;
        private JArray question;

        [Given(@"I make a GET request")]
        public async Task MakeaGETRequest()
        {
            var client = new HttpClient();
            var httpResponse = await client.GetAsync("https://opentdb.com/api.php?amount=50&category=21&difficulty=medium&type=multiple");

            httpResponse.IsSuccessStatusCode.Should().BeTrue("because the API request should succeed");

            var content = await httpResponse.Content.ReadAsStringAsync();
            var response = JObject.Parse(content);

            response["results"].Should().NotBeNull("because the response should contain 'results'");

            questions = (JArray)response["results"];
        }

        [Then(@"I validate that all questions have 3 incorrect answers")]
        public void ValidateAllQuestionsHaveThreeIncorrectAnswers()
        {
            questions.Should().NotBeNull("questions should be populated from the API response");
            questions.Should().NotBeEmpty("there should be at least one question in the API response");

            foreach (var question in questions)
            {
                var incorrectAnswers = (JArray)question["incorrect_answers"];
                incorrectAnswers.Count.Should().Be(3, $"because question '{question["question"]}' should have exactly 3 incorrect answers.");
            }
        }

        [Then(@"I extract and print the correct answer for the question ""(.*)""")]
        public void ExtractAndPrintCorrectAnswerForSpecificQuestion(string specificQuestion)
        {
            var targetQuestion = questions.FirstOrDefault(q => q["question"].ToString().Contains(specificQuestion));

            targetQuestion.Should().NotBeNull($"because the question '{specificQuestion}' should exist in the API response");

            var correctAnswer = targetQuestion["correct_answer"].ToString();
            Console.WriteLine($"Question: {targetQuestion["question"]}, Correct Answer: {correctAnswer}");
        }

        

        [Given(@"Make a GET Request to ""(.*)""")]
        public async Task GetRequestAnswer(string urll)
        {
            var url = "https://opentdb.com/api.php?amount=10&category=9&difficulty=easy&type=boolean";
            var client = new HttpClient();
            var httpResponse = await client.GetAsync(url);

            httpResponse.IsSuccessStatusCode.Should().BeTrue("because the API request should succeed");

            var content = await httpResponse.Content.ReadAsStringAsync();

            Console.WriteLine("API Response: " + content);

            var response = JObject.Parse(content);

            response["results"].Should().NotBeNull("because the response should contain 'results'");

            question = (JArray)response["results"];
        }

        [Then(@"validate that at least one question has correct_answer: True")]
        public async Task ValidateAnswerTrue()
        {
            question.Should().NotBeNull("because questions should be populated from the API response");

            question.Should().NotBeEmpty("because there should be at least one question in the API response");

            var trueAnswers = question.Where(q => q["correct_answer"].ToString() == "True").ToList();

            trueAnswers.Should().NotBeEmpty("because at least one question should have 'correct_answer' set to 'True'");

            foreach (var question in trueAnswers)
            {
                Console.WriteLine($"Question: {question["question"]}, Correct Answer: {question["correct_answer"]}");
            }
        }
    }
}
