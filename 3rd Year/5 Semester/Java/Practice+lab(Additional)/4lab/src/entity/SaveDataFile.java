package entity;

import java.util.List;

public record SaveDataFile(List<String> linesToWrite, String contentForHash) {
}
