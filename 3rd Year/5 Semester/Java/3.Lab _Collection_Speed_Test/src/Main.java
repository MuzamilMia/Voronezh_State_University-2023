import java.util.*;
import java.util.jar.JarOutputStream;

public class Main {
    public static void main(String[] args) {
        System.out.println("------------ Adding Tests ------------ ");
        System.out.println(Collection_Speed.add(new ArrayList<>()));
        System.out.println(Collection_Speed.add(new LinkedList<>()));
        System.out.println(Collection_Speed.add(new HashSet<>()));
        System.out.println(Collection_Speed.add(new TreeSet<>()));

        System.out.println("------------ Contains Tests ---------");
        System.out.println(Collection_Speed.test_contains(new ArrayList<>()));
        System.out.println(Collection_Speed.test_contains(new LinkedList<>()));
        System.out.println(Collection_Speed.test_contains(new HashSet<>()));
        System.out.println(Collection_Speed.test_contains(new TreeSet<>()));

        System.out.println("------------ Add Beginning ----------");
        System.out.println(Collection_Speed.add_beginning(new ArrayList<>()));
        System.out.println(Collection_Speed.add_beginning(new LinkedList<>()));

        System.out.println("------------ Add End Tests ----------");
        System.out.println(Collection_Speed.add_end(new ArrayList<>()));
        System.out.println(Collection_Speed.add_end(new LinkedList<>()));

        System.out.println("------------------- Comments for everyone ------------- ");
        System.out.println("ArrayList: Fast add to end, slow to beginning, very slow in contains");
        System.out.println("LinkedList: Fast add to both end and beginning, very slow for contains");
        System.out.println("HashSet: Fast add and very fast contains (Searching)");
        System.out.println("TreeSet: SLower add (Cause of sorting), fact contains (Search) ");
    }
}