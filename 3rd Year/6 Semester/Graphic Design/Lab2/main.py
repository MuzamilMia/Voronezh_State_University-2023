# main.py
import tkinter as tk
from tkinter import ttk
import math

from point3d import Point3D
from geometry_builder import GeometryBuilder
from lighting import Lighting
from projection import Projection
from renderer import Renderer
"""
Number:bca 
1.	Тип фигуры: 
          a. правильная трапеция с заданной высотой, радиусом основания и количеством боковых граней;
2.	Модель освещения фигуры:
          c.	один бесконечно удалённый источник с заданным направлением света;
3.	Тип проекции:
          a. параллельная (задаются углы поворота);
"""

class App:
    def __init__(self, root):
        self.root = root
        root.title("3D Усечённая пирамида (трапеция) — управление мышью")

        # Параметры по умолчанию
        self.params = {
            'n': 6,
            'height': 4.0,
            'radius_bottom': 3.0,
            'radius_top': 1.5,
            'angle_x': 30.0,
            'angle_y': 45.0,
            'light_x': 1.0,
            'light_y': 1.0,
            'light_z': 1.0
        }

        # For the mouse
        self.drag_start_x = 0
        self.drag_start_y = 0
        self.start_angle_x = 0.0
        self.start_angle_y = 0.0
        self.dragging = False

        self._create_controls()
        self._create_canvas()

        # Начальная геометрия
        self.vertices, self.faces = GeometryBuilder.build(
            self.params['n'], self.params['height'],
            self.params['radius_bottom'], self.params['radius_top']
        )
        self._update_scene()

        # Привязка событий мыши к холсту
        self.canvas.bind("<ButtonPress-1>", self.on_mouse_down)
        self.canvas.bind("<B1-Motion>", self.on_mouse_drag)
        self.canvas.bind("<ButtonRelease-1>", self.on_mouse_up)
        self.canvas.bind("<MouseWheel>", self.on_mouse_wheel)
        self.canvas.bind("<Button-4>", self.on_mouse_wheel)
        self.canvas.bind("<Button-5>", self.on_mouse_wheel)

    def _create_controls(self):
        control_frame = ttk.Frame(self.root, padding=5)
        control_frame.pack(side=tk.LEFT, fill=tk.Y)

        row = 0
        labels = [
            ("Граней (n):", 'n', int),
            ("Высота H:", 'height', float),
            ("R нижн.:", 'radius_bottom', float),
            ("R верхн.:", 'radius_top', float),
            ("Угол X (град):", 'angle_x', float),
            ("Угол Y (град):", 'angle_y', float),
            ("Свет Lx:", 'light_x', float),
            ("Свет Ly:", 'light_y', float),
            ("Свет Lz:", 'light_z', float),
        ]
        self.entries = {}
        for label_text, key, typ in labels:
            ttk.Label(control_frame, text=label_text).grid(row=row, column=0, sticky=tk.W, pady=2)
            var = tk.StringVar(value=str(self.params[key]))
            entry = ttk.Entry(control_frame, textvariable=var, width=10)
            entry.grid(row=row, column=1, pady=2)
            self.entries[key] = (var, typ)
            row += 1

        btn = ttk.Button(control_frame, text="Построить", command=self._rebuild)
        btn.grid(row=row, column=0, columnspan=2, pady=10)

    def _create_canvas(self):
        self.canvas = tk.Canvas(self.root, bg='white', width=600, height=500)
        self.canvas.pack(side=tk.RIGHT, fill=tk.BOTH, expand=True)
        self.renderer = Renderer(self.canvas, 600, 500)

    # --- Обработка мыши ---
    def on_mouse_down(self, event):
        self.drag_start_x = event.x
        self.drag_start_y = event.y
        self.start_angle_x = self.params['angle_x']
        self.start_angle_y = self.params['angle_y']
        self.dragging = True

    def on_mouse_drag(self, event):
        if not self.dragging:
            return

        dx = event.x - self.drag_start_x
        dy = event.y - self.drag_start_y
        self.params['angle_y'] = self.start_angle_y + dx * 0.5
        self.params['angle_x'] = self.start_angle_x + dy * 0.5
        # Обновляем поля ввода (чтобы видеть текущие углы)
        self.entries['angle_x'][0].set(f"{self.params['angle_x']:.1f}")
        self.entries['angle_y'][0].set(f"{self.params['angle_y']:.1f}")
        self._update_scene()

    def on_mouse_up(self, event):
        self.dragging = False

    def on_mouse_wheel(self, event):
        # Определяем направление прокрутки
        if event.delta:
            delta = event.delta / 120.0
        elif event.num == 4:
            delta = 1
        elif event.num == 5:
            delta = -1
        else:
            delta = 0
        self.renderer.set_zoom(delta)
        self._update_scene()


    def _rebuild(self):
        try:
            n = int(float(self.entries['n'][0].get()))
            self.params['n'] = max(3, n)
            self.params['height'] = float(self.entries['height'][0].get())
            self.params['radius_bottom'] = float(self.entries['radius_bottom'][0].get())
            self.params['radius_top'] = float(self.entries['radius_top'][0].get())
            self.params['angle_x'] = float(self.entries['angle_x'][0].get())
            self.params['angle_y'] = float(self.entries['angle_y'][0].get())
            lx = float(self.entries['light_x'][0].get())
            ly = float(self.entries['light_y'][0].get())
            lz = float(self.entries['light_z'][0].get())

            length = math.hypot(math.hypot(lx, ly), lz)
            if length > 1e-8:
                self.params['light_x'] = lx / length
                self.params['light_y'] = ly / length
                self.params['light_z'] = lz / length
            else:
                self.params['light_x'], self.params['light_y'], self.params['light_z'] = 1.0, 0.0, 0.0

            self.vertices, self.faces = GeometryBuilder.build(
                self.params['n'], self.params['height'],
                self.params['radius_bottom'], self.params['radius_top']
            )
            self._update_scene()
        except ValueError:
            pass

    def _update_scene(self):
        light_dir = Point3D(self.params['light_x'],
                            self.params['light_y'],
                            self.params['light_z'])
        Lighting.compute_intensities(self.faces, light_dir)
        Projection.sort_faces(self.faces, self.vertices,
                              self.params['angle_x'], self.params['angle_y'])
        self.renderer.clear()
        self.renderer.draw(self.vertices, self.faces,
                           self.params['angle_x'], self.params['angle_y'])


if __name__ == "__main__":
    root = tk.Tk()
    root.geometry("850x550")
    app = App(root)
    root.mainloop()