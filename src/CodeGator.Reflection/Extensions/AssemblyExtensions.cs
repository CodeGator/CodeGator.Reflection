
#pragma warning disable IDE0130
namespace System.Reflection;
#pragma warning restore IDE0130

/// <summary>
/// This class utility contains extension methods related to the <see cref="Assembly"/>
/// type.
/// </summary>
public static partial class AssemblyExtensions
{
    // *******************************************************************
    // Public methods.
    // *******************************************************************

    #region Public methods

    /// <summary>
    /// This method returns a list of types, from the given assembly, that
    /// are decorated with the specified <typeparamref name="T"/> type.
    /// </summary>
    /// <typeparam name="T">The type of attribute to use for the search.</typeparam>
    /// <param name="assembly">The assembly to use for the search.</param>
    /// <returns>A list of matching <see cref="Type"/> objects.</returns>
    public static IEnumerable<Type> GetDecoratedTypes<T>(
        this Assembly assembly
        ) where T : Attribute
    {
        foreach (Type type in assembly.GetTypes())
        {
            if (type.GetCustomAttributes(typeof(T), true).Length > 0)
            {
                yield return type;
            }
        }
    }

    // *******************************************************************

    /// <summary>
    /// Reads the value of the <see cref="AssemblyCompanyAttribute"/>
    /// attribute for the given assembly.
    /// </summary>
    /// <param name="assembly">The assembly to read from.</param>
    /// <returns>The value of the given assembly's company attribute.</returns>
    public static string ReadCompany(this Assembly assembly)
    {
        // Attempt to read the assembly's company attribute.
        object[] attributes = assembly.GetCustomAttributes(
            typeof(AssemblyCompanyAttribute),
            true
            );

        // Did we fail?
        if (attributes.Length == 0)
        {
            return string.Empty;
        }

        // Attempt to recover a reference to the attribute.
        if (attributes[0] is not AssemblyCompanyAttribute attr || attr.Company.Length == 0)
        {
            return string.Empty;
        }

        // Return the text for the attribute.
        return attr.Company;
    }

    // *******************************************************************

    /// <summary>
    /// Reads the value of the <see cref="AssemblyCopyrightAttribute"/> 
    /// attribute for the given assembly.
    /// </summary>
    /// <param name="assembly">The assembly to read from.</param>
    /// <returns>The value of the given assembly's copyright attribute.</returns>
    public static string ReadCopyright(this Assembly assembly)
    {
        // Attempt to read the assembly's copyright attribute.
        object[] attributes = assembly.GetCustomAttributes(
            typeof(AssemblyCopyrightAttribute),
            true
            );

        // Did we fail?
        if (attributes.Length == 0)
        {
            return string.Empty;
        }

        // Attempt to recover a reference to the attribute.
        if (attributes[0] is not AssemblyCopyrightAttribute attr || attr.Copyright.Length == 0)
        {
            return string.Empty;
        }

        // Return the text for the attribute.
        return attr.Copyright;
    }

    // *******************************************************************

    /// <summary>
    /// Reads the value of the <see cref="AssemblyTitleAttribute"/>
    /// attribute for the given assembly.
    /// </summary>
    /// <param name="assembly">The assembly to read from.</param>
    /// <returns>The value of the given assembly's title attribute.</returns>
    public static string ReadTitle(this Assembly assembly)
    {
        // Attempt to read the assembly's title attribute.
        object[] attributes = assembly.GetCustomAttributes(
            typeof(AssemblyTitleAttribute),
            true
            );

        // Did we fail?
        if (attributes.Length == 0)
        {
            return string.Empty;
        }

        // Attempt to recover a reference to the attribute.
        if (attributes[0] is not AssemblyTitleAttribute attr || attr.Title.Length == 0)
        {
            return string.Empty;
        }

        // Return the text for the attribute.
        return attr.Title;
    }

    // ******************************************************************

    /// <summary>
    /// Reads the value of the <see cref="AssemblyDescriptionAttribute"/>
    /// attribute for the given assembly.
    /// </summary>
    /// <param name="assembly">The assembly to read from.</param>
    /// <returns>The value of the given assembly's description attribute.</returns>
    public static string ReadDescription(this Assembly assembly)
    {
        // Attempt to read the assembly's description attribute.
        object[] attributes = assembly.GetCustomAttributes(
            typeof(AssemblyDescriptionAttribute),
            true
            );

        // Did we fail?
        if (attributes.Length == 0)
        {
            return string.Empty;
        }

        // Attempt to recover a reference to the attribute.
        if (attributes[0] is not AssemblyDescriptionAttribute attr || attr.Description.Length == 0)
        {
            return string.Empty;
        }

        // Return the text for the attribute.
        return attr.Description;
    }

    // ******************************************************************

    /// <summary>
    /// Reads the value of the <see cref="AssemblyProductAttribute"/>
    /// attribute for the given assembly.
    /// </summary>
    /// <param name="assembly">The assembly to read from.</param>
    /// <returns>The value of the given assembly's product attribute.</returns>
    public static string ReadProduct(this Assembly assembly)
    {
        // Attempt to read the assembly's product attribute.
        object[] attributes = assembly.GetCustomAttributes(
            typeof(AssemblyProductAttribute),
            true
            );

        // Did we fail?
        if (attributes.Length == 0)
        {
            return string.Empty;
        }

        // Attempt to recover a reference to the attribute.
        if (attributes[0] is not AssemblyProductAttribute attr || attr.Product.Length == 0)
        {
            return string.Empty;
        }

        // Return the text for the attribute.
        return attr.Product;
    }

    // ******************************************************************

    /// <summary>
    /// Reads the value of the <see cref="AssemblyTrademarkAttribute"/>
    /// attribute for the given assembly.
    /// </summary>
    /// <param name="assembly">The assembly to read from.</param>
    /// <returns>The value of the given assembly's trademark attribute.</returns>
    public static string ReadTrademark(this Assembly assembly)
    {
        // Attempt to read the assembly's trademark attribute.
        object[] attributes = assembly.GetCustomAttributes(
            typeof(AssemblyTrademarkAttribute),
            true
            );

        // Did we fail?
        if (attributes.Length == 0)
        {
            return string.Empty;
        }

        // Attempt to recover a reference to the attribute.
        if (attributes[0] is not AssemblyTrademarkAttribute attr || attr.Trademark.Length == 0)
        {
            return string.Empty;
        }

        // Return the text for the attribute.
        return attr.Trademark;
    }

    // ******************************************************************

    /// <summary>
    /// Reads the value of the <see cref="AssemblyVersionAttribute"/>
    /// attribute for the given assembly.
    /// </summary>
    /// <param name="assembly">The assembly to read from.</param>
    /// <returns>The value of the given assembly's version attribute.</returns>
    public static string ReadAssemblyVersion(this Assembly assembly)
    {
        // Attempt to read the assembly's version attribute.
        object[] attributes = assembly.GetCustomAttributes(
            typeof(AssemblyVersionAttribute),
            true
            );

        // Did we fail?
        if (attributes.Length == 0)
        {
            return string.Empty;
        }

        // Attempt to recover a reference to the attribute.
        if (attributes[0] is not AssemblyVersionAttribute attr || attr.Version.Length == 0)
        {
            return string.Empty;
        }

        // Return the text for the attribute.
        return attr.Version;
    }

    // ******************************************************************

    /// <summary>
    /// Reads the value of the <see cref="AssemblyInformationalVersionAttribute"/>
    /// attribute for the given assembly.
    /// </summary>
    /// <param name="assembly">The assembly to read from.</param>
    /// <returns>The value of the given assembly's informational version attribute.</returns>
    public static string ReadInformationalVersion(this Assembly assembly)
    {
        // Attempt to read the assembly's version attribute.
        object[] attributes = assembly.GetCustomAttributes(
            typeof(AssemblyInformationalVersionAttribute),
            true
            );

        // Did we fail?
        if (attributes.Length == 0)
        {
            return string.Empty;
        }

        // Attempt to recover a reference to the attribute.
        if (attributes[0] is not AssemblyInformationalVersionAttribute attr || attr.InformationalVersion.Length == 0)
        {
            return string.Empty;
        }

        // Look for a '+' character, which, if found, signifies the start
        //   of semver 2.0 version info and should be removed.
        var index = attr.InformationalVersion.IndexOf('+');

        // Did we find it?
        if (0 < index)
        {
            // Strip off everything past the '+' character.
            return attr.InformationalVersion[
                ..(attr.InformationalVersion.Length - index - 2)
                ];
        }
        else
        {
            // Return the text for the attribute.
            return attr.InformationalVersion;
        }
    }

    #endregion
}
