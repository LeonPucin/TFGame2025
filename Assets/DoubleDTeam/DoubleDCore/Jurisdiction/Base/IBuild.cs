using System;

namespace DoubleDCore.Jurisdiction.Base
{
    public interface IBuild : IDisposable
    {
        public void Build();
        public void Run();
    }
}