import java.io.*;
import java.util.ArrayList;
import java.util.List;
import java.util.logging.Logger;

public class students {
    // to track events and errors
    private static final Logger LOGGER = Logger.getLogger(students.class.getName());

    public List<String> loadAndFilterFIO(String fileName) {
        List<String> validFIO = new ArrayList<>();

        try (BufferedReader reader = new BufferedReader(new FileReader(fileName))) {
            String line;
            // // Read the file line by line
            while ((line = reader.readLine()) != null) {
                //remove the white space trailing/leading
                line = line.trim();
                if (!line.isEmpty()) {
                    if (isValidFIO(line)) {
                        validFIO.add(line);
                    } else {
                        LOGGER.warning("Invalid FIO: " + line);
                    }
                }
            }
        } catch (Exception e) {
            LOGGER.severe("File read error: " + e.getMessage());
        }

        LOGGER.info("Processed " + validFIO.size() + " valid FIO");
        return validFIO;
    }
    //(Regax) - это шаблон для поиска и проверки текста

    private boolean isValidFIO(String fio) {
        // ^ start of the row, [] the set of letters, \\s Пробельный символ , +Один или более раз , $ 	Конец строки
        if (!fio.matches("^[a-zA-Z\\s]+$")) {
            return false;
        }

        String[] parts = fio.trim().split("\\s+");
        if (parts.length != 3) {
            return false;
        }

        for (String part : parts) {
            if (part.isEmpty() || !Character.isUpperCase(part.charAt(0))) {
                return false;
            }
        }

        return true;
    }
}
