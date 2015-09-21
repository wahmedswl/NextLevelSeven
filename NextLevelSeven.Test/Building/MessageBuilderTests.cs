﻿using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NextLevelSeven.Building;
using NextLevelSeven.Core;

namespace NextLevelSeven.Test.Building
{
    [TestClass]
    sealed public class MessageBuilderTests : BuildingTestFixture
    {
        [TestMethod]
        public void MessageBuilder_CanGetValue()
        {
            var val0 = Randomized.StringCaps(3) + "|" + Randomized.String();
            var val1 = Randomized.StringCaps(3) + "|" + Randomized.String();
            var message = string.Format("MSH|^~\\&\r{0}\r{1}", val0, val1);
            var builder = Message.Build(message);
            Assert.AreEqual(builder.Value, message);
        }

        [TestMethod]
        public void MessageBuilder_CanGetValues()
        {
            var val0 = Randomized.StringCaps(3) + "|" + Randomized.String();
            var val1 = Randomized.StringCaps(3) + "|" + Randomized.String();
            var builder = Message.Build(string.Format("MSH|^~\\&\r{0}\r{1}",
                val0, val1));
            AssertEnumerable.AreEqual(builder.Values, new[] {"MSH|^~\\&", val0, val1});
        }

        [TestMethod]
        public void MessageBuilder_CanBeCloned()
        {
            var builder = Message.Build(ExampleMessages.Standard);
            var clone = builder.Clone();
            Assert.AreNotSame(builder, clone, "Builder and its clone must not refer to the same object.");
            Assert.AreEqual(builder.ToString(), clone.ToString(), "Clone data doesn't match source data.");
        }

        [TestMethod]
        public void MessageBuilder_CanBuildFields_Individually()
        {
            var builder = Message.Build();
            var field3 = Randomized.String();
            var field5 = Randomized.String();

            builder
                .SetField(1, 3, field3)
                .SetField(1, 5, field5);
            Assert.AreEqual(string.Format("MSH|^~\\&|{0}||{1}", field3, field5), builder.Value,
                @"Unexpected result.");
        }

        [TestMethod]
        public void MessageBuilder_CanBuildFields_OutOfOrder()
        {
            var builder = Message.Build();
            var field3 = Randomized.String();
            var field5 = Randomized.String();

            builder
                .SetField(1, 5, field5)
                .SetField(1, 3, field3);
            Assert.AreEqual(string.Format("MSH|^~\\&|{0}||{1}", field3, field5), builder.Value,
                @"Unexpected result.");
        }

        [TestMethod]
        public void MessageBuilder_CanBuildFields_Sequentially()
        {
            var builder = Message.Build();
            var field3 = Randomized.String();
            var field5 = Randomized.String();

            builder
                .SetFields(1, 3, field3, null, field5);
            Assert.AreEqual(string.Format("MSH|^~\\&|{0}||{1}", field3, field5), builder.Value,
                @"Unexpected result.");
        }

        [TestMethod]
        public void MessageBuilder_CanBuildRepetitions_Individually()
        {
            var builder = Message.Build();
            var repetition1 = Randomized.String();
            var repetition2 = Randomized.String();

            builder
                .SetFieldRepetition(1, 3, 1, repetition1)
                .SetFieldRepetition(1, 3, 2, repetition2);
            Assert.AreEqual(string.Format("MSH|^~\\&|{0}~{1}", repetition1, repetition2), builder.Value,
                @"Unexpected result.");
        }

        [TestMethod]
        public void MessageBuilder_CanBuildRepetitions_OutOfOrder()
        {
            var builder = Message.Build();
            var repetition1 = Randomized.String();
            var repetition2 = Randomized.String();

            builder
                .SetFieldRepetition(1, 3, 2, repetition2)
                .SetFieldRepetition(1, 3, 1, repetition1);
            Assert.AreEqual(string.Format("MSH|^~\\&|{0}~{1}", repetition1, repetition2), builder.Value,
                @"Unexpected result.");
        }

        [TestMethod]
        public void MessageBuilder_CanBuildRepetitions_Sequentially()
        {
            var builder = Message.Build();
            var repetition1 = Randomized.String();
            var repetition2 = Randomized.String();

            builder
                .SetFieldRepetitions(1, 3, repetition1, repetition2);
            Assert.AreEqual(string.Format("MSH|^~\\&|{0}~{1}", repetition1, repetition2), builder.Value,
                @"Unexpected result.");
        }

        [TestMethod]
        public void MessageBuilder_CanBuildSegments_Individually()
        {
            var builder = Message.Build();
            var segment2 = "ZZZ|" + Randomized.String();
            var segment3 = "ZAA|" + Randomized.String();

            builder
                .SetSegment(2, segment2)
                .SetSegment(3, segment3);
            Assert.AreEqual(string.Format("MSH|^~\\&\xD{0}\xD{1}", segment2, segment3), builder.Value,
                @"Unexpected result.");
        }

        [TestMethod]
        public void MessageBuilder_CanBuildSegments_OutOfOrder()
        {
            var builder = Message.Build();
            var segment2 = "ZOT|" + Randomized.String();
            var segment3 = "ZED|" + Randomized.String();

            builder
                .SetSegment(4, segment3)
                .SetSegment(2, segment2);
            Assert.AreEqual(string.Format("MSH|^~\\&\xD{0}\xD{1}", segment2, segment3), builder.Value,
                @"Unexpected result.");
        }

        [TestMethod]
        public void MessageBuilder_CanBuildSegments_Sequentially()
        {
            var builder = Message.Build();
            var segment2 = "ZIP|" + Randomized.String();
            var segment3 = "ZAP|" + Randomized.String();

            builder
                .SetSegments(2, segment2, segment3);
            Assert.AreEqual(string.Format("MSH|^~\\&\xD{0}\xD{1}", segment2, segment3), builder.Value,
                @"Unexpected result.");
        }

        [TestMethod]
        public void MessageBuilder_CanBuildComponents_Individually()
        {
            var builder = Message.Build();
            var component1 = Randomized.String();
            var component2 = Randomized.String();

            builder
                .SetComponent(1, 3, 1, 1, component1)
                .SetComponent(1, 3, 1, 2, component2);
            Assert.AreEqual(string.Format("MSH|^~\\&|{0}^{1}", component1, component2), builder.Value,
                @"Unexpected result.");
        }

        [TestMethod]
        public void MessageBuilder_CanBuildComponents_OutOfOrder()
        {
            var builder = Message.Build();
            var component1 = Randomized.String();
            var component2 = Randomized.String();

            builder
                .SetComponent(1, 3, 1, 2, component2)
                .SetComponent(1, 3, 1, 1, component1);
            Assert.AreEqual(string.Format("MSH|^~\\&|{0}^{1}", component1, component2), builder.Value,
                @"Unexpected result.");
        }

        [TestMethod]
        public void MessageBuilder_CanBuildComponents_Sequentially()
        {
            var builder = Message.Build();
            var component1 = Randomized.String();
            var component2 = Randomized.String();

            builder
                .SetComponents(1, 3, 1, component1, component2);
            Assert.AreEqual(string.Format("MSH|^~\\&|{0}^{1}", component1, component2), builder.Value,
                @"Unexpected result.");
        }

        [TestMethod]
        public void MessageBuilder_CanBuildSubcomponents_Individually()
        {
            var builder = Message.Build();
            var subcomponent1 = Randomized.String();
            var subcomponent2 = Randomized.String();

            builder
                .SetSubcomponent(1, 3, 1, 1, 1, subcomponent1)
                .SetSubcomponent(1, 3, 1, 1, 2, subcomponent2);
            Assert.AreEqual(string.Format("MSH|^~\\&|{0}&{1}", subcomponent1, subcomponent2), builder.Value,
                @"Unexpected result.");
        }

        [TestMethod]
        public void MessageBuilder_CanBuildSubcomponents_OutOfOrder()
        {
            var builder = Message.Build();
            var subcomponent1 = Randomized.String();
            var subcomponent2 = Randomized.String();

            builder
                .SetSubcomponent(1, 3, 1, 1, 2, subcomponent2)
                .SetSubcomponent(1, 3, 1, 1, 1, subcomponent1);
            Assert.AreEqual(string.Format("MSH|^~\\&|{0}&{1}", subcomponent1, subcomponent2), builder.Value,
                @"Unexpected result.");
        }

        [TestMethod]
        public void MessageBuilder_CanBuildSubcomponents_Sequentially()
        {
            var builder = Message.Build();
            var subcomponent1 = Randomized.String();
            var subcomponent2 = Randomized.String();

            builder
                .SetSubcomponents(1, 3, 1, 1, 1, subcomponent1, subcomponent2);
            Assert.AreEqual(string.Format("MSH|^~\\&|{0}&{1}", subcomponent1, subcomponent2), builder.Value,
                @"Unexpected result.");
        }

        [TestMethod]
        public void MessageBuilder_ConvertsToMessage()
        {
            var builder = Message.Build(ExampleMessages.Standard);
            var beforeMessageString = builder.Value;
            var message = builder.ToParser();
            Assert.AreEqual(beforeMessageString, message.Value, "Conversion from builder to message failed.");
        }

        [TestMethod]
        public void MessageBuilder_ConvertsFromMessage()
        {
            var message = Message.Parse(ExampleMessages.Standard);
            var beforeBuilderString = message.Value;
            var afterBuilder = Message.Build(message);
            Assert.AreEqual(beforeBuilderString, afterBuilder.Value, "Conversion from message to builder failed.");
        }

        [TestMethod]
        public void MessageBuilder_ConvertsMshCorrectly()
        {
            var builder = Message.Build(ExampleMessages.MshOnly);
            Assert.AreEqual(ExampleMessages.MshOnly, builder.Value);
        }

        [TestMethod]
        public void MessageBuilder_UsesReasonableMemory_WhenParsingLargeMessages()
        {
            var before = GC.GetTotalMemory(true);
            var message = Message.Build();
            message.SetField(1000000, 1000000, Randomized.String());
            var messageString = message.Value;
            var usage = GC.GetTotalMemory(false) - before;
            var overhead = usage - (messageString.Length << 1);
            var usePerCharacter = (overhead/(messageString.Length << 1));
            Assert.IsTrue(usePerCharacter < 10);
        }

        [TestMethod]
        public void MessageBuilder_HasProperDefaultFieldDelimiter()
        {
            var builder = Message.Build();
            Assert.AreEqual('|', builder.FieldDelimiter);
        }

        [TestMethod]
        public void MessageBuilder_HasProperDefaultComponentDelimiter()
        {
            var builder = Message.Build();
            Assert.AreEqual('^', builder.ComponentDelimiter);
        }

        [TestMethod]
        public void MessageBuilder_HasProperDefaultSubcomponentDelimiter()
        {
            var builder = Message.Build();
            Assert.AreEqual('&', builder.SubcomponentDelimiter);
        }

        [TestMethod]
        public void MessageBuilder_HasProperDefaultEscapeDelimiter()
        {
            var builder = Message.Build();
            Assert.AreEqual('\\', builder.EscapeDelimiter);
        }

        [TestMethod]
        public void MessageBuilder_HasProperDefaultRepetitionDelimiter()
        {
            var builder = Message.Build();
            Assert.AreEqual('~', builder.RepetitionDelimiter);
        }

        [TestMethod]
        public void MessageBuilder_ContainsSegmentBuilders()
        {
            var builder = Message.Build();
            Assert.IsInstanceOfType(builder[1], typeof (ISegmentBuilder));
        }

        [TestMethod]
        public void MessageBuilder_ReturnsSegmentValues()
        {
            var id1 = Randomized.String();
            var id2 = Randomized.String();
            var builder = Message.Build(string.Format("MSH|^~\\&|{0}\xDPID|{1}", id1, id2));
            var builderValues = builder.Values.ToList();
            Assert.AreEqual(string.Format("MSH|^~\\&|{0}", id1), builderValues[0]);
            Assert.AreEqual(string.Format("PID|{0}", id2), builderValues[1]);
        }

        [TestMethod]
        public void MessageBuilder_ReturnsSegmentValuesAsArray()
        {
            var id1 = Randomized.String();
            var id2 = Randomized.String();
            var builder = Message.Build(string.Format("MSH|^~\\&|{0}\xDPID|{1}", id1, id2));
            var builderValues = builder.Values.ToArray();
            Assert.AreEqual(string.Format("MSH|^~\\&|{0}", id1), builderValues[0]);
            Assert.AreEqual(string.Format("PID|{0}", id2), builderValues[1]);
        }

        [TestMethod]
        public void MessageBuilder_CanSetMsh1()
        {
            var builder = Message.Build(ExampleMessages.Minimum + "|");
            builder.SetField(1, 1, ":");
            Assert.AreEqual("MSH:^~\\&:", builder.Value);
        }

        [TestMethod]
        public void MessageBuilder_CanSetMsh1ToDefaultWithNull()
        {
            var builder = Message.Build(ExampleMessages.Minimum + "|");
            builder.SetField(1, 1, null);
            Assert.AreEqual("MSH|^~\\&|", builder.Value);
        }

        [TestMethod]
        public void MessageBuilder_CanSetMsh2()
        {
            var builder = Message.Build(ExampleMessages.Minimum + "|");
            builder.SetField(1, 2, "@#$%");
            Assert.AreEqual("MSH|@#$%|", builder.Value);
        }

        [TestMethod]
        public void MessageBuilder_CanSetTypeToMsh()
        {
            var builder = Message.Build();
            builder.SetField(2, 0, "MSH");
        }

        [TestMethod]
        public void MessageBuilder_CanNotChangeTypeFromMsh()
        {
            // NOTE: by design.
            // (change this test if the design changes.)
            var builder = Message.Build();
            It.Throws<BuilderException>(() => builder.SetField(1, 0, "PID"));
        }

        [TestMethod]
        public void MessageBuilder_CanNotChangeTypeToMsh()
        {
            // NOTE: by design.
            // (change this test if the design changes.)
            var builder = Message.Build();
            builder.SetField(2, 0, "PID");
            It.Throws<BuilderException>(() => builder.SetField(2, 0, "MSH"));
        }

        [TestMethod]
        public void MessageBuilder_CanSetMsh2Component()
        {
            var id1 = Randomized.String();
            var id2 = Randomized.String();
            var builder = Message.Build(string.Format("MSH|^~\\&|{0}^{1}", id1, id2));
            builder.SetField(1, 2, "$~\\&");
            Assert.AreEqual(string.Format("MSH|$~\\&|{0}${1}", id1, id2), builder.Value);
        }

        [TestMethod]
        public void MessageBuilder_CanSetMsh2Repetition()
        {
            var id1 = Randomized.String();
            var id2 = Randomized.String();
            var builder = Message.Build(string.Format("MSH|^~\\&|{0}~{1}", id1, id2));
            builder.SetField(1, 2, "^$\\&");
            Assert.AreEqual(string.Format("MSH|^$\\&|{0}${1}", id1, id2), builder.Value);
        }

        [TestMethod]
        public void MessageBuilder_CanSetMsh2Escape()
        {
            // NOTE: changing escape code does not affect anything but MSH-2 for design reasons.
            // (change this message if the functionality is ever added and this test updated.)
            var id1 = Randomized.String();
            var id2 = Randomized.String();
            var builder = Message.Build(string.Format("MSH|^~\\&|\\H\\{0}\\N\\{1}", id1, id2));
            builder.SetField(1, 2, "^~$&");
            Assert.AreEqual(string.Format("MSH|^~$&|\\H\\{0}\\N\\{1}", id1, id2), builder.Value);
        }

        [TestMethod]
        public void MessageBuilder_CanSetMsh2Subcomponent()
        {
            var id1 = Randomized.String();
            var id2 = Randomized.String();
            var builder = Message.Build(string.Format("MSH|^~\\&|{0}&{1}", id1, id2));
            builder.SetField(1, 2, "^~\\$");
            Assert.AreEqual(string.Format("MSH|^~\\$|{0}${1}", id1, id2), builder.Value);
        }

        [TestMethod]
        public void MessageBuilder_CanSetMsh2ToDefaultWithNull()
        {
            var builder = Message.Build(ExampleMessages.Minimum + "|");
            builder.SetField(1, 2, null);
            Assert.AreEqual("MSH|^~\\&|", builder.Value);
        }

        [TestMethod]
        public void MessageBuilder_CanSetMsh2Partially()
        {
            var builder = Message.Build(ExampleMessages.Minimum + "|");
            builder.SetField(1, 2, "$");
            Assert.AreEqual("MSH|$~\\&|", builder.Value);
        }

        [TestMethod]
        public void MessageBuilder_CanUseDifferentFieldDelimiter()
        {
            var id = Randomized.String();
            const char delimiter = ':';
            var builder = Message.Build(string.Format("MSH{0}^~\\&{0}{1}", delimiter, id));
            Assert.AreEqual(delimiter, builder.FieldDelimiter);
            Assert.AreEqual(id, builder[1][3].Value);
        }

        [TestMethod]
        public void MessageBuilder_CanChangeFieldDelimiter()
        {
            var id = Randomized.String();
            const char delimiter = ':';
            var builder = Message.Build(string.Format("MSH|^~\\&|{0}", id));
            builder.FieldDelimiter = delimiter;
            Assert.AreEqual(delimiter, builder.FieldDelimiter);
            Assert.AreEqual(id, builder[1][3].Value);
        }

        [TestMethod]
        public void MessageBuilder_CanUseDifferentEscapeDelimiter()
        {
            var id = Randomized.String();
            const char delimiter = ':';
            var builder = Message.Build(string.Format("MSH|^~{0}&|{1}", delimiter, id));
            Assert.AreEqual(delimiter, builder.EscapeDelimiter);
            Assert.AreEqual(id, builder[1][3].Value);
        }

        [TestMethod]
        public void MessageBuilder_CanChangeEscapeDelimiter()
        {
            var id = Randomized.String();
            const char delimiter = ':';
            var builder = Message.Build(string.Format("MSH|^~\\&|{0}", id));
            builder.FieldDelimiter = delimiter;
            Assert.AreEqual(delimiter, builder.FieldDelimiter);
            Assert.AreEqual(id, builder[1][3].Value);
        }

        [TestMethod]
        public void MessageBuilder_CanMapSegments()
        {
            var id = Randomized.String();
            IMessage tree = Message.Build(string.Format("MSH|^~\\&|{0}", id));
            Assert.AreEqual(string.Format("MSH|^~\\&|{0}", id), tree.GetValue(1));
        }

        [TestMethod]
        public void MessageBuilder_CanMapFields()
        {
            var id = Randomized.String();
            IMessage tree = Message.Build(string.Format("MSH|^~\\&|{0}", id));
            Assert.AreEqual(id, tree.GetValue(1, 3));
        }

        [TestMethod]
        public void MessageBuilder_CanMapRepetitions()
        {
            var id1 = Randomized.String();
            var id2 = Randomized.String();
            IMessage tree = Message.Build(string.Format("MSH|^~\\&|{0}~{1}", id1, id2));
            Assert.AreEqual(id1, tree.GetValue(1, 3, 1));
            Assert.AreEqual(id2, tree.GetValue(1, 3, 2));
        }

        [TestMethod]
        public void MessageBuilder_CanMapComponents()
        {
            var id1 = Randomized.String();
            var id2 = Randomized.String();
            IMessage tree = Message.Build(string.Format("MSH|^~\\&|{0}^{1}", id1, id2));
            Assert.AreEqual(id1, tree.GetValue(1, 3, 1, 1));
            Assert.AreEqual(id2, tree.GetValue(1, 3, 1, 2));
        }

        [TestMethod]
        public void MessageBuilder_CanMapSubcomponents()
        {
            var id1 = Randomized.String();
            var id2 = Randomized.String();
            IMessage tree = Message.Build(string.Format("MSH|^~\\&|{0}&{1}", id1, id2));
            Assert.AreEqual(id1, tree.GetValue(1, 3, 1, 1, 1));
            Assert.AreEqual(id2, tree.GetValue(1, 3, 1, 1, 2));
        }
    }
}