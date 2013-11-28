using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading;
using EventStore.ClientAPI;
using TwitterQuiz.Domain.QuizEvents;
using TwitterQuiz.EventStore;
using TwitterQuiz.EventStore.Logic;

namespace TwitterQuiz.FakeAnswerer
{
    internal class Contestant
    {
        public string Username { get; set; }
        public string ImageUrl { get; set; }
    }
    public class Program
    {
        private static readonly Contestant[] Contenstants = new[]
            {
                new Contestant { Username = "relentlessdev", ImageUrl ="https://pbs.twimg.com/profile_images/648313920/relentlessmonkey_nobg_bigger.png" },
                new Contestant { Username = "alexdgarland", ImageUrl ="https://pbs.twimg.com/profile_images/378800000708021861/b9928ff4470f54dcb6635cd476261988_bigger.png" },
                new Contestant { Username = "ritasker", ImageUrl ="https://pbs.twimg.com/profile_images/2865070744/c2c67b52dd7cf3bc0058e14644276d89_bigger.png" },
                new Contestant { Username = "_richardg", ImageUrl ="https://pbs.twimg.com/profile_images/1215470063/Me_bigger.jpg" },
                new Contestant { Username = "pauljstead", ImageUrl ="https://pbs.twimg.com/profile_images/378800000012468441/4f5022abc02f5546d13c019717ed5153_bigger.jpeg" },
                new Contestant { Username = "mat_mcloughlin", ImageUrl ="https://pbs.twimg.com/profile_images/1569677619/Invalid-Arg-Doodle2_vectorized_bigger.png" },
                new Contestant { Username = "autonomatt", ImageUrl ="https://pbs.twimg.com/profile_images/3160770254/720c92aa87908f1bc12ffe0985037b7a_bigger.jpeg" },
                new Contestant { Username = "macsdickinson", ImageUrl ="https://pbs.twimg.com/profile_images/2150827924/248034_10150617276055442_512260441_18597238_7286119_n_bigger.jpg" },
                new Contestant { Username = "chrisdobby", ImageUrl ="https://pbs.twimg.com/profile_images/2746389391/7dbeb26e5f4ae62904c1699e8730c3d5_bigger.jpeg" },
            };

        private static IEventStoreConnection _connection;
        private static QuizLogic _quizLogic;
        static void Main(string[] args)
        {
            _connection = EventStoreConnectionProvider.EventStore;
            _connection.Connect();
            _quizLogic = new QuizLogic(_connection);

            bool exit = false;
            int counter = 0;
            var username = SetUsername();
            var selectedQuiz = SelectQuiz(username);

            bool showMenu = true;

            while (!exit)
            {
                if (showMenu)
                {
                    Console.WriteLine("");
                    Console.WriteLine("Please select from one of the following options:");
                    Console.WriteLine("");
                    Console.WriteLine("1. Connect to quiz");
                    Console.WriteLine("2. Submit fake answer");
                    Console.WriteLine("3. Auto-run");
                    Console.WriteLine("4. Change username");
                    Console.WriteLine("5. Exit"); 
                }
                

                var input = Console.ReadLine();
                if (!showMenu && string.IsNullOrEmpty(input))
                {
                    input = "2";
                }

                if (input != null)
                    switch (input.ToLower())
                    {
                        case "1":
                            selectedQuiz = SelectQuiz(username);
                            showMenu = true;
                            // List Quizzes
                            break;
                        case "2":
                            // create fake answer
                            counter = CreateFakeAnswer(selectedQuiz, counter);
                            showMenu = false;
                            break;
                        case "3":
                            while (true)
                            {
                                counter = CreateFakeAnswer(selectedQuiz, counter);
                                Thread.Sleep(1000);
                                if (Console.KeyAvailable)
                                {
                                    break;
                                }
                            }
                            showMenu = true;
                            break;
                        case "4":
                            username = SetUsername();
                            showMenu = true;
                            break;
                        case "5":
                            exit = true;
                            break;
                        default:
                            Console.WriteLine("> Unrecognised input");
                            break;
                    }
            }
        }

        private static int CreateFakeAnswer(string selectedQuiz, int counter)
        {
            if (!string.IsNullOrEmpty(selectedQuiz))
            {
                var answer = new AnswerReceived
                    {
                        Answer = "Nonsense answer",
                        ImageUrl = Contenstants[counter%Contenstants.Length].ImageUrl,
                        Username = Contenstants[counter%Contenstants.Length].Username
                    };
                _quizLogic.CreateFakeAnswer(answer, selectedQuiz);
                Console.WriteLine("> @{0} answered a question", Contenstants[counter%Contenstants.Length].Username);
                counter++;
            }
            else
            {
                Console.WriteLine("> Please first select a quiz");
            }
            return counter;
        }

        private static string SetUsername()
        {
            Console.WriteLine("> What is your username?");
            Console.WriteLine("");
            var username = Console.ReadLine().Trim();
            Console.WriteLine("> Hello {0}", username);
            return username;
        }

        private static string SelectQuiz(string username)
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
                    return quiz.InternalName;
                }
                Console.WriteLine("> Selection not recognised");
            }
            catch (Exception)
            {
                Console.WriteLine("> This user does not have any quizzes :(");
            }
            Console.WriteLine("");
            return string.Empty;
        }
    }
}
