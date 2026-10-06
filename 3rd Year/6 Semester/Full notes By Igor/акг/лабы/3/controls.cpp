#define GL_SILENCE_DEPRECATION
#include <GLUT/glut.h>
#include <iostream>

#include "cylinder.h"
#include "lighting.h"

extern float eyeX,eyeY,eyeZ;

void keyboard(unsigned char key,int x,int y)
{
    switch(key)
    {
        case 27: exit(0);

        case 'a': eyeX-=0.3f; break;
        case 'd': eyeX+=0.3f; break;
        case 'w': eyeZ-=0.3f; break;
        case 's': eyeZ+=0.3f; break;

        case 'r': R+=0.1f; break;
        case 'f': R-=0.1f; break;

        case 't': H+=0.1f; break;
        case 'g': H-=0.1f; break;
    }

    glutPostRedisplay();
}