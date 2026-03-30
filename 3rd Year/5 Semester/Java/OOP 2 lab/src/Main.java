import java.sql.SQLOutput;

//TIP To <b>Run</b> code, press <shortcut actionId="Run"/> or
// click the <icon src="AllIcons.Actions.Execute"/> icon in the gutter.
public class Main {
    public static void main(String[] args)
    {
        Owner owner1=new Owner("Ahmad", 700881004);
        Owner owner2=new Owner("Khan", 902333222);

        Animals bird1= new Bird("Yel_parrot",4,"yellow",0.02,"noods",owner1,0.6,true);
        Animals bird2= new Bird("Gre_Parrot",3,"green",0.4,"veg",owner2, 0.5,false);
        display(bird1);
        System.out.println("------------------------------------- ");
        display(bird2);

    }
    static public void display(Animals animal)
    {
        System.out.println("Owner Name: "+animal.getOwner().getName());
        System.out.println("Owner PhNO: "+animal.getOwner().getPhone());
        System.out.println("Animal Name: "+animal.getName());
        System.out.println("Animal Age: "+animal.getAge());
        System.out.println("Animal Color: "+animal.getColor());
        System.out.println("Animal wight: "+animal.getWeight());
        System.out.println("Animal feeds: "+animal.getFeeds());
        System.out.println("Animal wings: "+((Bird)animal).getWings());
        animal.makesound();
        animal.move();

    }
}
