#include <SDL2/SDL.h>
#include <iostream>

#include "geometry.h"
#include "renderer.h"

int main(int argc,char**argv)
{
    double R1=120;
    double R2=60;
    double H=200;
    int N=32;

    double azimuth=40;
    double elevation=25;

    Vec3 light={0.2,0,1};

    if(argc>=10)
    {
        R1=atof(argv[1]);
        R2=atof(argv[2]);
        H=atof(argv[3]);
        N=atoi(argv[4]);

        azimuth=atof(argv[5]);
        elevation=atof(argv[6]);

        light={
            atof(argv[7]),
            atof(argv[8]),
            atof(argv[9])
        };
    }

    SDL_Init(SDL_INIT_VIDEO);

    int W=900;
    int Hwin=700;

    SDL_Window* win =
        SDL_CreateWindow(
            "3D Truncated Cone",
            SDL_WINDOWPOS_CENTERED,
            SDL_WINDOWPOS_CENTERED,
            W,Hwin,0);

    SDL_Renderer* ren =
        SDL_CreateRenderer(win,-1,0);

    bool run=true;

    while(run)
    {
        SDL_Event e;

        while(SDL_PollEvent(&e))
        {
            if(e.type==SDL_QUIT) run=false;

            if(e.type==SDL_KEYDOWN)
            {
                if(e.key.keysym.sym==SDLK_ESCAPE) run=false;

                if(e.key.keysym.sym==SDLK_LEFT)  azimuth-=3;
                if(e.key.keysym.sym==SDLK_RIGHT) azimuth+=3;
                if(e.key.keysym.sym==SDLK_UP)    elevation+=3;
                if(e.key.keysym.sym==SDLK_DOWN)  elevation-=3;
            }
        }

        auto faces = generateFrustum(R1,R2,H,N);

        transformFaces(faces,azimuth,elevation);

        SDL_SetRenderDrawColor(ren,0,0,0,255);
        SDL_RenderClear(ren);

        renderFaces(ren,faces,light,W,Hwin);

        SDL_RenderPresent(ren);

        SDL_Delay(16);
    }

    SDL_Quit();

    return 0;
}