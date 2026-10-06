#include <SDL2/SDL.h>
#include <iostream>

#include "bresenham.h"

int main(int argc, char* argv[])
{
    if (SDL_Init(SDL_INIT_VIDEO) != 0)
    {
        std::cout << "SDL init error\n";
        return 1;
    }

    SDL_Window* window = SDL_CreateWindow(
        "Bresenham Line Algorithm",
        SDL_WINDOWPOS_CENTERED,
        SDL_WINDOWPOS_CENTERED,
        WIDTH,
        HEIGHT,
        SDL_WINDOW_SHOWN
    );

    SDL_Renderer* renderer = SDL_CreateRenderer(
        window,
        -1,
        SDL_RENDERER_ACCELERATED
    );

    SDL_SetRenderDrawColor(renderer, 0, 0, 0, 255);
    SDL_RenderClear(renderer);

    /*
        Тестовые линии
    */

    // горизонтальная
    drawLine(renderer, 100, 100, 700, 100, 0xFF0000);

    // вертикальная
    drawLine(renderer, 200, 100, 200, 500, 0x00FF00);

    // угол 45°
    drawLine(renderer, 100, 100, 400, 400, 0x0000FF);

    // крутой наклон
    drawLine(renderer, 500, 100, 550, 500, 0xFFFF00);

    // обратное направление
    drawLine(renderer, 700, 500, 100, 200, 0xFF00FF);

    SDL_RenderPresent(renderer);

    bool running = true;
    SDL_Event event;

    while (running)
    {
        while (SDL_PollEvent(&event))
        {
            if (event.type == SDL_QUIT)
                running = false;
        }
    }

    SDL_DestroyRenderer(renderer);
    SDL_DestroyWindow(window);
    SDL_Quit();

    return 0;
}