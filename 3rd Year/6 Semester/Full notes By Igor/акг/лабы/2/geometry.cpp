#include "geometry.h"
#include <algorithm>

Vec3 computeNormal(const std::vector<Vec3>& v)
{
    Vec3 a = v[1] - v[0];
    Vec3 b = v[2] - v[0];

    return normalize(cross(a,b));
}

double avgZ(const std::vector<Vec3>& v)
{
    double z=0;

    for(auto&p:v)
        z+=p.z;

    return z/v.size();
}

std::vector<Face> generateFrustum(
    double R1,
    double R2,
    double H,
    int N)
{
    std::vector<Vec3> bottom;
    std::vector<Vec3> top;

    for(int i=0;i<N;i++)
    {
        double a=2*M_PI*i/N;

        bottom.push_back({R1*cos(a),-H/2,R1*sin(a)});
        top.push_back({R2*cos(a), H/2,R2*sin(a)});
    }

    std::vector<Face> faces;

    // боковые грани
    for(int i=0;i<N;i++)
    {
        int j=(i+1)%N;

        Face f;

        f.verts={
            bottom[i],
            bottom[j],
            top[j],
            top[i]
        };

        faces.push_back(f);
    }

    // верхнее основание
    Face topf;
    topf.verts=top;
    faces.push_back(topf);

    // нижнее основание
    Face botf;
    botf.verts=bottom;
    std::reverse(botf.verts.begin(),botf.verts.end());
    faces.push_back(botf);

    return faces;
}