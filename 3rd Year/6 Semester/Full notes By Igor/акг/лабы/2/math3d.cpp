#include "math3d.h"

Vec3 Vec3::operator+(const Vec3& v) const
{
    return {x+v.x,y+v.y,z+v.z};
}

Vec3 Vec3::operator-(const Vec3& v) const
{
    return {x-v.x,y-v.y,z-v.z};
}

Vec3 Vec3::operator*(double s) const
{
    return {x*s,y*s,z*s};
}

double dot(Vec3 a, Vec3 b)
{
    return a.x*b.x + a.y*b.y + a.z*b.z;
}

Vec3 cross(Vec3 a, Vec3 b)
{
    return {
        a.y*b.z - a.z*b.y,
        a.z*b.x - a.x*b.z,
        a.x*b.y - a.y*b.x
    };
}

Vec3 normalize(Vec3 v)
{
    double len = std::sqrt(dot(v,v));
    if(len==0) return {0,0,0};
    return {v.x/len,v.y/len,v.z/len};
}

// вращение вокруг вертикальной оси
Vec3 rotateY(Vec3 p,double a)
{
    double c=cos(a),s=sin(a);

    return {
        p.x*c + p.z*s,
        p.y,
        -p.x*s + p.z*c
    };
}

// вращение вокруг горизонтальной оси
Vec3 rotateX(Vec3 p,double a)
{
    double c=cos(a),s=sin(a);

    return {
        p.x,
        p.y*c - p.z*s,
        p.y*s + p.z*c
    };
}