using System;
using System.Collections.Generic;
using System.Linq;
using WhoWantsToBeAMillionaire.Models.Data.Quiz;
using WhoWantsToBeAMillionaire.Models.Data.Users;
using WhoWantsToBeAMillionaire.Models.Lifecycle.Users;

namespace WhoWantsToBeAMillionaire.Models.Data
{
    public static class DbInitializer
    {
        public static void Initialize(IRepository<User> userRepository, IRepository<Category> categoryRepository,
            IRepository<QuizQuestion> quizQuestionRepository, IRepository<QuizAnswer> quizAnswerRepository)
        {
            if (!categoryRepository.List.Any())
            {
                var automotives = new Category
                {
                    Name = "Automotives",
                    Questions = new List<QuizQuestion>
                    {
                        new QuizQuestion("Who invented Ferrari?",
                            "Enzo Ferrari", "Marco Ferrari", "Ferrrari Murcielago", "Ferruccio Ferrari"),
                        new QuizQuestion("What is both a French wine region and a luxury American automobile?",
                            "Cadillac", "Chevrolet", "Mercury", "Burgundy")
                    }
                };

                var travel = new Category
                {
                    Name = "Travel",
                    Questions = new List<QuizQuestion>
                    {
                        new QuizQuestion("Which is a German airline?",
                            "Lufthansa", "Air Germany", "JetBlue", "easyJet")
                    }
                };

                var geography = new Category
                {
                    Name = "Geography",
                    Questions = new List<QuizQuestion>
                    {
                        new QuizQuestion("How many stars does the American flag have?",
                            "50", "One", "48", "13"),
                        new QuizQuestion("In which European city can you find the home of Anne Frank?",
                            "Amsterdam", "Rotterdam", "Stockholm", "Copenhagen"),
                        new QuizQuestion("How long is the Great Wall of China?",
                            "21,000 km (13,000 miles)", "25,0000 km (15,000 miles)", "30,000 km (19,000 miles)",
                            "40,000 km (25,0000 miles)"),
                        new QuizQuestion("Which animal is on the Flemish flag?",
                            "Lion", "Bear", "Eagle", "Unicorn"),
                        new QuizQuestion("How many stars are featured on the flag of New Zealand?",
                            "Four", "13", "Ten", "Five"),
                        new QuizQuestion("What is the capital of Australia?",
                            "Canberra", "Sydney", "Melbourne", "Brisbane")
                    }
                };

                var history = new Category
                {
                    Name = "History",
                    Questions = new List<QuizQuestion>
                    {
                        new QuizQuestion("In which English town did Adolf Hitler study art?",
                            "Liverpool", "London", "Birmingham", "York"),
                        new QuizQuestion("Which Italian artist painted the Birth of Venus?",
                            "Botticelli", "Michelangelo", "Raphael", "Caravaggio"),
                        new QuizQuestion("Who was the original author of Dracula?",
                            "Bram Stoker", "Oscar Wilde", "Florence Balcombe", "Mary Shelley"),
                        new QuizQuestion("In which Spanish city die the Joan Miro museum open in 1975?",
                            "Barcelona", "Madrid", "Valencia", "Granada"),
                        new QuizQuestion("Which two months are named after Roman emperors?",
                            "July and August", "June and July", "August and September", "June and August"),
                        new QuizQuestion("In what year did Princess Diana die?",
                            "1997", "1995", "1989", "2001"),
                        new QuizQuestion("Which famous British murderer of the 19th century was never arrested?",
                            "Jack the Ripper", "Amelia Dyer", "Peter Sutcliffe", "Mary Ann Cotton")
                    }
                };

                var popCulture = new Category
                {
                    Name = "Pop Culture",
                    Questions = new List<QuizQuestion>
                    {
                        new QuizQuestion(
                            "Which actor is known for his roles in \"The Hurt Locker\", \"The Bourne Legacy\", and \"American Hustle\"?",
                            "Jeremy Renner", "Anthony Mackie", "Matt Damon", "Robert Downey Jr"),
                        new QuizQuestion(
                            "Which actor appeared in famous films, such as \"Gone in 60 Seconds\", \"Face/Off\", \"Ghost Rider\"",
                            "Nicholas Cage", "Giovanni Ribisi", "John Travolta", "Sam Elliott"),
                        new QuizQuestion("Which actor played James Bond in 1990?",
                            "Pierce Brosnan", "Sean Connery", "Roger Moore", "Timothy Dalton"),
                        new QuizQuestion(
                            "Which actor played the role of a famous fictional serial killer Hannibal Lecter?",
                            "Anthony Hopkins", "Gary Oldman", "Rhys Ifans", "Richard Brake"),
                        new QuizQuestion("Who is the most famous Hemsworth brother?",
                            "Chris Hemsworth", "Arnold Hemsworth", "Liam Hemsworth", "Luke Hemsworth"),
                        new QuizQuestion("Which actor was awarded Oscars for the films \"Glory\" and \"Training Day\"?",
                            "Denzel Washington", "Andre Braugher", "Matthew Broderick", "Ethan Hawke"),
                        new QuizQuestion("Which coffee chain did Madonna work at?",
                            "Dunkin Donuts", "Starbucks", "Tim Hortons", "McCafé"),
                        new QuizQuestion("Which actor traveled with the circus at the age of 15 and was a tamer?",
                            "Chuck Norris", "Scott Adkins", "Steven Seagal", "Donnie Yen")
                    }
                };


                var biology = new Category
                {
                    Name = "Biology",
                    Questions = new List<QuizQuestion>
                    {
                        new QuizQuestion("What animal has three hearts?",
                            "Octopus", "Squid", "Cockroach", "Hagfish"),
                        new QuizQuestion("What is the smallest bird?",
                            "Colibri", "Hummingbird", "Finch", "Goldcrest"),
                        new QuizQuestion("What is the fastest land animal?",
                            "Cheetah", "Greyhound", "Lion", "Springbok"),
                        new QuizQuestion("What is the thinnest natural thread?",
                            "Spider web", "Human hair", "Dog hair", "Horse hair"),
                        new QuizQuestion("Which birds have scales on their wings?",
                            "Penguins", "Ducks", "Swans", "Puffins"),
                        new QuizQuestion("Which animal has green-colored fat?",
                            "Crocodile", "Alligator", "Fox", "Dolphin"),
                        new QuizQuestion("What is the largest spider in the world?",
                            "Goliath Birdeater", "Giant Huntsman", "Grammostola Anthracina", "Colombian Giant Tarantula"),
                        new QuizQuestion("What is an evergreen tree or bush that grows in Australia?",
                            "Eucalyptus", "Bambus", "Peppermint", "Basil")
                    }
                };
                var culture = new Category
                {
                    Name = "Culture",
                    Questions = new List<QuizQuestion>
                    {
                        new QuizQuestion("What color are the domes of churches in Russia commonly?",
                            "Gold", "Red", "Silver", "Blue"),
                        new QuizQuestion("In which city is the famous Manneken Pis fountain?",
                            "Brussels", "Amsterdam", "Bruges", "Antwerp"),
                        new QuizQuestion("Which artist painted The Scream?",
                            "Edvard Munch", "Pablo Picasso", "Hans Gude", "Anders Zorn"),
                        new QuizQuestion("Which artist painted the Mona Lisa?",
                            "Leonardo Da Vinci", "Pablo Picasso", "Vincent van Gogh", "Claude Monet"),
                        new QuizQuestion("Which famous French Engineer designed two bridges for the city of Porto?",
                            "Gustave Eiffel", "Jules Dupuit", "Georges Claude", "Clément Ader"),
                        new QuizQuestion("In which city can you see Michelangelo's David?",
                            "Florence", "Rome", "Paris", "Berlin"),
                        new QuizQuestion("Who painted the ceiling of the Sistine Chapel?",
                            "Michelangelo", "Donatello", "Gian Lorenzo Bernini", "Amedeo Modigliani"),
                        new QuizQuestion("In which country was the famous painter El Greco born?",
                            "Greece", "Spain", "Turkey", "Italy"),
                        new QuizQuestion("In which city was the composer Frédéric Chopin buried?",
                            "Paris", "Marseille", "Lyon", "Nantes"),
                        new QuizQuestion("Which artist painted the famous painting Guernica?",
                            "Pablo Picasso", "Vincent van Gogh", "Leonardo Da Vinci", "Edvard Munch")
                    }
                };

                var englishLiterature = new Category
                {
                    Name = "English Literature",
                    Questions = new List<QuizQuestion>
                    {
                        new QuizQuestion("In which city did Romeo and Juliet live?",
                            "Verona", "Mantua", "Venice", "Florence")
                    }
                };

                var bigBangTheory = new Category
                {
                    Name = "Big Bang Theory",
                    Questions = new List<QuizQuestion>
                    {
                        new QuizQuestion("What did Sheldon and Amy win his Nobel Prize for?",
                            "Super-Asymmetry", "Dark Matter", "Proton Decay", "Neutrino"),
                        new QuizQuestion("What's the name of Howard and Bernadette's first child?",
                            "Halley", "Michael", "John", "Sarah"),
                        new QuizQuestion("What school did Leonard go to?",
                            "Princeton", "MIT", "Stanford", "Cambridge"),
                        new QuizQuestion("What school did Rajesh go to?",
                            "Cambridge", "MIT", "Stanford", "Princeton"),
                        new QuizQuestion("What school did Howard go to?",
                            "MIT", "Cambridge", "Stanford", "Princeton"),
                        new QuizQuestion("At what age did Sheldon receive his PhD?",
                            "16", "14", "17", "19"),
                        new QuizQuestion("In what state did Penny grow up?",
                            "Nebraska", "South Dakota", "Illinois", "Iowa")
                    }
                };

                var categories = new List<Category>
                {
                    automotives, travel, geography, history, popCulture, biology, culture, englishLiterature,
                    bigBangTheory
                };

                foreach (var category in categories)
                {
                    var categoryId = categoryRepository.Create(category);

                    foreach (var question in category.Questions)
                    {
                        question.CategoryId = categoryId;

                        var correctAnswers = question.Answers.Where(a => a.Correct);
                        if (correctAnswers.Count() != 1 || question.Answers.Count() != 4)
                        {
                            Console.WriteLine($"Question with invalid parameters found: {question.Question}");
                            continue;
                        }

                        var questionId = quizQuestionRepository.Create(question);

                        foreach (var answer in question.Answers)
                        {
                            answer.QuestionId = questionId;
                            quizAnswerRepository.Create(answer);
                        }
                    }
                }
            }

            if (!userRepository.List.Any())
            {
                var hasher = new PasswordHasher();

                hasher.GenerateSalt().HashPassword(hasher.Salt, "admin123");
                var admin = new User("TestAdmin", hasher.Salt, hasher.Hashed, true);

                hasher.GenerateSalt().HashPassword(hasher.Salt, "user123");
                var user = new User("TestUser", hasher.Salt, hasher.Hashed);

                userRepository.Create(admin);
                userRepository.Create(user);
            }
        }
    }
}