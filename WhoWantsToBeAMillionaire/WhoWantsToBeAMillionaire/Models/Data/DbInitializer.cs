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
            if (!userRepository.List.Any())
            {
                var hasher = new PasswordHasher();

                hasher.GenerateSalt().HashPassword(hasher.Salt, "admin123");
                var admin = new User("TestAdmin", hasher.Salt, hasher.Hashed);

                hasher.GenerateSalt().HashPassword(hasher.Salt, "user123");
                var user = new User("TestUser", hasher.Salt, hasher.Hashed);

                userRepository.Create(admin);
                userRepository.Create(user);
            }

            if (!categoryRepository.List.Any())
            {
                var automotives = new Category
                {
                    Name = "Automotives",
                    Questions = new List<QuizQuestion>
                    {
                        new QuizQuestion
                        {
                            Question = "Who invented Ferrari?",
                            Answers = new List<QuizAnswer>
                            {
                                new QuizAnswer("Enzo Ferrari", true),
                                new QuizAnswer("Marco Ferrari"),
                                new QuizAnswer("Ferrrari Murcielago"),
                                new QuizAnswer("Ferruccio Ferrari")
                            }
                        },
                        new QuizQuestion
                        {
                            Question = "What is both a French wine region and a luxury American automobile?",
                            Answers = new List<QuizAnswer>
                            {
                                new QuizAnswer("Cadillac", true),
                                new QuizAnswer("Chevrolet"),
                                new QuizAnswer("Mercury"),
                                new QuizAnswer("Burgundy")
                            }
                        }
                    }
                };

                var travel = new Category
                {
                    Name = "Travel",
                    Questions = new List<QuizQuestion>
                    {
                        new QuizQuestion
                        {
                            Question = "Which is a German airline?",
                            Answers = new List<QuizAnswer>
                            {
                                new QuizAnswer("Lufthansa", true),
                                new QuizAnswer("Air Germany"),
                                new QuizAnswer("JetBlue"),
                                new QuizAnswer("easyJet")
                            }
                        }
                    }
                };

                var geography = new Category
                {
                    Name = "Geography",
                    Questions = new List<QuizQuestion>
                    {
                        new QuizQuestion
                        {
                            Question = "How many stars does the American flag have?",
                            Answers = new List<QuizAnswer>
                            {
                                new QuizAnswer("50", true),
                                new QuizAnswer("One"),
                                new QuizAnswer("48"),
                                new QuizAnswer("13")
                            }
                        },
                        new QuizQuestion
                        {
                            Question = "In which European city can you find the home of Anne Frank?",
                            Answers = new List<QuizAnswer>
                            {
                                new QuizAnswer("Amsterdam", true),
                                new QuizAnswer("Rotterdam"),
                                new QuizAnswer("Stockholm"),
                                new QuizAnswer("Copenhagen")
                            }
                        },
                        new QuizQuestion
                        {
                            Question = "How long is the Great Wall of China?",
                            Answers = new List<QuizAnswer>
                            {
                                new QuizAnswer("21,000 km (13,000 miles)", true),
                                new QuizAnswer("25,0000 km (15,000 miles)"),
                                new QuizAnswer("30,000 km (19,000 miles)"),
                                new QuizAnswer("40,000 km (25,0000 miles)")
                            }
                        },
                        new QuizQuestion
                        {
                            Question = "Which animal is on the Flemish flag?",
                            Answers = new List<QuizAnswer>
                            {
                                new QuizAnswer("Lion", true),
                                new QuizAnswer("Bear"),
                                new QuizAnswer("Eagle"),
                                new QuizAnswer("Unicorn")
                            }
                        },
                        new QuizQuestion
                        {
                            Question = "How many stars are featured on the flag of New Zealand?",
                            Answers = new List<QuizAnswer>
                            {
                                new QuizAnswer("Four", true),
                                new QuizAnswer("13"),
                                new QuizAnswer("Ten"),
                                new QuizAnswer("Five")
                            }
                        }
                    }
                };

                var history = new Category
                {
                    Name = "History",
                    Questions = new List<QuizQuestion>
                    {
                        new QuizQuestion
                        {
                            Question = "Which two months are named after Roman emperors?",
                            Answers = new List<QuizAnswer>
                            {
                                new QuizAnswer("July and August", true),
                                new QuizAnswer("June and July"),
                                new QuizAnswer("August and September"),
                                new QuizAnswer("June and August")
                            }
                        },
                        new QuizQuestion
                        {
                            Question = "In what year did Princess Diana die?",
                            Answers = new List<QuizAnswer>
                            {
                                new QuizAnswer("1997", true),
                                new QuizAnswer("1995"),
                                new QuizAnswer("1989"),
                                new QuizAnswer("2001")
                            }
                        },
                        new QuizQuestion
                        {
                            Question = "Which famous British murderers of the 19th century was never arrested?",
                            Answers = new List<QuizAnswer>
                            {
                                new QuizAnswer("Jack the Ripper", true),
                                new QuizAnswer("Amelia Dyer"),
                                new QuizAnswer("Peter Sutcliffe"),
                                new QuizAnswer("Mary Ann Cotton")
                            }
                        }
                    }
                };

                var popCulture = new Category
                {
                    Name = "Pop Culture",
                    Questions = new List<QuizQuestion>
                    {
                        new QuizQuestion
                        {
                            Question =
                                "Which actor is known for his roles in \"The Hurt Locker\", \"The Bourne Legacy\", and \"American Hustle\"?",
                            Answers = new List<QuizAnswer>
                            {
                                new QuizAnswer("Jeremy Renner", true),
                                new QuizAnswer("Anthony Mackie"),
                                new QuizAnswer("Matt Damon"),
                                new QuizAnswer("Robert Downey Jr")
                            }
                        },
                        new QuizQuestion
                        {
                            Question =
                                "Which actor appeared in famous films, such as \"Gone in 60 Seconds\", \"Face/Off\", \"Ghost Rider\"",
                            Answers = new List<QuizAnswer>
                            {
                                new QuizAnswer("Nicholas Cage", true),
                                new QuizAnswer("Giovanni Ribisi"),
                                new QuizAnswer("John Travolta"),
                                new QuizAnswer("Sam Elliott")
                            }
                        },
                        new QuizQuestion
                        {
                            Question = "Which actor played James Bond in 1990?",
                            Answers = new List<QuizAnswer>
                            {
                                new QuizAnswer("Pierce Brosnan", true),
                                new QuizAnswer("Sean Connery"),
                                new QuizAnswer("Roger Moore"),
                                new QuizAnswer("Timothy Dalton")
                            }
                        },
                        new QuizQuestion
                        {
                            Question =
                                "Which actor played the role of a famous fictional serial killer Hannibal Lecter?",
                            Answers = new List<QuizAnswer>
                            {
                                new QuizAnswer("Anthony Hopkins", true),
                                new QuizAnswer("Gary Oldman"),
                                new QuizAnswer("Rhys Ifans"),
                                new QuizAnswer("Richard Brake")
                            }
                        },
                        new QuizQuestion
                        {
                            Question = "Who is the most famous Hemsworth brother?",
                            Answers = new List<QuizAnswer>
                            {
                                new QuizAnswer("Chris Hemsworth", true),
                                new QuizAnswer("Arnold Hemsworth"),
                                new QuizAnswer("Liam Hemsworth"),
                                new QuizAnswer("Luke Hemsworth")
                            }
                        },
                        new QuizQuestion
                        {
                            Question = "Which actor was awarded Oscars for the films \"Glory\" and \"Training Day\"?",
                            Answers = new List<QuizAnswer>
                            {
                                new QuizAnswer("Denzel Washington", true),
                                new QuizAnswer("Andre Braugher"),
                                new QuizAnswer("Matthew Broderick"),
                                new QuizAnswer("Ethan Hawke")
                            }
                        },
                        new QuizQuestion
                        {
                            Question = "Which coffee chain did Madonna work at?",
                            Answers = new List<QuizAnswer>
                            {
                                new QuizAnswer("Dunkin Donuts", true),
                                new QuizAnswer("Starbucks"),
                                new QuizAnswer("Tim Hortons"),
                                new QuizAnswer("McCafé")
                            }
                        },
                        new QuizQuestion
                        {
                            Question = "Which actor traveled with the circus at the age of 15 and was a tamer?",
                            Answers = new List<QuizAnswer>
                            {
                                new QuizAnswer("Chuck Norris", true),
                                new QuizAnswer("Scott Adkins"),
                                new QuizAnswer("Steven Seagal"),
                                new QuizAnswer("Donnie Yen")
                            }
                        }
                    }
                };

                var animals = new Category
                {
                    Name = "Animals",
                    Questions = new List<QuizQuestion>
                    {
                        new QuizQuestion
                        {
                            Question = "What animal has three hearts?",
                            Answers = new List<QuizAnswer>
                            {
                                new QuizAnswer("Octopus", true),
                                new QuizAnswer("Squid"),
                                new QuizAnswer("Cockroach"),
                                new QuizAnswer("Hagfish")
                            }
                        },
                        new QuizQuestion
                        {
                            Question = "What is the smallest bird?",
                            Answers = new List<QuizAnswer>
                            {
                                new QuizAnswer("Colibri", true),
                                new QuizAnswer("Hummingbird"),
                                new QuizAnswer("Finch"),
                                new QuizAnswer("Goldcrest")
                            }
                        },
                        new QuizQuestion
                        {
                            Question = "What is the fastest land animal?",
                            Answers = new List<QuizAnswer>
                            {
                                new QuizAnswer("Cheetah", true),
                                new QuizAnswer("Greyhound"),
                                new QuizAnswer("Lion"),
                                new QuizAnswer("Springbok")
                            }
                        },
                        new QuizQuestion
                        {
                            Question = "What is the thinnest natural thread?",
                            Answers = new List<QuizAnswer>
                            {
                                new QuizAnswer("Spider web", true),
                                new QuizAnswer("Human hair"),
                                new QuizAnswer("Dog hair"),
                                new QuizAnswer("Horse hair")
                            }
                        },
                        new QuizQuestion
                        {
                            Question = "Which birds have scales on their wings?",
                            Answers = new List<QuizAnswer>
                            {
                                new QuizAnswer("Penguins", true),
                                new QuizAnswer("Ducks"),
                                new QuizAnswer("Swans"),
                                new QuizAnswer("Puffins")
                            }
                        },
                        new QuizQuestion
                        {
                            Question = "Which animal has green-colored fat?",
                            Answers = new List<QuizAnswer>
                            {
                                new QuizAnswer("Crocodile", true),
                                new QuizAnswer("Alligator"),
                                new QuizAnswer("Fox"),
                                new QuizAnswer("Dolphin")
                            }
                        },
                        new QuizQuestion
                        {
                            Question = "What is the largest spider in the world?",
                            Answers = new List<QuizAnswer>
                            {
                                new QuizAnswer("Goliath Birdeater", true),
                                new QuizAnswer("Giant Huntsman"),
                                new QuizAnswer("Grammostola Anthracina"),
                                new QuizAnswer("Colombian Giant Tarantula")
                            }
                        }
                    }
                };

                var categories = new List<Category> {automotives, travel, geography, history, popCulture, animals};

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
        }
    }
}