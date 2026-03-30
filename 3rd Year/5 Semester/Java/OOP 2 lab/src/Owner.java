public class Owner {
    private String name;
    private int phone;

    public Owner(String name, int phone) {
        this.name = name;
        this.phone = phone;
    }

    public void setName(String name) {
        this.name = name;
    }

    public void setPhone(int phone) {
        this.phone = phone;
    }

    public String getName() {
        return name;
    }

    public int getPhone() {
        return phone;
    }

    public void showInfo() {
        System.out.println("Owner: " + name);
        System.out.println("Phone: " + phone);
    }
}
