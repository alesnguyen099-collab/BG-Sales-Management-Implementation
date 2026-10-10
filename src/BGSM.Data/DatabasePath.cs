namespace BGSM.Data;
public static class DatabasePath
{
 public static string GetDefaultPath()
 { var root = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "BG Sales Management"); Directory.CreateDirectory(root); return Path.Combine(root, "bgsm.db"); }
}