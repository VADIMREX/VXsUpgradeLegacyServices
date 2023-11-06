using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Linq;
using LegacyServices.Models;

namespace LegacyServices
{
    [StructLayout(LayoutKind.Explicit)]
    struct UIntDouble
    {
        [FieldOffset(0)]
        public uint i;
        [FieldOffset(0)]
        public double d;
    }

    unsafe struct UIntT<T> where T : unmanaged
    {
        byte[] a;
        UIntT(byte[] value)
        {
            a = value;
        }
        public UIntT(T value)
        {
            a = new byte[sizeof(T)];
            fixed (byte* b = &a[0])
            {
                T* t = (T*)b;
                *t = value;
            }
        }
        public UIntT(UIntT<T> value)
        {
            a = new byte[sizeof(T)];
            value.a.CopyTo(a, 0);
        }

        public static UIntT<T> Empty => new UIntT<T>(new byte[sizeof(T)]);

        public T Value
        {
            get
            {
                fixed (byte* b = &a[0])
                {
                    T* t = (T*)b;
                    T r = *t;
                    return r;
                }
            }
            set
            {
                a = new byte[sizeof(T)];
                fixed (byte* b = &a[0])
                {
                    T* t = (T*)b;
                    *t = value;
                }
            }
        }

        public static UIntT<T> operator <<(UIntT<T> self, int offset)
        {
            var b = Empty;
            var uoffset = offset / 8;
            var loffset = offset % 8;
            for (var i = uoffset; i < sizeof(T); i++)
                b.a[i] = (byte)(self.a[i - uoffset] << loffset);
            return b;
        }

        public static UIntT<T> operator |(UIntT<T> self, byte offset)
        {
            var b = new UIntT<T>(self);
            b.a[0] |= offset;
            return b;
        }

        public string ToString(string type)
        {
            if ("X" == type) return a.Aggregate("", (x, y) => $"{x}{y.ToString("X")}");
            return ToString();
        }

        public override string ToString()
        {
            return a.Aggregate("", (x, y) => $"{x}({y.ToString("X")})");
        }
    }


    public class ServiceLogic
    {
        static TField GetField<TImpl, TField>(TImpl model, string field)
        {
            return (TField)typeof(TImpl).GetField(field)
                                        .GetValue(model);
        }

        static void SetField<TImpl, TField>(TImpl model, string field, TField value)
        {
            typeof(TImpl).GetField(field)
                         .SetValue(model, value);
        }

        static bool convStart<T>(byte[] bdata, out T[] tdata, int size)
        {
            if (null == bdata)
            {
                tdata = null;
                return true;
            }
            if (0 == bdata.Length)
            {
                tdata = new T[0];
                return true;
            }
            tdata = new T[(int)Math.Ceiling((double)bdata.Length / size)];
            return false;
        }

        static unsafe T[] AByteToA<T>(byte[] data) where T : unmanaged
        {
            T[] res;
            if (convStart(data, out res, sizeof(T))) return res;
            UIntT<T> buff = UIntT<T>.Empty;
            var i = 0;
            for (; i < data.Length; i++)
            {
                buff = buff << 8 | data[i];
                if (0 == (i + 1) % sizeof(T))
                {
                    res[i / sizeof(T)] = buff.Value;
                    buff = UIntT<T>.Empty;
                }
            }
            if (0 < i % sizeof(T)) res[i / sizeof(T)] = buff.Value;
            return res;
        }


        public static TImpl DoSomething<TImpl>(TImpl data) where TImpl : ISampleModel, new()
        {
            if (null == data) return new TImpl();
            if (null != GetField<TImpl, decimal[]>(data, "NotProperty") && GetField<TImpl, decimal[]>(data, "NotProperty").Length > 0) data.HidenDataTwo = GetField<TImpl, decimal[]>(data, "NotProperty")[0];
            if (DateTime.MinValue == data.SomeDate) data.SomeDate = DateTime.Now;
            data.Message = new StringBuilder().AppendLine(data.IsTruth ? "not fake" : "fake")
                                              .Append(" 1. - ")
                                              .AppendLine(data.HidenDataTwo.ToString())
                                              .Append(" 2. - ")
                                              .AppendLine(null == data.SubContractOne ? "(T_T)" : "(￣﹃￣)")
                                              .Append(" 3. - ")
                                              .AppendLine(null == data.SubContractTwo ? "😑" : "😌")
                                              .Append(" 4. - ")
                                              .AppendLine(null == data.CustomData ? "-" : "+")
                                              .Append(" 5. - ")
                                              .AppendLine(data.Message?.Substring(0, data.Message.Length < 20 ? data.Message.Length : 20) ?? "Х")
                                              .ToString();
            SetField(data, "HidenDataOne", data.Message);
            data.BinaryData = Encoding.UTF8.GetBytes(GetField<TImpl, string>(data, "HidenDataOne"));
            data.IntArray = AByteToA<int>(data.BinaryData);
            data.DoubleArray = AByteToA<double>(data.BinaryData);
            data.DecimalArray = AByteToA<decimal>(data.BinaryData);
            data.StringPairs = data.Message.Split('\n').ToDictionary(x => x, x => x.Length.ToString("X"));
            data.ListOfSomething = data.IntArray.Select(x => x.ToString("X")).ToList();
            if (null == data.SubContractOne) data.SubContractOne = new BaseContract();
            if (null == data.SubContractTwo) data.SubContractTwo = new CustomContract();
            if (null == data.CustomData) data.CustomData = new CustomType();
            data.KeyValues1 = new Dictionary<string, BaseContract>() {
                {"key-one", data.SubContractOne }
            };
            data.KeyValues2 = new Dictionary<string, CustomContract>() {
                {"key-two", data.SubContractTwo }
            };
            data.KeyValues3 = new Dictionary<string, CustomType>() {
                {"key-three", data.CustomData}
            };
            return data;
        }
    }
}