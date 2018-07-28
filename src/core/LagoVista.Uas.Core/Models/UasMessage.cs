using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LagoVista.Uas.Core.Models
{
    public class UasMessage
    {
        public byte CrcExtra
        {
            get;
            protected set;
        }

        public byte MessageId
        {
            get { return mMessageId; }
        }


        public UasMessageMetadata GetMetadata()
        {
            if (mMetadata == null)
            {
                InitMetadata();
            }

            return mMetadata;
        }


        protected void NotifyUpdated()
        {

        }

        internal virtual void SerializeBody(BinaryWriter s)
        {

        }

        internal virtual void DeserializeBody(BinaryReader stream)
        {

        }

        protected virtual void InitMetadata()
        {

        }


        protected byte mMessageId;
        protected UasMessageMetadata mMetadata;
    }


    public class UasMessageMetadata
    {
        public string Description;
        public List<UasFieldMetadata> Fields = new List<UasFieldMetadata>();
    }


    public class UasFieldMetadata
    {
        public string Name;
        public string Description;
        public int NumElements = 1;
        public UasEnumMetadata EnumMetadata;
    }


    public class UasEnumMetadata
    {
        public string Name;
        public string Description;
        public List<UasEnumEntryMetadata> Entries = new List<UasEnumEntryMetadata>();
    }


    public class UasEnumEntryMetadata
    {
        public int Value;
        public string Name;
        public string Description;
        public List<string> Params;
    }
}
