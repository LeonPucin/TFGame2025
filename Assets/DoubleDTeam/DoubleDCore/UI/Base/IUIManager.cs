using System;
using System.Collections.Generic;

namespace DoubleDCore.UI.Base
{
    public interface IUIManager
    {
        public event Action<IPage> PageOpened;
        public event Action<IPage> PageClosed;

        public IEnumerable<IPage> ActivePages { get; }

        public bool ContainsPage<TPage>() where TPage : class, IPage;
        public bool ContainsPage(IPage page);

        public void RegisterPage<TPage>(TPage page) where TPage : class, IPage;
        public void RegisterPageByType(IPage page);

        public void RemovePage<TPage>() where TPage : class, IPage;
        public void RemovePage(IPage page);

        public void OpenPage<TPage>() where TPage : class, IUIPage;
        public void OpenPage<TPage, TPayload>(TPayload context) where TPage : class, IPayloadPage<TPayload>;

        public void ClosePage<TPage>() where TPage : class, IPage;

        public bool PageIsOpened<TPage>() where TPage : class, IPage;
        public bool PageIsOpened(IPage page);
    }
}