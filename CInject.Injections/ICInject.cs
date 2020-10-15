
using CInject.Injections.Library;
using System;
namespace CInject.Injections
{
    public interface ICInject : IDisposable
    {
        void OnInvoke(CInjection injection);
        void OnComplete();
    }
}