// Decompiled with JetBrains decompiler
// Type: OrbitOne.SharePoint.Importer.CommandLineParsing.CommandLineDictionary
// Assembly: OrbitOne.SharePoint.Importer, Version=1.2.2.0, Culture=neutral, PublicKeyToken=null
// MVID: D1FFDC9B-F8CC-4FBB-A43C-FDBC02BB1B73
// Assembly location: C:\Sample1\OrbitOne.SharePoint.Importer.exe

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.Serialization;

namespace OrbitOne.SharePoint.Importer.CommandLineParsing
{
    [Serializable]
    public class CommandLineDictionary : Dictionary<string, string>
    {
        private char KeyCharacter { get; set; }

        private char ValueCharacter { get; set; }

        public CommandLineDictionary()
        {
            this.KeyCharacter = '/';
            this.ValueCharacter = '=';
        }

        protected CommandLineDictionary(SerializationInfo info, StreamingContext context)
          : base(info, context)
        {
        }

        public static CommandLineDictionary FromArguments(IEnumerable<string> arguments)
        {
            return CommandLineDictionary.FromArguments(arguments, '/', '=');
        }

        public static CommandLineDictionary FromArguments(IEnumerable<string> arguments, char keyCharacter, char valueCharacter)
        {
            CommandLineDictionary commandLineDictionary = new CommandLineDictionary();
            commandLineDictionary.KeyCharacter = keyCharacter;
            commandLineDictionary.ValueCharacter = valueCharacter;
            foreach (string str in arguments)
                commandLineDictionary.AddArgument(str);
            return commandLineDictionary;
        }

        public override string ToString()
        {
            string empty = string.Empty;
            foreach (KeyValuePair<string, string> keyValuePair in (Dictionary<string, string>)this)
            {
                if (!string.IsNullOrEmpty(keyValuePair.Value))
                    empty += string.Format((IFormatProvider)CultureInfo.InvariantCulture, "{0}{1}{2}{3} ", (object)this.KeyCharacter, (object)keyValuePair.Key, (object)this.ValueCharacter, (object)keyValuePair.Value);
                else
                    empty += string.Format((IFormatProvider)CultureInfo.InvariantCulture, "{0}{1} ", new object[2]
                    {
            (object) this.KeyCharacter,
            (object) keyValuePair.Key
                    });
            }
            return empty.TrimEnd();
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

        private void AddArgument(string argument)
        {
            if (argument == null)
                throw new ArgumentNullException("argument");
            if (!argument.StartsWith(this.KeyCharacter.ToString(), StringComparison.OrdinalIgnoreCase))
                throw new ArgumentException("Unsupported  value line argument format.", argument);
            string[] strArray = argument.Substring(1).Split(new char[1]
            {
        this.ValueCharacter
            }, 2);
            this.Add(strArray[0], strArray.Length <= 1 ? string.Empty : string.Join("=", strArray, 1, strArray.Length - 1));
        }
    }
}
