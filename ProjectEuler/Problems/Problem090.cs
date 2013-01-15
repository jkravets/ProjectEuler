using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectEuler.Problems
{
    public class Problem090
    {

        /*
         * Rules:
         * 
         * 1. The numbers {0,1,2,3,4,5,6,8,9} must appear in at least one face of one of the cubes.
         * 
         * 2. The following numbers must be generated  01, 04, 09, 16, 25, 36, 49, 64, 81
         * 
         * {0} <=> {1}                  Handles 01
         * {0} <=> {1,4}                Handles 04
         * {0} <=> {1,4,9}              Handles 09
         * {0,6} <=> {1,4,9}            Handles 16 and 49 and 64
         * {0,6,2} <=> {1,4,9,5}        Handles 25
         * {0,6,2} <=> {1,4,9,5,3}      Handles 36
         * {0,6,2,8} <=> {1,4,9,5,3}    Handles 81
         * 
         * 
         * There are 10 numbers to choose from {0,1,2,3,4,5,6,7,8,9}
         * Cube 1: 10 choose 6 = 210
         * Cube 2: 10 choose 6 = 210
         * 
         * 210*210 = 44100
         * 
         * There is a maximum number of 44100 selections that can generate perfect cubes
         * 
         * 
         */


        public void Solve()
        {
            var initialChoices = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            var faceChoices = initialChoices.Combinations(6, new IntegerListCompare());

            int count = 0;
            foreach (var face1 in faceChoices)
            {
                foreach (var face2 in faceChoices)
                {
                    
                    Func<int,int,bool> containCheck = (item1,item2) =>
                    {
                        return (face1.Contains(item1) && face2.Contains(item2)) || (face1.Contains(item2) && face2.Contains(item1));
                    };
                    //01, 04, 09, 16, 25, 36, 49, 64, 81

                    if (containCheck(0, 1) &&
                       containCheck(0, 4) &&
                       (containCheck(0, 9) || containCheck(0, 6)) &&
                       (containCheck(1, 6) || containCheck(1, 9)) &&
                       containCheck(2, 5) &&
                       (containCheck(3, 6) || containCheck(3, 9)) &&
                       (containCheck(4, 9) || containCheck(4, 6)) &&
                       (containCheck(6, 4) || containCheck(9, 4)) &&
                       containCheck(8, 1))
                    {
                        count++;
                    }
                   

                }
            }

            //Since it doesn't matter which faces are in cube1 or cube2
            var answer = count/2;
            
        
        }

    }

}
