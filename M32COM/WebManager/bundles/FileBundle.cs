using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;

namespace WebManager
{
    public class FileBundle
    {
        public static readonly string Key = typeof(FileBundle).AssemblyQualifiedName;

        private readonly Dictionary<string, IList<string>> _headScritpFiles;
        private readonly Dictionary<string, IList<string>> _footScritpFiles;
        private readonly Dictionary<string, IList<string>> _cssFiles;

        public FileBundle()
        {
            _headScritpFiles = new Dictionary<string, IList<string>>();
            _footScritpFiles = new Dictionary<string, IList<string>>();
            _cssFiles = new Dictionary<string, IList<string>>();
        }

        public IHtmlString ScriptsRender(bool bHead = true)
        {
            var scriptFiles = bHead ? _headScritpFiles : _footScritpFiles;
            var paths = new List<string>();
            foreach (var key in scriptFiles.Keys)
            {
                var path = GetvirtualPath(key, scriptFiles[key]);
                if (BundleTable.Bundles.GetBundleFor(path) == null)
                    BundleTable.Bundles.Add(new ScriptBundle(path).Include(scriptFiles[key].Distinct().ToArray()));

                paths.Add(path);
            }

            return Scripts.Render(paths.ToArray());
        }

        public IHtmlString StylesRender()
        {
            var paths = new List<string>();
            foreach (var key in _cssFiles.Keys)
            {
                var path = GetvirtualPath(key, _cssFiles[key]);
                if (BundleTable.Bundles.GetBundleFor(path) == null)
                    BundleTable.Bundles.Add(new StyleBundle(path).Include(_cssFiles[key].Distinct().ToArray()));

                paths.Add(path);
            }

            return Styles.Render(paths.ToArray());
        }

        /// <summary>
        /// add css file to head
        /// </summary>
        /// <param name="key">A virtual path for the bundle.</param>
        /// <param name="parts">The virtual path of the file or file pattern to be included in the bundle.</param>
        /// <returns></returns>
        public FileBundle AppendCss(string key, params string[] parts)
        {
            Append(_cssFiles, key, parts);
            return this;
        }

        public FileBundle AppendScript(bool bHead, string key, params string[] parts)
        {
            var scriptFiles = bHead ? _headScritpFiles : _footScritpFiles;
            Append(scriptFiles, key, parts);
            return this;
        }

        public FileBundle AddCss(string key, params string[] parts)
        {
            Add(_cssFiles, key, parts);
            return this;
        }

        public FileBundle AddScript(bool bHead, string key, params string[] parts)
        {
            var scriptFiles = bHead ? _headScritpFiles : _footScritpFiles;
            Add(scriptFiles, key, parts);
            return this;
        }

        private void Append(Dictionary<string, IList<string>> files, string key, params string[] parts)
        {
            if (string.IsNullOrEmpty(key) || parts == null)
                return;

            if (files.ContainsKey(key))
            {
                var index = 0;
                foreach (var part in parts)
                    files[key].Insert(index++, part);
            }
            else
            {
                files.Add(key, parts.ToList());
            }
        }

        private void Add(Dictionary<string, IList<string>> files, string key, params string[] parts)
        {
            if (string.IsNullOrEmpty(key) || parts == null)
                return;

            if (files.ContainsKey(key))
            {
            foreach (var part in parts)
            {
                files[key].Add(part);
            }
            }
            else
            {
                files.Add(key, parts.ToList());
            }
        }

        private string GetvirtualPath(string key, IList<string> files)
        {
            unchecked
            {
                var hash = files.Aggregate(files.Count, (x, y) => x = x * 31 + y.GetHashCode());
                return string.Format("{0}{1}", key, hash);
            }
        }
    }
}