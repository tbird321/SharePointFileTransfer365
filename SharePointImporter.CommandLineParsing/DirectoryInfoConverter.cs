using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;

namespace SharepointFileTransfer.SharePoint.Importer.CommandLineParsing
{
    public class DirectoryInfoConverter : TypeConverter
    {
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string && value != null)
                return (object)new DirectoryInfo((string)value);
            return (object)null;
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string);
        }
    }
}
