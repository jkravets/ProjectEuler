using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectEuler.Problems
{
    public class Problem098
    {
        /*
         * 
         * 
         * Largest word = 12 characters
         * 
         * 10 different substitutions
         * 
         * 0,1,2,3,4,5,6,7,8,9,10
         * 
         * no leading 0
         * 
         * largest possible number = 9^12
         * 
         * 282429536481
         * 
         * 
         * 
         * 
         */



        public void Solve()
        {
            var words = new List<string>();
            var wordsString = File.ReadAllText(@"Assets\\words.txt");
            words = wordsString.Replace("\"", "").Split(',').ToList();
            
            var maxWordLength = words.Max(g => g.Length);
            var squares = Squares((long)Math.Pow(9, maxWordLength)).Select(s => s.ToString()).ToList();

            var largest = 0;
            for (int i = 0; i < words.Count; i++)
            {
                for (int j = i + 1; j < words.Count; j++)
                {
                    var word1 = words[i];
                    var word2 = words[j];
                    //Since the word is an anagram we can now perfom some other checks
                    if (IsAnagram(word1, word2))
                    {
                        Console.WriteLine(string.Format("word 1 = {0}, word 2 = {1}", word1, word2));
                        var subs1 = NumberSubstitutions(word1, squares);
                        var subs2 = NumberSubstitutions(word2, squares);

                        foreach(var s1 in subs1)
                        {
                            foreach(var s2 in subs2)
                            {
                                var good = true;
                                var charMapping = new char[(int)'Z' + 1];
                                for (int z = 0; z < word1.Length; z++)
                                { 
                                    charMapping[(int)word1[z]] = s1[z];
                                }
                                for (int z = 0; z < word2.Length; z++)
                                {
                                    if (charMapping[(int)word2[z]] != s2[z])
                                    {
                                        good = false;
                                        break;
                                    }
                                }

                                if (good)
                                {
                                    Console.WriteLine(string.Format("sub 1 = {0}, sub 2 = {1}", s1, s2));
                                    largest = Math.Max(Int32.Parse(s1), largest);
                                    largest = Math.Max(Int32.Parse(s2), largest);
                                }   
                            }
                        }
                        


                    }
                }
            }

            var answer = words;


        }


        public List<string> NumberSubstitutions(string word, List<string> squares)
        {
            return squares.Where(s =>
            {
                if (s.Length != word.Length)
                {
                    return false;
                }
                
                var numbermapping = new char[11];
                var charmapping = new int[(int)'Z' + 1];
                for (int i = 0; i < 11; i++)
                {
                    numbermapping[i] = '\0';
                }
                for (int i = 0; i < (int)'Z' + 1; i++)
                {
                    charmapping[i] = -1;
                }


                for (int i = 0; i < s.Length; i++)
                {
                    var number = (int)Char.GetNumericValue(s[i]);
                    var character = word[i];
                    if (numbermapping[number] == '\0' && charmapping[word[i]] == -1)
                    {
                        numbermapping[number] = word[i];
                        charmapping[word[i]] = number;
                    }

                    if (numbermapping[number] != word[i] && charmapping[word[i]] != number)
                    {
                        return false;
                    }

                }
                return true;
            }).ToList();

        }

        public bool IsAnagram(string word1, string word2)
        {
            if (word1.Length != word2.Length) return false;

            var sum1 = 0;
            var sum2 = 0;

            var charCountArray1 = new int[(int)'Z'+1];
            var charCountArray2 = new int[(int)'Z'+1];

            for (int i = 0; i < word1.Length; i++)
            {
                charCountArray1[(int)word1[i]]++;
                charCountArray2[(int)word2[i]]++;
            }

            for (int i = 0; i < charCountArray1.Length; i++)
            {
                if (charCountArray1[i] != charCountArray2[i])
                {
                    return false;
                }
            }
            
            //If this word is an anagram
            return true;
        }

        public List<long> Squares(long max)
        {
            var squares = new List<long>();

            for (long i = 1; i <= max; i++)
            {
                if (i * i > max)
                {
                    break;
                }
                squares.Add(i * i);
            }

            return squares;
        }
    }
}
