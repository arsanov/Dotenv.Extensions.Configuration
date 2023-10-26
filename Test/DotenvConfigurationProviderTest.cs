using Microsoft.Extensions.Configuration;
using Dotenv.Extensions.Configuration;
using Xunit;
using System;
using FluentAssertions;
using System.Linq;
using System.IO;

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

            Action action = () => builder.AddDotenvFile(fileName, optional: false);

            File.Exists(fileName).Should().BeFalse();
            action.Should().Throw<FileNotFoundException>();
        }
    }
}