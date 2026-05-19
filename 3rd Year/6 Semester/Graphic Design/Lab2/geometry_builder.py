# geometry_builder.py
import math
from point3d import Point3D
from face import Face

class GeometryBuilder:
    """Построение вершин и граней усечённой правильной пирамиды (трапеции в 3D)"""
    @staticmethod
    def build(n, height, radius_bottom, radius_top):
        if n < 3:
            n = 3
        h = height
        r_bot = radius_bottom
        r_top = radius_top

        vertices = []

        # Bottom base (z = -h/2)
        for i in range(n):
            angle = 2 * math.pi * i / n
            x = r_bot * math.cos(angle)
            y = r_bot * math.sin(angle)
            vertices.append(Point3D(x, y, -h/2.0))

        bottom_start = 0
        top_start = n

        # Upper base (z = +h/2)
        for i in range(n):
            angle = 2 * math.pi * i / n
            x = r_top * math.cos(angle)
            y = r_top * math.sin(angle)
            vertices.append(Point3D(x, y, h/2.0))

        faces = []

        # Нижняя грань (Bottom edge)
        bottom_face = Face([bottom_start + i for i in range(n)])
        bottom_face.normal = Point3D(0, 0, -1)
        faces.append(bottom_face)

        # Верхняя грань (Upper edge)
        top_face = Face([top_start + i for i in range(n)])
        top_face.normal = Point3D(0, 0, 1)
        faces.append(top_face)

        # Боковые грани (трапеции)
        for i in range(n):
            next_i = (i + 1) % n
            i_bot = bottom_start + i
            i_bot_next = bottom_start + next_i
            i_top = top_start + i
            i_top_next = top_start + next_i

            face = Face([i_bot, i_bot_next, i_top_next, i_top])

            # Вычисление нормали (векторное произведение)
            v0 = vertices[i_bot]
            v1 = vertices[i_bot_next]
            v2 = vertices[i_top]
            e1 = Point3D(v1.x - v0.x, v1.y - v0.y, v1.z - v0.z)
            e2 = Point3D(v2.x - v0.x, v2.y - v0.y, v2.z - v0.z)
            nrm = Point3D.cross(e1, e2)
            nrm = Point3D.normalize(nrm)

            # Корректировка направления наружу
            center = Point3D()
            for idx in face.indices:
                center.x += vertices[idx].x
                center.y += vertices[idx].y
                center.z += vertices[idx].z
            center.x /= 4
            center.y /= 4
            center.z /= 4
            radial = Point3D(center.x, center.y, 0)
            if abs(radial.x) < 1e-8 and abs(radial.y) < 1e-8:
                radial = Point3D(1, 0, 0)
            radial = Point3D.normalize(radial)
            if Point3D.dot(nrm, radial) < 0:
                nrm = Point3D(-nrm.x, -nrm.y, -nrm.z)

            face.normal = nrm
            faces.append(face)

        return vertices, faces