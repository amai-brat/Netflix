using System.Reflection;

namespace Application.Helpers;

public static class AssemblyReference
{
    public static Assembly Assembly => typeof(AssemblyReference).Assembly; 
}