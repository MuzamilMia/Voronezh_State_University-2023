#define GL_SILENCE_DEPRECATION
#include <GLUT/glut.h>
#include <cmath>

#include "cylinder.h"

const int SEGMENTS = 48;

float H = 2.0f;
float R = 1.0f;

void drawCylinder()
{
    float half = H/2.0f;

    for(int i=0;i<SEGMENTS;i++)
    {
        float a0 = 2*M_PI*i/SEGMENTS;
        float a1 = 2*M_PI*(i+1)/SEGMENTS;

        float x0 = cos(a0)*R;
        float z0 = sin(a0)*R;

        float x1 = cos(a1)*R;
        float z1 = sin(a1)*R;

        glBegin(GL_QUADS);

        glNormal3f(cos(a0),0,sin(a0));
        glVertex3f(x0,-half,z0);
        glVertex3f(x0,half,z0);

        glNormal3f(cos(a1),0,sin(a1));
        glVertex3f(x1,half,z1);
        glVertex3f(x1,-half,z1);

        glEnd();
    }
}