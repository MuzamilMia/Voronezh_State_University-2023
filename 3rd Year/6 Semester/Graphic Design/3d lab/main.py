# main.py
import threading
from OpenGL.GLUT import glutPostRedisplay
from parameters import Parameters
from opengl_window import OpenGLWindow
from control_panel import ControlPanel

"""
# Number:cbab
Фигура: цилиндр с заданной высотой и радиусом основания.
Прозрачность: заданный коэффициент α ∈ [0,1].
Освещение: одноточечный источник с задаваемым положением.
Проекция: из точки (перспективная)."""

def main():
    params = Parameters()
    opengl_win = OpenGLWindow(params)

    control = ControlPanel(params, opengl_win)

    def redraw():
        glutPostRedisplay()
    control.set_redraw_callback(redraw)

    # Запуск Tkinter в отдельном потоке (так как GLUT требует главный поток)
    tk_thread = threading.Thread(target=control.run, daemon=True)
    tk_thread.start()

    # Запуск GLUT в главном потоке
    opengl_win.run()

if __name__ == "__main__":
    main()