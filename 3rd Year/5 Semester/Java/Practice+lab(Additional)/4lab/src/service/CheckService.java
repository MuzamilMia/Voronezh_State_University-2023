package service;

import entity.Profile;
import entity.ProfileFileData;
import exception.ValidationException;

import java.util.Map;
import static ui.ConsoleInterface.LOGGER;

public class CheckService {
    public void validateCheckSum(ProfileFileData profileFileData){
        if(profileFileData.fileHash()!=profileFileData.fioToProfile().values().toString().hashCode()){
            throw new ValidationException("Invalid checksum");
        }
    }

    public void validateFormatFile(ProfileFileData profileFileData) {
        for (Profile profile : profileFileData.fioToProfile().values()) {
            if (profile.age() < 0) {
                throw new ValidationException("Invalid age");
            }
        }
    }

    public boolean validateStringFio(String fio, Map<String, Profile> fioToProfile) {
        if (fioToProfile.containsKey(fio)) {
            LOGGER.warning("Attempt to add the existing profile: " + fio);
            System.out.println("Profile with this name is already exists.");
            return false;
        }
        return true;
    }
}
