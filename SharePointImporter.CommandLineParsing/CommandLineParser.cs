// Decompiled with JetBrains decompiler
// Type: OrbitOne.SharePoint.Importer.CommandLineParsing.CommandLineParser
// Assembly: OrbitOne.SharePoint.Importer, Version=1.2.2.0, Culture=neutral, PublicKeyToken=null
// MVID: D1FFDC9B-F8CC-4FBB-A43C-FDBC02BB1B73
// Assembly location: C:\Sample1\OrbitOne.SharePoint.Importer.exe

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;

namespace OrbitOne.SharePoint.Importer.CommandLineParsing
{
    public static class CommandLineParser
    {
        static CommandLineParser()
        {
            TypeDescriptor.AddAttributes(typeof(DirectoryInfo), new Attribute[1]
            {
        (Attribute) new TypeConverterAttribute(typeof (DirectoryInfoConverter))
            });
            TypeDescriptor.AddAttributes(typeof(FileInfo), new Attribute[1]
            {
        (Attribute) new TypeConverterAttribute(typeof (FileInfoConverter))
            });
        }

        public static void ParseArguments(this object valueToPopulate, IEnumerable<string> args, char keyCharacter, char valueCharacter)
        {
            CommandLineDictionary commandLineDictionary = CommandLineDictionary.FromArguments(args, keyCharacter, valueCharacter);
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(valueToPopulate);
            foreach (PropertyDescriptor propertyDescriptor in properties)
            {
                if (propertyDescriptor.Attributes.Cast<Attribute>().Any<Attribute>((Func<Attribute, bool>)(attribute => attribute is RequiredAttribute)) && !commandLineDictionary.ContainsKey(propertyDescriptor.Name))
                    throw new ArgumentException("A value for the " + propertyDescriptor.Name + " property is required.");
            }
            foreach (KeyValuePair<string, string> keyValuePair in (Dictionary<string, string>)commandLineDictionary)
            {
                PropertyDescriptor propertyDescriptor1 = (PropertyDescriptor)null;
                foreach (PropertyDescriptor propertyDescriptor2 in properties)
                {
                    if (string.Equals(propertyDescriptor2.Name, keyValuePair.Key, StringComparison.OrdinalIgnoreCase))
                        propertyDescriptor1 = propertyDescriptor2;
                }
                if (propertyDescriptor1 == null)
                    throw new ArgumentException("A matching property of name " + keyValuePair.Key + " on type " + (object)valueToPopulate.GetType() + " could not be found.");
                if (string.IsNullOrEmpty(keyValuePair.Value) && (propertyDescriptor1.PropertyType == typeof(bool) || propertyDescriptor1.PropertyType == typeof(bool?)))
                {
                    propertyDescriptor1.SetValue(valueToPopulate, (object)true);
                }
                else
                {
                    object obj;
                    switch (propertyDescriptor1.PropertyType.Name)
                    {
                        case "IEnumerable`1":
                        case "ICollection`1":
                        case "IList`1":
                        case "List`1":
                            obj = typeof(CommandLineParser).GetMethod("FromCommaSeparatedList", BindingFlags.Static | BindingFlags.NonPublic).MakeGenericMethod(propertyDescriptor1.PropertyType.GetGenericArguments()).Invoke((object)null, new object[1]
                            {
                (object) keyValuePair.Value
                            });
                            break;
                        default:
                            TypeConverter converter = TypeDescriptor.GetConverter(propertyDescriptor1.PropertyType);
                            if (converter == null || !converter.CanConvertFrom(typeof(string)))
                                throw new ArgumentException("Unable to convert from a string to a property of type " + (object)propertyDescriptor1.PropertyType + ".");
                            obj = converter.ConvertFromInvariantString(keyValuePair.Value);
                            break;
                    }
                    propertyDescriptor1.SetValue(valueToPopulate, obj);
                }
            }
        }

        public static void ParseArguments(this object valueToPopulate, IEnumerable<string> args)
        {
            valueToPopulate.ParseArguments(args, '/', '=');
        }

        public static void PrintUsage(object component)
        {
            IEnumerable<PropertyDescriptor> source = TypeDescriptor.GetProperties(component).Cast<PropertyDescriptor>().Except<PropertyDescriptor>(TypeDescriptor.GetProperties(component.GetType().BaseType).Cast<PropertyDescriptor>());
            IEnumerable<string> strings = CommandLineParser.FormatNamesAndDescriptions(source.Select<PropertyDescriptor, string>((Func<PropertyDescriptor, string>)(property => property.Name)), source.Select<PropertyDescriptor, string>((Func<PropertyDescriptor, string>)(property => property.Description)), Console.WindowWidth);
            Console.WriteLine("Possible arguments:");
            foreach (string str in strings)
                Console.WriteLine(str);
        }

        public static void PrintCommands(IEnumerable<Command> commands)
        {
            IEnumerable<string> strings = CommandLineParser.FormatNamesAndDescriptions(commands.Select<Command, string>((Func<Command, string>)(command => command.Name)), commands.Select<Command, string>((Func<Command, string>)(command => command.GetAttribute<DescriptionAttribute>().Description)), Console.WindowWidth);
            Console.WriteLine("Possible commands:");
            foreach (string str in strings)
                Console.WriteLine(str);
        }

        public static string ToString(object valueToConvert)
        {
            IEnumerable<PropertyDescriptor> propertyDescriptors = TypeDescriptor.GetProperties(valueToConvert).Cast<PropertyDescriptor>().Except<PropertyDescriptor>(TypeDescriptor.GetProperties(valueToConvert.GetType().BaseType).Cast<PropertyDescriptor>());
            CommandLineDictionary commandLineDictionary = new CommandLineDictionary();
            foreach (PropertyDescriptor propertyDescriptor in propertyDescriptors)
                commandLineDictionary[propertyDescriptor.Name] = propertyDescriptor.GetValue(valueToConvert).ToString();
            return commandLineDictionary.ToString();
        }

        private static IEnumerable<string> FormatNamesAndDescriptions(IEnumerable<string> names, IEnumerable<string> descriptions, int maxLineLength)
        {
            if (names.Count<string>() != descriptions.Count<string>())
                throw new ArgumentException("Collection sizes are not equal", "names");
            int num = names.Max<string>((Func<string, int>)(commandName => commandName.Length));
            List<string> stringList = new List<string>();
            for (int index = 0; index < names.Count<string>(); ++index)
            {
                string str1 = names.ElementAt<string>(index).PadRight(num + 2);
                foreach (string str2 in CommandLineParser.WordWrap(descriptions.ElementAt<string>(index), maxLineLength - num - 3))
                {
                    string str3 = str1 + str2;
                    stringList.Add(str3);
                    str1 = new string(' ', num + 2);
                }
            }
            return (IEnumerable<string>)stringList;
        }

        private static List<T> FromCommaSeparatedList<T>(this string commaSeparatedList)
        {
            List<T> objList = new List<T>();
            TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));
            if (converter.CanConvertFrom(typeof(string)))
            {
                string str1 = commaSeparatedList;
                char[] chArray = new char[1] { ',' };
                foreach (string str2 in str1.Split(chArray))
                    objList.Add((T)converter.ConvertFromInvariantString(str2.Trim()));
            }
            return objList;
        }

        private static T GetAttribute<T>(this object value) where T : Attribute
        {
            return (T)TypeDescriptor.GetAttributes(value).Cast<Attribute>().First<Attribute>((Func<Attribute, bool>)(attribute => attribute is T));
        }

        private static IEnumerable<string> WordWrap(string text, int maxLineLength)
        {
            List<string> stringList = new List<string>();
            string empty = string.Empty;
            string str1 = text;
            char[] chArray = new char[1] { ' ' };
            foreach (string str2 in str1.Split(chArray))
            {
                if (empty.Length + str2.Length > maxLineLength)
                {
                    stringList.Add(empty);
                    empty = string.Empty;
                }
                empty += str2;
                if (empty.Length != maxLineLength)
                    empty += " ";
            }
            if (empty.Trim() != string.Empty)
                stringList.Add(empty);
            return (IEnumerable<string>)stringList;
        }
    }
}
