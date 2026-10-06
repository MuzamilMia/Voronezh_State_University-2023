CC := clang++
CC_FLAGS := -std=c++17

TARGET := main

all:
	$(CC) $(CC_FLAGS) main.cpp cylinder.cpp lighting.cpp controls.cpp -framework OpenGL -framework GLUT -o $(TARGET)

clean:
	rm $(TARGET)

open: clean all
	./$(TARGET) < input.txt