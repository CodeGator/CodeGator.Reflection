
#pragma warning disable IDE0130
namespace System.Reflection;
#pragma warning restore IDE0130

/// <summary>
/// This class adds reflection helpers for reading common assembly metadata fields.
/// </summary>
public static partial class AssemblyExtensions
{

    /// <summary>
    /// This method yields each type in the assembly that carries attribute type T.
    /// </summary>
    /// <remarks>
    /// Enumeration calls <see cref="Assembly.GetTypes"/>, which loads all types in the assembly.
    /// </remarks>
    /// <typeparam name="T">The attribute type to match.</typeparam>
    /// <param name="assembly">The assembly whose types are scanned.</param>
    /// <returns>Types that declare <typeparamref name="T"/>.</returns>
    /// <exception cref="ReflectionTypeLoadException">
    /// Thrown when type loading fails for the assembly.
    /// </exception>
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

    /// <summary>
    /// This method returns the company attribute value, or an empty string if absent.
    /// </summary>
    /// <param name="assembly">The assembly to read from.</param>
    /// <returns>The company value, or <see cref="string.Empty"/>.</returns>
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

    /// <summary>
    /// This method returns the copyright attribute value, or an empty string if absent.
    /// </summary>
    /// <param name="assembly">The assembly to read from.</param>
    /// <returns>The copyright value, or <see cref="string.Empty"/>.</returns>
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

    /// <summary>
    /// This method returns the title attribute value, or an empty string if absent.
    /// </summary>
    /// <param name="assembly">The assembly to read from.</param>
    /// <returns>The title value, or <see cref="string.Empty"/>.</returns>
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

    /// <summary>
    /// This method returns the description value, or an empty string if absent.
    /// </summary>
    /// <param name="assembly">The assembly to read from.</param>
    /// <returns>The description value, or <see cref="string.Empty"/>.</returns>
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

    /// <summary>
    /// This method returns the product attribute value, or an empty string if absent.
    /// </summary>
    /// <param name="assembly">The assembly to read from.</param>
    /// <returns>The product value, or <see cref="string.Empty"/>.</returns>
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

    /// <summary>
    /// This method returns the trademark attribute value, or an empty string if absent.
    /// </summary>
    /// <param name="assembly">The assembly to read from.</param>
    /// <returns>The trademark value, or <see cref="string.Empty"/>.</returns>
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

    /// <summary>
    /// This method returns the version attribute value, or an empty string if absent.
    /// </summary>
    /// <param name="assembly">The assembly to read from.</param>
    /// <returns>The version value, or <see cref="string.Empty"/>.</returns>
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

    /// <summary>
    /// This method returns the Git commit fragment from informational version metadata.
    /// </summary>
    /// <param name="assembly">The assembly to read from.</param>
    /// <returns>The substring after '+', or the dummy local-build token when absent.</returns>
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

    /// <summary>
    /// This method returns processed informational version text from assembly metadata.
    /// </summary>
    /// <remarks>
    /// When a '+' appears, returns the prefix segment produced by the same length rules as
    /// the first <see cref="AssemblyInformationalVersionAttribute"/> value; otherwise returns
    /// the full informational version string.
    /// </remarks>
    /// <param name="assembly">The assembly to read from.</param>
    /// <returns>The trimmed informational version, or <see cref="string.Empty"/>.</returns>
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

    /// <summary>
    /// This method returns RepositoryUrl metadata, or an empty string if absent.
    /// </summary>
    /// <param name="asm">The assembly whose repository URL metadata is read.</param>
    /// <returns>The metadata value, or an empty string when not found.</returns>
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

}
