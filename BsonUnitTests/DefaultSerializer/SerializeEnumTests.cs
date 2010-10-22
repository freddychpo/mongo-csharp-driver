﻿/* Copyright 2010 10gen Inc.
*
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
* http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;

using MongoDB.Bson;
using MongoDB.Bson.DefaultSerializer;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;

namespace MongoDB.BsonUnitTests.DefaultSerializer {
    [TestFixture]
    public class SerializeEnumTests {
        // TODO: add unit tests for other underlying types
        private enum E {
            A = 1,
            B = 2
        }

        private class C {
            [BsonUseCompactRepresentation]
            public E CE { get; set; }
            public E FE { get; set; }
        }

        [Test]
        public void TestSerializeZero() {
            C c = new C { CE = 0, FE = 0 };
            var json = c.ToJson();
            var expected = ("{ 'CE' : 0, 'FE' : '0' }").Replace("'", "\"");
            Assert.AreEqual(expected, json);

            var bson = c.ToBson();
            var rehydrated = BsonSerializer.DeserializeDocument<C>(bson);
            Assert.IsTrue(bson.SequenceEqual(rehydrated.ToBson()));
        }

        [Test]
        public void TestSerializeA() {
            C c = new C { CE = E.A, FE = E.A };
            var json = c.ToJson();
            var expected = ("{ 'CE' : 1, 'FE' : 'A' }").Replace("'", "\"");
            Assert.AreEqual(expected, json);

            var bson = c.ToBson();
            var rehydrated = BsonSerializer.DeserializeDocument<C>(bson);
            Assert.IsTrue(bson.SequenceEqual(rehydrated.ToBson()));
        }

        [Test]
        public void TestSerializeB() {
            C c = new C { CE = E.B, FE = E.B };
            var json = c.ToJson();
            var expected = ("{ 'CE' : 2, 'FE' : 'B' }").Replace("'", "\"");
            Assert.AreEqual(expected, json);

            var bson = c.ToBson();
            var rehydrated = BsonSerializer.DeserializeDocument<C>(bson);
            Assert.IsTrue(bson.SequenceEqual(rehydrated.ToBson()));
        }

        [Test]
        public void TestSerializeInvalid() {
            C c = new C { CE = (E) 123, FE = (E) 123 };
            var json = c.ToJson();
            var expected = ("{ 'CE' : 123, 'FE' : '123' }").Replace("'", "\"");
            Assert.AreEqual(expected, json);

            var bson = c.ToBson();
            var rehydrated = BsonSerializer.DeserializeDocument<C>(bson);
            Assert.IsTrue(bson.SequenceEqual(rehydrated.ToBson()));
        }
    }
}
