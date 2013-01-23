/* Objective function */
min: +x1 +x2 +x3;

/* Constraints */
C1: +x1 +x2 -x3 >= 1e-005;
R2: +x1 -x2 +x3 >= 1e-005;
R3: -x1 +x2 +x3 >= 1e-005;
R4: -x1 +x2 >= 1e-005;
R5: -x2 +x3 >= 1e-005;

/* Integer definitions */
int x1,x2,x3;
