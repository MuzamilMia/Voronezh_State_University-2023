#pragma once

#include <vector>
#include <SDL2/SDL.h>
#include "geometry.h"

// применяет поворот камеры
void transformFaces(
    std::vector<Face>& faces,
    double azimuth,
    double elevation);

// рисует все грани
void renderFaces(
    SDL_Renderer* ren,
    std::vector<Face>& faces,
    Vec3 light,
    int screenW,
    int screenH);