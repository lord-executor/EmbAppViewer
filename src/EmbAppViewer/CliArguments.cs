using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmbAppViewer
{
    public class CliArguments
    {
        private static char[] ParamSeparator = new[] { '=' };

        private readonly IDictionary<string, string> _args;

        private CliArguments(IDictionary<string, string> args)
        {
            _args = args;
        }

        private string SafeGet(string name)
        {
            return _args.ContainsKey(name) ? _args[name] : null;
        }

        public string Instance => SafeGet("instance");
        public string Name => SafeGet("name");
        public string Cmd => SafeGet("cmd");
        public string Args => SafeGet("args");

        public static CliArguments Parse(IList<string> args)
        {
            var dict = new Dictionary<string, string>();
            var stack = new Stack<string>(args.Reverse());
            while (stack.Count > 0)
            {
                var name = stack.Pop();
                string value;

                if (!name.StartsWith("--"))
                {
                    throw new Exception($"Expected argument starting with '--' but got {name}");
                }

                name = name.Substring(2);
                if (name.Contains("="))
                {
                    var parts = name.Split(ParamSeparator, 2);
                    name = parts[0];
                    value = parts[1];
                }
                else
                {
                    if (stack.Count == 0)
                    {
                        throw new Exception($"Expected parameter value for '{name}' but no more arguments found");
                    }
                    value = stack.Pop();
                }

                dict[name] = value;
            }

            return new CliArguments(dict);
        }
    }
}
