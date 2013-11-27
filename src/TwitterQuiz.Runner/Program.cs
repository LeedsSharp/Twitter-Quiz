using System;
using System.Linq;
using System.Threading;
using EventStore.ClientAPI;
using TwitterQuiz.Domain;
using TwitterQuiz.Domain.QuizEvents;
using TwitterQuiz.EventStore;
using TwitterQuiz.EventStore.Logic;

namespace TwitterQuiz.Runner
{
    public class Program
    {
        private static IEventStoreConnection _connection;
        private static QuizLogic _quizLogic;
        static void Main(string[] args)
        {
            _connection = EventStoreConnectionProvider.EventStore;
            _connection.Connect();
            _quizLogic = new QuizLogic(_connection);

            Console.ReadLine();
            Console.WriteLine("");
            Console.WriteLine(@"                     ___          ___                   ___     ");
            Console.WriteLine(@"                    /\  \        /\__\        ___      /\  \    ");
            Console.WriteLine(@"                   /::\  \      /:/  /       /\  \     \:\  \   ");
            Console.WriteLine(@"                  /:/\:\  \    /:/  /        \:\  \     \:\  \  ");
            Console.WriteLine(@"                  \:\~\:\  \  /:/  /  ___    /::\__\     \:\  \ ");
            Console.WriteLine(@"                   \:\ \:\__\/:/__/  /\__\__/:/\/__/______\:\__\");
            Console.WriteLine(@"                    \:\/:/  /\:\  \ /:/  /\/:/  /  \::::::::/__/");
            Console.WriteLine(@"                     \::/  /  \:\  /:/  /\::/__/    \:\~~\~~    ");
            Console.WriteLine(@"                     /:/  /    \:\/:/  /  \:\__\     \:\  \     ");
            Console.WriteLine(@"                    /:/  /      \::/  /    \/__/      \:\__\    ");
            Console.WriteLine(@"                    \/__/        \/__/                 \/__/                   ");
            Console.WriteLine(@"      ___          ___          ___          ___          ___          ___     ");
            Console.WriteLine(@"     /\  \        /\__\        /\__\        /\__\        /\  \        /\  \    ");
            Console.WriteLine(@"    /::\  \      /:/  /       /::|  |      /::|  |      /::\  \      /::\  \   ");
            Console.WriteLine(@"   /:/\:\  \    /:/  /       /:|:|  |     /:|:|  |     /:/\:\  \    /:/\:\  \  ");
            Console.WriteLine(@"  /::\~\:\  \  /:/  /  ___  /:/|:|  |__  /:/|:|  |__  /::\~\:\  \  /::\~\:\  \ ");
            Console.WriteLine(@" /:/\:\ \:\__\/:/__/  /\__\/:/ |:| /\__\/:/ |:| /\__\/:/\:\ \:\__\/:/\:\ \:\__\");
            Console.WriteLine(@" \/_|::\/:/  /\:\  \ /:/  /\/__|:|/:/  /\/__|:|/:/  /\:\~\:\ \/__/\/_|::\/:/  /");
            Console.WriteLine(@"    |:|::/  /  \:\  /:/  /     |:/:/  /     |:/:/  /  \:\ \:\__\     |:|::/  / ");
            Console.WriteLine(@"    |:|\/__/    \:\/:/  /      |::/  /      |::/  /    \:\ \/__/     |:|\/__/  ");
            Console.WriteLine(@"    |:|  |       \::/  /       /:/  /       /:/  /      \:\__\       |:|  |    ");
            Console.WriteLine(@"     \|__|        \/__/        \/__/        \/__/        \/__/        \|__|    ");
            Console.WriteLine("");
            Console.WriteLine("");

            var username = SetUsername();

            var exit = false;
            while (!exit)
            {
                Console.WriteLine("Please select from one of the following options:");
                Console.WriteLine("");
                Console.WriteLine("1. List Quizzes");
                Console.WriteLine("2. Run Quiz");
                Console.WriteLine("3. Change username");
                Console.WriteLine("4. Exit");

                var input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        ListQuizzes(username);
                        // List Quizzes
                        break;
                    case "2":
                        var quiz = GetQuiz(username);
                        Console.WriteLine("Startin quiz {0}...", quiz.Name);
                        Console.WriteLine("");
                        PlayQuiz(quiz);
                        // Start Quiz
                        break;
                    case "3":
                        username = SetUsername();
                        break;
                    case "4":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Unrecognised input...");
                        Console.WriteLine("");
                        break;
                }
            }
        }

        private static void PlayQuiz(Quiz quiz)
        {
            foreach (var round in quiz.Rounds.OrderBy(x => x.Sequence))
            {
                var roundTweet = new RoundStarted
                    {
                        Host = quiz.Host,
                        Round = round.Sequence,
                        Tweet = string.Format("Round {0}: {1}", round.Sequence, round.Name)
                    };
                _connection.AppendToStream(roundTweet, quiz.InternalName);
                // Send tweet
                Console.WriteLine("Round {0} - {1}", round.Sequence, round.Name);
                Console.WriteLine("");
                foreach (var question in round.Questions.OrderBy(x => x.Sequence))
                {
                    var questionTweet = new QuestionSent
                        {
                            Host = quiz.Host,
                            Question = question.Sequence,
                            Round = round.Sequence,
                            Tweet = question.Tweet
                        };
                    _connection.AppendToStream(questionTweet, quiz.InternalName);
                    // Send tweet
                    Console.WriteLine("{0}. {1}", question.Sequence, question.Tweet);
                    Thread.Sleep(quiz.FrequencyOfQuestions * 6000);
                }
                Console.WriteLine("");
            }
            var quizEnded = new QuizEnded
                {
                    Host = quiz.Host,
                    Tweet = "The quiz is now over"
                };
            _connection.AppendToStream(quizEnded, quiz.InternalName);
        }

        private static string SetUsername()
        {
            Console.WriteLine("What is your username?");
            Console.WriteLine("");
            var username = Console.ReadLine().Trim();
            Console.WriteLine("Hello {0}", username);
            Console.WriteLine("");
            return username;
        }

        private static Quiz GetQuiz(string username)
        {
            var quiz = new Quiz();
            var quizFound = false;
            while (!quizFound)
            {
                Console.WriteLine("");
                Console.WriteLine("Enter Quiz Id:");

                var input = Console.ReadLine();

                if (input == "cancel")
                {
                    break;
                }
                var quizId = 0;
                if (!int.TryParse(input, out quizId))
                {
                    Console.WriteLine("No quiz found with Id of {0} - type cancel to go back to the menu", input);
                }
                else
                {
                    int id = quizId;
                    var quizzes = _quizLogic.GetQuizzes(username);
                    if (quizzes.Any(x => x.Id == id))
                    {
                        quiz = quizzes.First(x => x.Id == id);
                        quizFound = true;
                    }
                    else
                    {
                        Console.WriteLine("No quiz found with Id of {0} - type cancel to go back to the menu", input);
                    }
                }
            }
            return quiz;
        }

        private static void ListQuizzes(string username)
        {
            Console.WriteLine("");
            Console.WriteLine("Listing Quizzes...");
            Console.WriteLine("");
            Console.WriteLine("Id\t\tName");
            try
            {
                var quizzes = _quizLogic.GetQuizzes(username);

                foreach (var quiz in quizzes)
                {
                    Console.WriteLine("{0}\t\t{1}", quiz.Id, quiz.Name);
                }
            }
            catch (Exception)
            {
                Console.WriteLine("This user does not have any quizzes :(");
            }
            finally
            {
                Console.WriteLine("");
            }
        }
    }
}
