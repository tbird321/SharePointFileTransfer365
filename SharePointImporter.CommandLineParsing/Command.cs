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
