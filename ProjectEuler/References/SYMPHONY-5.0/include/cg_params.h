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

#ifndef _CUT_GEN_PARAMS_H
#define _CUT_GEN_PARAMS_H

/*stores the parameters needed by the cut generator*/
typedef struct CG_PARAMS{
   int     verbosity;
   int     do_findcuts;
}cg_params;

#endif
