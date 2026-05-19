# geometry.py
import math

from OpenGL.raw.GL.KHR.debug import GL_VERTEX_ARRAY
from OpenGL.raw.GL.VERSION.GL_1_1 import GL_NORMAL_ARRAY


class Cylinder:
    """Создаёт списки вершин, нормалей и индексов для цилиндра."""
    def __init__(self, radius=1.0, height=2.0, slices=36):
        self.radius = radius
        self.height = height
        self.slices = slices
        self.vertices = []      # список кортежей (x,y,z)
        self.normals = []       # список кортежей (nx,ny,nz)
        self.indices = []       # список индексов вершин (треугольники)
        self._build()

    def _build(self):
        """Генерация геометрии: боковая поверхность + два основания."""
        h2 = self.height / 2.0
        r = self.radius
        n = self.slices
        da = 2.0 * math.pi / n

        # ---- 1. Боковая поверхность (треугольные полоски) ----
        for i in range(n):
            a1 = i * da
            a2 = (i + 1) * da
            x1 = r * math.cos(a1)
            z1 = r * math.sin(a1)
            x2 = r * math.cos(a2)
            z2 = r * math.sin(a2)

            v_bot1 = (x1, -h2, z1)
            v_top1 = (x1,  h2, z1)
            v_bot2 = (x2, -h2, z2)
            v_top2 = (x2,  h2, z2)

            # Нормали для боковых граней направлены от оси
            n1 = (x1 / r, 0.0, z1 / r)
            n2 = (x2 / r, 0.0, z2 / r)

            idx_start = len(self.vertices)
            self.vertices.extend([v_bot1, v_top1, v_bot2, v_top2])
            self.normals.extend([n1, n1, n2, n2])
            self.indices.extend([
                idx_start,     idx_start+1, idx_start+2,
                idx_start+1,   idx_start+3, idx_start+2
            ])

        # ---- 2. Нижнее основание (треугольный веер) ----
        center_bot = (0.0, -h2, 0.0)
        normal_bot = (0.0, -1.0, 0.0)
        idx_center = len(self.vertices)
        self.vertices.append(center_bot)
        self.normals.append(normal_bot)

        for i in range(n):
            a = i * da
            x = r * math.cos(a)
            z = r * math.sin(a)
            self.vertices.append((x, -h2, z))
            self.normals.append(normal_bot)
            if i > 0:
                self.indices.extend([idx_center, idx_center + i, idx_center + i + 1])
        # Замыкающий треугольник
        self.indices.extend([idx_center, idx_center + n, idx_center + 1])

        # ---- 3. Верхнее основание ----
        center_top = (0.0, h2, 0.0)
        normal_top = (0.0, 1.0, 0.0)
        idx_center = len(self.vertices)
        self.vertices.append(center_top)
        self.normals.append(normal_top)

        for i in range(n):
            a = i * da
            x = r * math.cos(a)
            z = r * math.sin(a)
            self.vertices.append((x, h2, z))
            self.normals.append(normal_top)
            if i > 0:
                self.indices.extend([idx_center, idx_center + i, idx_center + i + 1])
        self.indices.extend([idx_center, idx_center + n, idx_center + 1])

    def draw(self):
        """Отрисовка цилиндра через OpenGL массивы."""
        from OpenGL.GL import glEnableClientState, glVertexPointer, glNormalPointer, \
                              glDrawElements, GL_TRIANGLES, GL_FLOAT, GL_UNSIGNED_INT, \
                              glDisableClientState
        import ctypes
        glEnableClientState(GL_VERTEX_ARRAY)
        glEnableClientState(GL_NORMAL_ARRAY)

        # Преобразуем списки в плоские массивы для OpenGL
        vert_flat = [coord for v in self.vertices for coord in v]
        norm_flat = [coord for n in self.normals for coord in n]

        glVertexPointer(3, GL_FLOAT, 0, (ctypes.c_float * len(vert_flat))(*vert_flat))
        glNormalPointer(GL_FLOAT, 0, (ctypes.c_float * len(norm_flat))(*norm_flat))

        glDrawElements(GL_TRIANGLES, len(self.indices), GL_UNSIGNED_INT,
                       (ctypes.c_uint * len(self.indices))(*self.indices))

        glDisableClientState(GL_NORMAL_ARRAY)
        glDisableClientState(GL_VERTEX_ARRAY)