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

#ifndef COMPILE_IN_CG

#include <stdlib.h>
#include <malloc.h>
#include <stdio.h>

#include "proccomm.h"
#include "messages.h"
#include "cg.h"
#include "timemeas.h"
#include "BB_constants.h"
#include "BB_macros.h"

/*===========================================================================*/

/*===========================================================================*\
 * This is the main() that is used if the CG is running as a separate        
 * process. This file is only used in that case.                             
\*===========================================================================*/

int main(void)
{
   int r_bufid = 0, s_bufid = 0;
   cg_prob *p;
   int num_cuts = 0;
   double elapsed;
   struct timeval tout = {15, 0};

   p = (cg_prob *) calloc(1, sizeof(cg_prob));
   
   cg_initialize(p, 0);
  
   /*------------------------------------------------------------------------*\
    * The main loop -- executes continuously until the program exits         
   \*------------------------------------------------------------------------*/
  
   while (TRUE){
      /* Wait until a message arrives */
      do{
	 r_bufid = treceive_msg(ANYONE, ANYTHING, &tout);
	 if (!r_bufid){
	    if (pstat(p->tree_manager) != PROCESS_OK){
	       printf("TM has died -- CG exiting\n\n");
	       exit(-401);
	    }
	 }
      }while (!r_bufid);
      if (cg_process_message(p, r_bufid) == USER_ERROR)
	 p->msgtag = USER_ERROR;
      /* If there is still something in the queue, process it */
      do{
	 r_bufid = nreceive_msg(ANYONE, ANYTHING);
	 if (r_bufid > 0)
	    if (cg_process_message(p, r_bufid) == USER_ERROR)
	       p->msgtag = USER_ERROR;
      }while (r_bufid != 0);

      /*---------------------------------------------------------------------
       * Now the message queue is empty. If the last message was NOT some
       * kind of LP_SOLUTION then we can't generate solutions now.
       * Otherwise, generate solutions!
       *---------------------------------------------------------------------*/
      if (p->msgtag == LP_SOLUTION_NONZEROS || p->msgtag == LP_SOLUTION_USER ||
	  p->msgtag == LP_SOLUTION_FRACTIONS){
	 if (p->par.do_findcuts)
	    if ((termcode = find_cuts_u(p, NULL, &num_cuts)) < 0)
	       printf("Warning: User error detected in cut generator\n\n");
	 /*-- send signal back to the LP that the cut generator is done -----*/
	 s_bufid = init_send(DataInPlace);
	 send_int_array(&num_cuts, 1);
	 elapsed = used_time(&p->tt);
	 send_dbl_array(&elapsed, 1);
	 send_int_array(&p->cur_sol.xindex, 1);
	 send_int_array(&p->cur_sol.xiter_num, 1);
	 send_msg(p->cur_sol.lp, NO_MORE_CUTS);
	 freebuf(s_bufid);
	 FREE(p->cur_sol.xind);
	 FREE(p->cur_sol.xval);
      }
   }
   
   return(0);
}

#endif
