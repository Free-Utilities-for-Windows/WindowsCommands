﻿using System.Security.Principal;

namespace WindowsCommands.AdminCheck;

public class AdministratorChecker : IAdministratorChecker
{
    public bool IsCurrentUserAdmin()
    {
        var identity = WindowsIdentity.GetCurrent();
        var principal = new WindowsPrincipal(identity);
        return principal.IsInRole(WindowsBuiltInRole.Administrator);
    }
}