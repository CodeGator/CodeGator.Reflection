
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
        [NotNull] this Assembly assembly
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
    public static string ReadCompany(
        [NotNull] this Assembly assembly
        )
    {
        var attributes = assembly.GetCustomAttributes(
            typeof(AssemblyCompanyAttribute),
            true
            );

        if (attributes.Length == 0)
        {
            return string.Empty;
        }

        if (attributes[0] is not AssemblyCompanyAttribute attr || 
            attr.Company.Length == 0)
        {
            return string.Empty;
        }

        return attr.Company;
    }

    // *******************************************************************

    /// <summary>
    /// Reads the value of the <see cref="AssemblyCopyrightAttribute"/> 
    /// attribute for the given assembly.
    /// </summary>
    /// <param name="assembly">The assembly to read from.</param>
    /// <returns>The value of the given assembly's copyright attribute.</returns>
    public static string ReadCopyright(
        [NotNull] this Assembly assembly
        )
    {
        var attributes = assembly.GetCustomAttributes(
            typeof(AssemblyCopyrightAttribute),
            true
            );

        if (attributes.Length == 0)
        {
            return string.Empty;
        }

        if (attributes[0] is not AssemblyCopyrightAttribute attr || 
            attr.Copyright.Length == 0)
        {
            return string.Empty;
        }

        return attr.Copyright;
    }

    // *******************************************************************

    /// <summary>
    /// Reads the value of the <see cref="AssemblyTitleAttribute"/>
    /// attribute for the given assembly.
    /// </summary>
    /// <param name="assembly">The assembly to read from.</param>
    /// <returns>The value of the given assembly's title attribute.</returns>
    public static string ReadTitle(
        [NotNull] this Assembly assembly
        )
    {
        var attributes = assembly.GetCustomAttributes(
            typeof(AssemblyTitleAttribute),
            true
            );

        if (attributes.Length == 0)
        {
            return string.Empty;
        }

        if (attributes[0] is not AssemblyTitleAttribute attr || 
            attr.Title.Length == 0)
        {
            return string.Empty;
        }

        return attr.Title;
    }

    // ******************************************************************

    /// <summary>
    /// Reads the value of the <see cref="AssemblyDescriptionAttribute"/>
    /// attribute for the given assembly.
    /// </summary>
    /// <param name="assembly">The assembly to read from.</param>
    /// <returns>The value of the given assembly's description attribute.</returns>
    public static string ReadDescription(
        [NotNull] this Assembly assembly
        )
    {
        var attributes = assembly.GetCustomAttributes(
            typeof(AssemblyDescriptionAttribute),
            true
            );

        if (attributes.Length == 0)
        {
            return string.Empty;
        }

        if (attributes[0] is not AssemblyDescriptionAttribute attr || 
            attr.Description.Length == 0)
        {
            return string.Empty;
        }

        return attr.Description;
    }

    // ******************************************************************

    /// <summary>
    /// Reads the value of the <see cref="AssemblyProductAttribute"/>
    /// attribute for the given assembly.
    /// </summary>
    /// <param name="assembly">The assembly to read from.</param>
    /// <returns>The value of the given assembly's product attribute.</returns>
    public static string ReadProduct(
        [NotNull] this Assembly assembly
        )
    {
        var attributes = assembly.GetCustomAttributes(
            typeof(AssemblyProductAttribute),
            true
            );

        if (attributes.Length == 0)
        {
            return string.Empty;
        }

        if (attributes[0] is not AssemblyProductAttribute attr || 
            attr.Product.Length == 0)
        {
            return string.Empty;
        }

        return attr.Product;
    }

    // ******************************************************************

    /// <summary>
    /// Reads the value of the <see cref="AssemblyTrademarkAttribute"/>
    /// attribute for the given assembly.
    /// </summary>
    /// <param name="assembly">The assembly to read from.</param>
    /// <returns>The value of the given assembly's trademark attribute.</returns>
    public static string ReadTrademark(
        [NotNull] this Assembly assembly
        )
    {
        var attributes = assembly.GetCustomAttributes(
            typeof(AssemblyTrademarkAttribute),
            true
            );

        if (attributes.Length == 0)
        {
            return string.Empty;
        }

        if (attributes[0] is not AssemblyTrademarkAttribute attr || 
            attr.Trademark.Length == 0)
        {
            return string.Empty;
        }

        return attr.Trademark;
    }

    // ******************************************************************

    /// <summary>
    /// Reads the value of the <see cref="AssemblyVersionAttribute"/>
    /// attribute for the given assembly.
    /// </summary>
    /// <param name="assembly">The assembly to read from.</param>
    /// <returns>The value of the given assembly's version attribute.</returns>
    public static string ReadAssemblyVersion(
        [NotNull] this Assembly assembly
        )
    {
        var attributes = assembly.GetCustomAttributes(
            typeof(AssemblyVersionAttribute),
            true
            );

        if (attributes.Length == 0)
        {
            return string.Empty;
        }

        if (attributes[0] is not AssemblyVersionAttribute attr || 
            attr.Version.Length == 0)
        {
            return string.Empty;
        }

        return attr.Version;
    }

    // ******************************************************************

    /// <summary>
    /// This method returns the GIT commit hash for the given assembly.
    /// </summary>
    /// <param name="assembly">The assembly to use for the operation.</param>
    /// <returns>The GIT hash for the assembly.</returns>
    public static string ReadCommit(
        [NotNull] this Assembly assembly
        )
    {
        // This method is based on the following blog post: 
        // https://www.hanselman.com/blog/adding-a-git-commit-hash-and-azure-devops-build-number-and-build-id-to-an-aspnet-website

        var version = "1.0.0+LOCALBUILD"; // Dummy version for local dev

        var infoVerAttr = assembly.GetCustomAttributes<AssemblyInformationalVersionAttribute>()
            .FirstOrDefault();

        if (infoVerAttr != null && infoVerAttr.InformationalVersion.Length > 6)
        {
            // Hash is embedded in the version after a '+' symbol, e.g. 1.0.0+a34a913742f8845d3da5309b7b17242222d41a21
            version = infoVerAttr.InformationalVersion;
        }
        return version.Substring(version.IndexOf('+') + 1);
    }

    // ******************************************************************

    /// <summary>
    /// Reads the value of the <see cref="AssemblyInformationalVersionAttribute"/>
    /// attribute for the given assembly.
    /// </summary>
    /// <param name="assembly">The assembly to read from.</param>
    /// <returns>The value of the given assembly's informational version attribute.</returns>
    public static string ReadInformationalVersion(
        [NotNull] this Assembly assembly
        )
    {
        var attributes = assembly.GetCustomAttributes(
            typeof(AssemblyInformationalVersionAttribute),
            true
            );

        if (attributes.Length == 0)
        {
            return string.Empty;
        }

        if (attributes[0] is not AssemblyInformationalVersionAttribute attr || 
            attr.InformationalVersion.Length == 0)
        {
            return string.Empty;
        }

        var index = attr.InformationalVersion.IndexOf('+');
        if (0 < index)
        {
            return attr.InformationalVersion[
                ..(attr.InformationalVersion.Length - index - 2)
                ];
        }
        else
        {
            return attr.InformationalVersion;
        }
    }

    // *******************************************************************

    /// <summary>
    /// This method returns the value of the assembly's repository URL attribute.
    /// </summary>
    /// <param name="asm">the assembly to use for the operation.</param>
    /// <returns>The value of the repository URL attribute, or an empty string
    /// if no value was found.</returns>
    public static string ReadRepositoryUrl(
        [NotNull] this Assembly asm
        )
    {
        var customAttributes = asm.GetCustomAttributes(
            typeof(AssemblyMetadataAttribute),
            inherit: true
            );

        if (customAttributes.Length == 0)
        {
            return "";
        }

        var repositoryUrlAttribute = customAttributes.OfType<AssemblyMetadataAttribute>()
            .FirstOrDefault(x => x.Key == "RepositoryUrl");

        if (repositoryUrlAttribute is null)
        {
            return "";
        }

        return repositoryUrlAttribute.Value ?? "";
    }

    #endregion
}
