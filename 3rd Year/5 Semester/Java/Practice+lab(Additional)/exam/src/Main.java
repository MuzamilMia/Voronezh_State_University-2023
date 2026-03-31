import java.util.ArrayList;
import java.util.List;
import  java.io.*;
//TIP To <b>Run</b> code, press <shortcut actionId="Run"/> or
// click the <icon src="AllIcons.Actions.Execute"/> icon in the gutter.
public class Main {
    public static void main(String[] args) {
        List<String> phonnumber=readFile("phone.txt");
        List<String> validmyphones= new ArrayList<>();
        List<String>notphone=new ArrayList<>();


        for(String phone:phonnumber)
        {
            if(validmyphones(phone))
            {
                String clean= cleanphon(phone);
            }
            else
            {
                notphone.add(phone);
            }
        }

        public boolean validmyphones(String phones)
        {
            for (int i=0; i<phone.length();i++)
            {
                char c=phone.charAt(i);
                if(!(Character.isDigit(c)|| c == ')' || c == '+')){
                    return false;
                }
            }

            int digitcount=0;
            for(int i=0;i<phone.length();i++)
            {
                if(Character.isDigit(phone.charAt(i))){
                    digitcount++;
                }
            }
            return digitcount==11;
        }
    }
}