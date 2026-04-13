import pygame
import sys
import math

# -----------------------------
# Линейная интерполяция
# -----------------------------
def lerp(A, B, t):
    return (A[0] * (1 - t) + B[0] * t,A[1] * (1 - t) + B[1] * t)

# -----------------------------
# Алгоритм де Кастельжо
# -----------------------------
def de_casteljau(points, t):
    temp = list(points)
    n = len(points)
    for k in range(1, n):
        for i in range(n - k):
            temp[i] = lerp(temp[i], temp[i + 1], t)
    return temp[0]

# -----------------------------
# Рисование кривой пикселями
# -----------------------------
def draw_bezier(screen, points, color):
    t = 0.0
    while t <= 1.0:
        x, y = de_casteljau(points, t)
        pygame.draw.circle(screen, color, (int(x), int(y)), 1)
        t += 0.001


# -----------------------------
# Основная программа
# -----------------------------
pygame.init()
screen = pygame.display.set_mode((800, 600), pygame.RESIZABLE)
pygame.display.set_caption("Bezier Curve — de Casteljau")

# Контрольные точки
points = [(100, 500), (200, 100), (600, 100), (700, 500)]

running = True
while running:
    for event in pygame.event.get():
        if event.type == pygame.QUIT:
            running = False

    screen.fill((0, 0, 0))

    # Рисуем контрольный многоугольник
    pygame.draw.lines(screen, (100, 100, 255), False, points, 1)

    # Рисуем контрольные точки
    for p in points:
        pygame.draw.circle(screen, (255, 0, 0), p, 5)

    # Рисуем кривую Безье
    draw_bezier(screen, points, (255, 255, 255))

    pygame.display.flip()

pygame.quit()
sys.exit()