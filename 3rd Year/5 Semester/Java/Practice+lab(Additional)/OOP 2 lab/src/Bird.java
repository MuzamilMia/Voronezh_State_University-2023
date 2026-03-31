public class Bird extends Animals
{
    private boolean fly;
    private double wings;
    public Bird(String name, int age, String color, double weight, String feeds, Owner owner, double wings, boolean canfly) {
        super(name, age, color, weight, feeds, owner);
        this.wings=wings;
        this.fly=canfly;
    }

    public void setWings(double wings) {this.wings = wings;}
    public void setFly(boolean canfly){this.fly= canfly;}

    public double getWings(){return wings;}
    public boolean getfFly() {return fly;}

    @Override
    public void makesound()
    {
        System.out.println(getName()+" is singing the song ");
    }

    @Override
    public void move() {
        if(fly)
            System.out.println(getName()+ " is flying very good!!! ");
        else System.out.println(getName()+" Unfortunately");
    }
}
