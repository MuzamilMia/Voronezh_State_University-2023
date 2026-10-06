#ifndef BRESENHAM_CPP
#define BRESENHAM_CPP

#include <SDL2/SDL.h>
#include <cmath>

constexpr int WIDTH = 800;
constexpr int HEIGHT = 600;

void put_pixel(SDL_Renderer* renderer, int x, int y, int color);

void drawLine(SDL_Renderer* renderer, int x0, int y0, int x1, int y1, int color);

#endif