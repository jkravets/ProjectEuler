using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectEuler.Problems
{
    public class Problem103
    {
        public void Solve()
        {
            

            //This is the maximum sum we are looking for
            var max = SetGuess(7).Sum();
        
        
        }




        public List<int> SetGuess(int n)
        {

            var set = new List<int>() { 1 };
            if (n == 1)
            {
                return set;
            }
            set.Add(2);
            if (n == 2)
            {
                return set;
            }


            for(int i = 2; i < n; i++)
            {
            
                //Find the middle value in the set
                var mid = (int)Math.Ceiling((set.Count - 1) / 2.0);
                var b = set[mid];

                set.Insert(0, 0);
                set = set.Select(a => a + b).ToList();
                
            }


            return set;

        }
    }
}
