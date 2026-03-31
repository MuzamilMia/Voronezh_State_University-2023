package repository;

import entity.Profile;
import entity.ProfileFileData;
import entity.SaveDataFile;
import entity.Sex;
import exception.ValidationException;

import java.io.*;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

import static ui.ConsoleInterface.LOGGER;

public class ProfileFileRepository {
public Profile createProfile(String fio, Integer age, String phoneNumber, Sex gender, String address){
return new Profile(fio,age,phoneNumber,gender, address);
}
public void addUser(Profile profile, Map<String, Profile> fioToProfile){
    fioToProfile.put(profile.fio(),profile);
}
public Profile removeUser(String fio, Map<String, Profile> fioToProfile) {
    return fioToProfile.remove(fio);
}
public Profile searchUser(String fio, Map<String, Profile> fioToProfile) {
    return fioToProfile.get(fio);
}
    public Profile loadProfileFromData(String[] args) {
        if (args.length != 5) {
            throw new ValidationException("Incorrect format of the date.");
        }
        String fio = args[0].trim().toUpperCase();
        int age;
        try {
            age = Integer.parseInt(args[1].trim());
        } catch (Exception e) {
            throw new ValidationException("Incorrect age number.");
        }

        String phoneNumber = args[2].trim();
        Sex gender = null;
        try {
            gender = Sex.valueOf(args[3].trim().toUpperCase());
        } catch (Exception e) {
            throw new ValidationException("Incorrect gender.");
        }
        String address = args[4].trim();

        return createProfile(fio, age, phoneNumber, gender, address);
    }
    public ProfileFileData loadFile(String fileName) {
        Map<String, Profile> fioToProfile = new HashMap<>();
        List<String> lines = new ArrayList<>();
        try (BufferedReader reader = new BufferedReader(new FileReader(fileName))) {
            String line;
            while ((line = reader.readLine()) != null) {
                lines.add(line);
            }
        } catch (Exception e) {
            throw new ValidationException(e);
        }
        Integer fileCheckSum = null;
        try {
            fileCheckSum = Integer.parseInt(lines.getFirst());
        } catch (Exception e) {
            throw new ValidationException("Incorrect format of file checksum.");
        }
        for (int i = 1; i < lines.size(); i++) {
            String line = lines.get(i).trim();
            String[] args = line.split(";");
            if (args.length != 5) {
                throw new ValidationException("Incorrect format of file checksum.");
            }
            Profile profile = loadProfileFromData(args);
            if (fioToProfile.containsKey(profile.fio())) {
                LOGGER.warning("Duplicate of the FIO was found: " + profile.fio() + ", this profile will'be rewritten");
            }
            fioToProfile.put(profile.fio(), profile);
        }
        return new ProfileFileData(fioToProfile, fileCheckSum);
    }
    public SaveDataFile getDataForSave(Map<String, Profile> fioToProfile) {
        String contentForHash = String.valueOf(fioToProfile.values().toString().hashCode()) + '\n';
        List<String> linesToWrite = new ArrayList<>();
        for (Profile profile : fioToProfile.values()) {
            linesToWrite.add(profile.toString());
        }
        return new SaveDataFile(linesToWrite, contentForHash);
    }
    public void saveDataForFile(String path, List<String> linesToWrite) {
        try (Writer writer = new BufferedWriter(new FileWriter(path))) {
            for (String line : linesToWrite) {
                writer.write(line);
            }
        } catch (Exception e) {
            throw new ValidationException(e);
        }
    }
}
