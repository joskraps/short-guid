using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace ShortGuid.Tests
{
    [TestClass]
    public class GuidTests
    {
        [TestMethod]
        public void ShortGuidEncodingUsingGuidConstructor()
        {
            var g = Guid.NewGuid();
            var sg = new ShortGuid(g);

            Assert.AreEqual(g, sg.Guid);
        }

        [TestMethod]
        public void ShortGuidEncodingUsingShortGuidConstructor()
        {
            var g = Guid.NewGuid();
            var sg = new ShortGuid(g).Value;
            var newSg = new ShortGuid(sg);

            Assert.AreEqual(g, newSg.Guid);
        }

        [TestMethod]
        public void SerializationTest()
        {
            var sg = new ShortGuid(Guid.NewGuid());
            var strSg = JsonConvert.SerializeObject(sg);
            var expected = "\"" + sg + "\"";

            Assert.AreEqual(expected, strSg);
        }

        [TestMethod]
        public void DeSerializationTest()
        {
            var origional = new ShortGuid(Guid.NewGuid());
            var jsonData = "\"" + origional.Value + "\"";
            var deserialized = JsonConvert.DeserializeObject<ShortGuid>(jsonData);

            Assert.AreEqual(origional.Guid, deserialized.Guid);
        }
    }
}