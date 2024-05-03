using System;

namespace Microsoft.ClearScript
{
    internal class Windows
    {
        internal class V8
        {
            internal class V8ScriptEngine
            {
                public object Script { get; internal set; }

                public string output;

                internal void AddHostType(string v, Type type)
                {
                    throw new NotImplementedException();
                }

                internal void Dispose()
                {
                    throw new NotImplementedException();
                }

                internal void Execute(string v)
                {
                    throw new NotImplementedException();
                }
            }
        }
    }
}