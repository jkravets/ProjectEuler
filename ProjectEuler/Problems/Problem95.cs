using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectEuler.Problems
{
    public class Problem95
    {

        public Dictionary<int, List<int>> _primeFactorization { get; set; }
        
        public Problem95()
        {
            _primeFactorization = new Dictionary<int, List<int>>();
            _primeFactorization[0] = new List<int>();
            _primeFactorization[1] = new List<int>();
        }


        public void Solve()
        {
            int max = 1000000;
            var primes = Primes(max);

            var test = DivisorSum(220,primes);

            var test2 = PrimeFactorization(220, primes);

            
            int[] values = new int[max + 1];
            for (int i = 0; i <= max; i++)
            {
                values[i] = -1;
            }

            for (int i = 1; i <= max; i++)
            {
                var current = i;
                while (current <= max && values[current] == -1)
                {

                    values[current] = DivisorSum(current,primes);
                    current = values[current];

                }
            }


            var map = new Dictionary<int, List<int>>();

            for (int i = 1; i <= max; i++)
            {
                var leads = new List<int>();
                var current = i;
                var firstTime = true;

                while (current <= max && !leads.Contains(i) && !leads.Contains(current))
                {
                    if (!firstTime)
                    {
                        leads.Add(current);
                    }
                    else 
                    {
                        firstTime = false;
                    }
                    current = values[current];
                }

                if (leads.Contains(i))
                {
                    map[i] = leads;
                }
                
            }


            var chains = map.Where(kvp => kvp.Value.Contains(kvp.Key)).ToList();
            var maxChainLength = chains.Max(kvp => kvp.Value.Count);
            var maxChains = chains.Where(kvp => kvp.Value.Count == maxChainLength);

            var largeChain = new List<int>();
            foreach (var chain in maxChains)
            {
                largeChain.AddRange(chain.Value);
            }

            var answer = largeChain.Min();
    
            /* The answer is 14316
             *
             * Things I learned. When trying to create a set of combinations of numbers you can
             * do the following with simple math!
             * 
             * (1 + 2 + 4 + 6)(1 + 2 + 4 + 6)(1 + 2 + 4 + 6)
             * 
             * Gives you the sum of all combinations of the set of (2,4,6)
             * 
             * 2*1, 2*2, 2*2*2, 2*4*2
             * 
             * 
             */
        
        }

        public int DivisorSum(int number, List<int> primes)
        {
            var factorization = PrimeFactorization(number, primes);
            //This is prime
            if (factorization.Count == 1)
            {
                return number;
            }
            var factors = factorization.GroupBy(f => f);
            var sum = 1;
            foreach (var factor in factors)
            {
                var currentSum = 1;
                for (int i = 1; i <= factor.Count(); i++)
                {
                    currentSum += (int)Math.Pow(factor.Key, i);
                }
                sum *= currentSum;
            }
            return sum - number;
        }

        public List<int> PrimeFactorization(int number, List<int> primes)
        {
            if (_primeFactorization.ContainsKey(number))
            {
                return _primeFactorization[number];
            }

            var firstPrime = primes.FirstOrDefault(p =>  number % p == 0);
            if (firstPrime == number)
            {
                _primeFactorization[number] = new List<int>(){firstPrime};
            }
            else
            {

                var factorization = new List<int>() { firstPrime };
                factorization.AddRange(PrimeFactorization(number / firstPrime, primes));
                _primeFactorization[number] = factorization;
            }
            return _primeFactorization[number];
        }

        public int DivisorSumOld(int number, List<int> primes)
        {
            if (number == 0)
            {
                return 0;
            }
                        
            var divisorPrimes = new List<int>();
            for (int i = 0; i < primes.Count; i++)
            {
                if (primes[i] == 1)
                {
                    continue;
                }
                if (primes[i] > number)
                {
                    break;
                }
                if (number % primes[i] == 0)
                {
                    divisorPrimes.Add(primes[i]);
                }
            }

            if (divisorPrimes.Count == 1)
            {
                return 1;
            }


            //Find the integer factorization
            var sum = 1;
            
            foreach (var prime in divisorPrimes)
            {
                var primeSum = 1;
                for(int i = 1; i <= number; i++)
                {
                    var d = Math.Pow(prime, i);
                    if (number % d == 0)
                    {
                        primeSum += (int)d;
                    }
                    else 
                    {
                        break;
                    }
                }
                sum *= primeSum;
            }

            
            return sum - number;
        }

       
        public List<int> Primes(int max)
        {
            var primes = new List<int>();

            primes.Add(2);

            for (int i = 3; i <= max; i++)
            {
                if (!primes.Any(p => p != 1 && i % p == 0))
                {
                    primes.Add(i);
                }
            }

            return primes;
        }

    }
}
