/*
The MIT License (MIT)

Copyright (c) 2014, Håkon K. Olafsen

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*/
using System;
using System.Threading;
using System.Collections.Concurrent;
using LagoVista.Core.PlatformSupport;
using System.Threading.Tasks;
using System.Linq;
using LagoVista.Uas.Core.Models;
using LagoVista.Uas.Core.Utils;
using LagoVista.Core.Models;

namespace LagoVista.Uas.Core.MavLink
{
    public class SerialPortTransport : Transport
    {
        public int HeartBeatUpdateRateMs = 1000;

        private ConcurrentQueue<byte[]> _receiveQueue = new ConcurrentQueue<byte[]>();
        private ConcurrentQueue<UasMessage> _sendQueue = new ConcurrentQueue<UasMessage>();
        private AutoResetEvent _receiveSignal = new AutoResetEvent(true);
        private AutoResetEvent _sendSignal = new AutoResetEvent(true);
        private MavLinkAsyncWalker _mavLinkAsyncWalker = new MavLinkAsyncWalker();
        private ISerialPort _serialPort;
        private bool _isActive = true;


        public override void Initialize()
        {
            InitializeMavLink();
        }

        public override void Dispose()
        {
            _isActive = false;
            _serialPort.Dispose();
            _serialPort = null;
            _receiveSignal.Set();
            _sendSignal.Set();
        }

        private void InitializeMavLink()
        {
            _mavLinkAsyncWalker.PacketReceived += HandlePacketReceived;
        }

        public void Open(ISerialPort serialPort)
        {
            _serialPort = serialPort;

            ThreadPool.QueueUserWorkItem(new WaitCallback(ProcessReceiveQueue), null);
            ThreadPool.QueueUserWorkItem(new WaitCallback(ProcessSendQueue));
        }

        private void StartListening()
        {
            var buffer = new byte[1024];
            Task.Run(async () =>
            {
                var bytesRead = await _serialPort.ReadAsync(buffer, 0, 1024);
                var outputBuffer = buffer.Take(bytesRead);
                _receiveQueue.Enqueue(outputBuffer.ToArray());
                _receiveSignal.Set();
            });
        }

        private void ProcessReceiveQueue(object state)
        {
            while (true)
            {
                byte[] buffer;

                if (_receiveQueue.TryDequeue(out buffer))
                {
                    _mavLinkAsyncWalker.ProcessReceivedBytes(buffer, 0, buffer.Length);
                }
                else
                {
                    _receiveSignal.WaitOne();

                    if (!_isActive) break;
                }
            }

            HandleReceptionEnded(this);
        }

        private async void ProcessSendQueue(object state)
        {
            while (true)
            {
                UasMessage msg;

                if (_sendQueue.TryDequeue(out msg))
                {
                    await SendMavlinkMessage(msg);
                }
                else
                {
                    _sendSignal.WaitOne();

                    if (!_isActive) break;
                }
            }
        }

        private async Task SendMavlinkMessage(UasMessage msg)
        {
            byte[] buffer = _mavLinkAsyncWalker.SerializeMessage(msg, MavlinkSystemId, MavlinkComponentId, true);

            await _serialPort.WriteAsync(buffer);
        }

        public void BeginHeartBeatLoop()
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(HeartBeatLoop), null);
        }

        private void HeartBeatLoop(object state)
        {
            while (true)
            {
                foreach (UasMessage m in UavState.GetHeartBeatObjects())
                {
                    SendMessage(m);
                }

                Thread.Sleep(HeartBeatUpdateRateMs);
            }
        }

        public override void SendMessage(UasMessage msg)
        {
            _sendQueue.Enqueue(msg);
            _sendSignal.Set();
        }

        protected override void HandlePacketReceived(object sender, MavLinkPacket e)
        {
            throw new NotImplementedException();
        }

        protected override void HandleReceptionEnded(object sender)
        {
            throw new NotImplementedException();
        }

        public TimeSpan Timeout
        {
            get; set;
        }


        public async Task CloseAsync()
        {
            await _serialPort.CloseAsync();
            _serialPort.Dispose();
            _serialPort = null;

            IsConected = false;
        }

        public async Task OpenAsync(ISerialPort serialPort)
        {
            _serialPort = serialPort;

            await _serialPort.OpenAsync();

            StartListening();

            IsConected = true;
        }

    }
}
