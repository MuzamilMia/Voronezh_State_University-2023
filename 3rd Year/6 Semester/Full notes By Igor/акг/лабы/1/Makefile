CC := clang++
CC_FLAGS := -std=c++11

TARGET := bresenham

all:
	$(CC) $(CC_FLAGS) main.cpp bresenham.cpp -o $(TARGET) \
  -I/opt/homebrew/include \
  -L/opt/homebrew/lib \
  -lSDL2

clean:
	rm bresenham

open: clean all
	./bresenham