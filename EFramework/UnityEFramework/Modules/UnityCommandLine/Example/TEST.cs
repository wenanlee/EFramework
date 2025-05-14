using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class TEST : MonoBehaviour
{
    private void Start()
    {
        Main();
    }
    void Main()
        {
            // BindingFlags.InvokeMethod
            // Call a static method.
            Type t = typeof(TestClass);

        //Debug.Log("\n");
        //Debug.Log("Invoking a static method.");
        //Debug.Log("-------------------------");
        //t.InvokeMember("SayHello", BindingFlags.InvokeMethod | BindingFlags.Public |
        //    BindingFlags.Static, null, null, new object[] { });

        //// BindingFlags.InvokeMethod
        //// Call an instance method.
        //TestClass c = new TestClass();
        //Debug.Log("\n");
        //Debug.Log("Invoking an instance method.");
        //Debug.Log("----------------------------");
        //c.GetType().InvokeMember("AddUp", BindingFlags.InvokeMethod, null, c, new object[] { });
        //c.GetType().InvokeMember("AddUp", BindingFlags.InvokeMethod, null, c, new object[] { });

        //// BindingFlags.InvokeMethod
        //// Call a method with parameters.
        //object[] args = new object[] { 100.09, 184.45 };
        //object result;
        //Debug.Log("\n");
        //Debug.Log("Invoking a method with parameters.");
        //Debug.Log("---------------------------------");
        //result = t.InvokeMember("ComputeSum", BindingFlags.InvokeMethod, null, null, args);
        //Debug.Log($"{args[0]} + {args[1]} = {result}");

        //// BindingFlags.GetField, SetField
        //Debug.Log("\n");
        //Debug.Log("Invoking a field (getting and setting.)");
        //Debug.Log("--------------------------------------");
        //// Get a field value.
        //result = t.InvokeMember("Name", BindingFlags.GetField, null, c, new object[] { });
        //Debug.Log($"Name == {result}");
        //// Set a field.
        //t.InvokeMember("Name", BindingFlags.SetField, null, c, new object[] { "NewName" });
        //result = t.InvokeMember("Name", BindingFlags.GetField, null, c, new object[] { });
        //Debug.Log($"Name == {result}");

        //Debug.Log("\n");
        //Debug.Log("Invoking an indexed property (getting and setting.)");
        //Debug.Log("--------------------------------------------------");
        //// BindingFlags.GetProperty
        //// Get an indexed property value.
        //int index = 3;
        //result = t.InvokeMember("Item", BindingFlags.GetProperty, null, c, new object[] { index });
        //Debug.Log($"Item[{index}] == {result}");
        //// BindingFlags.SetProperty
        //// Set an indexed property value.
        //index = 3;
        //t.InvokeMember("Item", BindingFlags.SetProperty, null, c, new object[] { index, "NewValue" });
        //result = t.InvokeMember("Item", BindingFlags.GetProperty, null, c, new object[] { index });
        //Debug.Log("Item[{0}] == {1}", index, result);

        //Debug.Log("\n");
        //Debug.Log("Getting a field or property.");
        //Debug.Log("----------------------------");
        //// BindingFlags.GetField
        //// Get a field or property.
        //result = t.InvokeMember("Name", BindingFlags.GetField | BindingFlags.GetProperty, null, c,
        //    new object[] { });
        //Debug.Log("Name == {0}", result);
        //// BindingFlags.GetProperty
        //result = t.InvokeMember("Value", BindingFlags.GetField | BindingFlags.GetProperty, null, c,
        //    new object[] { });
        //Debug.Log("Value == {0}", result);

        //Debug.Log("\n");
        //Debug.Log("Invoking a method with named parameters.");
        //Debug.Log("---------------------------------------");
        //// BindingFlags.InvokeMethod
        //// Call a method using named parameters.
        //object[] argValues = new object[] { "Mouse", "Micky" };
        //String[] argNames = new String[] { "lastName", "firstName" };
        //t.InvokeMember("PrintName", BindingFlags.InvokeMethod, null, null, argValues, null, null,
        //    argNames);

        //Debug.Log("\n");
        //Debug.Log("Invoking a default member of a type.");
        //Debug.Log("------------------------------------");
        //// BindingFlags.Default
        //// Call the default member of a type.
        //Type t3 = typeof(TestClass2);
        //t3.InvokeMember("", BindingFlags.InvokeMethod | BindingFlags.Default, null, new TestClass2(),
        //    new object[] { });

        //// BindingFlags.Static, NonPublic, and Public
        //// Invoking a member with ref parameters.
        //Debug.Log("\n");
        //Debug.Log("Invoking a method with ref parameters.");
        //Debug.Log("--------------------------------------");
        //MethodInfo m = t.GetMethod("Swap");
        //args = new object[2];
        //args[0] = 1;
        //args[1] = 2;
        //m.Invoke(new TestClass(), args);
        //Debug.Log("{0}, {1}", args[0], args[1]);

        //// BindingFlags.CreateInstance
        //// Creating an instance with a parameterless constructor.
        //Debug.Log("\n");
        //Debug.Log("Creating an instance with a parameterless constructor.");
        //Debug.Log("------------------------------------------------------");
        //object cobj = t.InvokeMember("TestClass", BindingFlags.Public |
        //    BindingFlags.Instance | BindingFlags.CreateInstance,
        //    null, null, new object[] { });
        //Debug.Log($"Instance of {cobj.GetType().Name} created.");

        //// Creating an instance with a constructor that has parameters.
        //Debug.Log("\n");
        //Debug.Log("Creating an instance with a constructor that has parameters.");
        //Debug.Log("------------------------------------------------------------");
        //cobj = t.InvokeMember("TestClass", BindingFlags.Public |
        //    BindingFlags.Instance | BindingFlags.CreateInstance,
        //    null, null, new object[] { "Hello, World!" });
        //Debug.Log("Instance of {0} created with initial value '{1}'.", cobj.GetType().Name,
        //    cobj.GetType().InvokeMember("Name", BindingFlags.GetField, null, cobj, null));

        // BindingFlags.DeclaredOnly


        //--------------------------------
        //Debug.Log("\n");
        //Debug.Log("DeclaredOnly instance members.");
        //Debug.Log("------------------------------");
        //System.Reflection.MemberInfo[] memInfo =
        //    t.GetMembers(BindingFlags.DeclaredOnly | BindingFlags.Instance |
        //    BindingFlags.Public);
        //for (int i = 0; i < memInfo.Length; i++)
        //{
        //    Debug.Log(memInfo[i].Name);
        //}

        //// BindingFlags.IgnoreCase
        //Debug.Log("\n");
        //Debug.Log("Using IgnoreCase and invoking the PrintName method.");
        //Debug.Log("---------------------------------------------------");
        //t.InvokeMember("printname", BindingFlags.IgnoreCase | BindingFlags.Static |
        //    BindingFlags.Public | BindingFlags.InvokeMethod, null, null, new object[]
        //    {"Brad","Smith"});

        //// BindingFlags.FlattenHierarchy
        //Debug.Log("\n");
        //Debug.Log("Using FlattenHierarchy to get inherited static protected and public members.");
        //Debug.Log("----------------------------------------------------------------------------");
        //FieldInfo[] finfos = typeof(MostDerived).GetFields(BindingFlags.NonPublic | BindingFlags.Public |
        //      BindingFlags.Static | BindingFlags.FlattenHierarchy);
        //foreach (FieldInfo finfo in finfos)
        //{
        //    Debug.Log($"{finfo.Name} defined in {finfo.DeclaringType.Name}.");
        //}

        //Debug.Log("\n");
        //Debug.Log("Without FlattenHierarchy.");
        //Debug.Log("-------------------------");
        //finfos = typeof(MostDerived).GetFields(BindingFlags.NonPublic | BindingFlags.Public |
        //      BindingFlags.Static);
        //foreach (FieldInfo finfo in finfos)
        //{
        //    Debug.Log($"{finfo.Name} defined in {finfo.DeclaringType.Name}.");
        //}
    }
    

    public class TestClass
    {
        public String Name;
        private System.Object[] values = new System.Object[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

        public System.Object this[int index]
        {
            get
            {
                return values[index];
            }
            set
            {
                values[index] = value;
            }
        }

        public System.Object Value
        {
            get
            {
                return "the value";
            }
        }

        public TestClass() : this("initialName") { }
        public TestClass(string initName)
        {
            Name = initName;
        }

        int methodCalled = 0;

        public static void SayHello()
        {
            Debug.Log("Hello");
        }

        public void AddUp()
        {
            methodCalled++;
            Debug.Log($"AddUp Called {methodCalled} times");
        }

        public static double ComputeSum(double d1, double d2)
        {
            return d1 + d2;
        }

        public static void PrintName(String firstName, String lastName)
        {
            Debug.Log( lastName+"   "+firstName);
        }

        public void PrintTime()
        {
            Debug.Log(DateTime.Now);
        }

        public void Swap(ref int a, ref int b)
        {
            int x = a;
            a = b;
            b = x;
        }
    }

    [DefaultMemberAttribute("PrintTime")]
    public class TestClass2
    {
        public void PrintTime()
        {
            Debug.Log(DateTime.Now);
        }
    }

    public class Base
    {
        protected static int BaseOnly = 0;
    }
    public class Derived : Base
    {
        public static int DerivedOnly = 0;
    }
    public class MostDerived : Derived { }
}
