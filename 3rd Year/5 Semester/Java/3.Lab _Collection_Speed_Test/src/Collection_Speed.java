import java.util.Collection;
import java.util.List;
import java.util.Random;

public class Collection_Speed {
    private static final int BENCHMARK_SIZE = 10000;
    private static final int CONTAINS_TEST_SIZE = 1000;

    public static String add(Collection<Integer> collection) {
        collection.clear();
        long starttime = System.currentTimeMillis();
        for (int i = 0; i < BENCHMARK_SIZE; i++) {
            collection.add(i);
        }
        long endtime = System.currentTimeMillis();
        long result_time = endtime - starttime;
        String realClassName = collection.getClass().getSimpleName();
        return String.format("Method add for class %s for size %d took %d ms",
                realClassName, BENCHMARK_SIZE, result_time);
    }

    public static String test_contains(Collection<Integer> collection) {
        collection.clear();
        for (int i = 0; i < BENCHMARK_SIZE; i++) {
            collection.add(i);
        }
        Random random = new Random();
        long start_time = System.currentTimeMillis();
        for (int i = 0; i < CONTAINS_TEST_SIZE; i++) {
            int search_Number = random.nextInt(BENCHMARK_SIZE * 2);
            collection.contains(search_Number);
        }
        long end_time = System.currentTimeMillis();
        long result_time = end_time - start_time;
        String realClassName = collection.getClass().getSimpleName();
        return String.format("Method contains for class %s for %d searches took %d ms",
                realClassName, CONTAINS_TEST_SIZE, result_time);

    }

    public static String add_beginning(List<Integer> list) {
        list.clear();
        long start_time = System.currentTimeMillis();
        for (int i = 0; i < BENCHMARK_SIZE; i++) {
            list.add(0, i);
        }
        long end_time = System.currentTimeMillis();
        long result_time = end_time - start_time;
        String realClassName = list.getClass().getSimpleName();
        return String.format("Method add for class %s for size %d took %d ms",
                realClassName, BENCHMARK_SIZE, result_time);
    }

    public static String add_end(List<Integer> list) {
        list.clear();
        long start_time = System.currentTimeMillis();
        for (int i = 0; i < BENCHMARK_SIZE; i++) {
            list.add(i);
        }
        long end_time = System.currentTimeMillis();
        long result_time = end_time - start_time;
        String realClassName = list.getClass().getSimpleName();
        return String.format("Method add(end) for class %s for size %d took %d ms",
                realClassName, BENCHMARK_SIZE, result_time);
    }
}
