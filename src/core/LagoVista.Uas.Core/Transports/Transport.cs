using LagoVista.Core.ServiceCommon;
using LagoVista.Core.Validation;
using LagoVista.Uas.Core.Models;
using System;
using System.Linq;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using LagoVista.Core;

namespace LagoVista.Uas.Core.MavLink
{
    public abstract class Transport : ServiceBase, IDisposable, ITransport
    {
        private class WaitOnRequest
        {
            public WaitOnRequest(UasMessages msgId)
            {
                CompletionSource = new TaskCompletionSource<MavLinkPacket>();
                MsgId = msgId;
                Details = new List<string>();
            }

            public List<String> Details { get; private set; }

            public UasMessages MsgId { get; private set; }

            public DateTime Enqueued { get; private set; }

            public TaskCompletionSource<MavLinkPacket> CompletionSource { get; private set; }
        }

        IDispatcherServices _dispatcher;

        public Transport(IDispatcherServices dispatcher)
        {
            _dispatcher = dispatcher;
        }

        private ConcurrentDictionary<UasMessages, WaitOnRequest> Sessions { get; } = new ConcurrentDictionary<UasMessages, WaitOnRequest>();

        public byte MavlinkSystemId = 0xFF;
        public byte MavlinkComponentId = 0xBE;
        public MavLinkState UavState = new MavLinkState();

        public event EventHandler OnReceptionEnded;
        public event EventHandler<MavLinkPacket> OnPacketReceived;

        public abstract void Initialize();
        public abstract void Dispose();
        public abstract void SendMessage(UasMessage msg);

        protected virtual void HandlePacketReceived(object sender, MavLinkPacket e)
        {
            _dispatcher.Invoke(() =>
            {
                if(e.MessageId == UasMessages.Statustext)
                {
                    StatusMessages.Insert(0, DateTime.Now.ToString() + " - " +  System.Text.ASCIIEncoding.ASCII.GetString(e.Payload));
                }

                var msg = Messages.Where(m => m.MessageId == e.MessageId).FirstOrDefault();
                if (msg != null)
                {
                    msg.ReceiptCount++;
                }
                else
                {
                    e.ReceiptCount++;
                    Messages.Add(e);
                }

                OnPacketReceived?.Invoke(this, e);
                Complete(e);
            });
        }

        protected abstract void HandleReceptionEnded(object sender);

        protected void Complete(MavLinkPacket msg)
        {
            if (Sessions.TryGetValue(msg.MessageId, out var tcs))
            {
                if (!tcs.CompletionSource.Task.IsCompleted)
                {
                    tcs.CompletionSource.SetResult(msg);
                }
            }
        }

        public async Task<InvokeResult<TMavlinkPacket>> WaitForMessageAsync<TMavlinkPacket>(UasMessages messageId, TimeSpan timeout) where TMavlinkPacket : class
        {
            try
            {
                var wor = new WaitOnRequest(messageId);
                Sessions[messageId] = wor;

                MavLinkPacket message = null;

                for (var idx = 0; (idx < timeout.TotalMilliseconds / 100) && message == null; ++idx)
                {
                    if (wor.CompletionSource.Task.IsCompleted)
                    {
                        message = wor.CompletionSource.Task.Result;
                    }
                    await Task.Delay(100);
                }

                if (message == null)
                {
                    return InvokeResult<TMavlinkPacket>.FromError("Timeout waiting for message.");
                }
                else
                {
                    if (message.Message is TMavlinkPacket)
                    {
                        return InvokeResult<TMavlinkPacket>.Create(message.Message as TMavlinkPacket);
                    }
                    else
                    {
                        return InvokeResult<TMavlinkPacket>.FromError("Invalid type.");
                    }
                }
            }
            catch (Exception ex)
            {
                return InvokeResult<TMavlinkPacket>.FromException("AsyncCoupler_WaitOnAsync", ex);
            }
            finally
            {
                Sessions.TryRemove(messageId, out WaitOnRequest obj);
            }
        }

        public Task<InvokeResult<TMavlinkPacket>> RequestDataAsync<TMavlinkPacket>(UasMessage msg, UasMessages incomingMessageId, TimeSpan timeout) where TMavlinkPacket : class
        {
            SendMessage(msg);
            return WaitForMessageAsync<TMavlinkPacket>(incomingMessageId, timeout);
        }

        public Task<InvokeResult<TMavlinkPacket>> RequestDataAsync<TMavlinkPacket>(UasMessage msg, UasMessages incomingMessageId) where TMavlinkPacket : class
        {
            SendMessage(msg);
            return WaitForMessageAsync<TMavlinkPacket>(incomingMessageId, TimeSpan.FromSeconds(2));
        }

        public ObservableCollection<MavLinkPacket> Messages { get; } = new ObservableCollection<MavLinkPacket>();

        public ObservableCollection<String> StatusMessages { get; } = new ObservableCollection<String>();


        private int _messagesReceived = 0;
        public int MessagesReceived
        {
            get { return _messagesReceived; }
            private set { Set(ref _messagesReceived, value); }
        }

        private int _errors;
        public int Errors
        {
            get { return _errors; }
            private set { Set(ref _errors, value); }
        }

        private long _bytesReceived = 0;
        public long BytesReceived
        {
            get { return _bytesReceived; }
            private set { Set(ref _bytesReceived, value); }
        }
    }
}
