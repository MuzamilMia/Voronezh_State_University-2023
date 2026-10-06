#pragma once
#include <cmath>

struct Vec3
{
    double x,y,z;

    Vec3 operator+(const Vec3& v) const;
    Vec3 operator-(const Vec3& v) const;
    Vec3 operator*(double s) const;
};

// скалярное произведение
double dot(Vec3 a, Vec3 b);

// векторное произведение
Vec3 cross(Vec3 a, Vec3 b);

// нормализация вектора
Vec3 normalize(Vec3 v);

// вращение вокруг оси Y (azimuth)
Vec3 rotateY(Vec3 p,double a);

// вращение вокруг оси X (elevation)
Vec3 rotateX(Vec3 p,double a);