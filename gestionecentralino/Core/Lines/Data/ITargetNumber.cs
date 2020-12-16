using System;
using LanguageExt;

namespace gestionecentralino.Core.Lines.Data
{
    public interface ITargetNumber
    {
        string Value { get; }
        ICallLine CreateCall(CallData callData);
    }
}