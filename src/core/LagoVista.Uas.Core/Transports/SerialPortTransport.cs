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
using System.Diagnostics;
using LagoVista.Core;

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
        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        public SerialPortTransport(IDispatcherServices dispatcher) : base(dispatcher)
        {

        }

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

        private void StartListening()
        {
            var buffer = new byte[256];
            Task.Run(async () =>
            {
                while (_serialPort != null && _isActive)
                {
                    try
                    {
                        var bytesRead = await _serialPort.ReadAsync(buffer, 0, 256, _cancellationTokenSource.Token);
                        if (bytesRead > 0)
                        {
                            var outputBuffer = buffer.Take(bytesRead);
                            _receiveQueue.Enqueue(outputBuffer.ToArray());
                            _receiveSignal.Set();
                        }
                    }
                    catch (TaskCanceledException) {/* nop */}
                }
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
            byte[] buffer = _mavLinkAsyncWalker.SerializeMessage(msg, SystemId, ComponentId, true);
            await _serialPort.WriteAsync(buffer);
        }

        public override void SendMessage(UasMessage msg)
        {
            MessagesSent++;
            _sendQueue.Enqueue(msg);
            _sendSignal.Set();
        }

        protected override void HandleReceptionEnded(object sender)
        {
            
        }

        public TimeSpan Timeout
        {
            get; set;
        }


        public async Task CloseAsync()
        {
            _isActive = false;
            _cancellationTokenSource.Cancel();
            await _serialPort.CloseAsync();
            _serialPort.Dispose();
            _serialPort = null;

            IsConnected = false;
        }

        public async Task OpenAsync(ISerialPort serialPort)
        {
            _serialPort = serialPort;

            await _serialPort.OpenAsync();

            StartListening();
            ThreadPool.QueueUserWorkItem(new WaitCallback(ProcessReceiveQueue), null);
            ThreadPool.QueueUserWorkItem(new WaitCallback(ProcessSendQueue));

            IsConnected = true;
        }

    }
}
