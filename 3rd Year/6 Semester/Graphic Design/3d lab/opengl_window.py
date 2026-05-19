# opengl_window.py
import sys
import threading
from OpenGL.GL import *
from OpenGL.GLU import *
from OpenGL.GLUT import *
from geometry import Cylinder
from parameters import Parameters

class OpenGLWindow:
    def __init__(self, params):
        self.params = params
        self.cylinder = None
        self.last_x = 0
        self.last_y = 0
        self.rotating = False
        self.init_glut()
        self.init_opengl()
        self.update_geometry()

    def init_glut(self):
        glutInit(sys.argv)
        glutInitDisplayMode(GLUT_DOUBLE | GLUT_RGB | GLUT_DEPTH | GLUT_ALPHA | GLUT_MULTISAMPLE)
        glutInitWindowSize(800, 600)
        glutCreateWindow(b"3D Cylinder - OpenGL (Task #3)")
        glutDisplayFunc(self.display)
        glutReshapeFunc(self.reshape)
        glutMouseFunc(self.mouse_click)
        glutMotionFunc(self.mouse_drag)

    def init_opengl(self):
        glClearColor(0.1, 0.1, 0.2, 1.0)
        glEnable(GL_DEPTH_TEST)
        glEnable(GL_LIGHTING)
        glEnable(GL_LIGHT0)
        glEnable(GL_NORMALIZE)
        glEnable(GL_BLEND)
        glBlendFunc(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA)

        # Материал (серебристый, с прозрачностью)
        glMaterialfv(GL_FRONT, GL_AMBIENT_AND_DIFFUSE, (0.7, 0.7, 0.7, self.params.alpha))
        glMaterialfv(GL_FRONT, GL_SPECULAR, (0.3, 0.3, 0.3, 1.0))
        glMaterialf(GL_FRONT, GL_SHININESS, 50.0)

        # Начальная позиция источника
        glLightfv(GL_LIGHT0, GL_POSITION, (self.params.light_x, self.params.light_y, self.params.light_z, 1.0))
        glLightfv(GL_LIGHT0, GL_DIFFUSE, (1.0, 1.0, 1.0, 1.0))
        glLightfv(GL_LIGHT0, GL_SPECULAR, (1.0, 1.0, 1.0, 1.0))

    def update_geometry(self):
        """Called when the radius or the height is changing."""
        self.cylinder = Cylinder(self.params.radius, self.params.height, slices=36)

    def update_material_alpha(self):
        glMaterialfv(GL_FRONT, GL_AMBIENT_AND_DIFFUSE, (0.7, 0.7, 0.7, self.params.alpha))

    def update_light_position(self):
        """Обновляет положение точечного источника."""
        glLightfv(GL_LIGHT0, GL_POSITION, (self.params.light_x, self.params.light_y, self.params.light_z, 1.0))

    def display(self):
        glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT)
        glLoadIdentity()

        # Перспективная камера (Perspective camera)
        gluLookAt(0.0, 0.0, self.params.zoom,   # позиция камеры(Position)
                  0.0, 0.0, 0.0,                # центр
                  0.0, 1.0, 0.0)               # вектор вверх

        glRotatef(self.params.rot_x, 1.0, 0.0, 0.0)
        glRotatef(self.params.rot_y, 0.0, 1.0, 0.0)

        self.update_material_alpha()
        self.update_light_position()

        if self.cylinder:
            self.cylinder.draw()

        glutSwapBuffers()

    def reshape(self, w, h):
        glViewport(0, 0, w, h)
        glMatrixMode(GL_PROJECTION)
        glLoadIdentity()
        gluPerspective(45.0, w / h if h != 0 else 1.0, 0.1, 100.0)
        glMatrixMode(GL_MODELVIEW)

    def mouse_click(self, button, state, x, y):
        if button == GLUT_LEFT_BUTTON and state == GLUT_DOWN:
            self.last_x, self.last_y = x, y
            self.rotating = True
        elif button == GLUT_LEFT_BUTTON and state == GLUT_UP:
            self.rotating = False
        elif button == 3:   # колёсико вверх
            self.params.zoom += 0.5
            glutPostRedisplay()
        elif button == 4:   # колёсико вниз
            self.params.zoom -= 0.5
            glutPostRedisplay()

    def mouse_drag(self, x, y):
        if self.rotating:
            dx = x - self.last_x
            dy = y - self.last_y
            self.params.rot_y += dx * 0.5
            self.params.rot_x += dy * 0.5
            self.last_x, self.last_y = x, y
            glutPostRedisplay()

    def run(self):
        glutMainLoop()