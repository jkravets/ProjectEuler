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

#ifndef _PROCCOMM_H
#define _PROCCOMM_H

#include "proto.h"
#include "timemeas.h"

#ifdef __PVM__
#include <pvm3.h>
#define DataInPlace PvmDataRaw
#define TaskHost PvmTaskHost
#define PROCESS_OK PvmOk
#define PVM_FUNC(info, func)   if ((info = func) < 0) PVM_ERROR(info);
#else
#define PROCESS_OK 1
#define DataInPlace 0
#define TaskHost 0
#endif

int register_process PROTO((void));
int init_send PROTO((int data_packing));
int send_char_array PROTO((char *array, int size));
int send_int_array PROTO((int *array, int size));
int send_dbl_array PROTO((double *array, int size));
int send_float_array PROTO((float *array, int size));
int send_str PROTO((char *str));
int send_msg PROTO((int recipient, int msgtag));
int msend_msg PROTO((int *recipients, int number, int msgtag));
int receive_msg PROTO((int who, int what));
int treceive_msg PROTO((int who, int what, struct timeval *timeout));
int nreceive_msg PROTO((int who, int what));
int bufinfo PROTO((int r_bufid, int *bytes, int *msgtag, int *sender));
int freebuf PROTO((int bufid));
int receive_char_array PROTO((char *array, int size));
int receive_int_array PROTO((int *array, int size));
int receive_dbl_array PROTO((double *array, int size));
int receive_float_array PROTO((float *array, int size));
int receive_str PROTO((char *str));
int spawn PROTO((char *task, char **argv, int flag, char *where, int ntask,
		 int *tids));
int pstat PROTO((int tid));
void kill_proc PROTO((int tid));
void comm_exit PROTO((void));
void setsbuf PROTO((int sbufid));
void setrbuf PROTO((int rbufid));
     
void PVM_ERROR(int info);

#endif
