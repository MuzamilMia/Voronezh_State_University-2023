# face.py
class Face:
    """Грань многогранника: список индексов вершин + свойства"""
    def __init__(self, indices):
        self.indices = indices      # list[int]
        self.normal = None          # Point3D
        self.intensity = 1.0        # освещённость (0..1)
        self.depth = 0.0            # средняя глубина для сортировки

    def __repr__(self):
        return f"Face(indices={self.indices})"