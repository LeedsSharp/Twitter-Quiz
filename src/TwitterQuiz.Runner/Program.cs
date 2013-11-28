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
            var quiz = SelectQuiz(username);

            var exit = false;
            while (!exit)
            {
                Console.WriteLine("> Please select from one of the following options:");
                Console.WriteLine("");
                Console.WriteLine("1. Change Quiz");
                Console.WriteLine("2. Run Quiz");
                Console.WriteLine("3. Change username");
                Console.WriteLine("4. Exit");

                var input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        quiz = SelectQuiz(username);
                        // List Quizzes
                        break;
                    case "2":
                        if (quiz != new Quiz())
                        {
                            Console.WriteLine("> Starting quiz {0}...", quiz.Name);
                            Console.WriteLine("");
                            PlayQuiz(quiz);
                        }
                        else
                        {
                            Console.WriteLine("> Please first select a quiz");
                        }
                        break;
                    case "3":
                        username = SetUsername();
                        break;
                    case "4":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("> Unrecognised input...");
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
                Console.WriteLine("> Round {0}: {1}", round.Sequence, round.Name);
                Thread.Sleep(quiz.FrequencyOfQuestions * 500);
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
                    Console.WriteLine("> {0}. {1}", question.Sequence, question.Tweet);
                    Thread.Sleep(quiz.FrequencyOfQuestions * 1000);
                }
                Console.WriteLine("");
            }
            var quizEnded = new QuizEnded
                {
                    Host = quiz.Host,
                    Tweet = "> The quiz is now over"
                };
            _connection.AppendToStream(quizEnded, quiz.InternalName);
        }

        private static string SetUsername()
        {
            Console.WriteLine("> What is your username?");
            var username = Console.ReadLine().Trim();
            Console.WriteLine("> Hello {0}", username);
            Console.WriteLine("");
            return username;
        }

        private static Quiz SelectQuiz(string username)
        {
            Console.WriteLine("> Select which quiz you want to connect to by entering the id");
            Console.WriteLine("");
            Console.WriteLine("Id\t\tName");
            try
            {
                var quizzes = _quizLogic.GetQuizzes(username).ToList();

                foreach (var quiz in quizzes)
                {
                    Console.WriteLine("{0}\t\t{1}", quiz.Id, quiz.Name);
                }

                var input = Console.ReadLine();
                Console.WriteLine("");
                if (quizzes.Any(x => x.Id.ToString() == input))
                {
                    var quiz = quizzes.First(x => x.Id.ToString() == input);
                    Console.WriteLine("> Connected to {0}", quiz.Name);
                    return quiz;
                }
                Console.WriteLine("> Selection not recognised");
            }
            catch (Exception)
            {
                Console.WriteLine("> This user does not have any quizzes :(");
            }
            Console.WriteLine("");
            return new Quiz();
        }
    }
}
