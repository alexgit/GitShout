using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GitShout
{
    /// <summary>
    /// Takes in a <see cref="CommitObjects"/> and returns a human readable message describing the commit message.
    /// </summary>
    public interface IMessageFormatter
    {
        string Format(CommitMessage commitMessage);
    }
}
