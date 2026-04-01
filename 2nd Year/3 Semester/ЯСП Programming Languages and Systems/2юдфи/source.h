#include <iostream>
#include <cmath>
#include <string>
#include <fstream>
#include <sstream>
#include <numbers>
#include <compare> 

// Define M_PI if missing
#ifndef M_PI
#define M_PI std::numbers::pi
#endif

class Angle {
private:
    int degrees;
    int minutes;

    void normalize() {
        int totalMinutes = degrees * 60 + minutes;
        totalMinutes %= 360 * 60;
        if (totalMinutes < 0) totalMinutes += 360 * 60;
        degrees = totalMinutes / 60;
        minutes = totalMinutes % 60;
    }

public:
    
    Angle() : degrees(0), minutes(0) {}
    Angle(int deg, int min) : degrees(deg), minutes(min) {
        normalize();
    }

    double to_radians() const {
        double totalDegrees = degrees + minutes / 60.0;
        return totalDegrees * M_PI / 180.0;
    }

    double sin_value() const {
        return sin(to_radians());
    }
    void increase(int deg, int min) {
        degrees += deg;
        minutes += min;
        normalize();
    }

    void decrease(int deg, int min) {
        degrees -= deg;
        minutes -= min;
        normalize();
    }

    Angle operator+(const Angle& other) const {
        return Angle(degrees + other.degrees, minutes + other.minutes);
    }

    bool operator==(const Angle& other) const = default;

    Angle operator*(double scalar) const {
        int totalMinutes = (degrees * 60 + minutes) * scalar;
        return Angle(totalMinutes / 60, totalMinutes % 60);
    }

    std::strong_ordering operator<=>(const Angle& other) const {
        return (degrees * 60 + minutes) <=> (other.degrees * 60 + other.minutes);
    }

   

    void input_from_keyboard() {
        std::cout << "Enter Degrees: ";
        std::cin >> degrees;
        std::cout << "Enter Minutes: ";
        std::cin >> minutes;
        normalize();
    }

    void input_from_file(const std::string& filename) {
        std::ifstream file(filename + ".txt");
        if (!file.is_open()) throw std::runtime_error("File is not open.");
        file >> degrees >> minutes;
        normalize();
    }

    void output_to_screen() const {
        std::cout << degrees << "° " << minutes << "'\n";
    }


    void output_to_file(const std::string& filename) const {
        std::ofstream file(filename + ".txt");
        if (!file.is_open()) throw std::runtime_error("File is not open.");
        file << degrees << " " << minutes;
    }


    std::string to_string() const {
        std::ostringstream oss;
        oss << degrees << "° " << minutes << "'";
        return oss.str();
    }
};

