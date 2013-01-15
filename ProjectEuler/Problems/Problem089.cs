using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectEuler
{
    public class Problem89
    {

        public void Solve()
        {
            var file = new FileStream("Assets\\roman.txt", FileMode.Open);
            var numerals = new List<string>();
            using(var reader = new StreamReader(file))
            {
                while (!reader.EndOfStream)
                {
                    numerals.Add(reader.ReadLine());
                }
            }


            var test = Reduce("MCCCCCCVI");
            var count = numerals.Aggregate(0, (sum, numeral) => sum + numeral.Length);
            var newCount = numerals.Select(n => Reduce(n)).Aggregate(0, (sum, numeral) => sum + numeral.Length);

        }


        /*
          Given a roman numeral to most that it can be reduced to is a size of 1
          the least it can be reduced to is the same size that it is.
         
          The reduction length is therefore 1 <= Reduction.Length <= N.Length
          
            I = 1
            V = 5
            X = 10
            L = 50
            C = 100
            D = 500
            M = 1000
         
            Extra Rules
         
            1. Only I, X, and C can be used as the leading numeral in part of a subtractive pair.
            2. I can only be placed before V and X.
            3. X can only be placed before L and C.
            4. C can only be placed before D and M.
         
            
         * Algorithm.
         * 1. Convert the roman numeral to a number.
         * 2. Given a number write the number in the least number of characters as possible
         
         
         
          
         */

        public int Value(char numeralChar)
        {
            switch(numeralChar)
            {
                case 'M': return 1000;
                case 'D': return 500;
                case 'C': return 100;
                case 'L': return 50;
                case 'X': return 10;
                case 'V': return 5;
                case 'I': return 1;
                default: return 0;
            }
        }


        public int NumeralCharCompare(char numeralChar1, char numeralChar2)
        {
            return Value(numeralChar1).CompareTo(Value(numeralChar2));
        }
        public int Value(string numeral)
        {
            var value = 0;

            //Create a bunch of numeral pairs
            for (var i = 0; i < numeral.Length; i++)
            {
                //If the next numeral is greater than the current numeral it is a subtractive pair
                if (i < (numeral.Length - 1) && NumeralCharCompare(numeral[i], numeral[i + 1]) == -1)
                {
                    value += Value(numeral[i + 1]) - Value(numeral[i]);
                    i++;
                }
                else 
                {
                    value += Value(numeral[i]);
                }
            }


            return value;
        }


        /*
         * 
         *  1 = I
         * 2 = II
         * 3 = III or IIV
         * 4 = IV
         *  5 = V
         * 6 = VI
         * 7 = VII
         * 8 = IIX
         * 9 = IX
         *  10 = X
         * 11 = X,I
         * 12 = X,II
         * 13 = X,III or X,IIV
         * ...
         *  20 = XX
         * ...
         *  30 = XXX or XXL
         * ...
         *  40 = XL
         * ...
         *  50 = L
         * ...
         *  60 = L,X
         *  70 = L,XX
         *  80 = XXC
         *  90 = XC
         * ...
         *  100 = C
         *  200 = CC
         *  300 = CCC or CCD
         *  400 = CD
         *  500 = D
         * 600 = D,C
         * 700 = D,CC
         * ...
         * 1000 = M
         *  
         *  
         * 
         */

        public string WriteSmallestNumeral(int value)
        {
            var reducedNumeral = "";
            var compares = new List<Tuple<string, int>>()
            {
                new Tuple<string,int>("M",1000),
                new Tuple<string,int>("CM",900),
                new Tuple<string,int>("DCCC",800),
                new Tuple<string,int>("DCC",700),
                new Tuple<string,int>("DC",600),
                new Tuple<string,int>("D",500),
                new Tuple<string,int>("CD",400),
                new Tuple<string,int>("CCC",300),
                new Tuple<string,int>("CC",200),
                new Tuple<string,int>("C",100),
                new Tuple<string,int>("XC",90),
                new Tuple<string,int>("LXXX",80),
                new Tuple<string,int>("LXX",70),
                new Tuple<string,int>("LX",60),
                new Tuple<string,int>("L",50),
                new Tuple<string,int>("XL",40),
                new Tuple<string,int>("XXX",30),
                new Tuple<string,int>("XX",20),
                new Tuple<string,int>("X",10),
                new Tuple<string,int>("IX",9),
                new Tuple<string,int>("VIII",8),
                new Tuple<string,int>("VII",7),
                new Tuple<string,int>("VI",6),
                new Tuple<string,int>("V",5),
                new Tuple<string,int>("IV",4),
                new Tuple<string,int>("III",3),
                new Tuple<string,int>("II",2),
                new Tuple<string,int>("I",1),

            };

            while (value > 0)
            {
                foreach (var compare in compares)
                {
                    if (value >= compare.Item2)
                    {
                        reducedNumeral += compare.Item1;
                        value -= compare.Item2;
                        break;
                    }
                }
            }
            return reducedNumeral;
        }
        public string Reduce(string numeral)
        {
            var value = Value(numeral);

            var reducedNumeral = WriteSmallestNumeral(value);

            if (Value(reducedNumeral) != Value(numeral) || numeral.Length < reducedNumeral.Length)
            {
                throw new Exception();
            }

            return reducedNumeral;
        }
    }

}
