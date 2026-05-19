# control_panel.py
import tkinter as tk
from tkinter import ttk
import sys

class ControlPanel:
    def __init__(self, params, opengl_window):
        self.params = params
        self.opengl = opengl_window
        self.root = tk.Tk()
        self.root.title("Управление параметрами")
        self.root.geometry("340x450")
        self.root.protocol("WM_DELETE_WINDOW", self.on_close)

        # Привязка переменных Tkinter к параметрам
        self.radius_var = tk.DoubleVar(value=params.radius)
        self.height_var = tk.DoubleVar(value=params.height)
        self.alpha_var = tk.DoubleVar(value=params.alpha)
        self.lx_var = tk.DoubleVar(value=params.light_x)
        self.ly_var = tk.DoubleVar(value=params.light_y)
        self.lz_var = tk.DoubleVar(value=params.light_z)

        self.create_widgets()
        self.update_from_gui()

    def create_widgets(self):
        frame = ttk.Frame(self.root, padding=10)
        frame.pack(fill=tk.BOTH, expand=True)

        # --- Геометрия цилиндра ---
        ttk.Label(frame, text="Геометрия цилиндра", font=("Arial", 10, "bold")).grid(row=0, column=0, columnspan=2, pady=5, sticky=tk.W)
        ttk.Label(frame, text="Радиус:").grid(row=1, column=0, sticky=tk.W)
        ttk.Scale(frame, from_=0.3, to=3.0, variable=self.radius_var, orient=tk.HORIZONTAL, command=lambda _: self.on_shape_change()).grid(row=1, column=1, sticky=tk.EW)
        ttk.Label(frame, text="Высота:").grid(row=2, column=0, sticky=tk.W)
        ttk.Scale(frame, from_=0.5, to=5.0, variable=self.height_var, orient=tk.HORIZONTAL, command=lambda _: self.on_shape_change()).grid(row=2, column=1, sticky=tk.EW)

        # --- Прозрачность ---
        ttk.Label(frame, text="Прозрачность (α)", font=("Arial", 10, "bold")).grid(row=3, column=0, columnspan=2, pady=(10,5), sticky=tk.W)
        ttk.Scale(frame, from_=0.0, to=1.0, variable=self.alpha_var, orient=tk.HORIZONTAL, command=lambda _: self.on_alpha_change()).grid(row=4, column=0, columnspan=2, sticky=tk.EW)
        self.alpha_label = ttk.Label(frame, text=f"α = {self.params.alpha:.2f}")
        self.alpha_label.grid(row=5, column=0, columnspan=2)

        # --- Точечный источник света ---
        ttk.Label(frame, text="Точечный источник", font=("Arial", 10, "bold")).grid(row=6, column=0, columnspan=2, pady=(10,5), sticky=tk.W)
        ttk.Label(frame, text="X:").grid(row=7, column=0, sticky=tk.W)
        ttk.Scale(frame, from_=-5.0, to=5.0, variable=self.lx_var, orient=tk.HORIZONTAL, command=lambda _: self.on_light_change()).grid(row=7, column=1, sticky=tk.EW)
        ttk.Label(frame, text="Y:").grid(row=8, column=0, sticky=tk.W)
        ttk.Scale(frame, from_=-5.0, to=5.0, variable=self.ly_var, orient=tk.HORIZONTAL, command=lambda _: self.on_light_change()).grid(row=8, column=1, sticky=tk.EW)
        ttk.Label(frame, text="Z:").grid(row=9, column=0, sticky=tk.W)
        ttk.Scale(frame, from_=-5.0, to=5.0, variable=self.lz_var, orient=tk.HORIZONTAL, command=lambda _: self.on_light_change()).grid(row=9, column=1, sticky=tk.EW)

        # --- Информация для пользователя ---
        info = ttk.Label(frame, text="\nУправление в окне OpenGL:\n• Левая кнопка + движение — вращение\n• Колёсико мыши — масштаб", justify=tk.LEFT, foreground="gray")
        info.grid(row=10, column=0, columnspan=2, pady=15)

        # Кнопка принудительного обновления
        ttk.Button(frame, text="Применить", command=self.force_update).grid(row=11, column=0, columnspan=2, pady=5)

        frame.columnconfigure(1, weight=1)

    def on_shape_change(self):
        self.params.radius = self.radius_var.get()
        self.params.height = self.height_var.get()
        self.opengl.update_geometry()
        self._redraw()

    def on_alpha_change(self):
        a = self.alpha_var.get()
        self.params.set_alpha(a)
        self.alpha_label.config(text=f"α = {a:.2f}")
        self._redraw()

    def on_light_change(self):
        self.params.light_x = self.lx_var.get()
        self.params.light_y = self.ly_var.get()
        self.params.light_z = self.lz_var.get()
        self._redraw()

    def force_update(self):
        self.on_shape_change()
        self.on_alpha_change()
        self.on_light_change()

    def update_from_gui(self):
        # Приводим Tkinter-переменные в соответствие с текущими параметрами
        self.radius_var.set(self.params.radius)
        self.height_var.set(self.params.height)
        self.alpha_var.set(self.params.alpha)
        self.lx_var.set(self.params.light_x)
        self.ly_var.set(self.params.light_y)
        self.lz_var.set(self.params.light_z)
        self.on_shape_change()
        self.on_alpha_change()
        self.on_light_change()

    def _redraw(self):
        if hasattr(self, 'redraw_callback'):
            self.redraw_callback()
        else:
            # Если callback не установлен, ничего не делаем
            pass

    def set_redraw_callback(self, callback):
        self.redraw_callback = callback

    def on_close(self):
        sys.exit(0)

    def run(self):
        self.root.mainloop()