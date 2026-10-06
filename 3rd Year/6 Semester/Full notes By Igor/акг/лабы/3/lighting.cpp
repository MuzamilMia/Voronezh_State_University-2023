#define GL_SILENCE_DEPRECATION
#include <GLUT/glut.h>
#include <iostream>

#include "lighting.h"

Light lights[4];
int lightCount = 1;

void inputLights()
{
    for(int i=0;i<lightCount;i++)
    {
        std::cout<<"Light "<<i+1<<" position x y z: ";
        std::cin>>lights[i].pos[0]>>lights[i].pos[1]>>lights[i].pos[2];
        lights[i].pos[3]=1;

        std::cout<<"Light "<<i+1<<" color r g b: ";
        std::cin>>lights[i].color[0]>>lights[i].color[1]>>lights[i].color[2];
        lights[i].color[3]=1;
    }
}

void setupLights()
{
    glEnable(GL_LIGHTING);

    for(int i=0;i<lightCount;i++)
    {
        GLenum id = GL_LIGHT0 + i;

        glEnable(id);
        glLightfv(id,GL_POSITION,lights[i].pos);
        glLightfv(id,GL_DIFFUSE,lights[i].color);
        glLightfv(id,GL_SPECULAR,lights[i].color);
    }
}