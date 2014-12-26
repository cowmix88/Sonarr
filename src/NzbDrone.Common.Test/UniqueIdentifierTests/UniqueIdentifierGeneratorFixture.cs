using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using NzbDrone.Common.UniqueIdentifier;
using NzbDrone.Test.Common;
using FluentAssertions;

namespace NzbDrone.Common.Test.UniqueIdentifierTests
{
    [TestFixture]
    public class UniqueIdentifierGeneratorFixture: TestBase<UniqueIdentifierGenerator>
    {
        [Test]
        public void should_return_positive_generated_id()
        {
            var id = Subject.Get("abc");

            id.Should().BeGreaterThan(0);
        }

        [Test]
        public void should_generated_unique_id()
        {
            var id1 = Subject.Get("abc");
            var id2 = Subject.Get("abc2");

            id2.Should().NotBe(id1);
        }

        [Test]
        public void should_return_same_generated_id()
        {
            var id1 = Subject.Get("abc");
            var id2 = Subject.Get("abc");

            id2.Should().Be(id1);
        }

        [Test]
        public void should_wrap_around()
        {
            var fieldInfo = Subject.GetType().GetField("_lastIdentifier", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);
            fieldInfo.SetValue(null, 0x7fffffff);

            var id = Subject.Get("abc");

            id.Should().Be(1);
        }
    }
}
