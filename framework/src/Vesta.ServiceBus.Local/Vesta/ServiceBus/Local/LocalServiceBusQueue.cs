using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vesta.ServiceBus.Local
{
    public class LocalServiceBusQueue : IDisposable
    {
        private ConcurrentQueue<LocalServiceBusMessage> _queue;

        public EventHandler<MessageReceivedEventArgs> MessageReceived;
        public EventHandler<ChangedEventArgs> Changed;
        public EventHandler<MessageSentEventArgs> MessageSent;

        private bool _isDisposed;

        public LocalServiceBusQueue()
        {
            _queue = new ConcurrentQueue<LocalServiceBusMessage>();
        }

        public virtual void Enqueue(LocalServiceBusMessage message)
        {
            int oldTotalMessajeNumber = _queue.Count();

            _queue.Enqueue(message);
            OnMessageReceived(message);

            int currentTotalMessajeNumber = _queue.Count();
            OnChanged(oldTotalMessajeNumber, currentTotalMessajeNumber);

        }

        public virtual bool TryDequeue(out LocalServiceBusMessage message)
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

        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {

                MessageReceived = null;
                Changed= null;
                MessageSent = null;

                _queue.Clear();

                _isDisposed = true;
            }
        }

        public void Dispose()
        {
            // No cambie este código. Coloque el código de limpieza en el método "Dispose(bool disposing)".
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
