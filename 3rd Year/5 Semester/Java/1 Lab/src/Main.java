//public class Main
//{
//    public static void main(String[] args)
//    {
//        int x=3;
//        double sumWithFor=getSumWithFor(x);
//        System.out.println("sum with for: ");
//        System.out.println(sumWithFor);
//
//        double sum=getSumaWithWhile(x);
//        System.out.println("Sum with While is: ");
//        System.out.println(Math.pow(Math.E,x));
//
//        System.out.println("Real Sum is: ");
//        System.out.println(Math.pow(Math.E,x));
//
//    }
//    private static double getSumaWithWhile(int x)
//    {
//        double sum=1;
//        double sumand=1;
//        double precision =0.0001;
//        int i=1;
//        while(sumand>precision){
//            double newSumAnd=sumand*x/i;
//            sum+=newSumAnd;
//            sumand=newSumAnd;
//            i++;
//        }
//        return sum;
//    }
//    private static double getSumWithFor(int x)
//    {
//        int loopcount=500;
//        double sum=1;
//        double sumand=1;
//        for(int i=1;i<loopcount;++i)
//        {
//            double newSumand=sumand*x/i;
//            sum+=newSumand;
//            sumand=newSumand;
//        }
//        return sum;
//    }
//}

public class Main {
    public static void main(String[] args) {
        double x = Math.PI / 3; // 60 degrees

        double sinWithFor = getSinWithFor(x);
        double sinWithWhile = getSinWithWhile(x);

        System.out.println("Sin with for: " + sinWithFor);
        System.out.println("Sin with while: " + sinWithWhile);
        System.out.println("Real sin: " + Math.sin(x));
    }

    private static double getSinWithFor(double x) {
        int loopcount = 50;
        double sum = 0;
        double term = x;
        int sign = 1;

        for (int i = 0; i < loopcount; i++) {
            sum += sign * term;
            term = term * x * x / ((2 * i + 2) * (2 * i + 3)); // next term which we got the formula
            sign *= -1; //Here the sign is going to change form min to pos or the opposite.
        }
        return sum;
    }

    private static double getSinWithWhile(double x) {
        double sum = 0;
        double term = x;
        int sign = 1;
        double precision = 0.000001;
        int i = 0;

        while (Math.abs(term) > precision) {
            sum += sign * term;
            term = term * x * x / ((2 * i + 2) * (2 * i + 3));
            sign *= -1;
            i++;
        }
        return sum;
    }
}
