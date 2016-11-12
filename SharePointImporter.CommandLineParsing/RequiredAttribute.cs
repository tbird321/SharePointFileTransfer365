using System;

namespace SharepointFileTransfer.SharePoint.Importer.CommandLineParsing
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class RequiredAttribute : Attribute
    {
    }
}
