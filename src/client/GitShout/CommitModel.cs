using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GitShout
{
    public class CommitMessage
    {
        public string Before { get; set; }
        public string After { get; set; }
        public string Ref { get; set; }
        public Repository Repository { get; set; }
        public Commit[] Commits { get; set; }                
    }

    public class Repository
    {
        public string Url { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Watchers { get; set; }
        public int Forks { get; set; }
        public bool Private { get; set; }
        public Committer Owner { get; set; }
    }

    public class Commit
    {
        public string Id { get; set; }
        public string Url { get; set; }
        public Committer Author { get; set; }
        public string Message { get; set; }
        public string TimeStamp { get; set; } // maybe this should be an actual timestamp
        public string[] Added { get; set; }
        public string[] Modified { get; set; }
        public string[] Removed { get; set; }
    }

    public class Committer
    {
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
