#define GL_SILENCE_DEPRECATION
#include <GLUT/glut.h>
#include <iostream>

#include "cylinder.h"
#include "lighting.h"
#include "controls.h"

float eyeX, eyeY, eyeZ;

void initGL()
{
    glEnable(GL_DEPTH_TEST);
    glShadeModel(GL_SMOOTH);

    float globalAmbient[] = {0.2f,0.2f,0.2f,1};
    glLightModelfv(GL_LIGHT_MODEL_AMBIENT,globalAmbient);
}

void display()
{
    glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
    glLoadIdentity();

    gluLookAt(eyeX,eyeY,eyeZ,0,0,0,0,1,0);

    setupLights();

    drawCylinder();

    glutSwapBuffers();
}

void reshape(int w,int h)
{
    glViewport(0,0,w,h);

    glMatrixMode(GL_PROJECTION);
    glLoadIdentity();

    gluPerspective(60,(float)w/h,0.1,100);

    glMatrixMode(GL_MODELVIEW);
}

int main(int argc,char** argv)
{
    std::cout<<"Cylinder height: ";
    std::cin>>H;

    std::cout<<"Cylinder radius: ";
    std::cin>>R;

    std::cout<<"Number of lights: ";
    std::cin>>lightCount;

    inputLights();

    std::cout<<"Camera position x y z: ";
    std::cin>>eyeX>>eyeY>>eyeZ;

    glutInit(&argc,argv);
    glutInitDisplayMode(GLUT_DOUBLE | GLUT_RGB | GLUT_DEPTH);
    glutInitWindowSize(800,600);
    glutCreateWindow("Cylinder");

    initGL();

    glutDisplayFunc(display);
    glutReshapeFunc(reshape);
    glutKeyboardFunc(keyboard);

    glutMainLoop();

    return 0;
}