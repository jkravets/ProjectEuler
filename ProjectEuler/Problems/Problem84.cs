using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectEuler.Problems
{
    public class Problem84
    {

        public GameBoard Board { get; set; }

        public Problem84()
        {
            Board = new GameBoard();
        }

        //Calculates the probability for each square
        public List<Tuple<Square,double>> Simulate(int numRolls, int diceSides)
        {
            var timesVisited = Board.Squares.ToDictionary(s => s, s => 0);

            var random = new Random();

            var wasDoubleMinus1 = false;
            var wasDoubleMinus2 = false;

            var currentSquare = Board.Squares.First();

            var rofl = Board.NextSquare(currentSquare, 37);

            for (int i = 0; i < numRolls; i++)
            {
                var dice1 = random.Next(1, diceSides + 1);
                var dice2 = random.Next(1, diceSides + 1);

                var isDouble = dice1 == dice2;

                Square nextSquare = null;
                //If three doubles are rolled go directly to jail and restart the double count
                if (isDouble && wasDoubleMinus1 && wasDoubleMinus2)
                {
                    nextSquare = Board.JAIL;
                    wasDoubleMinus1 = false;
                    wasDoubleMinus2 = false;
                }
                //Now check landing
                else
                {
                    nextSquare = Board.NextSquare(currentSquare, dice1 + dice2);

                    //Handle for warps
                    if (nextSquare == Board.G2J)
                    {
                        nextSquare = Board.JAIL;
                    }
                    else if (nextSquare.Name.StartsWith("CC"))
                    {
                        nextSquare = Board.NextCC(nextSquare);
                    }
                    else if (nextSquare.Name.StartsWith("CH"))
                    {
                        nextSquare = Board.NextCH(nextSquare);
                    }
                    wasDoubleMinus2 = wasDoubleMinus1;
                    wasDoubleMinus1 = isDouble;

                }

                
                timesVisited[nextSquare]++;
                currentSquare = nextSquare;
                

            }


            return timesVisited.Select(kvp => new Tuple<Square, double>(kvp.Key, (double)kvp.Value/(numRolls + 0.000000001)))
                            .OrderByDescending(tuple => tuple.Item2).ToList();

        }
                


        public List<KeyValuePair<string,int>> Answer()
        {
            var results = new List<string>();

            int numberGames = 100;
            int gameNumberTurns = 100000;
            int diceSides = 4;

            for (int i = 0; i < numberGames; i++)
            {
                var probabilities = Simulate(gameNumberTurns, diceSides);
                results.Add(probabilities.Take(3).Aggregate("", (module, tuple) => module + tuple.Item1.Id));
            }
            
            return results.Distinct().ToDictionary(r => r, r => results.Count(s => s == r)).OrderByDescending(k => k.Value).ToList();;
        }


        public class GameBoard
        {
            public List<Square> Squares { get; set; }


            public Square G2J { get; set; }
            public Square JAIL { get; set; }
            public Square CC { get; set; }
            public Square CH { get; set; }


           

            public GameBoard()
            {
                Squares = new List<Square>()
                {
                    new Square("00","GO"),
                    new Square("01","A1"),
                    new Square("02","CC1"),
                    new Square("03","A2"),
                    new Square("04","T1"),
                    new Square("05","R1"),
                    new Square("06","B1"),
                    new Square("07","CH1"),
                    new Square("08","B2"),
                    new Square("09","B3"),
                    new Square("10","JAIL"),
                    new Square("11","C1"),
                    new Square("12","U1"),
                    new Square("13","C2"),
                    new Square("14","C3"),
                    new Square("15","R2"),
                    new Square("16","D1"),
                    new Square("17","CC2"),
                    new Square("18","D2"),
                    new Square("19","D3"),
                    new Square("20","FP"),
                    new Square("21","E1"),
                    new Square("22","CH2"),
                    new Square("23","E2"),
                    new Square("24","E3"),
                    new Square("25","R3"),
                    new Square("26","F1"),
                    new Square("27","F2"),
                    new Square("28","U2"),
                    new Square("29","F3"),
                    new Square("30","G2J"),
                    new Square("31","G1"),
                    new Square("32","G2"),
                    new Square("33","CC3"),
                    new Square("34","G3"),
                    new Square("35","R4"),
                    new Square("36","CH3"),
                    new Square("37","H1"),
                    new Square("38","T2"),
                    new Square("39","H2")
                };

                G2J = Squares.FirstOrDefault(s => s.Name == "G2J");
                JAIL = Squares.FirstOrDefault(s => s.Name == "JAIL");
                CC = Squares.FirstOrDefault(s => s.Name == "CC");
                CH = Squares.FirstOrDefault(s => s.Name == "CH");


                var random = new Random();
                CommunityChest = new List<ChanceTransition>();
                CommunityChest.Add(ChanceTransition.AdvanceToGo);
                CommunityChest.Add(ChanceTransition.GoToJail);
                for(int i = 0; i < 14; i++)
                {
                    CommunityChest.Add(ChanceTransition.Stay);
                }


                CommunityChest = CommunityChest.OrderBy(s => random.Next(1, CommunityChest.Count)).ToList();

                Chance = new List<ChanceTransition>();
                Chance.Add(ChanceTransition.AdvanceToGo);
                Chance.Add(ChanceTransition.GoToJail);
                Chance.Add(ChanceTransition.GoToC1);
                Chance.Add(ChanceTransition.GoToE3);
                Chance.Add(ChanceTransition.GoToH2);
                Chance.Add(ChanceTransition.GoToR1);
                Chance.Add(ChanceTransition.GoToNextR);
                Chance.Add(ChanceTransition.GoToNextR);
                Chance.Add(ChanceTransition.GoToNextU);
                Chance.Add(ChanceTransition.GoBack3);
                for(int i = 0; i < 6; i++)
                {
                    Chance.Add(ChanceTransition.Stay);
                }

                Chance = Chance.OrderBy(s => random.Next(1, Chance.Count)).ToList();

            }

            enum ChanceTransition
            {
                Stay,
                AdvanceToGo,
                GoToJail,
                GoToC1,
                GoToE3,
                GoToH2,
                GoToR1,
                GoToNextR,
                GoToNextU,
                GoBack3
            };


            private List<ChanceTransition> CommunityChest { get; set; }
            public Square NextCC(Square currentSquare)
            {
                var transition = CommunityChest.Last();
                CommunityChest.RemoveAt(CommunityChest.Count - 1);
                CommunityChest.Insert(0, transition);

                return HandleTransition(currentSquare, transition);
            }


            private List<ChanceTransition> Chance { get; set; }
            public Square NextCH(Square currentSquare)
            {
                var transition = Chance.Last();
                Chance.RemoveAt(Chance.Count - 1);
                Chance.Insert(0, transition);


                return HandleTransition(currentSquare, transition);
            }

            private Square HandleTransition(Square currentSquare, ChanceTransition transition)
            {
                switch(transition)
                {
                    case ChanceTransition.Stay: return currentSquare;
                    case ChanceTransition.AdvanceToGo: return Squares.FirstOrDefault(s => s.Name == "GO");
                    case ChanceTransition.GoToJail: return JAIL;
                    case ChanceTransition.GoToC1: return Squares.FirstOrDefault(s => s.Name == "C1");
                    case ChanceTransition.GoToE3: return Squares.FirstOrDefault(s => s.Name == "E3");
                    case ChanceTransition.GoToH2: return Squares.FirstOrDefault(s => s.Name == "H2");
                    case ChanceTransition.GoToR1: return Squares.FirstOrDefault(s => s.Name == "R1");
                    case ChanceTransition.GoBack3: return NextSquare(currentSquare, -3);
                    case ChanceTransition.GoToNextR:
                    {

                        //First get all the rail roads
                        var railRoads = Squares.Where(s => s.Name.StartsWith("R"));
                        var railRoad = railRoads.FirstOrDefault(s => Squares.IndexOf(s) > Squares.IndexOf(currentSquare));
                        if (railRoad == null)
                        {
                            return railRoads.FirstOrDefault(s => s.Name == "R1");
                        }
                        return railRoad;

                    }
                    case ChanceTransition.GoToNextU:
                    {
                        var utilities = Squares.Where(s => s.Name.StartsWith("U"));
                        var utility = utilities.FirstOrDefault(s => s.Name.StartsWith("U") && Squares.IndexOf(s) > Squares.IndexOf(currentSquare));
                        if (utility == null)
                        {
                            return utilities.FirstOrDefault(s => s.Name == "U1");
                        }

                        return utility;
                    }
                    default: return currentSquare;
                }

            }

            public Square NextSquare(Square currentSquare, int steps)
            {
                var currentIndex = Squares.IndexOf(currentSquare) + steps;
                if (currentIndex < 0)
                {
                    currentIndex = Squares.Count - currentIndex - 1;
                }
                return Squares[currentIndex % Squares.Count];
            }

        }

        public class Square
        {
            public string Id { get; set; }
            public string Name { get; set; }

            public Square(string id, string name)
            {
                Id = id;
                Name = name;
            }
        }
    }


}
