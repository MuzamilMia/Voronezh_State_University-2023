# lighting.py
from point3d import Point3D

class Lighting:
    """Диффузное освещение от бесконечно удалённого источника"""
    @staticmethod
    def compute_intensities(faces, light_dir):
        """
        faces: список Face
        light_dir: Point3D (направление на источник, не нормированное)
        """
        l = Point3D.normalize(light_dir)
        for face in faces:
            intensity = Point3D.dot(face.normal, l)
            if intensity < 0:
                intensity = 0
            face.intensity = intensity