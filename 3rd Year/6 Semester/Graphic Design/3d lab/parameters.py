# parameters.py
class Parameters:
    """Хранилище всех изменяемых параметров сцены."""
    def __init__(self):
        # Геометрия цилиндра
        self.radius = 1.0
        self.height = 2.0

        # Прозрачность (0..1)
        self.alpha = 0.6

        # Точечный источник света (координаты)
        self.light_x = 3.0
        self.light_y = 4.0
        self.light_z = 90.0

        # Вращение и масштаб камеры (rotation and scale)
        self.rot_x = 30.0      # градус поворота вокруг X
        self.rot_y = 45.0      # градус поворота вокруг Y
        self.zoom = -8.0       # расстояние камеры (отрицательное по Z)

    def set_alpha(self, a):
        self.alpha = max(0.0, min(1.0, a))

    def set_shape(self, radius, height):
        self.radius = radius
        self.height = height

    def set_light(self, x, y, z):
        self.light_x, self.light_y, self.light_z = x, y, z