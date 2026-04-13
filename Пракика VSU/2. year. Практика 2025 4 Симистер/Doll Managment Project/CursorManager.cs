using Doll_Managment_Project.Properties;
using System.IO;
using System.Windows.Forms;

public class CursorManager
{
    public Cursor GetCustomCursor()
    {
        try
        {
            using (var stream = new MemoryStream(Resources.diamond))
            {
                return new Cursor(stream);
            }
        }
        catch
        {
            return Cursors.Default;
        }
    }
}