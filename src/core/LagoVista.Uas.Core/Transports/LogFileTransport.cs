using System.IO;
using LagoVista.Core;
using LagoVista.Uas.Core.Models;
using LagoVista.Uas.Core.Utils;

namespace LagoVista.Uas.Core.MavLink
{
    public class LogFileTransport: Transport
    {
        private string mLogFileName;
        
        public LogFileTransport(string logFileName, IDispatcherServices dispatcher) : base(dispatcher)
        {
            mLogFileName = logFileName;
        }

        public override void Initialize()
        {
            Parse();
        }

        public override void Dispose()
        {
            
        }

        public override void SendMessage(UasMessage msg)
        {
            // No messages are sent on this transport (only read from the logfile)
        }


        // __ Impl ____________________________________________________________


        private void Parse()
        {
            try
            {
                using (FileStream s = new FileStream(mLogFileName, FileMode.Open))
                {
                    using (BinaryReader reader = new BinaryReader(s))
                    {
                        while (true)
                        {
                            SyncStream(reader);
                            var packet = MavLinkPacket.Deserialize(reader, 0);

                            if (packet.IsValid)
                            {
                                HandlePacketReceived(this, packet);
                            }
                        }
                    }
                }
            }
            catch (EndOfStreamException)
            { 
                
            }

            HandleReceptionEnded(this);
        }

        private void SyncStream(BinaryReader s)
        {
            while (s.ReadByte() != MavLinkGenericPacketWalker.PacketSignalByte)
            {
                // Skip bytes until a packet start is found
            }
        }

        protected override void HandlePacketReceived(object sender, MavLinkPacket e)
        {
         
        }

        protected override void HandleReceptionEnded(object sender)
        {
         
        }
    }
}
