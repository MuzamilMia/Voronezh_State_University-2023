import java.util.Collection;

public class List
{
    private int[]arr=new int[50];

    private int lastindexoffilledelement=0;

    public void add(Integer element)
    {
        arr[lastindexoffilledelement]=element;
        lastindexoffilledelement++;
    }

    public void addAll(Collection<Integer> elements)
    {
        for (Integer elemet:elements)
            add(elemet);
    }
}
