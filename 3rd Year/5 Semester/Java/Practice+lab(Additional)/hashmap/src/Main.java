import java.math.BigDecimal;
import java.util.Map;
    public static void main(String[] args)
    {
        Map<String,BigDecimal> nametosize=Map.of("mia", BigDecimal.valueOf(2900),
                "Khan", BigDecimal.valueOf(290340),
                "Muzmail", BigDecimal.valueOf(2000)
        );
        //-------------------- First this code-----------
//        Map<String, BigDecimal>nameTosize=new HashMap<>();
//        nameTosize.put("mia", BigDecimal.valueOf(2900));
//        nameTosize.put("Muzmil", BigDecimal.ZERO);
//        nameTosize.put("Ahmad", BigDecimal.valueOf(50000).multiply(BigDecimal.valueOf(86)));
//
        System.out.println(nametosize.getOrDefault("Mia", BigDecimal.ONE));
        System.out.println(nametosize.entrySet());
        System.out.println(nametosize.values());
        System.out.println(nametosize);

        System.out.println(nametosize);



    }