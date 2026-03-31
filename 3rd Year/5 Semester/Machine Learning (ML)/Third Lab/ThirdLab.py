
import pandas as pd
import numpy as np
import tensorflow as tf
from tensorflow import keras
from keras import layers, callbacks
import matplotlib.pyplot as plt
from sklearn.model_selection import train_test_split
from sklearn.preprocessing import StandardScaler
import warnings

warnings.filterwarnings('ignore')

# 1. Загрузка и подготовка данных
print("1. Загрузка и подготовка данных...")

# Создадим синтетические данные для демонстрации
# Генерируем данные для регрессии
np.random.seed(42)
n_samples = 1000
n_features = 10

X = np.random.randn(n_samples, n_features)
# Создаем целевую переменную с нелинейной зависимостью
y = (X[:, 0] ** 2 + np.sin(X[:, 1] * 3) +
     X[:, 2] * X[:, 3] + np.random.randn(n_samples) * 0.1)

# Разделение на тренировочную и тестовую выборки
X_train, X_test, y_train, y_test = train_test_split(
    X, y, test_size=0.2, random_state=42
)

# Масштабирование данных
scaler = StandardScaler()
X_train_scaled = scaler.fit_transform(X_train)
X_test_scaled = scaler.transform(X_test)

print(f"Размер тренировочной выборки: {X_train_scaled.shape}")
print(f"Размер тестовой выборки: {X_test_scaled.shape}")

# 2. Создание нескольких моделей с различной структурой
print("\n2. Создание моделей с различной структурой...")


# Модель 1: Базовая модель (мелкая сеть)
def create_basic_model():
    model = keras.Sequential([
        layers.Dense(64, activation='relu', input_shape=[n_features]),
        layers.Dense(32, activation='relu'),
        layers.Dense(1)  # Выходной слой для регрессии
    ])
    model.compile(
        optimizer='adam',
        loss='mse',
        metrics=['mae']
    )
    return model


# Модель 2: Широкая сеть
def create_wide_model():
    model = keras.Sequential([
        layers.Dense(256, activation='relu', input_shape=[n_features]),
        layers.Dense(128, activation='relu'),
        layers.Dense(64, activation='relu'),
        layers.Dense(1)
    ])
    model.compile(
        optimizer='adam',
        loss='mse',
        metrics=['mae']
    )
    return model


# Модель 3: Глубокая сеть
def create_deep_model():
    model = keras.Sequential([
        layers.Dense(64, activation='relu', input_shape=[n_features]),
        layers.Dense(64, activation='relu'),
        layers.Dense(64, activation='relu'),
        layers.Dense(64, activation='relu'),
        layers.Dense(32, activation='relu'),
        layers.Dense(1)
    ])
    model.compile(
        optimizer='adam',
        loss='mse',
        metrics=['mae']
    )
    return model


# Модель 4: Сеть с регуляризацией
def create_regularized_model():
    model = keras.Sequential([
        layers.Dense(128, activation='relu', input_shape=[n_features]),
        layers.BatchNormalization(),
        layers.Dropout(0.3),

        layers.Dense(64, activation='relu'),
        layers.BatchNormalization(),
        layers.Dropout(0.3),

        layers.Dense(32, activation='relu'),
        layers.Dropout(0.2),

        layers.Dense(1)
    ])
    model.compile(
        optimizer='adam',
        loss='mse',
        metrics=['mae']
    )
    return model


# Модель 5: Сеть с различными функциями активации
def create_mixed_activation_model():
    model = keras.Sequential([
        layers.Dense(128, activation='relu', input_shape=[n_features]),
        layers.Dense(64, activation='tanh'),
        layers.Dense(32, activation='swish'),
        layers.Dense(16, activation='relu'),
        layers.Dense(1)
    ])
    model.compile(
        optimizer='adam',
        loss='mse',
        metrics=['mae']
    )
    return model


# Создаем модели
models = {
    'Basic Model': create_basic_model(),
    'Wide Model': create_wide_model(),
    'Deep Model': create_deep_model(),
    'Regularized Model': create_regularized_model(),
    'Mixed Activation Model': create_mixed_activation_model()
}

print("Модели созданы:")
for name, model in models.items():
    print(f"  - {name}: {model.count_params()} параметров")

# 3. Обучение моделей и сбор истории
print("\n3. Обучение моделей...")

# Callback для ранней остановки
early_stopping = callbacks.EarlyStopping(
    monitor='val_loss',
    patience=15,
    restore_best_weights=True,
    verbose=0
)

# Словарь для хранения историй обучения
histories = {}

# Обучаем каждую модель
for name, model in models.items():
    print(f"Обучение {name}...")

    history = model.fit(
        X_train_scaled, y_train,
        validation_data=(X_test_scaled, y_test),
        epochs=100,
        batch_size=32,
        callbacks=[early_stopping],
        verbose=0
    )

    histories[name] = history
    print(f"  {name} обучена за {len(history.history['loss'])} эпох")

# 4. Построение графиков потерь и сравнение моделей
print("\n4. Построение графиков и сравнение моделей...")

# Создаем subplot для графиков
fig, axes = plt.subplots(2, 3, figsize=(18, 10))
axes = axes.flatten()

# Цвета для разных моделей
colors = ['blue', 'red', 'green', 'orange', 'purple']

# Графики для каждой модели
for idx, (name, history) in enumerate(histories.items()):
    ax = axes[idx]

    # График потерь на тренировочной и валидационной выборке
    ax.plot(history.history['loss'], color=colors[idx], linestyle='-', label='Training Loss')
    ax.plot(history.history['val_loss'], color=colors[idx], linestyle='--', label='Validation Loss')

    ax.set_title(f'{name}\nParams: {models[name].count_params()}')
    ax.set_xlabel('Epochs')
    ax.set_ylabel('Loss (MSE)')
    ax.legend()
    ax.grid(True, alpha=0.3)

# Сравнительный график валидационных потерь всех моделей
ax = axes[5]
for idx, (name, history) in enumerate(histories.items()):
    ax.plot(history.history['val_loss'], color=colors[idx], label=name, linewidth=2)

ax.set_title('Сравнение валидационных потерь всех моделей')
ax.set_xlabel('Epochs')
ax.set_ylabel('Validation Loss (MSE)')
ax.legend()
ax.grid(True, alpha=0.3)

plt.tight_layout()
plt.show()

# Детальный анализ результатов
print("\n5. ДЕТАЛЬНЫЙ АНАЛИЗ РЕЗУЛЬТАТОВ:")
print("=" * 50)

# Собираем финальные метрики
results = []
for name, history in histories.items():
    final_train_loss = history.history['loss'][-1]
    final_val_loss = history.history['val_loss'][-1]
    best_val_loss = min(history.history['val_loss'])
    epochs_trained = len(history.history['loss'])
    params = models[name].count_params()

    results.append({
        'Model': name,
        'Parameters': params,
        'Epochs': epochs_trained,
        'Final Train Loss': final_train_loss,
        'Final Val Loss': final_val_loss,
        'Best Val Loss': best_val_loss
    })

# Создаем DataFrame для удобного отображения
results_df = pd.DataFrame(results)
results_df = results_df.sort_values('Best Val Loss')

print("Результаты моделей (отсортированы по лучшей валидационной ошибке):")
print(results_df.to_string(index=False))

# Визуализация сравнения лучших валидационных потерь
plt.figure(figsize=(12, 6))

# График лучших валидационных потерь
plt.subplot(1, 2, 1)
best_losses = [min(history.history['val_loss']) for history in histories.values()]
model_names = list(histories.keys())
bars = plt.bar(model_names, best_losses, color=colors[:len(models)])
plt.title('Лучшие валидационные потери по моделям')
plt.ylabel('Validation Loss (MSE)')
plt.xticks(rotation=45)
# Добавляем значения на столбцы
for bar, value in zip(bars, best_losses):
    plt.text(bar.get_x() + bar.get_width() / 2, bar.get_height() + 0.001,
             f'{value:.4f}', ha='center', va='bottom')

# График количества параметров vs качество
plt.subplot(1, 2, 2)
params = [model.count_params() for model in models.values()]
plt.scatter(params, best_losses, s=100, c=colors[:len(models)])
for i, name in enumerate(model_names):
    plt.annotate(name, (params[i], best_losses[i]),
                 xytext=(5, 5), textcoords='offset points')
plt.xlabel('Количество параметров')
plt.ylabel('Лучшая валидационная ошибка')
plt.title('Сложность модели vs Качество')
plt.grid(True, alpha=0.3)

plt.tight_layout()
plt.show()

# 6. Выводы
print("\n6. ВЫВОДЫ :")
print("=" * 50)

best_model_name = results_df.iloc[0]['Model']
best_loss = results_df.iloc[0]['Best Val Loss']
worst_model_name = results_df.iloc[-1]['Model']
worst_loss = results_df.iloc[-1]['Best Val Loss']

print(f"✓ Лучшая модель: {best_model_name}")
print(f"  - Лучшая валидационная ошибка: {best_loss:.4f}")
print(f"  - Количество параметров: {results_df.iloc[0]['Parameters']}")
print(f"  - Эпох обучения: {results_df.iloc[0]['Epochs']}")

print(f"\n✗ Худшая модель: {worst_model_name}")
print(f"  - Лучшая валидационная ошибка: {worst_loss:.4f}")

# Анализ переобучения
print(f"\nАНАЛИЗ ПЕРЕОБУЧЕНИЯ:")
for _, row in results_df.iterrows():
    overfitting_ratio = row['Final Val Loss'] / row['Final Train Loss']
    print(f"  {row['Model']}: отношение val/train loss = {overfitting_ratio:.2f}")

# Дополнительная информация о сходимости (additional Information)
print(f"\nАНАЛИЗ СХОДИМОСТИ:")
for name, history in histories.items():
    final_epoch = len(history.history['loss'])
    improvement = history.history['val_loss'][0] - history.history['val_loss'][-1]
    print(f"  {name}: улучшение за {final_epoch} эпох = {improvement:.4f}")

# Сохранение лучшей модели
best_model = models[best_model_name]
print(f"\nЛучшая модель '{best_model_name}' сохранена для дальнейшего использования.")