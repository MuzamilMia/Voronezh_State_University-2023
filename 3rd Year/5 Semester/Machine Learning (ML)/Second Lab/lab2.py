import pandas as pd
from sklearn.model_selection import train_test_split, cross_val_score
from sklearn.ensemble import RandomForestRegressor
from sklearn.impute import SimpleImputer
from sklearn.preprocessing import OneHotEncoder
from sklearn.compose import ColumnTransformer
from sklearn.pipeline import Pipeline
from sklearn.metrics import mean_absolute_error
from xgboost import XGBRegressor

# 1. Прочитать данные из датасета с помощью функции read_csv
print("1. Загрузка данных...")
data = pd.read_csv('data.csv')
print(f"Размер датасета: {data.shape}")

# 2. Выбрать столбец для прогнозирования (y)
print("\n2. Выбор целевой переменной...")
y = data['House_Price']
print(f"Целевая переменная: House_Price")
print(f"Диапазон цен: ${y.min():,} - ${y.max():,}")

# 3. Среди оставшихся столбцов найти те, чья кардинальность не превышает 8 значений
print("\n3. Отбор признаков...")
X = data.drop(['House_Price'], axis=1)
# Категориальные столбцы с кардинальностью <= 8
categorical_cols = [col for col in X.columns
                   if X[col].dtype == 'object' and X[col].nunique() <= 8]
# Числовые столбцы
numerical_cols = [col for col in X.columns
                 if X[col].dtype in ['int64', 'float64']]

# Объединяем выбранные столбцы
selected_cols = categorical_cols + numerical_cols
X = X[selected_cols]

print(f"Категориальные столбцы ({len(categorical_cols)}): {categorical_cols}")
print(f"Числовые столбцы ({len(numerical_cols)}): {numerical_cols}")
print(f"Всего признаков: {len(selected_cols)}")

# 4. Разделить данные на обучающий и тестовый набор
print("\n4. Разделение данных...")
X_train, X_test, y_train, y_test = train_test_split(
    X, y, train_size=0.8, test_size=0.2, random_state=0
)
print(f"Обучающая выборка: {X_train.shape}")
print(f"Тестовая выборка: {X_test.shape}")
# 5----------point----------
print("\n5. Создание импьютеров...")
# Создаем несколько вариантов SimpleImputer для числовых столбцов
numerical_imputer_median = SimpleImputer(strategy='median')
numerical_imputer_mean = SimpleImputer(strategy='mean')
numerical_imputer_constant = SimpleImputer(strategy='constant', fill_value=0)

print("Созданы SimpleImputer для числовых данных:")
print("  1. Median strategy")
print("  2. Mean strategy")
print("  3. Constant strategy (fill_value=0)")

# 6. Создать конвейер для категориальных столбцов
print("\n6. Создание пайплайнов для категориальных данных...")
# Создаем несколько вариантов пайплайнов для категориальных данных
categorical_pipeline_frequent = Pipeline(steps=[
    ('imputer', SimpleImputer(strategy='most_frequent')),
    ('onehot', OneHotEncoder(handle_unknown='ignore', sparse_output=False))
])

categorical_pipeline_constant = Pipeline(steps=[
    ('imputer', SimpleImputer(strategy='constant', fill_value='missing')),
    ('onehot', OneHotEncoder(handle_unknown='ignore', sparse_output=False))
])

print("Пайплайны для категориальных данных созданы:")
print("  1. SimpleImputer (most_frequent) → OneHotEncoder")
print("  2. SimpleImputer (constant, fill_value='missing') → OneHotEncoder")

# 7. Создать несколько препроцессоров с помощью ColumnTransformer
print("\n7. Создание препроцессоров...")
# Создаем несколько комбинаций препроцессоров
preprocessor_combinations = {}

# Комбинация 1: Median + Most Frequent
preprocessor_1 = ColumnTransformer(
    transformers=[
        ('num', numerical_imputer_median, numerical_cols),
        ('cat', categorical_pipeline_frequent, categorical_cols)
    ]
)
preprocessor_combinations['median_frequent'] = preprocessor_1

# Комбинация 2: Mean + Most Frequent
preprocessor_2 = ColumnTransformer(
    transformers=[
        ('num', numerical_imputer_mean, numerical_cols),
        ('cat', categorical_pipeline_frequent, categorical_cols)
    ]
)
preprocessor_combinations['mean_frequent'] = preprocessor_2

# Комбинация 3: Median + Constant
preprocessor_3 = ColumnTransformer(
    transformers=[
        ('num', numerical_imputer_median, numerical_cols),
        ('cat', categorical_pipeline_constant, categorical_cols)
    ]
)
preprocessor_combinations['median_constant'] = preprocessor_3

# Комбинация 4: Mean + Constant
preprocessor_4 = ColumnTransformer(
    transformers=[
        ('num', numerical_imputer_mean, numerical_cols),
        ('cat', categorical_pipeline_constant, categorical_cols)
    ]
)
preprocessor_combinations['mean_constant'] = preprocessor_4

print("Созданы препроцессоры с разными комбинациями:")
for name in preprocessor_combinations.keys():
    print(f"  - {name}")

# Тестируем все препроцессоры и находим лучший
print("\nТестирование препроцессоров для поиска наилучшей комбинации...")

best_mae = float('inf')
best_preprocessor_name = None
best_preprocessor = None

for preprocessor_name, preprocessor in preprocessor_combinations.items():
    print(f"Тестирование {preprocessor_name}...")

    # Создаем временный пайплайн для тестирования
    temp_pipeline = Pipeline(steps=[
        ('preprocessor', preprocessor),
        ('model', RandomForestRegressor(n_estimators=50, random_state=42))  # Уменьшили для скорости
    ])

    # Быстрая кросс-валидация
    try:
        scores = cross_val_score(temp_pipeline, X_train, y_train,
                                 cv=3, scoring='neg_mean_absolute_error')
        mae_scores = -scores
        avg_mae = mae_scores.mean()

        print(f"  Средняя MAE: ${avg_mae:,.2f}")

        if avg_mae < best_mae:
            best_mae = avg_mae
            best_preprocessor_name = preprocessor_name
            best_preprocessor = preprocessor

    except Exception as e:
        print(f"  Ошибка: {e}")

print(f"\nЛучший препроцессор: {best_preprocessor_name} с MAE: ${best_mae:,.2f}")

# Используем лучший препроцессор для дальнейшей работы
preprocessor = best_preprocessor
print("Выбран лучший препроцессор для использования в моделях")

# 8. Создать итоговый конвейер с RandomForestRegressor
print("\n8. Создание итогового пайплайна с RandomForest...")
rf_pipeline = Pipeline(steps=[
    ('preprocessor', preprocessor),
    ('model', RandomForestRegressor(n_estimators=100, random_state=0))
])
print("Пайплайн создан: Preprocessor → RandomForestRegressor")

# 9. Провести кроссвалидацию для RandomForest
print("\n9. Кроссвалидация для RandomForest...")
rf_scores = cross_val_score(rf_pipeline, X_train, y_train,
                           cv=5, scoring='neg_mean_absolute_error')
rf_mae_scores = -rf_scores
print("Результаты кроссвалидации (MAE):")
for i, score in enumerate(rf_mae_scores, 1):
    print(f"  Fold {i}: ${score:,.2f}")
print(f"Средняя MAE: ${rf_mae_scores.mean():,.2f} (±${rf_mae_scores.std():,.2f})")

# 10. Обучить RandomForest на полном тренировочном наборе и оценить на тестовом
print("\n10. Обучение RandomForest на полных данных...")
rf_pipeline.fit(X_train, y_train)
rf_predictions = rf_pipeline.predict(X_test)
rf_mae = mean_absolute_error(y_test, rf_predictions)
print(f"MAE RandomForest на тестовых данных: ${rf_mae:,.2f}")

# 11. Создать конвейер с XGBRegressor
print("\n11. Создание пайплайна с XGBoost...")
xgb_pipeline = Pipeline(steps=[
    ('preprocessor', preprocessor),
    ('model', XGBRegressor(n_estimators=1000, learning_rate=0.05, random_state=0))
])
print("Done!!! Пайплайн создан: Preprocessor → XGBRegressor")

# 12. Обучение, предсказание и оценка XGBoost
print("\n12. Обучение и оценка XGBoost...")
xgb_pipeline.fit(X_train, y_train)
xgb_predictions = xgb_pipeline.predict(X_test)
xgb_mae = mean_absolute_error(y_test, xgb_predictions)
print(f"MAE XGBoost на тестовых данных: ${xgb_mae:,.2f}")

# 13. Выводы об эффективности алгоритмов
print("\n13. СРАВНЕНИЕ И ВЫВОДЫ:")
print("=" * 50)
print(f"RandomForest Regressor:")
print(f"  - Кроссвалидация MAE: ${rf_mae_scores.mean():,.2f} (±${rf_mae_scores.std():,.2f})")
print(f"  - Тестовая MAE: ${rf_mae:,.2f}")

print(f"\nXGBoost Regressor:")
print(f"  - Тестовая MAE: ${xgb_mae:,.2f}")

# Сравнение эффективности
improvement = ((rf_mae - xgb_mae) / rf_mae) * 100
if xgb_mae < rf_mae:
    print(f"\n✓ XGBoost показал лучшие результаты!")
    print(f"  - Улучшение: {improvement:.1f}%")
    print(f"  - Абсолютное улучшение: ${rf_mae - xgb_mae:,.2f}")
else:
    print(f"\n✓ RandomForest показал лучшие результаты!")
    print(f"  - Разница: {-improvement:.1f}%")

print(f"\nАНАЛИЗ ЭФФЕКТИВНОСТИ:")
print("1. RandomForest с кроссвалидацией:")
print("   - Более стабильные оценки благодаря кроссвалидации")
print("   - Меньший риск переобучения")
print("   - Хорошая интерпретируемость")

print("2. XGBoost (градиентный бустинг):")
print("   - Потенциально лучшая точность за счет последовательного обучения")
print("   - Эффективная работа с различными типами данных")
print("   - Встроенная регуляризация для борьбы с переобучением")

print("3. Общие выводы:")
print("   - Оба метода показывают хорошие результаты")
print("   - Кроссвалидация дает более надежную оценку качества RandomForest")
print("   - XGBoost часто требует более тонкой настройки гиперпараметров")
print("   - Выбор модели зависит от конкретной задачи и требований")

# Дополнительная(Additional) информация о данных
print(f"\nДОПОЛНИТЕЛЬНАЯ ИНФОРМАЦИЯ:")
print(f"  - Исходные признаки: {data.shape[1]}")
print(f"  - Отобранные признаки: {len(selected_cols)}")
print(f"  - Категориальные: {len(categorical_cols)}")
print(f"  - Числовые: {len(numerical_cols)}")
print(f"  - Пропущенные значения в тренировочных данных: {X_train.isnull().sum().sum()}")