package service;

import entity.Profile;
import entity.ProfileFileData;
import entity.SaveDataFile;
import entity.Sex;
import exception.ValidationException;
import repository.ProfileFileRepository;
import util.FileUtils;

import java.io.BufferedWriter;
import java.io.FileWriter;
import java.io.Writer;
import java.util.HashMap;
import java.util.Map;
import java.util.Scanner;

import static ui.ConsoleInterface.LOGGER;

public class ProfileService {
    private Map<String, Profile> fioToProfile = new HashMap<>();
    private final ProfileFileRepository profileFileRepository;
    private final CheckService checkService;


    public ProfileService(CheckService checkService, ProfileFileRepository profileFileRepository) {
        this.profileFileRepository = profileFileRepository;
        this.checkService = checkService;
    }

    public void createFile(String fileName) {
        try {
            if (!FileUtils.fileExists((fileName))) {
                // we are creating the file from the beginning structure.
                try (Writer writer = new BufferedWriter(new FileWriter(fileName))) {
                    // We are writing a checksum for empty collection.
                    Map<String, Profile> emptyMap = new HashMap<>();
                    int emptyHash = emptyMap.values().toString().hashCode();
                    writer.write(String.valueOf(emptyHash) + "\n");
                }
                LOGGER.info("File " + fileName + " was created with empty structure");

                if (!fioToProfile.isEmpty()) {
                    fioToProfile.clear();
                }
            } else {
                System.out.println("File " + fileName + " already exists");
            }
        } catch (Exception e) {
            LOGGER.severe("Error with creating a file: " + e.getMessage());
            System.out.println("Error creating file: " + e.getMessage());
        }
    }


    public void loadFile(String fileName) {
        try {
            if (FileUtils.fileExists((fileName))) {
                ProfileFileData profileFileData = profileFileRepository.loadFile(fileName);
                checkService.validateCheckSum(profileFileData);
                checkService.validateFormatFile(profileFileData);
                fioToProfile = profileFileData.fioToProfile();
                LOGGER.info("File " + fileName + " was loaded");
                System.out.println("File " + fileName + " was loaded");
            } else {
                LOGGER.warning("Attempt to open file " + fileName + " that does not exist");
                System.out.println("File " + fileName + " does not exist");
            }
        } catch (Exception e) {
            LOGGER.severe("Error with loading a file: " + e.getMessage());
        }
    }

    public String inputFileName(Scanner sc) {
        System.out.print("Input filename: ");
        return sc.nextLine();
    }

    public String inputFio(Scanner sc) {
        System.out.print("Input FIO: ");
        return sc.nextLine().trim().toUpperCase();
    }

    public Profile inputUser(Scanner sc) {
        String fio = inputFio(sc);

        int age = 0;
        try {
            System.out.print("Input age: ");
            age = Integer.parseInt(sc.nextLine());
        } catch (Exception e) {
            throw new ValidationException("Invalid age");
        }

        System.out.print("Input phone number: ");
        String phone = sc.nextLine().trim();

        Sex gender = null;
        try {
            System.out.print("Input gender (MALE or FEMALE): ");
            gender = Sex.valueOf(sc.nextLine().trim().toUpperCase());
        } catch (Exception e) {
            throw new ValidationException("Invalid gender");
        }

        System.out.print("Input address: ");
        String address = sc.nextLine().trim();

        return profileFileRepository.createProfile(fio, age, phone, gender, address);
    }


    public void addUser(Scanner sc) {
        try {
            Profile profile = inputUser(sc);
            checkService.validateStringFio(profile.fio(), fioToProfile);
            profileFileRepository.addUser(profile, fioToProfile);
        } catch (ValidationException e) {
            LOGGER.warning("Error: " + e.getMessage());
            System.out.println("Incorrect format");
        }
    }

    public void save(String path) {
        SaveDataFile saveDataFile = profileFileRepository.getDataForSave(fioToProfile);
        saveDataFile.linesToWrite().addFirst(saveDataFile.contentForHash());
        try {
            profileFileRepository.saveDataForFile(path, saveDataFile.linesToWrite());
            LOGGER.info("File " + path + " was saved");
            System.out.println("File " + path + " was saved");
        } catch (Exception e) {
            LOGGER.severe("Error with saving a file: " + e.getMessage());
            System.out.println("Error with saving a file");
        }
    }

    public void searchUser(Scanner sc) {
        String fio = inputFio(sc);
        Profile profile = profileFileRepository.searchUser(fio, fioToProfile);
        if (profile == null) {
            LOGGER.warning("Profile: " + fio + " was not found");
            System.out.println("Profile: " + fio + " was not found");
        } else {
            System.out.println("Profile was found");
            System.out.println(profile);
        }
    }

    public void removeUser(Scanner sc) {
        String fio = inputFio(sc);
        if (profileFileRepository.removeUser(fio, fioToProfile) == null) {
            LOGGER.warning("Attempt to delete profile: " + fio + " that does not exist");
            System.out.println("Profile: " + fio + " was not found");
        } else {
            LOGGER.info("Deleting profile: " + fio);
            System.out.println("Profile: " + fio + " was deleted");
        }
    }
}
