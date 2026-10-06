#pragma once

#include <vector>
#include "math3d.h"

struct Face
{
    std::vector<Vec3> verts;
    Vec3 normal;
    double depth;
};

Vec3 computeNormal(const std::vector<Vec3>& v);

double avgZ(const std::vector<Vec3>& v);

std::vector<Face> generateFrustum(
    double R1,
    double R2,
    double H,
    int N
);