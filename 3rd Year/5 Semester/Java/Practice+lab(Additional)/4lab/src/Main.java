import repository.ProfileFileRepository;
import service.CheckService;
import service.ProfileService;
import ui.ConsoleInterface;

public class Main {
    public static void main(String[] args) {
        ConsoleInterface console = new ConsoleInterface(new ProfileService(new CheckService(), new ProfileFileRepository()));
        console.start();
    }
}
