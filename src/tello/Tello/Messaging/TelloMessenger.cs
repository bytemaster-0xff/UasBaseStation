using Messenger;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Tello.Messaging
{
    public class TelloMessenger : Messenger<string>
    {
        private readonly ConcurrentQueue<Command> _commands = new ConcurrentQueue<Command>();
        private readonly ITransceiver _transceiver;

        public TelloMessenger(ITransceiver transceiver)
        {
            _transceiver = transceiver ?? throw new ArgumentNullException(nameof(transceiver));
            ProcessCommandQueue();
        }

        private void Enqueue(Command command)
        {
            _commands.Enqueue(command);
        }

        public Task<TelloResponse> SendAsync(Commands command, params object[] args)
        {
            return SendAsync(new Command(command, args));
        }

        public async Task<TelloResponse> SendAsync(Command command)
        {
            if (command.Immediate)
            {
                return new TelloResponse(await _transceiver.SendAsync(new TelloRequest(command)));
            }
            else
            {
                Enqueue(command);
                return await Task.FromResult<TelloResponse>(null);
            }
        }

        private async void ProcessCommandQueue()
        {
            await Task.Run(async () =>
            {
                var spinWait = new SpinWait();
                while (true)
                {
                    try
                    {
                        if (!_commands.IsEmpty && _commands.TryDequeue(out var command))
                        {                            
                            var request = new TelloRequest(command);
                            var response = new TelloResponse(await _transceiver.SendAsync(request));

                            Debug.WriteLine($"{nameof(ProcessCommandQueue)}: len: {_commands.Count}, msg: '{request.Message}', timeout {request.Timeout}, success '{response.Success}', resp msg '{response.Message}'");

                            if (command.Rule.Response != Responses.None)
                            {
                                ReponseReceived(response);
                            }
                        }
                        else
                        {
                            spinWait.SpinOnce();
                        }
                    }
                    catch (Exception ex)
                    {
                        ExceptionThrown(ex);
                    }
                }
            });
        }
    }
}
