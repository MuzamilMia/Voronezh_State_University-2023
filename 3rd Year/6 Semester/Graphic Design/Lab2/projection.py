# projection.py
import math
from point3d import Point3D

class Projection:
    """Параллельная (ортографическая) проекция и сортировка граней"""

    @staticmethod
    def rotate_point(p, angle_x_deg, angle_y_deg):
        """Поворот точки вокруг осей X и Y (углы в градусах)"""
        ax = math.radians(angle_x_deg)
        ay = math.radians(angle_y_deg)
        cosx, sinx = math.cos(ax), math.sin(ax)
        cosy, siny = math.cos(ay), math.sin(ay)

        # Поворот вокруг Y
        x1 = p.x * cosy + p.z * siny
        y1 = p.y
        z1 = -p.x * siny + p.z * cosy

        # Поворот вокруг X
        x2 = x1
        y2 = y1 * cosx - z1 * sinx
        z2 = y1 * sinx + z1 * cosx

        return Point3D(x2, y2, z2)

    @staticmethod
    def sort_faces(faces, vertices, angle_x, angle_y):
        """
        sorting the edge on the middle
        """
        for face in faces:
            sum_z = 0.0
            for idx in face.indices:
                p = Projection.rotate_point(vertices[idx], angle_x, angle_y)
                sum_z += p.z
            face.depth = sum_z / len(face.indices)
        faces.sort(key=lambda f: f.depth)
