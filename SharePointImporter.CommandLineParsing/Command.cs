// Decompiled with JetBrains decompiler
// Type: SharepointFileTransfer.SharePoint.Importer.CommandLineParsing.Command
// Assembly: SharepointFileTransfer.SharePoint.Importer, Version=1.2.2.0, Culture=neutral, PublicKeyToken=null
// MVID: D1FFDC9B-F8CC-4FBB-A43C-FDBC02BB1B73
// Assembly location: C:\Sample1\SharepointFileTransfer.SharePoint.Importer.exe

using System;

namespace SharepointFileTransfer.SharePoint.Importer.CommandLineParsing
{
    public abstract class Command
    {
        public virtual string Name
        {
            get
            {
                string name = this.GetType().Name;
                if (name.Contains("Command"))
                    return name.Remove(name.LastIndexOf("Command", StringComparison.Ordinal));
                return name;
            }
        }

        public abstract void Execute();
    }
}
