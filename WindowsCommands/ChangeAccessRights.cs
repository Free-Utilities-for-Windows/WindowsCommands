using System.Security.AccessControl;
using WindowsCommands.Logger;

namespace WindowsCommands;

public static class ChangeAccessRights
{
    public static void ShowAccessRights(string path)
    {
        try
        {
            FileSystemSecurity fileSystemSecurity;
            if (File.Exists(path))
            {
                var fileInfo = new FileInfo(path);
                fileSystemSecurity = fileInfo.GetAccessControl();
            }
            else
            {
                var dirInfo = new DirectoryInfo(path);
                fileSystemSecurity = dirInfo.GetAccessControl();
            }

            AuthorizationRuleCollection acl =
                fileSystemSecurity.GetAccessRules(true, true, typeof(System.Security.Principal.NTAccount));
            foreach (FileSystemAccessRule ace in acl)
            {
                string logMessage = $"{ace.IdentityReference.Value} has {ace.FileSystemRights} rights.";
                Console.WriteLine(logMessage);
                StaticFileLogger.LogInformation(logMessage);
            }
        }
        catch (Exception e)
        {
            string errorMessage = "An error occurred while showing access rights: " + e.Message;
            Console.WriteLine(errorMessage);
            StaticFileLogger.LogError(errorMessage);
        }
    }

    public static void ChangeAccessRightsMenu(string path)
    {
        try
        {
            Console.Write("Enter the username: ");
            string username = Console.ReadLine();

            Console.Write("Enter the rights (Read, Write, FullControl): ");
            string rights = Console.ReadLine();

            FileSystemRights fileSystemRights;
            if (!Enum.TryParse(rights, out fileSystemRights))
            {
                string errorMessage = "Invalid rights. Please enter Read, Write, or FullControl.";
                Console.WriteLine(errorMessage);
                StaticFileLogger.LogError(errorMessage);
                return;
            }

            Console.Write("Do you want to add or remove these rights? (add/remove): ");
            string action = Console.ReadLine();

            AccessControlType controlType;
            if (action.ToLower() == "add")
            {
                controlType = AccessControlType.Allow;
            }
            else if (action.ToLower() == "remove")
            {
                controlType = AccessControlType.Deny;
            }
            else
            {
                string errorMessage = "Invalid action. Please enter add or remove.";
                Console.WriteLine(errorMessage);
                StaticFileLogger.LogError(errorMessage);
                return;
            }

            ChangeYourAccessRights(path, username, fileSystemRights, controlType);

            string successMessage = "Access rights changed successfully.";
            Console.WriteLine(successMessage);
            StaticFileLogger.LogInformation(successMessage);
        }
        catch (Exception e)
        {
            string errorMessage = "An error occurred while changing access rights: " + e.Message;
            Console.WriteLine(errorMessage);
            StaticFileLogger.LogError(errorMessage);
        }
    }

    public static void ChangeYourAccessRights(string path, string username, FileSystemRights rights,
        AccessControlType controlType)
    {
        try
        {
            FileSystemSecurity security;
            if (File.Exists(path))
            {
                var fileInfo = new FileInfo(path);
                security = fileInfo.GetAccessControl();
            }
            else if (Directory.Exists(path))
            {
                var dirInfo = new DirectoryInfo(path);
                security = dirInfo.GetAccessControl();
            }
            else
            {
                string errorMessage = "The specified path does not exist.";
                Console.WriteLine(errorMessage);
                StaticFileLogger.LogError(errorMessage);
                return;
            }

            FileSystemAccessRule rule = new FileSystemAccessRule(username, rights, controlType);

            if (controlType == AccessControlType.Allow)
            {
                security.AddAccessRule(rule);
            }
            else
            {
                security.RemoveAccessRule(rule);
            }

            if (File.Exists(path))
            {
                var fileInfo = new FileInfo(path);
                fileInfo.SetAccessControl((FileSecurity)security);
            }
            else if (Directory.Exists(path))
            {
                var dirInfo = new DirectoryInfo(path);
                dirInfo.SetAccessControl((DirectorySecurity)security);

                foreach (string entry in Directory.GetFileSystemEntries(path))
                {
                    ChangeYourAccessRights(entry, username, rights, controlType);
                }
            }

            string successMessage = $"Access rights for {path} changed successfully.";
            StaticFileLogger.LogInformation(successMessage);
        }
        catch (Exception e)
        {
            string errorMessage = "An error occurred while changing access rights: " + e.Message;
            Console.WriteLine(errorMessage);
            StaticFileLogger.LogError(errorMessage);
        }
    }
}