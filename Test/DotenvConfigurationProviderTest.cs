using System;
using Xunit;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Dotenv.Extensions.Configuration;
using System.Linq;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Test
{
    public class DotenvConfigurationProviderTest
    {
        [Fact]
        public void TestDifferentValueFormats()
        {
            var builder = new ConfigurationBuilder();

            builder.AddDotenvFile("Assets/test1.env");
            var configuration = builder.Build();

            configuration["SomeValue"].Should().Be("123");
            configuration["AnotherValue_InnerValue"].Should().Be("asd");
            configuration["AnotherValue:InnerValue"].Should().Be("qwe");
            configuration["AnotherValue__InnerValue"].Should().BeNull();
            configuration["AnotherValue:InnerValue:0"].Should().Be("qwe\"=zxc");
        }

        [Fact]
        public void TestDuplicates()
        {
            var builder = new ConfigurationBuilder();

            builder.AddDotenvFile("Assets/duplicates.env");
            var configuration = builder.Build();

            configuration["SomeValue"].Should().Be("123");
            configuration["AnotherValue:InnerValue"].Should().Be("zxc");
            configuration["AnotherValue"].Should().BeNull();

            var allKeys = configuration.AsEnumerable().Select(p => p.Key).ToList();

            allKeys.Should().Contain("SomeValue");
            allKeys.Should().Contain("AnotherValue");
            allKeys.Should().Contain("AnotherValue:InnerValue");
            allKeys.Count.Should().Be(3);
        }

        [Fact]
        public void TestCaseInsensitive()
        {
            var builder = new ConfigurationBuilder();

            builder.AddDotenvFile("Assets/case_sensitivity.env");
            var configuration = builder.Build();

            configuration["TEST1"].Should().Be("Test1");
            // when there is same key(but different case - case insensitive) the value is overwritten
            configuration["TEST1"].Should().NotBe("TEST1");
            configuration["Test1"].Should().Be("Test1");
            configuration["TEST2"].Should().Be("TeSt2");
            configuration["Test2"].Should().Be("TeSt2");
            configuration["TeSt2"].Should().Be("TeSt2");

            var allKeys = configuration.AsEnumerable().Select(p => p.Key).ToList();

            // but when a value overwritten, the first key is still used(i.e. key preserves character case, but value)
            allKeys.Should().Contain("TEST1");
            allKeys.Should().Contain("TeST2");
            allKeys.Count.Should().Be(2);
        }

        [Fact]
        public void TestPrefix_PrefixSpecified_OnlyPrefixedLoaded()
        {
            var builder = new ConfigurationBuilder();

            builder.AddDotenvFile("Assets/prefix.env", prefix: "Prefixed_");
            var configuration = builder.Build();
            var allKeys = configuration.AsEnumerable().ToList();

            configuration["SomeValue"].Should().Be("123");

            allKeys.Count.Should().Be(1);
        }

        [Fact]
        public void TestPrefix_NoPrefixSpecified_AllItemsLoaded()
        {
            var builder = new ConfigurationBuilder();

            builder.AddDotenvFile("Assets/prefix.env");
            var configuration = builder.Build();
            var allKeys = configuration.AsEnumerable().ToList();

            configuration["Prefixed_SomeValue"].Should().Be("123");
            configuration["AnotherValue:InnerValue"].Should().Be("qwe");
            configuration["AnotherValuex"].Should().BeNull();

            allKeys.Count.Should().Be(3);
        }

        [Fact]
        public void TestPrefix_FileNotExistsOptionalTrue_NothingHappens()
        {
            var fileName = "Assets/nonexistingfile.env";
            var builder = new ConfigurationBuilder();

            builder.AddDotenvFile(fileName, optional: true);
            var configuration = builder.Build();
            var allKeys = configuration.AsEnumerable().ToList();

            File.Exists(fileName).Should().BeFalse();
            allKeys.Count.Should().Be(0);
        }

        [Fact]
        public void TestPrefix_FileNotExistsOptionalFalse_FileNotFoundExceptionThrown()
        {
            var fileName = "Assets/nonexistingfile.env";
            var builder = new ConfigurationBuilder();

            builder.AddDotenvFile(fileName, optional: false);
            Action action = () => builder.Build();

            File.Exists(fileName).Should().BeFalse();
            action.Should().Throw<FileNotFoundException>();
        }

        [Fact]
        public async Task Test_ReloadOnChange_SettingsChanged()
        {
            var sourceFileName = "Assets/test1.env";
            var fileName = "Assets/test1ReloadOnChange.env";
            File.Copy(sourceFileName, fileName, overwrite: true);
            var builder = new ConfigurationBuilder();

            builder.AddDotenvFile(fileName, optional: false, "", reloadOnChange: true);
            var configuration = builder.Build();
            var allKeys = configuration.AsEnumerable().ToList();

            var changeToken = configuration.GetReloadToken();
            using var cancellationSource = new CancellationTokenSource();
            var completionSource = new TaskCompletionSource();
            cancellationSource.CancelAfter(TimeSpan.FromSeconds(3));
            using (cancellationSource.Token.Register(() => completionSource.SetCanceled(cancellationSource.Token)))
            using (changeToken.RegisterChangeCallback(o => completionSource.SetResult(), null))
            {
                configuration["AnotherValue:InnerValue:0"].Should().Be("qwe\"=zxc");
                File.WriteAllText(fileName, "AnotherValue__InnerValue__0=tst");
                await completionSource.Task;
                configuration["AnotherValue:InnerValue:0"].Should().Be("tst");
            }
        }
    }
}