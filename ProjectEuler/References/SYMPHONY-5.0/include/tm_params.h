/*===========================================================================*/
/*                                                                           */
/* This file is part of the SYMPHONY Branch, Cut, and Price Library.         */
/*                                                                           */
/* SYMPHONY was jointly developed by Ted Ralphs (tkralphs@lehigh.edu) and    */
/* Laci Ladanyi (ladanyi@us.ibm.com).                                        */
/*                                                                           */
/* (c) Copyright 2000-2004 Ted Ralphs. All Rights Reserved.                  */
/*                                                                           */
/* This software is licensed under the Common Public License Version 1.0.    */
/* Please see accompanying file for terms.                                   */
/*                                                                           */
/*===========================================================================*/

#ifndef _TM_PARAMS_H
#define _TM_PARAMS_H

#include "proto.h"

/*===========================================================================*\
 * The params structure contains all of the user-specified parameters   
 * to be read in from the parameter file.
\*===========================================================================*/

typedef struct TM_PARAMS{
   int         verbosity;
   double      granularity;
   char        lp_exe[MAX_FILE_NAME_LENGTH +1];
   char        cg_exe[MAX_FILE_NAME_LENGTH +1];
   char        cp_exe[MAX_FILE_NAME_LENGTH +1];
   int         lp_debug;
   int         cg_debug;
   int         cp_debug;
   int         max_active_nodes;
   int         max_cp_num;

   /* if a ..._machine_num is not 0 and there MUST be that many machine
      names listed in ..._machines (one name can be listed more than once) */
   int         lp_mach_num;
   char      **lp_machs;
   int         cg_mach_num;
   char      **cg_machs;
   int         cp_mach_num;
   char      **cp_machs;

   int         use_cg;

   int         random_seed;
   double      unconditional_dive_frac;
   int         diving_strategy;
   int         diving_k;
   double      diving_threshold;
   int         node_selection_rule;

   int         keep_description_of_pruned;
   int         vbc_emulation;
   char        vbc_emulation_file_name[MAX_FILE_NAME_LENGTH +1];
   int         warm_start;
   int         logging;
   int         logging_interval;
   int         cp_logging;
   char        pruned_node_file_name[MAX_FILE_NAME_LENGTH +1];
   char        warm_start_tree_file_name[MAX_FILE_NAME_LENGTH +1];
   char        warm_start_cut_file_name[MAX_FILE_NAME_LENGTH +1];
   char        tree_log_file_name[MAX_FILE_NAME_LENGTH +1];
   char        cut_log_file_name[MAX_FILE_NAME_LENGTH +1];
   int         price_in_root;
   int         trim_search_tree;

   int         colgen_strat[2]; /* the column generattion strategy for the LP
				   in the two phases */
   int         not_fixed_storage_size;
   double      time_limit;
   double      gap_limit;
   int         node_limit;
   int         find_first_feasible;

   int         sensitivity_analysis;

}tm_params;

#endif
