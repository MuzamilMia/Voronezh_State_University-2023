# ============ ЛАБОРАТОРНАЯ РАБОТА №4: Классификация котов и собак ============

import os
import numpy as np
import tensorflow as tf
from tensorflow import keras
from keras import layers
import matplotlib.pyplot as plt
import pandas as pd
from sklearn.metrics import classification_report, confusion_matrix
import seaborn as sns

print("=" * 80)
print("ЛАБОРАТОРНАЯ РАБОТА №4: Классификация изображений котов и собак")
print("=" * 80)


# ============ 1. НАСТРОЙКА ВОСПРОИЗВОДИМОСТИ ============
def set_seed(seed=42):
    """
    ФИКСИРУЕТ СЛУЧАЙНЫЕ ЗНАЧЕНИЯ для одинаковых результатов при каждом запуске
    Важно для научной воспроизводимости экспериментов!
    """
    np.random.seed(seed)
    tf.random.set_seed(seed)
    os.environ['PYTHONHASHSEED'] = str(seed)
    os.environ['TF_DETERMINISTIC_OPS'] = '1'
    print(f"✓ Случайное зерно установлено: {seed}")


set_seed(42)

# ============ 2. АНАЛИЗ НАБОРА ДАННЫХ ============
print("\n" + "=" * 80)
print("ШАГ 1: Информация о наборе данных")
print("=" * 80)

# Автоматическое определение путей к папкам train/test
BASE_DIR = os.path.dirname(os.path.abspath(__file__))
TRAIN_DIR = os.path.join(BASE_DIR, 'train')  # train/cats, train/dogs
TEST_DIR = os.path.join(BASE_DIR, 'test')  # test/cats, test/dogs

"""
    ПОСЧИТЫВАЕТ КОЛИЧЕСТВО ИЗОБРАЖЕНИЙ В КАЖДОМ КЛАССЕ
    Показывает баланс классов котов/собак
    """
def count_images(directory):
    counts = {}
    for class_name in os.listdir(directory):
        class_path = os.path.join(directory, class_name)
        if os.path.isdir(class_path):
            # Считаем только изображения
            images = [f for f in os.listdir(class_path)
                      if f.lower().endswith(('.png', '.jpg', '.jpeg'))]
            counts[class_name] = len(images)
    return counts


# Вывод статистики
train_counts = count_images(TRAIN_DIR)
test_counts = count_images(TEST_DIR)
print("\n📊 СТАТИСТИКА НАБОРА ДАННЫХ:")
print(f"Обучающий набор: {sum(train_counts.values())} изображений")
print(f"Тестовый набор: {sum(test_counts.values())} изображений")

# ============ 3. ЗАГРУЗКА И ПРЕДОБРАБОТКА ДАННЫХ ============
print("\n" + "=" * 80)
print("ШАГ 2: Загрузка данных с помощью Keras")
print("=" * 80)

IMG_SIZE = (128, 128)  # Размер изображений для ускорения
BATCH_SIZE = 64  # Большой батч для GPU

# **КЛЮЧЕВАЯ ФУНКЦИЯ**: image_dataset_from_directory автоматически
train_ds = keras.preprocessing.image_dataset_from_directory(
    TRAIN_DIR,
    image_size=IMG_SIZE,
    batch_size=BATCH_SIZE,
    label_mode='binary',  # 0/1 для двух классов
    shuffle=True,
    seed=42,
    color_mode='rgb'
)

test_ds = keras.preprocessing.image_dataset_from_directory(
    TEST_DIR,
    image_size=IMG_SIZE,
    batch_size=BATCH_SIZE,
    label_mode='binary',
    shuffle=False,  # Для теста не перемешиваем
    color_mode='rgb'
)

class_names = train_ds.class_names
print(f"✓ КЛАССЫ: {class_names[0]}={0}, {class_names[1]}={1}")

# ============ 4. ВИЗУАЛИЗАЦИЯ ПРИМЕРОВ ============
print("\n" + "=" * 80)
print("ШАГ 3: Проверка данных")
print("=" * 80)

plt.figure(figsize=(12, 8))
for images, labels in train_ds.take(1):
    for i in range(12):
        ax = plt.subplot(3, 4, i + 1)
        plt.imshow(images[i].numpy().astype("uint8"))
        plt.title(f"{'Кот' if labels[i] == 0 else 'Собака'}")
        plt.axis("off")
plt.suptitle(" ПРИМЕРЫ ИЗОБРАЖЕНИЙ", fontsize=16, fontweight='bold')
plt.tight_layout()
plt.show()

# ============ 5. ОПТИМИЗАЦИЯ ДАННЫХ ============
print("\n" + "=" * 80)
print("ШАГ 4: Аугментация и нормализация")
print("=" * 80)

# **АУГМЕНТАЦИЯ**: искусственно увеличивает датасет
data_augmentation = keras.Sequential([
    layers.RandomFlip("horizontal"),  # Переворот по горизонтали
    layers.RandomRotation(0.1),  # Поворот ±10%
    layers.RandomZoom(0.1),  # Масштабирование
    layers.RandomContrast(0.1),  # Контрастность
])

##НОРМАЛИЗАЦИЯ
def preprocess(image, label):
    image = tf.cast(image, tf.float32) / 255.0
    return image, label


def augment(image, label):
    """ПРИМЕНЯЕТ АУГМЕНТАЦИЮ ТОЛЬКО К ОБУЧАЮЩИМ ДАННЫМ"""
    image = data_augmentation(image, training=True)
    return image, label


AUTOTUNE = tf.data.AUTOTUNE

# **ОПТИМИЗАЦИЯ**: cache(кэш в RAM) + prefetch(параллельная загрузка)
train_ds_processed = (
    train_ds
    .map(preprocess, num_parallel_calls=AUTOTUNE)
    .map(augment, num_parallel_calls=AUTOTUNE)
    .cache()
    .shuffle(500)
    .prefetch(buffer_size=AUTOTUNE)
)

test_ds_processed = (
    test_ds
    .map(preprocess, num_parallel_calls=AUTOTUNE)
    .cache()
    .prefetch(buffer_size=AUTOTUNE)
)

# ============ 6. ТРИ МОДЕЛИ CNN ============
print("\n" + "=" * 80)
print("ШАГ 5: СОЗДАНИЕ ТРЁХ МОДЕЛЕЙ РАЗЛИЧНОЙ СЛОЖНОСТИ")
print("=" * 80)

# **МОДЕЛЬ 1: ПРОСТАЯ CNN** (2 блока Conv → Dense)
model1 = keras.Sequential([
    layers.Input(shape=IMG_SIZE + (3,)),
    layers.Conv2D(32, 3, activation='relu', padding='same'),
    layers.MaxPooling2D(2),
    layers.Dropout(0.25),

    layers.Conv2D(64, 3, activation='relu', padding='same'),  # 64 фильтра
    layers.MaxPooling2D(2),
    layers.Dropout(0.25),

    layers.Flatten(),  #  Вектор
    layers.Dense(128, activation='relu'),
    layers.Dropout(0.5),
    layers.Dense(1, activation='sigmoid')
], name="Simple_CNN")

# **МОДЕЛЬ 2: СРЕДНЯЯ CNN** (+ BatchNormalization, 3 блока)
model2 = keras.Sequential([
    layers.Input(shape=IMG_SIZE + (3,)),
    layers.Conv2D(32, 3, activation='relu', padding='same'),
    layers.BatchNormalization(),  #  Стабилизирует обучение
    layers.MaxPooling2D(2),

    layers.Conv2D(64, 3, activation='relu', padding='same'),
    layers.BatchNormalization(),
    layers.MaxPooling2D(2),

    layers.Conv2D(128, 3, activation='relu', padding='same'),  #  +128 фильтров
    layers.BatchNormalization(),
    layers.MaxPooling2D(2),
    layers.Dropout(0.3),

    layers.Flatten(),
    layers.Dense(128, activation='relu'),
    layers.Dropout(0.5),
    layers.Dense(1, activation='sigmoid')
], name="Medium_CNN")

# **МОДЕЛЬ 3: ГЛУБОКАЯ CNN** (4 блока + 256 фильтров)
model3 = keras.Sequential([
    layers.Input(shape=IMG_SIZE + (3,)),
    layers.Conv2D(32, 3, activation='relu', padding='same'),
    layers.BatchNormalization(),
    layers.MaxPooling2D(2),

    layers.Conv2D(64, 3, activation='relu', padding='same'),
    layers.BatchNormalization(),
    layers.MaxPooling2D(2),

    layers.Conv2D(128, 3, activation='relu', padding='same'),
    layers.BatchNormalization(),
    layers.MaxPooling2D(2),

    layers.Conv2D(256, 3, activation='relu', padding='same'),  #  Максимальная глубина
    layers.BatchNormalization(),
    layers.MaxPooling2D(2),
    layers.Dropout(0.4),

    layers.Flatten(),
    layers.Dense(256, activation='relu'),  # Больше нейронов
    layers.Dropout(0.5),
    layers.Dense(1, activation='sigmoid')
], name="Deep_CNN")

# РЕГИСТР МОДЕЛЕЙ
models = {
    "Модель 1 (Простая)": model1,
    "Модель 2 (Средняя)": model2,
    "Модель 3 (Глубокая)": model3
}

# **КОМПИЛЯЦИЯ**: Adam + Binary Crossentropy
for name, model in models.items():
    model.compile(
        optimizer=keras.optimizers.Adam(0.0005),  #  Малый learning rate
        loss='binary_crossentropy',  #  Для бинарной классификации
        metrics=['accuracy']
    )
    print(f"✓ {name}: {model.count_params():,} параметров")

# ============ 7. ОБУЧЕНИЕ ============
print("\n" + "=" * 80)
print("ШАГ 6: Обучение всех моделей")
print("=" * 80)

EPOCHS = 5  #  Достаточно для демонстрации кривых
histories = {}

for name, model in models.items():
    print(f"\nОбучение {name}...")
    history = model.fit(
        train_ds_processed,  # Обучающие данные с аугментацией
        validation_data=test_ds_processed,  # Тест как валидация
        epochs=EPOCHS,
        verbose=1
    )
    histories[name] = history
    print(f"✓ {name} обучена")

# ============ 8. ГРАФИКИ ПОТЕРЕЙ (ТРЕБОВАНИЕ No:4) ============
print("\n" + "=" * 80)
print("ШАГ 7: АНАЛИЗ КРИВЫХ ОБУЧЕНИЯ (3 модели × 2 графика)")
print("=" * 80)

# **6 ГРАФИКОВ В ОДНОЙ СЕТКЕ** для сравнения
fig, axes = plt.subplots(3, 2, figsize=(15, 18))
axes = axes.ravel()

for idx, (name, history) in enumerate(histories.items()):
    # **ГРАФИК ТОЧНОСТИ**
    axes[idx * 2].plot(history.history['accuracy'], label='Обучающая точность', linewidth=2)
    axes[idx * 2].plot(history.history['val_accuracy'], label='Валидационная точность', linewidth=2)
    axes[idx * 2].set_title(f"{name} - ТОЧНОСТЬ", fontweight='bold', fontsize=14)
    axes[idx * 2].legend()
    axes[idx * 2].grid(True, alpha=0.3)

    # **ГРАФИК ПОТЕРЕЙ** (главное требование!)
    axes[idx * 2 + 1].plot(history.history['loss'], label='Обучающая потеря', linewidth=2)
    axes[idx * 2 + 1].plot(history.history['val_loss'], label='Валидационная потеря', linewidth=2)
    axes[idx * 2 + 1].set_title(f"{name} - ПОТЕРИ", fontweight='bold', fontsize=14)
    axes[idx * 2 + 1].legend()
    axes[idx * 2 + 1].grid(True, alpha=0.3)

plt.tight_layout()
plt.show()

# ============ 9. ИТОГОВАЯ ОЦЕНКА ============
print("\n" + "=" * 80)
print("ШАГ 8: СРАВНЕНИЕ МОДЕЛЕЙ")
print("=" * 80)

results = {}
for name, model in models.items():
    loss, acc = model.evaluate(test_ds_processed, verbose=0)
    results[name] = {'accuracy': acc, 'loss': loss}
    print(f"{name}: Точность={acc:.3f}, Потери={loss:.3f}")

best_model=max(results.keys(),key=lambda k:results[k]['accuracy'])
print(f"Best Model:{best_model}(accuracy:{results[best_model]['accuracy']:.3f}")