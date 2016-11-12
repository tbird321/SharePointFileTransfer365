// Decompiled with JetBrains decompiler
// Type: SharepointFileTransfer.SharePoint.Importer.Domain.ValidationResult
// Assembly: SharepointFileTransfer.SharePoint.Importer, Version=1.2.2.0, Culture=neutral, PublicKeyToken=null
// MVID: D1FFDC9B-F8CC-4FBB-A43C-FDBC02BB1B73
// Assembly location: C:\Sample1\SharepointFileTransfer.SharePoint.Importer.exe

using System;
using System.Collections.Generic;
using System.Linq;

namespace SharepointFileTransfer.SharePoint.Importer.Domain
{
    public class ValidationResult
    {
        private IList<ValidationMessage> m_messages;

        public string Source { get; set; }

        public IList<string> Errors
        {
            get
            {
                return (IList<string>)this.m_messages.Where<ValidationMessage>((Func<ValidationMessage, bool>)(m => m.MessageType == MessageType.Error)).Select<ValidationMessage, string>((Func<ValidationMessage, string>)(m => m.Message)).ToList<string>();
            }
        }

        public IList<string> Warnings
        {
            get
            {
                return (IList<string>)this.m_messages.Where<ValidationMessage>((Func<ValidationMessage, bool>)(m => m.MessageType == MessageType.Warning)).Select<ValidationMessage, string>((Func<ValidationMessage, string>)(m => m.Message)).ToList<string>();
            }
        }

        public bool IsValid
        {
            get
            {
                return this.m_messages.Count<ValidationMessage>((Func<ValidationMessage, bool>)(m => m.MessageType == MessageType.Error)) == 0;
            }
        }

        public ValidationResult()
        {
            this.m_messages = (IList<ValidationMessage>)new List<ValidationMessage>();
        }

        public void AddError(string message)
        {
            this.AddMessage(message, MessageType.Error);
        }

        public void AddWarning(string message)
        {
            this.AddMessage(message, MessageType.Warning);
        }

        private void AddMessage(string message, MessageType messageType)
        {
            this.m_messages.Add(new ValidationMessage()
            {
                Message = message,
                MessageType = messageType
            });
        }
    }
}
