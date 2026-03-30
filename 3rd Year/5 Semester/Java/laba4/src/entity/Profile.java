package entity;

public record Profile(String fio, int age, String phoneNumber, Sex gender, String address) {
    @Override
    public String toString() {
        return String.format("%s;%d;%s;%s;%s;",
                fio, age, phoneNumber, gender, address);
    }
}
