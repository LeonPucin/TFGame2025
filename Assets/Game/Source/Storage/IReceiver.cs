﻿namespace Game.Source.Storage
{
    public interface IReceiver<TObject>
    {
        public void Put(TObject obj);

        public TObject Take();

        public TObject Peek();

        public void TransferFrom(IReceiver<TObject> receiver);
    }
}