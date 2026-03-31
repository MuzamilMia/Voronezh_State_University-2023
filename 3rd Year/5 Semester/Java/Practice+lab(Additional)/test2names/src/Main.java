import java.io.PrintWriter;
import java.util.List;

public class Main {
    public static void main(String[] args) {

        students MyStudent = new students();
        List<String> validFIO = MyStudent.loadAndFilterFIO("fio.txt");
        // natural alphabetical order
        validFIO.sort(String::compareTo);
        java.util.Collections.reverse(validFIO);

        try (PrintWriter writer = new PrintWriter("output.txt")) {
            //  validFIO.forEach(name -> writer.println(name));Write each valid FIO
            validFIO.forEach(writer::println);
            System.out.println("Saved " + validFIO.size() + " valid FIO in reverse alphabetical order");
        } catch (Exception e) {
            System.out.println("Write error: " + e.getMessage());
        }
    }

}