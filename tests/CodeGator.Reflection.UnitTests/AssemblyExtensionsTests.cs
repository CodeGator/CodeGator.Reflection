using System.Reflection;
using System.Reflection.Emit;

namespace CodeGator.Reflection.UnitTests;

[TestClass]
public sealed class AssemblyExtensionsTests
{
    /// <summary>
    /// The CodeGator.Reflection assembly (resolved by assembly-qualified type name to avoid a
    /// compile-time clash with <c>System.Reflection.AssemblyExtensions</c> from the BCL).
    /// </summary>
    private static Assembly ReflectionAssembly { get; } = Type.GetType(
        "System.Reflection.AssemblyExtensions, CodeGator.Reflection",
        throwOnError: true)!.Assembly;

    private static Assembly ExecutingAssembly => Assembly.GetExecutingAssembly();

    #region GetDecoratedTypes

    [TestMethod]
    public void GetDecoratedTypes_ReturnsTypesWithAttribute()
    {
        var types = ExecutingAssembly.GetDecoratedTypes<TestClassAttribute>().ToList();

        CollectionAssert.IsSubsetOf(
            new[] { typeof(AssemblyExtensionsTests) },
            types);
    }

    [TestMethod]
    public void GetDecoratedTypes_ExcludesTypesWithoutAttribute()
    {
        var types = ExecutingAssembly.GetDecoratedTypes<ObsoleteAttribute>().ToList();

        Assert.IsFalse(types.Contains(typeof(AssemblyExtensionsTests)));
    }

    [AttributeUsage(AttributeTargets.Class)]
    private sealed class MarkerAttribute : Attribute;

    [Marker]
    private static class GetDecoratedTypesFixture;

    [TestMethod]
    public void GetDecoratedTypes_CustomAttribute_FindsDecoratedTypeOnly()
    {
        var types = ExecutingAssembly.GetDecoratedTypes<MarkerAttribute>().ToList();

        CollectionAssert.AreEquivalent(
            new[] { typeof(GetDecoratedTypesFixture) },
            types);
    }

    #endregion

    #region ReadCompany / ReadCopyright (executing assembly — Directory.Build.props)

    [TestMethod]
    public void ReadCompany_ExecutingAssembly_MatchesAssemblyCompanyAttribute()
    {
        var expected = ExecutingAssembly.GetCustomAttributes<AssemblyCompanyAttribute>()
            .FirstOrDefault()?.Company ?? string.Empty;

        if (expected.Length == 0)
        {
            Assert.AreEqual(string.Empty, ExecutingAssembly.ReadCompany());
        }
        else
        {
            Assert.AreEqual(expected, ExecutingAssembly.ReadCompany());
        }
    }

    [TestMethod]
    public void ReadCopyright_ExecutingAssembly_MatchesAssemblyCopyrightAttribute()
    {
        var expected = ExecutingAssembly.GetCustomAttributes<AssemblyCopyrightAttribute>()
            .FirstOrDefault()?.Copyright ?? string.Empty;

        if (expected.Length == 0)
        {
            Assert.AreEqual(string.Empty, ExecutingAssembly.ReadCopyright());
        }
        else
        {
            Assert.AreEqual(expected, ExecutingAssembly.ReadCopyright());
        }
    }

    #endregion

    #region ReadTitle / ReadDescription / ReadProduct / ReadTrademark / ReadAssemblyVersion

    [TestMethod]
    public void ReadTitle_ReflectionAssembly_MatchesAttributeOrEmpty()
    {
        AssertReadMatchesFirstStringAttribute<AssemblyTitleAttribute>(
            ReflectionAssembly,
            a => a.Title,
            ReflectionAssembly.ReadTitle);
    }

    [TestMethod]
    public void ReadDescription_ReflectionAssembly_MatchesAttributeOrEmpty()
    {
        AssertReadMatchesFirstStringAttribute<AssemblyDescriptionAttribute>(
            ReflectionAssembly,
            a => a.Description,
            ReflectionAssembly.ReadDescription);
    }

    [TestMethod]
    public void ReadProduct_ReflectionAssembly_MatchesAttributeOrEmpty()
    {
        AssertReadMatchesFirstStringAttribute<AssemblyProductAttribute>(
            ReflectionAssembly,
            a => a.Product,
            ReflectionAssembly.ReadProduct);
    }

    [TestMethod]
    public void ReadTrademark_ReflectionAssembly_MatchesAttributeOrEmpty()
    {
        AssertReadMatchesFirstStringAttribute<AssemblyTrademarkAttribute>(
            ReflectionAssembly,
            a => a.Trademark,
            ReflectionAssembly.ReadTrademark);
    }

    [TestMethod]
    public void ReadAssemblyVersion_ReflectionAssembly_MatchesAttributeOrEmpty()
    {
        AssertReadMatchesFirstStringAttribute<AssemblyVersionAttribute>(
            ReflectionAssembly,
            a => a.Version,
            ReflectionAssembly.ReadAssemblyVersion);
    }

    private static void AssertReadMatchesFirstStringAttribute<TAttr>(
        Assembly assembly,
        Func<TAttr, string> getValue,
        Func<string> read)
        where TAttr : Attribute
    {
        var attr = assembly.GetCustomAttributes<TAttr>().FirstOrDefault();
        var expected = attr is null ? string.Empty : getValue(attr);
        if (expected.Length == 0)
        {
            Assert.AreEqual(string.Empty, read());
        }
        else
        {
            Assert.AreEqual(expected, read());
        }
    }

    #endregion

    #region ReadRepositoryUrl

    [TestMethod]
    public void ReadRepositoryUrl_ReflectionAssembly_WhenMetadataPresent_ReturnsUrl()
    {
        var url = ReflectionAssembly.ReadRepositoryUrl();

        var fromMetadata = ReflectionAssembly.GetCustomAttributes<AssemblyMetadataAttribute>()
            .FirstOrDefault(x => x.Key == "RepositoryUrl")?.Value;

        if (string.IsNullOrEmpty(fromMetadata))
        {
            Assert.AreEqual(string.Empty, url);
        }
        else
        {
            Assert.AreEqual(fromMetadata, url);
        }
    }

    #endregion

    #region ReadInformationalVersion / ReadCommit

    [TestMethod]
    public void ReadInformationalVersion_MatchesDocumentedTransformationOfAttribute()
    {
        var attr = ReflectionAssembly.GetCustomAttributes<AssemblyInformationalVersionAttribute>()
            .FirstOrDefault();

        if (attr is null || attr.InformationalVersion.Length == 0)
        {
            Assert.AreEqual(string.Empty, ReflectionAssembly.ReadInformationalVersion());
            return;
        }

        var index = attr.InformationalVersion.IndexOf('+');
        string expected;
        if (index > 0)
        {
            expected = attr.InformationalVersion[..(attr.InformationalVersion.Length - index - 2)];
        }
        else
        {
            expected = attr.InformationalVersion;
        }

        Assert.AreEqual(expected, ReflectionAssembly.ReadInformationalVersion());
    }

    [TestMethod]
    public void ReadCommit_ReturnsSubstringAfterPlus_FromInformationalVersionOrLocalBuild()
    {
        var attr = ReflectionAssembly.GetCustomAttributes<AssemblyInformationalVersionAttribute>()
            .FirstOrDefault();

        string version;
        if (attr is not null && attr.InformationalVersion.Length > 6)
        {
            version = attr.InformationalVersion;
        }
        else
        {
            version = "1.0.0+LOCALBUILD";
        }

        var expected = version[(version.IndexOf('+') + 1)..];
        Assert.AreEqual(expected, ReflectionAssembly.ReadCommit());
    }

    #endregion

    #region Dynamic assembly (no assembly info attributes)

    [TestMethod]
    public void ReadStringAttributes_DynamicAssemblyWithoutAttributes_ReturnEmpty()
    {
        var asm = CreateEmptyDynamicAssembly();

        Assert.AreEqual(string.Empty, asm.ReadCompany());
        Assert.AreEqual(string.Empty, asm.ReadCopyright());
        Assert.AreEqual(string.Empty, asm.ReadTitle());
        Assert.AreEqual(string.Empty, asm.ReadDescription());
        Assert.AreEqual(string.Empty, asm.ReadProduct());
        Assert.AreEqual(string.Empty, asm.ReadTrademark());
        Assert.AreEqual(string.Empty, asm.ReadAssemblyVersion());
        Assert.AreEqual(string.Empty, asm.ReadInformationalVersion());
        Assert.AreEqual(string.Empty, asm.ReadRepositoryUrl());
    }

    [TestMethod]
    public void ReadCommit_DynamicAssemblyWithoutInfoVersion_ReturnsLocalBuild()
    {
        var asm = CreateEmptyDynamicAssembly();

        Assert.AreEqual("LOCALBUILD", asm.ReadCommit());
    }

    private static Assembly CreateEmptyDynamicAssembly()
    {
        var name = new AssemblyName("CodeGator.Reflection.UnitTests.EmptyDynamic");
        var builder = AssemblyBuilder.DefineDynamicAssembly(
            name,
            AssemblyBuilderAccess.Run);
        builder.DefineDynamicModule("Main");
        return builder;
    }

    #endregion
}
