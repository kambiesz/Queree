using System;
using System.Collections.Generic;
using System.Text;

namespace Queree.Commands
{
    [Serializable]
    public class DependencyNotRegisteredException : Exception
    {
        public DependencyNotRegisteredException(string message) : base(message)
        {

        }
    }
}
