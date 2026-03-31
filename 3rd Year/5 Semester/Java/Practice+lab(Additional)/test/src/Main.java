import java.io.PrintWriter;
import java.util.List;

public class Main {
    public static void main(String[] args)
    {
        phone myphone = new phone();
        List<String> validPhones = myphone.LoadPhones("phone.txt");

        validPhones.sort(String::compareTo);
        try (PrintWriter writer = new PrintWriter("output.txt")) {
            validPhones.forEach(writer::println);
            System.out.println("Saved " + validPhones.size() + " valid phones");
        } catch (Exception e) {
            System.out.println("Write error: " + e.getMessage());
        }
    }
}