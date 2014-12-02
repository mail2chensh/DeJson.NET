// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
//      Mono Runtime Version: 4.0.30319.1
// 
//      Changes to this file may cause incorrect behavior and will be lost if 
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using DeJson;

namespace DeJsonTest
{

    [TestFixture()]
    public class Test
    {
        // ----------------------------------------------
        // Classes for basic example
        public class Bar {
            public int z = 0;
            public int a = 0;
        }
        public class Foo {
            public int x = 0;
            public int y = 0;
            public Bar[] g = null;
            public int[][] b = null;
        }

        public class Moo {
            public string a;
            public int[] b;
            public Bar c;
        };

        // Classes for SomeUndefinedExample
        public class Something {
            public string a;
            public string b;
        }

        // ----------------------------------------------
        // Classes for Dervied example #1

        public class Fruit {
            public int fruitType; // 0 = Apple, 1 = Raspberry
        };

        public class Apple : Fruit {
            public float height;
            public float radius;
        };

        public class Raspberry : Fruit {
            public int numBulbs;
        }

        public class FruitCreator : Deserializer.CustomCreator {

            // Called when the Deserializer realizes it needs a Fruit (abstract)
            // so you get a chance to create a concrete instance of a Fruit (Apple, Raspberry)
            public override object Create(Dictionary<string, object> src,
                                          Dictionary<string, object> parentSrc) {

                // read the fruitType field
                int fruitType = Convert.ToInt32(src["fruitType"]);
                if (fruitType == 0) {
                    return new Apple();
                } else if (fruitType == 1) {
                    return new Raspberry();
                }
                return null;
            }

            // Tell the Deserializer the base type we create.
            public override System.Type TypeToCreate() {
                return typeof(Fruit);
            }
        }

        // ----------------------------------------------
        // Classes for Derived example #2

        public class Message {
            public string msgType;
            public MessageData data;
        }

        public class MessageData {
        }

        public class MessageDataMouseMove : MessageData {
            public int x;
            public int y;
        }

        public class MessageDataKeyDown : MessageData {
            public int keyCode;
        }

        public class MessageDataCreator : Deserializer.CustomCreator {
            // Called when the Deserializer realizes it needs a
            // MessageData (abstract) so you get a chance to create
            // a concrete instance of a MessageData (MessageDataMouseMove,
            // MessageDataKeyDown)
            public override object Create(Dictionary<string, object> src,
                                          Dictionary<string, object> parentSrc) {

                // read the msgType from Message
                // parentSrc is the fields from Message
                // src is the field from MessageData
                string msgType = Convert.ToString(parentSrc["msgType"]);
                if (msgType.Equals("mouseMove")) {
                    return new MessageDataMouseMove();
                } else if (msgType.Equals("keyDown")) {
                    return new MessageDataKeyDown();
                }
                return null;
            }

            // Tell the Deserializer the base type we create.
            public override System.Type TypeToCreate() {
                return typeof(MessageData);
            }
        }

        // ----------------------------------------------
        // Classes to test auto type serialization

        public class Animal {
        }

        public class Dog : Animal {
            public Dog() { }  // must have no param ctor for deseralization
            public Dog(int _barkiness) {
                barkiness = _barkiness;
            }
            public int barkiness;
        }

        public class Cat : Animal {
            public Cat() { }  // must have no param ctor for deseralization
            public Cat(string _stealthiness) {
                stealthiness = _stealthiness;
            }
            public string stealthiness;
        }

        // ----------------------------------------------
        // Classes for more advanced example.
        //
        // This is basically the same as Derived sample #2 except with
        // a few fancier classes to make adding new MessageCmdData derived
        // classes really easy.

        public class MessageCmdData {
        };

        public class MessageCmd {
            public string cmd;
            public MessageCmdData data;
        };

        public class MessageToClient {
            public string cmd;  // command 'server', 'update'
            public int id;      // id of client
            public MessageCmd data;
        };

        [CmdName("setColor")]
        public class MessageSetColor : MessageCmdData {
            public string color;
            public string style;
        };

        [CmdName("setName")]
        public class MessageSetName : MessageCmdData {
            public string name;
        };

        [CmdName("launch")]
        public class MessageLaunch : MessageCmdData {
        };

        [CmdName("die")]
        public class MessageDie : MessageCmdData {
            public string killer;
            public bool crash;
        };

        [AttributeUsage(AttributeTargets.Class)]
        public class CmdNameAttribute : System.Attribute
        {
            public readonly string CmdName;

            public CmdNameAttribute(string cmdName)
            {
                this.CmdName = cmdName;
            }
        }

        public class MessageCmdDataCreator : Deserializer.CustomCreator {

            public abstract class Creator {
                public abstract object Create();
            }

            public class TypedCreator<T> : Creator where T : new()  {
                public override object Create() {
                    return new T();
                }
            }

            public MessageCmdDataCreator() {
                m_creators = new Dictionary<string, Creator>();
            }

            public void RegisterCreator<T>() where T : new() {
                Type type = typeof(T);
                string name = null;

                //Querying Class Attributes
                foreach (Attribute attr in type.GetCustomAttributes(true)) {
                    CmdNameAttribute cmdNameAttr = attr as CmdNameAttribute;
                    if (cmdNameAttr != null) {
                        name = cmdNameAttr.CmdName;
                    }
                }

                if (name == null) {
                    System.InvalidOperationException ex = new System.InvalidOperationException("missing CmdNameAttribute");
                    throw ex;
                }

                m_creators[name] = new TypedCreator<T>();
            }

            public override object Create(Dictionary<string, object> src, Dictionary<string, object> parentSrc) {
                string typeName = (string)parentSrc["cmd"];
                Creator creator;
                if (m_creators.TryGetValue(typeName, out creator)) {
                    return creator.Create();
                }
                return null;
            }

            public override System.Type TypeToCreate() {
                return typeof(MessageCmdData);
            }

            Dictionary<string, Creator> m_creators;
        };

        public void CheckFoo(Foo f) {
            Assert.AreEqual(f.x, 123);
            Assert.AreEqual(f.y, 456);
            Assert.AreEqual(f.g[0].z, 5);
            Assert.AreEqual(f.g[0].a, 12);
            Assert.AreEqual(f.g[1].z, 4);
            Assert.AreEqual(f.g[1].a, 23);
            Assert.AreEqual(f.g[2].z, 3);
            Assert.AreEqual(f.g[2].a, 34);
            Assert.AreEqual(f.b.Length, 3);
            Assert.AreEqual(f.b[0].Length, 2);
            Assert.AreEqual(f.b[0][0], 1);
            Assert.AreEqual(f.b[0][1], 2);
            Assert.AreEqual(f.b[1].Length, 3);
            Assert.AreEqual(f.b[1][0], 4);
            Assert.AreEqual(f.b[1][1], 5);
            Assert.AreEqual(f.b[1][2], 6);
            Assert.AreEqual(f.b[2].Length, 2);
            Assert.AreEqual(f.b[2][0], 7);
            Assert.AreEqual(f.b[2][1], 6);
        }


        [Test()]
        public void Basic ()
        {
            Deserializer deserializer = new Deserializer();
            string json = "{\"x\":123,\"y\":456,\"g\":[{\"z\":5,\"a\":12},{\"z\":4,\"a\":23},{\"z\":3,\"a\":34}],\"b\":[[1,2],[4,5,6],[7,6]]}";

            Foo f = deserializer.Deserialize<Foo>(json);
            CheckFoo(f);

            string s = Serializer.Serialize(f);
            Foo f2 = deserializer.Deserialize<Foo>(s);

            CheckFoo(f2);
        }

        public void CheckArray(int[] k)
        {
            Assert.AreEqual(k.Length, 3);
            Assert.AreEqual(k[0], 4);
            Assert.AreEqual(k[1], 7);
            Assert.AreEqual(k[2], 9);
        }

        [Test()]
        public void ArrayTest()
        {
            Deserializer deserializer = new Deserializer();

            string kj = "[4,7,9]";

            int[] k = deserializer.Deserialize<int[]>(kj);

            CheckArray(k);

            string s = Serializer.Serialize(k);
            int[] k2 = deserializer.Deserialize<int[]>(s);

            CheckArray(k2);
        }

        [Test()]
        public void UndefinedTest()
        {
            Deserializer deserializer = new Deserializer();
            string u1 = "{}";

            Moo m = deserializer.Deserialize<Moo>(u1);
            Assert.AreEqual(m.b, null);
            Assert.AreEqual(m.a, null);
            Assert.AreEqual(m.c, null);

            string s = Serializer.Serialize(m);
            Assert.AreEqual(s, "{}");
        }

        [Test()]
        public void SomeUndefinedTest()
        {
            Deserializer deserializer = new Deserializer();
            string u1 = "{\"a\":\"b\"}";

            Something s = deserializer.Deserialize<Something>(u1);
            Assert.AreEqual(s.a, "b");
            Assert.AreEqual(s.b, null);

            string ss = Serializer.Serialize (s);

            Assert.AreEqual(ss, u1);

            Something s2 = deserializer.Deserialize<Something>(ss);

            Assert.AreEqual(s2.a, "b");
            Assert.AreEqual(s2.b, null);
        }

        public void CheckApple(Apple a)
        {
            Assert.AreEqual(a.fruitType, 0);
            Assert.AreEqual(a.height, 1.2f);
            Assert.AreEqual(a.radius, 7);
        }

        public void CheckRaspberry(Raspberry r)
        {
            Assert.AreEqual(r.fruitType, 1);
            Assert.AreEqual(r.numBulbs, 27);
        }

        [Test()]
        public void DerviedTest01()
        {
            Deserializer deserializer = new Deserializer();
            deserializer.RegisterCreator(new FruitCreator());

            string appleJson = "{\"fruitType\":0,\"height\":1.2,\"radius\":7}";
            string raspberryJson = "{\"fruitType\":1,\"numBulbs\":27}";

            Fruit apple = deserializer.Deserialize<Fruit>(appleJson);
            Fruit raspberry = deserializer.Deserialize<Fruit>(raspberryJson);

            CheckApple((Apple)apple);
            CheckRaspberry((Raspberry)raspberry);

            string sa = Serializer.Serialize(apple);
            string sr = Serializer.Serialize(raspberry);

            Fruit apple2 = deserializer.Deserialize<Fruit>(sa);
            Fruit raspberry2 = deserializer.Deserialize<Fruit>(sr);

            CheckApple((Apple)apple2);
            CheckRaspberry((Raspberry)raspberry2);
        }

        public void CheckMessageDataMouseMove(Message m)
        {
            Assert.AreEqual(m.msgType, "mouseMove");
            Assert.AreEqual(((MessageDataMouseMove)m.data).x, 123);
            Assert.AreEqual(((MessageDataMouseMove)m.data).y, 456);
        }

        public void CheckMessageDataKeyDown(Message m)
        {
            Assert.AreEqual(m.msgType, "keyDown");
            Assert.AreEqual(((MessageDataKeyDown)m.data).keyCode, 789);
        }

        [Test()]
        public void DerviedTest02()
        {
            Deserializer deserializer = new Deserializer();

            deserializer.RegisterCreator(new MessageDataCreator());

            string mouseMoveJson = "{\"msgType\":\"mouseMove\",\"data\":{\"x\":123,\"y\":456}}";
            string keyDownJson = "{\"msgType\":\"keyDown\",\"data\":{\"keyCode\":789}}";

            Message msg1 = deserializer.Deserialize<Message>(mouseMoveJson);
            Message msg2 = deserializer.Deserialize<Message>(keyDownJson);

            CheckMessageDataMouseMove(msg1);
            CheckMessageDataKeyDown(msg2);

            string s1 = Serializer.Serialize(msg1);
            string s2 = Serializer.Serialize(msg2);

            Message msg12 = deserializer.Deserialize<Message>(s1);
            Message msg22 = deserializer.Deserialize<Message>(s2);

            CheckMessageDataMouseMove(msg12);
            CheckMessageDataKeyDown(msg22);
        }

        [Test()]
        public void AutoSerializationTest()
        {
            Dog da = new Dog(123);
            Cat ca = new Cat("super");
            Animal[] animals = new Animal[2];
            animals[0] = da;
            animals[1] = ca;

            Deserializer deserializer = new Deserializer();

            string animalsJson = Serializer.Serialize(animals, true);
            Animal[] ani = deserializer.Deserialize<Animal[]>(animalsJson);

            Assert.AreEqual(((Dog)ani[0]).barkiness, 123);
            Assert.AreEqual(((Cat)ani[1]).stealthiness, "super");
        }

        public void CheckMessageSetColor(MessageToClient m)
        {
            Assert.AreEqual(m.cmd, "update");
            Assert.AreEqual(m.id, 123);
            Assert.AreEqual(((MessageSetColor)m.data.data).color, "red");
            Assert.AreEqual(((MessageSetColor)m.data.data).style, "bold");
        }

        public void CheckMessageSetName(MessageToClient m)
        {
            Assert.AreEqual(m.cmd, "update");
            Assert.AreEqual(m.id, 345);
            Assert.AreEqual(((MessageSetName)m.data.data).name, "gregg");
        }

        public void CheckMessageLaunch(MessageToClient m)
        {
            Assert.AreEqual(m.cmd, "update");
            Assert.AreEqual(m.id, 789);
        }

        public void CheckMessageDie(MessageToClient m)
        {
            Assert.AreEqual(m.cmd, "update");
            Assert.AreEqual(m.id, 101112);
            Assert.AreEqual(((MessageDie)m.data.data).killer, "jill");
            Assert.AreEqual(((MessageDie)m.data.data).crash, true);
        }

        [Test()]
        public void MoreAdvancedTest()
        {
            Deserializer deserializer = new Deserializer();

            MessageCmdDataCreator mcdc = new MessageCmdDataCreator();

            mcdc.RegisterCreator<MessageSetColor>();
            mcdc.RegisterCreator<MessageSetName>();
            mcdc.RegisterCreator<MessageLaunch>();
            mcdc.RegisterCreator<MessageDie>();

            deserializer.RegisterCreator(mcdc);

            string ja = "{\"cmd\":\"update\",\"id\":123,\"data\":{\"cmd\":\"setColor\",\"data\":{\"color\":\"red\",\"style\":\"bold\"}}}";
            string jb = "{\"cmd\":\"update\",\"id\":345,\"data\":{\"cmd\":\"setName\",\"data\":{\"name\":\"gregg\"}}}";
            string jc = "{\"cmd\":\"update\",\"id\":789,\"data\":{\"cmd\":\"launch\",\"data\":{}}}";
            string jd = "{\"cmd\":\"update\",\"id\":101112,\"data\":{\"cmd\":\"die\",\"data\":{\"killer\":\"jill\",\"crash\":true}}}";

            MessageToClient ma = deserializer.Deserialize<MessageToClient>(ja);
            MessageToClient mb = deserializer.Deserialize<MessageToClient>(jb);
            MessageToClient mc = deserializer.Deserialize<MessageToClient>(jc);
            MessageToClient md = deserializer.Deserialize<MessageToClient>(jd);

            CheckMessageSetColor(ma);
            CheckMessageSetName(mb);
            CheckMessageLaunch(mc);
            CheckMessageDie(md);

            string sa = Serializer.Serialize(ma);
            string sb = Serializer.Serialize(mb);
            string sc = Serializer.Serialize(mc);
            string sd = Serializer.Serialize(md);

            MessageToClient ma2 = deserializer.Deserialize<MessageToClient>(sa);
            MessageToClient mb2 = deserializer.Deserialize<MessageToClient>(sb);
            MessageToClient mc2 = deserializer.Deserialize<MessageToClient>(sc);
            MessageToClient md2 = deserializer.Deserialize<MessageToClient>(sd);

            CheckMessageSetColor(ma2);
            CheckMessageSetName(mb2);
            CheckMessageLaunch(mc2);
            CheckMessageDie(md2);
        }

        bool DictionariesAreSame(Dictionary<string, object> a, Dictionary<string, object> b)
        {
            foreach (string key in a.Keys) {
                object valueb;
                if (!b.TryGetValue(key, out valueb)) {
                    Console.Error.WriteLine(String.Format("b missing key: {0}", key));
                    return false;
                }
                object valuea = a[key];
                System.Type aType = valuea.GetType();
                if (aType != valueb.GetType()) {
                    Console.Error.WriteLine(String.Format("not same type for key: {0}", key));
                    return false;
                }

                if (aType.IsValueType) {
                    if (!valuea.Equals(valueb)) {
                        Console.Error.WriteLine(String.Format("{0} != {1} for key: {2}", valuea.ToString(), valueb.ToString(), key));
                        return false;
                    }
                } else if (aType == typeof(string)) {
                    if (valuea.ToString () != valueb.ToString ())
                    {
                        Console.Error.WriteLine(String.Format("{0} != {1} for key: {2}", valuea.ToString(), valueb.ToString(), key));
                        return false;
                    }
                } else {
                    // Need to handle none dictionaries
                    bool same = DictionariesAreSame((Dictionary<string,object>)valuea, (Dictionary<string,object>)valueb);
                    if (!same) {
                        return false;
                    }
                }
            }

            return true;
        }

        [Test()]
        public void RoundTrip01Test()
        {
            Deserializer deserializer = new Deserializer();

            string j = "{\"cmd\":\"update\",\"id\":123,\"data\":{\"cmd\":\"setColor\",\"data\":{\"color\":\"red\",\"style\":\"bold\"}}}";

            Dictionary<string, object>data1 = deserializer.Deserialize<Dictionary<string, object> >(j);

            string newJ = Serializer.Serialize(data1);

            Dictionary<string, object>data2 = deserializer.Deserialize<Dictionary<string, object> >(newJ);

            Assert.IsTrue(DictionariesAreSame(data1, data2));

            string anotherJ = Serializer.Serialize(data2);
            Assert.AreEqual(newJ, anotherJ);    //  this assumes given the same data we'll get the same string with fields in the same order
        }

        struct Vector2 {
            public float x;
            public float y;
        };

        [Test()]
        public void StructTest01()
        {
            Deserializer deserializer = new Deserializer();

            string j = "{\"x\":1.2,\"y\":3.4}";

            Vector2 v = deserializer.Deserialize<Vector2>(j);
            Assert.AreEqual(v.x, 1.2f);
            Assert.AreEqual(v.y, 3.4f);

            string newJ = Serializer.Serialize(v);

            Vector2 v2 = deserializer.Deserialize<Vector2>(newJ);

            Assert.AreEqual(v2.x, v.x);
            Assert.AreEqual(v2.y, v.y);
        }

        [Test()]
        public void StructTest02()
        {
            Deserializer deserializer = new Deserializer();

            string j = "[{\"x\":1.2,\"y\":3.4},{\"x\":5.6,\"y\":7.8}]";

            Vector2[] v = deserializer.Deserialize<Vector2[]>(j);
            Assert.AreEqual(v.Length, 2);
            Assert.AreEqual(v[0].x, 1.2f);
            Assert.AreEqual(v[0].y, 3.4f);
            Assert.AreEqual(v[1].x, 5.6f);
            Assert.AreEqual(v[1].y, 7.8f);

            string newJ = Serializer.Serialize(v);

            Vector2[] v2 = deserializer.Deserialize<Vector2[]>(newJ);

            Assert.AreEqual(v2.Length, 2);
            Assert.AreEqual(v2[0].x, v[0].x);
            Assert.AreEqual(v2[0].y, v[0].y);
            Assert.AreEqual(v2[1].x, v[1].x);
            Assert.AreEqual(v2[1].y, v[1].y);
        }

        struct HasStatic {
            public int someProp;
            public static int someStaticProp;
        }

        [Test()]
        public void StructWithStaticTest()
        {
            HasStatic s = new HasStatic();
            s.someProp = 123;
            HasStatic.someStaticProp = 456;

            string json = Serializer.Serialize(s);
            Assert.That(json, Is.Not.StringContaining("someStaticProp"));

            string j = "{\"someProp\":123,\"someStaticProp\":789}";
            Deserializer deserializer = new Deserializer();
            HasStatic s2 = deserializer.Deserialize<HasStatic>(j);
            Assert.AreEqual(s2.someProp, 123);
            Assert.AreEqual(HasStatic.someStaticProp, 456);
        }

        class Primitives {
            public Boolean someBoolean = true;
            public Byte someByte = 1;
            public SByte someSByte = -1;
            public Int16 someInt16 = -2;
            public UInt16 someUInt16 = 2;
            public Int32 someInt32 = -3;
            public UInt32 someUInt32 = 3;
            public Int64 someInt64 = -4;
            public UInt64 someUInt64 = 4;
            public Char someChar = 'a';
            public Double someDouble = 1.23;
            public Single someSingle = 2.34f;
            public float somefloat = 3.45f;
            public int someint = 5;
            public bool somebool = true;
        };

        [Test()]
        public void PrimitivesTest()
        {
            Primitives p = new Primitives();
            p.someBoolean = false;
            p.someByte = 6;
            p.someSByte = -6;
            p.someInt16 = -7;
            p.someUInt16 = 7;
            p.someInt32 = -8;
            p.someUInt32 = 8;
            p.someInt64 = -9;
            p.someUInt64 = 9;
            p.someChar = 'b';
            p.someDouble = 5.6;
            p.someSingle = 6.7f;
            p.somefloat = 7.8f;
            p.someint = 12;
            p.somebool = false;

            string json = Serializer.Serialize(p);
            Deserializer deserializer = new Deserializer();
            Primitives p2 = deserializer.Deserialize<Primitives>(json);

            Assert.AreEqual(p.someBoolean, p2.someBoolean);
            Assert.AreEqual(p.someByte, p2.someByte);
            Assert.AreEqual(p.someSByte, p2.someSByte);
            Assert.AreEqual(p.someInt16, p2.someInt16);
            Assert.AreEqual(p.someUInt16, p2.someUInt16);
            Assert.AreEqual(p.someInt32, p2.someInt32);
            Assert.AreEqual(p.someUInt32, p2.someUInt32);
            Assert.AreEqual(p.someInt64, p2.someInt64);
            Assert.AreEqual(p.someUInt64, p2.someUInt64);
            Assert.AreEqual(p.someChar, p2.someChar);
            Assert.AreEqual(p.someDouble, p2.someDouble);
            Assert.AreEqual(p.someSingle, p2.someSingle);
            Assert.AreEqual(p.somefloat, p2.somefloat);
            Assert.AreEqual(p.someint, p2.someint);
            Assert.AreEqual(p.somebool, p2.somebool);
        }
    }
}

