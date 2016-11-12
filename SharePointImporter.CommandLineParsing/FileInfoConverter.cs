// Decompiled with JetBrains decompiler
// Type: SharepointFileTransfer.SharePoint.Importer.CommandLineParsing.FileInfoConverter
// Assembly: SharepointFileTransfer.SharePoint.Importer, Version=1.2.2.0, Culture=neutral, PublicKeyToken=null
// MVID: D1FFDC9B-F8CC-4FBB-A43C-FDBC02BB1B73
// Assembly location: C:\Sample1\SharepointFileTransfer.SharePoint.Importer.exe

using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;

namespace SharepointFileTransfer.SharePoint.Importer.CommandLineParsing
{
    public class FileInfoConverter : TypeConverter
    {
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string && value != null)
                return (object)new FileInfo((string)value);
            return (object)null;
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string);
        }
    }
}
