package ui;

import service.ProfileService;

import java.util.Arrays;
import java.util.Scanner;
import java.util.logging.FileHandler;
import java.util.logging.Level;
import java.util.logging.Logger;
import java.util.logging.SimpleFormatter;

public class ConsoleInterface {
    private final ProfileService profileService;
    private static final Integer MAX_ERROR_COUNT = 3;
    Integer ERROR_COUNT = 0;
    private boolean running;
    public static final Logger LOGGER = Logger.getGlobal();
    private static final String FILE_LOGGER = "/Users/egor/IdeaProjects/laba4/logs.log";
    public static String CURRENT_FILE_PATH;

    public ConsoleInterface(ProfileService profileService) {this.profileService = profileService; this.running = true; }

    public void setupLogger() {
        try {
            FileHandler fh = new FileHandler(FILE_LOGGER, true);
            fh.setFormatter(new SimpleFormatter());
            LOGGER.addHandler(fh);
        } catch (Exception e) {
            LOGGER.log(Level.INFO, "Error: ", e);
        }
    }

    public void start() {
        setupLogger();
        Scanner sc = new Scanner(System.in);
        while (running && ERROR_COUNT < MAX_ERROR_COUNT) {
            printCommands();
            Command cmd = getCommand(sc);
            if (cmd == null) {
                continue;
            }
            handleCommand(cmd, sc);
        }
        if (ERROR_COUNT > 2) {
            System.out.println("Too much incorrect commands. Program is exiting...");
            LOGGER.info("Forced program termination");
        }
    }

    public void printCommands() {
        System.out.println(Arrays.toString(Command.values()));
    }

    public Command getCommand(Scanner scanner) {
        Command cmd = null;
        try {
            System.out.print("Input the command: ");
            cmd = Command.valueOf(scanner.nextLine().trim().toUpperCase());
        } catch (Exception e) {
            LOGGER.log(Level.WARNING, "Incorrect command");
            System.out.println("Invalid command");
            ++ERROR_COUNT;
        }
        return cmd;
    }

    public void handleCommand(Command command, Scanner scanner) {
        switch (command) {
            case Command.CREATEFILE:
                CURRENT_FILE_PATH = profileService.inputFileName(scanner);
                profileService.createFile(CURRENT_FILE_PATH);
                break;
            case Command.LOADFILE:
                CURRENT_FILE_PATH = profileService.inputFileName(scanner);
                profileService.loadFile(CURRENT_FILE_PATH);
                break;
            case Command.SEARCH:
                profileService.searchUser(scanner);
                break;
            case Command.ADDUSER:
                profileService.addUser(scanner);
                break;
            case Command.REMOVEUSER:
                profileService.removeUser(scanner);
                break;
            case Command.SAVEFILE:
                profileService.save(CURRENT_FILE_PATH);
                break;
            case Command.SAVEFILEAS:
                profileService.save(profileService.inputFileName(scanner));
                break;
            case Command.EXIT:
                running = false;
                break;
        }
    }



}
