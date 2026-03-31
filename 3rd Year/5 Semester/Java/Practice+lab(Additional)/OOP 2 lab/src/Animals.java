public class Animals {
    private String name;
    private int age;
    private String color;
    private double weight;
    private String feeds;
    private Owner owner;

    public Animals(String name, int age, String color, double weight, String feeds, Owner owner) {
        this.name = name;
        this.age = age;
        this.color = color;
        this.weight = weight;
        this.feeds = feeds;
        this.owner = owner;


    }

    public String getName() {
        return name;
    }

    public double getWeight() {
        return weight;
    }

    public String getFeeds() {
        return feeds;
    }

    public String getColor() {
        return color;
    }

    public int getAge() {
        return age;
    }

    public Owner getOwner() {
        return owner;
    }

    public void setName(String name) {
        this.name = name;
    }

    public void setAge(int age) {
        this.age = age;
    }

    public void setColor(String color) {
        this.color = color;
    }

    public void setWeight(double weight) {
        this.weight = weight;
    }

    public void setFeeds(String feeds) {
        this.feeds = feeds;
    }

    public void setOwner(Owner owner) {
        this.owner = owner;
    }

    public void makesound() {
        System.out.println(name + " is doing the sound");
    }

    public void move() {
        System.out.println(name + " is running");
    }

}
