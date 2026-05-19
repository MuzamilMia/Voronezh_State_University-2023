# point3d.py
import math

class Point3D:
    """Точка в трёхмерном пространстве"""
    def __init__(self, x=0.0, y=0.0, z=0.0):
        self.x = x
        self.y = y
        self.z = z

    def __repr__(self):
        return f"Point3D({self.x:.2f}, {self.y:.2f}, {self.z:.2f})"

    @staticmethod
    def dot(a, b):
        return a.x * b.x + a.y * b.y + a.z * b.z

    @staticmethod
    def cross(a, b):
        return Point3D(a.y * b.z - a.z * b.y,
                       a.z * b.x - a.x * b.z,
                       a.x * b.y - a.y * b.x)

    @staticmethod
    def normalize(v):
        length = math.sqrt(v.x*v.x + v.y*v.y + v.z*v.z)
        if length < 1e-8:
            return Point3D(0, 0, 0)
        return Point3D(v.x/length, v.y/length, v.z/length)