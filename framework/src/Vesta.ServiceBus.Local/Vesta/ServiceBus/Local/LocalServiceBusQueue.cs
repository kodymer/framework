using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vesta.ServiceBus.Local
{
    public class LocalServiceBusQueue
    {
        private ConcurrentQueue<LocalServiceBusMessage> _queue;

        public EventHandler<MessageReceivedEventArgs> MessageReceived;
        public EventHandler<ChangedEventArgs> Changed;
        public EventHandler<MessageSentEventArgs> MessageSent;

        public LocalServiceBusQueue()
        {
            _queue = new ConcurrentQueue<LocalServiceBusMessage>();
        }

        public void Enqueue(LocalServiceBusMessage message)
        {
            int oldTotalMessajeNumber = _queue.Count();

            _queue.Enqueue(message);
            OnMessageReceived(message);

            int currentTotalMessajeNumber = _queue.Count();
            OnChanged(oldTotalMessajeNumber, currentTotalMessajeNumber);

        }

        public bool TryDequeue(out LocalServiceBusMessage message)
        {
            int oldTotalMessajeNumber = _queue.Count();

            if (_queue.TryDequeue(out message))
            {
                OnMessageSent(message);

                int currentTotalMessajeNumber = _queue.Count();
                OnChanged(oldTotalMessajeNumber, currentTotalMessajeNumber);

                return true;
            }

            return false;
        }

        private void OnMessageReceived(LocalServiceBusMessage message)
        {
            MessageReceived?.Invoke(this, new MessageReceivedEventArgs(message));
        }

        private void OnMessageSent(LocalServiceBusMessage message)
        {
            MessageSent?.Invoke(this, new MessageSentEventArgs(message));
        }

        private void OnChanged(int oldTotalMessajeNumber, int currentTotalMessajeNumber)
        {
            Changed?.Invoke(this, new ChangedEventArgs(oldTotalMessajeNumber, currentTotalMessajeNumber));
        }
    }
}
