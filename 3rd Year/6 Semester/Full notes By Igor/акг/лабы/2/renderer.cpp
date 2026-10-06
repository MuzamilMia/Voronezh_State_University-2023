#include "renderer.h"
#include <SDL2/SDL2_gfxPrimitives.h>
#include <algorithm>

void transformFaces(
    std::vector<Face>& faces,
    double azimuth,
    double elevation)
{
    double az = azimuth*M_PI/180;
    double el = elevation*M_PI/180;

    for(auto&f:faces)
    {
        for(auto&v:f.verts)
        {
            v=rotateY(v,az);
            v=rotateX(v,el);
        }

        f.normal = computeNormal(f.verts);
        f.depth  = avgZ(f.verts);
    }
}

void renderFaces(
    SDL_Renderer* ren,
    std::vector<Face>& faces,
    Vec3 light,
    int W,
    int H)
{
    light = normalize(light);

    std::sort(
        faces.begin(),
        faces.end(),
        [](auto&a,auto&b){return a.depth<b.depth;}
    );

    for(auto&f:faces)
    {
        double I = std::max(0.0,dot(f.normal,light));

        double shade = 0.2 + 0.8*I;

        Uint8 c = Uint8(255*shade);

        int n = f.verts.size();

        std::vector<Sint16> vx(n);
        std::vector<Sint16> vy(n);

        for(int i=0;i<n;i++)
        {
            vx[i] = W/2 + f.verts[i].x;
            vy[i] = H/2 - f.verts[i].y;
        }

        filledPolygonRGBA(
            ren,
            vx.data(),
            vy.data(),
            n,
            c,c,c,255
        );
    }
}