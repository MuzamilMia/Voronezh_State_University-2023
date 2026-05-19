# renderer.py
from projection import Projection

class Renderer:
    def __init__(self, canvas, width, height):
        self.canvas = canvas
        self.width = width
        self.height = height
        self.zoom = 1.0   # коэффициент масштабирования

    def clear(self):
        self.canvas.delete("all")

    def set_zoom(self, delta):
        """Изменение зума (delta > 0 увеличение, < 0 уменьшение)"""
        self.zoom *= (1.0 + delta * 0.05)
        if self.zoom < 0.1:
            self.zoom = 0.1
        if self.zoom > 5.0:
            self.zoom = 5.0

    def draw(self, vertices, faces, angle_x, angle_y):
        rotated = [Projection.rotate_point(v, angle_x, angle_y) for v in vertices]
        xs = [p.x for p in rotated]
        ys = [p.y for p in rotated]
        if not xs:
            return

        min_x, max_x = min(xs), max(xs)
        min_y, max_y = min(ys), max(ys)
        range_x = max_x - min_x
        range_y = max_y - min_y
        if range_x < 1e-6:
            range_x = 1
        if range_y < 1e-6:
            range_y = 1

        # Масштабирование с учётом zoom
        base_scale = min(self.width / range_x, self.height / range_y)
        scale = 0.8 * base_scale * self.zoom
        offset_x = self.width / 2.0 - (min_x + max_x) / 2.0 * scale
        offset_y = self.height / 2.0 - (min_y + max_y) / 2.0 * scale

        for face in faces:
            points = []
            for idx in face.indices:
                p = Projection.rotate_point(vertices[idx], angle_x, angle_y)
                sx = p.x * scale + offset_x
                sy = p.y * scale + offset_y
                points.append((sx, sy))

            brightness = int(face.intensity * 255)
            brightness = max(0, min(255, brightness))
            color = f"#{brightness:02x}{brightness:02x}{brightness:02x}"
            self.canvas.create_polygon(points, fill=color, outline="black", width=1)