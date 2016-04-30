using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Anonet.Core
{
    static class BinarySerializer
    {
        public static byte[] Serialize(object instance)
        {
            var byteArray = new List<byte>();

            byteArray.Add((byte)BinarySerializerMarker.ObjectMarker);

            var type = instance.GetType();

            if (typeof(ICollection).IsAssignableFrom(type))
            {
                var index = 0;
                foreach (var element in (instance as ICollection))
                {
                    if (element == null) { continue; }

                    InsertProperty(byteArray, index.ToString(), element);
                }
            }
            else
            {
                foreach (var propertyInfo in type.GetProperties().Where(x => x.CanRead && x.CanWrite && x.GetCustomAttributes(typeof(BinarySerializableAttribute), false).Length > 0))
                {
                    var attr = propertyInfo.GetCustomAttributes(typeof(BinarySerializableAttribute), false)[0] as BinarySerializableAttribute;
                    if (attr.NonSerialized)
                    {
                        continue;
                    }

                    var propertyName = string.IsNullOrEmpty(attr.PropertyName) ? propertyInfo.Name : attr.PropertyName; 
                    var propertyValue = propertyInfo.GetValue(instance, null);

                    InsertProperty(byteArray, propertyName, propertyValue);
                }
            }

            byteArray.Add((byte)BinarySerializerMarker.ObjectMarker);

            return byteArray.ToArray();
        }

        private static void InsertProperty(List<byte> byteArray, string propertyName, object propertyValue)
        {
            byteArray.Add((byte)BinarySerializerMarker.PropertyNameMarker);
            byteArray.Add((byte)(propertyName.Length));
            byteArray.AddRange(Encoding.UTF8.GetBytes(propertyName.ToString()));

            var valueByteArray = BinarySerializerConverter.ConvertTo(propertyValue);
            if (valueByteArray != null)
            {
                if (valueByteArray.Length < (byte)BinarySerializerMarker.PropertyValueMarker)
                {
                    byteArray.Add((byte)valueByteArray.Length);
                    byteArray.AddRange(valueByteArray);
                }
                else if (valueByteArray.Length < BinarySerializerConstants.PropertyValueMaxLength)
                {
                    byteArray.Add((byte)BinarySerializerMarker.PropertyValueMarker);
                    byteArray.AddRange(new byte[] { (byte)(valueByteArray.Length >> 8), (byte)valueByteArray.Length });
                    byteArray.AddRange(valueByteArray);
                }
                else
                {
                    throw new Exception();
                }
            }
            else
            {
                byteArray.AddRange(Serialize(propertyValue));
            }
        }

        public static object Deserialize(byte[] byteArray, Type targetType)
        {
            var offset = 0;
            return Deserialize(byteArray, ref offset, targetType);
        }

        public static T Deserialize<T>(byte[] byteArray)
        {
            var offset = 0;
            return (T)Deserialize(byteArray, ref offset, typeof(T));
        }

        private static object Deserialize(byte[] byteArray, ref int offset, Type targetType)
        {
            var instance = Activator.CreateInstance(targetType);

            while (offset < byteArray.Length)
            {
                if (byteArray[offset] == (byte)BinarySerializerMarker.ObjectMarker)
                {
                    break;
                }
            }

            if (byteArray[offset++] != (byte)BinarySerializerMarker.ObjectMarker)
            {
                throw new Exception();
            }

            while (offset < byteArray.Length)
            {
                if (byteArray[offset] == (byte)BinarySerializerMarker.PropertyNameMarker)
                {
                    offset++;
                    var propertyNameLength = byteArray[offset++];
                    var propertyName = Encoding.UTF8.GetString(byteArray, offset, propertyNameLength);
                    offset += propertyNameLength;

                    if (typeof(ICollection).IsAssignableFrom(targetType))
                    {
                        var propertyType = targetType.GetGenericArguments().FirstOrDefault() ?? targetType.GetElementType();

                        if (byteArray[offset] == (byte)BinarySerializerMarker.ObjectMarker)
                        {
                            (instance as IList).Add(Deserialize(byteArray, ref offset, propertyType));
                        }
                        else if (byteArray[offset] == (byte)BinarySerializerMarker.PropertyValueMarker)
                        {
                            offset++;
                            var propertyValueLength = (byteArray[offset] << 8) + byteArray[offset + 1];
                            offset += 2;

                            (instance as IList).Add(BinarySerializerConverter.ConvertFrom(byteArray, offset, propertyValueLength, propertyType));

                            offset += propertyValueLength;
                        }
                        else if (byteArray[offset] < (byte)BinarySerializerMarker.PropertyValueMarker)
                        {
                            var propertyValueLength = byteArray[offset++];

                            (instance as IList).Add(BinarySerializerConverter.ConvertFrom(byteArray, offset, propertyValueLength, propertyType));

                            offset += propertyValueLength;
                        }
                        else
                        {
                            throw new Exception();
                        }
                    }
                    else
                    {
                        var propertyInfo = targetType.GetProperties().SingleOrDefault(x => x.CanRead && x.CanWrite
                            && ((x.GetCustomAttributes(typeof(BinarySerializableAttribute), false).Length > 0 && (x.GetCustomAttributes(typeof(BinarySerializableAttribute), false)[0] as BinarySerializableAttribute).PropertyName.Equals(propertyName))
                            || (x.Name.Equals(propertyName))));

                        if (byteArray[offset] == (byte)BinarySerializerMarker.ObjectMarker)
                        {
                            propertyInfo.SetValue(instance, Deserialize(byteArray, ref offset, propertyInfo.PropertyType), null);
                        }
                        else if (byteArray[offset] == (byte)BinarySerializerMarker.PropertyValueMarker)
                        {
                            offset++;
                            var propertyValueLength = (byteArray[offset] << 8) + byteArray[offset + 1];
                            offset += 2;

                            propertyInfo.SetValue(instance, BinarySerializerConverter.ConvertFrom(byteArray, offset, propertyValueLength, propertyInfo.PropertyType), null);

                            offset += propertyValueLength;
                        }
                        else if (byteArray[offset] < (byte)BinarySerializerMarker.PropertyValueMarker)
                        {
                            var propertyValueLength = byteArray[offset++];

                            propertyInfo.SetValue(instance, BinarySerializerConverter.ConvertFrom(byteArray, offset, propertyValueLength, propertyInfo.PropertyType), null);

                            offset += propertyValueLength;
                        }
                        else
                        {
                            throw new Exception();
                        }
                    }
                }
                else if (byteArray[offset] == (byte)BinarySerializerMarker.ObjectMarker)
                {
                    offset++;
                    return instance;
                }
                else
                {
                    throw new Exception();
                }
            }

            return instance;
        }
    }
}
