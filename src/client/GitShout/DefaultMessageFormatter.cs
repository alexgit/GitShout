using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GitShout
{
    public class DefaultMessageFormatter : IMessageFormatter
    {
        private const string template = "{0} has pushed to {1}. Comment: '{2}'";

        public string Format(CommitMessage commitMessage)
        {
            var commits = from commit in commitMessage.Commits
                          select new
                                     {
                                         Author = commit.Author.Name,
                                         Comment = commit.Message,
                                         URL = commit.Url,
                                         Repository = commitMessage.Repository.Name
                                     };

            var stringbuffer = new StringBuilder();
            foreach (var commit in commits)
            {
                stringbuffer.AppendLine(string.Format(template, commit.Author, commit.Repository, commit.Comment)).AppendLine();                
            }

            return stringbuffer.ToString();
        }
    }
}
