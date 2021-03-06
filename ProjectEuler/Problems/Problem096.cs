﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectEuler.Problems
{
    public class Problem096
    {

        public void Solve()
        {
            var file = new FileStream("Assets\\sudoku.txt", FileMode.Open);
            var puzzles = new List<SudokuPuzzle>();

            #region SETUP
            using (var reader = new StreamReader(file))
            {
                while (!reader.EndOfStream)
                {
                    var puzzle = new SudokuPuzzle();
                    puzzle.Name = reader.ReadLine();
                    for (int i = 0; i < 9; i++)
                    {
                        var line = reader.ReadLine();

                        for (int j = 0; j < 9; j++)
                        {
                            var value = Int32.Parse(line[j].ToString());
                            if (value != 0)
                            {
                                puzzle[i, j] = new List<int>() { value };
                            }
                        }
                    }
                    puzzles.Add(puzzle);
                }
            }
            #endregion

            int topLeftSum = 0;
            foreach (var puzzle in puzzles)
            {
                Console.WriteLine("Puzzle " + puzzle.Name);
                puzzle.Solve();
                topLeftSum += puzzle[0, 0].First()*100 + puzzle[0, 1].First()*10 + puzzle[0, 2].First();
            }

            var answer = topLeftSum;


        }
    }

    public class SudokuPuzzle
    {
        public string Name { get; set; }
        public List<int>[,] PMatrix { get; set; }

        public List<int> this[int i, int j]
        {
            get
            {
                return PMatrix[i, j];
            }
            set
            {
                PMatrix[i, j] = value;
            }
        }

        public SudokuPuzzle()
        {
            PMatrix = new List<int>[9, 9];
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    this[i, j] = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
                }
            }
        }

        public override string ToString()
        {
            var s = "\n";
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    var p = "";

                    foreach (var k in this[i, j])
                    {
                        p += k + " ";
                    }

                    s += String.Format("({0,-18})  ", p);
                }

                s += "\n";
            }

            return s;
        }

        public int PScore
        {
            get
            {
                var score = 0;
                for (int i = 0; i < 9; i++)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        score += this[i, j].Count();
                        if (this[i, j].Count == 0)
                        {
                            throw new Exception("Missing probability in section " + i + "," + j);
                        }
                    }
                }

                return score;
            }
        }


        public bool ConsistencyCheck()
        {
            try
            {
                //Also check consistency
                for (int n = 1; n <= 9; n++)
                {
                    for (int i = 0; i < 9; i++)
                    {
                        var found = 0;
                        PerformHLine(i, (ii, jj) => { found += this[ii, jj].Count == 1 && this[ii, jj].Contains(n) ? 1 : 0; });
                        if (found > 1)
                        {
                            throw new Exception("Horizontal line is inconsistent");
                        }

                    }

                    for (int j = 0; j < 9; j++)
                    {
                        var found = 0;
                        PerformVLine(j, (ii, jj) => { found += this[ii, jj].Count == 1 && this[ii, jj].Contains(n) ? 1 : 0; }); if (found > 1)
                        {
                            throw new Exception("Verical line is inconsistent");
                        }
                    }

                    for (int si = 0; si < 9; si += 3)
                    {
                        for (int sj = 0; sj < 9; sj += 3)
                        {
                            var found = 0;
                            PerformSection(si, sj, (ii, jj) => { found += this[ii, jj].Count == 1 && this[ii, jj].Contains(n) ? 1 : 0; });
                            if (found > 1)
                            {
                                throw new Exception("Section line is inconsistent");
                            }
                        }
                    }

                }
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        private bool _Solve()
        {
            try
            {

                var oldScore = PScore;
                while (PScore > 81)
                {
                    LookForLoneNumber();

                    LookForGroup(1);
                    LookForGroup(2);
                    LookForGroup(3);

                    LookForSectionCut();


                    if (!ConsistencyCheck())
                    {
                        throw new Exception("Not consistent");
                    }

                    //We need to make a guess
                    if (PScore >= oldScore)
                    {
                        var guesses = new List<Tuple<int, int>>();
                        for (int i = 0; i < 9; i++)
                        {
                            for (int j = 0; j < 9; j++)
                            {
                                if (this[i, j].Count > 1)
                                {
                                    guesses.Add(new Tuple<int, int>(i, j));
                                }
                            }
                        }


                        foreach (var guess in guesses)
                        {
                            foreach (var n in this[guess.Item1, guess.Item2])
                            {
                                var guessPuzzle = new SudokuPuzzle();
                                guessPuzzle.Name = guessPuzzle.Name;
                                for (int i = 0; i < 9; i++)
                                {
                                    for (int j = 0; j < 9; j++)
                                    {
                                        guessPuzzle[i, j] = this[i, j].ToList();
                                    }
                                }
                                guessPuzzle[guess.Item1, guess.Item2] = new List<int>() { n };
                                try
                                {
                                    guessPuzzle.Solve();
                                    for (int i = 0; i < 9; i++)
                                    {
                                        for (int j = 0; j < 9; j++)
                                        {
                                            this[i, j] = guessPuzzle[i, j].ToList();
                                        }
                                    }
                                    break;
                                }
                                catch (Exception e)
                                {
                                    //otherwise try another guess
                                }
                            }
                        }

                    }
                    oldScore = PScore;

                }
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        public void Solve()
        {
            if (!_Solve())
            {
                throw new Exception("Could not solve the puzzle");
            }
        }

        private void PerformHLine(int i, Action<int, int> lambda)
        {
            for (int jj = 0; jj < 9; jj++)
            {
                lambda(i, jj);
            }
        }

        private void PerformVLine(int j, Action<int, int> lambda)
        {
            for (int ii = 0; ii < 9; ii++)
            {
                lambda(ii, j);
            }
        }

        private void PerformSection(int i, int j, Action<int, int> lambda)
        {
            var si = (int)Math.Floor(i / 3.0) * 3;
            var sj = (int)Math.Floor(j / 3.0) * 3;
            for (int ii = si; ii < (si + 3); ii++)
            {
                for (int jj = sj; jj < (sj + 3); jj++)
                {
                    lambda(ii, jj);
                }
            }
        }




        private void LookForGroup(int size)
        {
            var groups = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 }.Combinations(size);


            //First look in a horizontal line
            for (int i = 0; i < 9; i++)
            {
                foreach (var group in groups)
                {
                    int seenGroupCount = 0, seenAnyCount = 0;
                    PerformHLine(i, (ii, jj) =>
                    {
                        seenGroupCount += this[ii, jj].ContainsRange(group) ? 1 : 0;
                        seenAnyCount += this[ii, jj].ContainsAnyRange(group) ? 1 : 0;
                    });

                    if (seenGroupCount == size && seenAnyCount == size)
                    {
                        PerformHLine(i, (ii, jj) =>
                        {
                            if (this[ii, jj].ContainsRange(group))
                            {
                                this[ii, jj] = group.ToList();
                            }
                        });
                    }
                }
            }

            //Second look in the vertical line
            for (int j = 0; j < 9; j++)
            {
                foreach (var group in groups)
                {
                    int seenGroupCount = 0, seenAnyCount = 0;
                    PerformVLine(j, (ii, jj) =>
                    {
                        seenGroupCount += this[ii, jj].ContainsRange(group) ? 1 : 0;
                        seenAnyCount += this[ii, jj].ContainsAnyRange(group) ? 1 : 0;
                    });

                    if (seenGroupCount == size && seenAnyCount == size)
                    {
                        PerformVLine(j, (ii, jj) =>
                        {
                            if (this[ii, jj].ContainsRange(group))
                            {
                                this[ii, jj] = group.ToList();
                            }

                        });
                    }
                }
            }


            for (int si = 0; si < 9; si += 3)
            {
                for (int sj = 0; sj < 9; sj += 3)
                {
                    foreach (var group in groups)
                    {
                        int seenGroupCount = 0, seenAnyCount = 0;
                        PerformSection(si, sj, (sii, sjj) =>
                        {
                            seenGroupCount += this[sii, sjj].ContainsRange(group) ? 1 : 0;
                            seenAnyCount += this[sii, sjj].ContainsAnyRange(group) ? 1 : 0;
                        });

                        if (seenGroupCount == size && seenAnyCount == size)
                        {
                            PerformSection(si, sj, (sii, sjj) =>
                            {
                                if (this[sii, sjj].ContainsRange(group))
                                {
                                    this[sii, sjj] = group.ToList();
                                }
                            });
                        }

                    }
                }
            }


        }

        private void LookForLoneNumber()
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (this[i, j].Count == 1)
                    {

                        //Clean the horizontal line
                        PerformHLine(i, (ii, jj) =>
                        {
                            if (jj != j)
                            {
                                this[i, jj].RemoveRange(this[i, j]);
                                if (this[i, jj].Count == 0)
                                {
                                    this[i, jj] = this[i, j].ToList();
                                }
                            }
                        });


                        //Clean the vertical line
                        PerformVLine(j, (ii, jj) =>
                        {
                            if (ii != i)
                            {
                                this[ii, j].RemoveRange(this[i, j]);
                                if (this[ii, j].Count == 0)
                                {
                                    this[ii, j] = this[i, j].ToList();
                                }
                            }
                        });

                        //Clean the section
                        PerformSection(i, j, (ii, jj) =>
                        {
                            if (!(ii == i && jj == j))
                            {
                                this[ii, jj].RemoveRange(this[i, j]);
                                if (this[ii, jj].Count == 0)
                                {
                                    this[ii, jj] = this[i, j].ToList();
                                }
                            }
                        });
                    }

                }
            }
        }

        private void LookForSectionCut()
        {
            for (int si = 0; si < 9; si += 3)
            {
                for (int sj = 0; sj < 9; sj += 3)
                {
                    for (int n = 1; n <= 9; n++)
                    {
                        var found = new List<Tuple<int, int>>();
                        PerformSection(si, sj, (sii, sjj) =>
                        {

                            if (this[sii, sjj].Contains(n))
                            {
                                found.Add(new Tuple<int, int>(sii, sjj));
                            }

                        });

                        //Check for horizontal
                        if (found.Count > 1)
                        {
                            //If the i's are equal we have a horizontal cut
                            if (found.All(f => f.Item1 == found[0].Item1))
                            {
                                PerformHLine(found[0].Item1, (ii, jj) =>
                                {
                                    if (!found.Any(f => f.Item2 == jj))
                                    {
                                        this[ii, jj].Remove(n);
                                    }
                                });
                            }

                            //If the j's are equal we have a vertical cut
                            if (found.All(f => f.Item2 == found[0].Item2))
                            {
                                PerformVLine(found[0].Item2, (ii, jj) =>
                                {
                                    if (!found.Any(f => f.Item1 == ii))
                                    {
                                        this[ii, jj].Remove(n);
                                    }
                                });
                            }
                        }
                    }

                }
            }
        }



    }



}
