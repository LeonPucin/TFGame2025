using System;

namespace Game.Source.Storage
{
    public class Receiver<TObject> : IReceiver<TObject> where TObject : class
    {
        public event Action<TObject> OnPut;
        public event Action<TObject> OnTake;

        private TObject Object { get; set; }

        public void Put(TObject obj)
        {
            if (Object != null)
                Take();

            Object = obj;
            OnPut?.Invoke(obj);
        }

        public TObject Take()
        {
            var temp = Object;
            Object = null;
            OnTake?.Invoke(temp);
            return temp;
        }

        public void TransferFrom(Receiver<TObject> receiver)
        {
            var temp = receiver.Take();
            Put(temp);
        }
    }
}