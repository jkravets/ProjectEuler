/*===========================================================================*/
/*                                                                           */
/* This file is part of a demonstration application for use with the         */
/* SYMPHONY Branch, Cut, and Price Library. This application is a solver for */
/* Capacitated Network Routing Problems.                                     */
/*                                                                           */
/* (c) Copyright 2000-2004 Ted Ralphs. All Rights Reserved.                  */
/*                                                                           */
/* This application was developed by Ted Ralphs (tkralphs@lehigh.edu)        */
/*                                                                           */
/* This software is licensed under the Common Public License Version 1.0.    */
/* Please see accompanying file for terms.                                   */
/*                                                                           */
/*===========================================================================*/

/* system include files */
#include <malloc.h>
#include <string.h>
#include <stdio.h>
#include <stdlib.h>
#include <math.h>

/* SYMPHONY include files */
#include "OsiSymSolverInterface.hpp"
#include "BB_macros.h"
#include "master.h"

/*===========================================================================*/

typedef struct SOLUTION_DATA{
   double  obj[2];
   double  gamma;
   double  tau;
   int     length;
   int    *indices;
   double *values;
}solution_data;

/*===========================================================================*/

typedef struct SOLUTION_PAIRS{
   int solution1;
   int solution2;
#ifdef BINARY_SEARCH
   double gamma1;
   double gamma2;
#endif
}solution_pairs;

/*===========================================================================*/

#define MAX_NUM_PAIRS 100
#define MAX_NUM_SOLUTIONS 100
#define MAX_NUM_INFEASIBLE 100

/*===========================================================================*/

/*===========================================================================*\
 * This file contains the main() for two different CNRP applications.
\*===========================================================================*/

#ifdef MULTI_CRITERIA

/*===========================================================================*\
 * This is main() for the multicriteria version of CNRP
\*===========================================================================*/

int main(int argc, char **argv)
{
   int i;
   problem *env;
   double gamma, gamma0, gamma1, tau, slope;
   double start_time;

   solution_data utopia1;
   solution_data utopia2;
   solution_data solutions[MAX_NUM_PAIRS];
   int numsolutions = 0, numprobs = 0, numinfeasible = 0;
   solution_pairs pairs[MAX_NUM_PAIRS];
   int numpairs = 0, cur_position = 0, first = 0, last = 0, previous = 0;
   double *indices, *values;
   int length;
   int solution1, solution2;
   double utopia[2];
   node_desc *root= NULL;
   base_desc *base = NULL;
   double compare_sol_tol, ub = 0.0;
   
   start_time = wall_clock(NULL);

   /* Initialize the SYMPHONY environment */
   OsiSymSolverInterface si;
   
   /* Get pointer to the SYMPHONY environment */
   env = si.getSymphonyEnvironment();

   /* Parse the command line */
   si.parseCommandLine(argc, argv);
   
   /* Read in the problem */
   si.loadProblem();

   /* Find a priori problem bounds */
   si.findInitialBounds();
   
   /* Set some parameters */
   compare_sol_tol = p->par.compare_solution_tolerance;
   si.setSymParam(OsiSymGranularity,-MAX(p->lp_par.rho, compare_sol_tol));

#ifdef BINARY_SEARCH
   printf("Using binary search with tolerance = %f...\n",
	  p->par.binary_search_tolerance);
#endif
#ifdef LIFO
   printf("Using LIFO search order...\n");
#endif
   if (p->lp_par.rho > 0){
      printf("Using secondary objective weight %.8f\n", cnrp->lp_par.rho);
   }
   printf("\n");

#ifdef SAVE_CUT_POOL
   printf("Saving the global cut pool between iterations...\n");
   si.createPermanentCutPools();
   si.setSymParam(OsiSymUsePermanentCutPools, TRUE);
#endif
   
   /* First, calculate the utopia point */
   p->lp_par.gamma = 1.0;
   p->lp_par.tau = 0.0;
      
   printf("***************************************************\n");
   printf("***************************************************\n");
   printf("Now solving with gamma = 1.0 tau = 0.0 \n", gamma, tau);  
   printf("***************************************************\n");
   printf("***************************************************\n\n");

   /* Solve */
   si.branchAndBound();
   numprobs++;
   
   /* Store the solution */
   length = solutions[numsolutions].length = p->best_sol.xlength;
   indices = solutions[numsolutions].indices = (int *) calloc(length, ISIZE);
   values = solutions[numsolutions].values = (double *) calloc(length, DSIZE);
   memcpy((char *) indices, p->bestsol.xind, length * ISIZE);
   memcpy((char *) values, p->bestsol.xval, length * DSIZE);
   solutions[numsolutions].gamma = 1.0;
   solutions[numsolutions].tau = 0.0;
   solutions[numsolutions].obj[0] = p->obj[0];
   solutions[numsolutions++].obj[1] = p->obj[1];
   utopia[0] = p->obj[0];
      
   cnrp->lp_par.gamma = 0.0;
   cnrp->cg_par.tau = cnrp->lp_par.tau = 1.0;
      
   printf("***************************************************\n");
   printf("***************************************************\n");
   printf("Now solving with gamma = 0.0 tau = 1.0 \n", gamma, tau);  
   printf("***************************************************\n");
   printf("***************************************************\n\n");

   /* Solve */
   si.branchAndBound();
   numprobs++;
   
   /* Store the solution */
   length = solutions[numsolutions].length = p->best_sol.xlength;
   indices = solutions[numsolutions].indices = (int *) calloc(length, ISIZE);
   values = solutions[numsolutions].values = (double *) calloc(length, DSIZE);
   memcpy((char *) indices, p->bestsol.xind, length * ISIZE);
   memcpy((char *) values, p->bestsol.xval, length * DSIZE);
   solutions[numsolutions].gamma = 0.0;
   solutions[numsolutions].tau = 1.0;
   solutions[numsolutions].obj[0] = p->obj[0];
   solutions[numsolutions++].obj[1] = p->obj[1];
   utopia[1] = p->obj[1];
   
   p->utopia[1] = utopia[1];
   p->utopia[0] = utopia[0];
   
   printf("***************************************************\n");
   printf("***************************************************\n");
   printf("Utopia point has fixed cost %.3f and variable cost %.3f \n",
	  utopia[0], utopia[1]);
   printf("***************************************************\n");
   printf("***************************************************\n\n");
   
   /* Add the first pair to the list */
#ifdef BINARY_SEARCH
   pairs[first].gamma1 = 1.0;
   pairs[first].gamma2 = 0.0;
#endif
   pairs[first].solution1 = 0;
   pairs[first].solution2 = 1;

   first = last = 0;
   numpairs = 1;

   /* Keep taking pairs off the list and processing them until there are none
      left */
   while (numpairs > 0 && numpairs < MAX_NUM_PAIRS &&
	  numsolutions < MAX_NUM_SOLUTIONS &&
	  numinfeasible < MAX_NUM_INFEASIBLE){

#ifdef LIFO
      solution1 = pairs[last].solution1;
      solution2 = pairs[last].solution2;
      cur_position = last;
      if (--last < 0){
	 last = MAX_NUM_PAIRS - 1;
      }
      numpairs--;
#else
      solution1 = pairs[first].solution1;
      solution2 = pairs[first].solution2;
      cur_position = first;
      if (++first > MAX_NUM_PAIRS-1)
	 first = 0;
      numpairs--;
#endif

#ifdef BINARY_SEARCH
      gamma = (pairs[cur_position].gamma1 + pairs[cur_position].gamma2)/2;
#elif defined(FIND_NONDOMINATED_SOLUTIONS)
      gamma = (utopia[1] - solutions[solution1].obj[1])/
	 (utopia[0] - solutions[solution2].obj[0] +
	  utopia[1] - solutions[solution1].obj[1]);
#else
      slope = (solutions[solution1].obj[1] -
	       solutions[solution2].obj[1])/
	      (solutions[solution2].obj[0] -
	       solutions[solution1].obj[0]);
      gamma = slope/(1+slope);
#endif
      tau = 1 - gamma;
      
      p->lp_par.gamma = gamma;
      p->lp_par.tau = tau;

      /* Find upper bound */

      env->has_ub = FALSE;
      env->ub = MAXDOUBLE;
#ifndef BINARY_SEARCH
      for (i = 0; i < numsolutions; i++){
#ifdef FIND_NONDOMINATED_SOLUTIONS
	 ub = MAX(gamma*(solutions[i].obj[0] - utopia[0]),
		  tau*(solutions[i].obj[1] - utopia[1]));
#else
	 ub = gamma*solutions[i].obj[0] + tau*solutions[i].obj[1];
#endif 
	 if (ub < env->ub){
	    env->has_ub = TRUE;
	    env->ub = ub - compare_sol_tol;
	 }
      }
#endif
      
      printf("***************************************************\n");
      printf("***************************************************\n");
      printf("Now solving with gamma = %.6f tau = %.6f \n", gamma, tau);  
      printf("***************************************************\n");
      printf("***************************************************\n\n");
      
      p->obj[0] = p->obj[1] = 0.0;
      
      si.branchAndBound();
      numprobs++;
      
#ifdef BINARY_SEARCH
      if (p->obj[0] - solutions[solution1].obj[0] <
	  compare_sol_tol &&
	  solutions[solution1].obj[1] - p->obj[1] <
	  compare_sol_tol){
	 if (pairs[cur_position].gamma1 - gamma >
	     cnrp->par.binary_search_tolerance){
	    if (++last > MAX_NUM_PAIRS - 1)
	       last = 0;
	    pairs[last].solution1 = solution1;
	    pairs[last].solution2 = solution2;
	    pairs[last].gamma1 = gamma;
	    pairs[last].gamma2 = pairs[cur_position].gamma2;
	    numpairs++;
	 }
	 continue;
      }
      if (solutions[solution2].obj[0] - p->obj[0] < compare_sol_tol
	  && p->obj[1] - solutions[solution2].obj[1] <
	  compare_sol_tol){
	 if (gamma - pairs[cur_position].gamma2 >
	     cnrp->par.binary_search_tolerance){
	    if (++last > MAX_NUM_PAIRS - 1)
	       last = 0;
	    pairs[last].solution1 = solution1;
	    pairs[last].solution2 = solution2;
	    pairs[last].gamma1 = pairs[cur_position].gamma1;
	    pairs[last].gamma2 = gamma;
	    numpairs++;
	 }
	 continue;
      }
#else
      if (p->obj[0] == 0.0 && p->obj[1] == 0.0){
	 numinfeasible++;
	 continue;
      }else if (p->obj[0] - solutions[solution1].obj[0] <
		compare_sol_tol &&
		solutions[solution1].obj[1] - p->obj[1] <
		compare_sol_tol){
	 numinfeasible++;
	 continue;
      }else if (solutions[solution2].obj[0] - p->obj[0] <
		compare_sol_tol &&
		p->obj[1] - solutions[solution2].obj[1] <
		compare_sol_tol){
	 numinfeasible++;
	 continue;
      }
#endif
      
      /* Insert new solution */
      numinfeasible = 0;
      if (last + 2 == MAX_NUM_PAIRS){
	 last = 0;
	 previous = MAX_NUM_PAIRS - 1;
      }else if (last + 2 == MAX_NUM_PAIRS + 1){
	 last = 1;
	 previous = 0;
      }else{
	 last += 2;
	 previous = last - 1;
      }
#ifdef BINARY_SEARCH
      pairs[previous].gamma1 = pairs[cur_position].gamma1;
      pairs[previous].gamma2 = gamma;
      pairs[last].gamma1 = gamma;
      pairs[last].gamma2 = pairs[cur_position].gamma2;
#endif
      pairs[previous].solution1 = solution1;
      pairs[previous].solution2 = solution2;
      pairs[last].solution1 = solution2;
      pairs[last].solution2 = solution2+1;
      numpairs += 2;
      for (i = numsolutions; i > solution2; i--){
	 solutions[i] = solutions[i-1];
      }
      numsolutions++;
#ifndef LIFO
      if (first < last){
	 for (i = first; i < last - 1; i++){
	    if (pairs[i].solution1 >= solution2){
	       pairs[i].solution1++;
	    }
	    if (pairs[i].solution2 >= solution2){
	       pairs[i].solution2++;
	    }
	 }
      }else{
	 for (i = first; i < MAX_NUM_PAIRS - (last == 0 ? 1 : 0); i++){
	    if (pairs[i].solution1 >= solution2){
	       pairs[i].solution1++;
	    }
	    if (pairs[i].solution2 >= solution2){
	       pairs[i].solution2++;
	    }
	 }
	 for (i = 0; i < last - 1; i++){
	    if (pairs[i].solution1 >= solution2){
	       pairs[i].solution1++;
	    }
	    if (pairs[i].solution2 >= solution2){
	       pairs[i].solution2++;
	    }
	 }
      }
	 
#endif
      length = solutions[solutions2].length = p->best_sol.xlength;
      indices = solutions[solutions2].indices = (int *) calloc(length, ISIZE);
      values = solutions[solutions2].values = (double *) calloc(length, DSIZE);
      memcpy((char *) indices, p->bestsol.xind, length * ISIZE);
      memcpy((char *) values, p->bestsol.xval, length * DSIZE);
      solutions[solution2].gamma = gamma;
      solutions[solution2].tau = tau;
      solutions[solution2].obj[0] = p->obj[0];
      solutions[solution2].obj[1] = p->obj[1];
   }

   printf("\n********************************************************\n");

   if (numsolutions >= MAX_NUM_SOLUTIONS){
      printf("Maximum number of solutions (%i) reached\n\n",
	     MAX_NUM_SOLUTIONS);
   }

   if (numinfeasible >= MAX_NUM_INFEASIBLE){
      printf("Maximum number of infeasible subproblems (%i) reached\n\n",
	     MAX_NUM_INFEASIBLE);
   }
   
   if (numpairs >= MAX_NUM_PAIRS){
      printf("Maximum number of solution pairs (%i) reached\n\n",
	     MAX_NUM_PAIRS);
      printf("\n********************************************************\n");
#ifdef FIND_NONDOMINATED_SOLUTIONS
      printf(  "* Found set of non-dominated solutions!!!!!!! *\n");
#else
      printf(  "* Found set of supported solutions!!!!!!!     *\n");
#endif
   }else{
      printf("\n********************************************************\n");
#ifdef FIND_NONDOMINATED_SOLUTIONS
      printf(  "* Found complete set of non-dominated solutions!!!!!!! *\n");
#else
      printf(  "* Found complete set of supported solutions!!!!!!!     *\n");
#endif
   }
   printf(  "* Now displaying stats...                              *\n");
   printf(  "********************************************************\n\n");

#ifdef SAVE_CUT_POOL
   for (i = 0; i < env->par.tm_par.max_cp_num; i++){
      env->comp_times.bc_time.cut_pool += env->cp[i]->cut_pool_time;
      env->warm_start->stat.cuts_in_pool += env->cp[i]->cut_num;
   }
#endif
   
   print_statistics(&(env->comp_times.bc_time), &(env->warm_start->stat), 0.0,
		    0.0, 0, start_time);

   printf("\nNumber of subproblems solved: %i\n", numprobs);
   printf("Number of solutions found: %i\n\n", numsolutions);
   
   printf("***************************************************\n");
   printf("***************************************************\n");
#ifdef FIND_NONDOMINATED_SOLUTIONS
   printf("Displaying non-dominated solution values and breakpoints\n");  
#else
   printf("Displaying supported solution values and breakpoints\n");  
#endif
   printf("***************************************************\n");
   printf("***************************************************\n\n");

   gamma0 = 1.0;
   for (i = 0; i < numsolutions - 1; i++){
#ifdef FIND_NONDOMINATED_SOLUTIONS
      gamma1 = (utopia[1] - solutions[i].obj[1])/
	 (utopia[0] - solutions[i+1].obj[0] +
	  utopia[1] - solutions[i].obj[1]);
#else
      slope = (solutions[i].obj[1] -
	       solutions[i+1].obj[1])/
	      (solutions[i+1].obj[0] -
	       solutions[i].obj[0]);
      gamma1 = slope/(1+slope);
#endif
      printf("First Objective: %.3f Second Objective: %.3f ",
	     solutions[i].obj[0], solutions[i].obj[1]);
      printf("Range: %.6f - %.6f\n", gamma1, gamma0);
      gamma0 = gamma1;
   }
   printf("First Objective: %.3f Second Objective: %.3f ",
	  solutions[i].obj[0], solutions[i].obj[1]);
   printf("Range: %.6f - %.6f\n", 0.0, gamma0);
   
   for (i = 0 ; i < numsolutions; i++){
      FREE(solutions[i].values);
      FREE(solutions[i].indices);
   }
   
   return(0);
}   

#else

/*===========================================================================*\
 * This is main() for the single criteria version of CNRP
\*===========================================================================*/

int main(int argc, char **argv)
{
   OsiSymSolverInterface si;

   si.loadProblem(argc, argv);

   si.setSymDblParam(OsiSymGranularity, 0.999999);

   si.branchAndBound();
   
   return(0);
}

#endif

/*===========================================================================*/

