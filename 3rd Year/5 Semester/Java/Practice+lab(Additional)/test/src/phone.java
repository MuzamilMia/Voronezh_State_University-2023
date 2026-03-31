import java.io.*;
import java.util.ArrayList;
import java.util.List;
import java.util.logging.Logger;

public class phone {
    private static final Logger LOGGER = Logger.getLogger(phone.class.getName());

    public List<String> LoadPhones(String fileName) {
        List<String> validCleaned = new ArrayList<>();

        try (BufferedReader reader = new BufferedReader(new FileReader(fileName))) {
            String line;
            while ((line = reader.readLine()) != null) {
                line = line.trim();
                if (line.isEmpty()) continue;

                if (line.matches("[0-9()+]+")) {
                    String cleaned = line.replaceAll("[^0-9]", "");
                    if (cleaned.length() == 11) {
                        validCleaned.add(cleaned);
                    } else {
                        LOGGER.warning("Invalid (wrong digits): " + line);
                    }
                } else {
                    LOGGER.warning("Invalid (bad chars): " + line);
                }
            }
        } catch (Exception e) {
            LOGGER.severe("File read error: " + e.getMessage());
        }

        LOGGER.info("Processed " + validCleaned.size() + " valid phones");
        return validCleaned;
    }
}
