#pragma once

struct Light
{
    float pos[4];
    float color[4];
};

extern Light lights[4];
extern int lightCount;

void setupLights();
void inputLights();