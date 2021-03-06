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

#ifndef _CP_U_H
#define _CP_U_H

#include "proto.h"
#include "BB_types.h"

/*===========================================================================*/
/*====================== User supplied functions ============================*/
/*===========================================================================*/

int user_receive_cp_data PROTO((void **user));
int user_free_cp PROTO((void **user));
int user_prepare_to_check_cuts PROTO((void *user, int varnum, int *indices,
				      double *values));
int user_check_cut PROTO((void *user, double lpetol, int varnum, int *indices,
			  double *values, cut_data *cut, int *is_violated,
			  double *quality));
int user_finished_checking_cuts PROTO((void *user));
int user_receive_lp_solution_cp PROTO((void *user));

#endif
