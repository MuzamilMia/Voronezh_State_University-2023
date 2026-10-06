CC := clang++
CC_FLAGS := -std=c++17

TARGET := main

all:
	$(CC) $(CC_FLAGS) main.cpp math3d.cpp geometry.cpp renderer.cpp -o $(TARGET) \
  -I/opt/homebrew/include \
  -L/opt/homebrew/lib \
  -lSDL2 \
	-lSDL2_gfx

clean:
	rm $(TARGET)

open: clean all
	./$(TARGET)