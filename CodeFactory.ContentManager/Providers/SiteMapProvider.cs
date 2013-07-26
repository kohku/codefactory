using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using CodeFactory.Web;
using System.Collections;
using System.Web;
using CodeFactory.Web.Core;

namespace CodeFactory.ContentManager.Providers
{
    public class SiteMapProvider : System.Web.SiteMapProvider
    {
        private Dictionary<Guid, IPublishable<Guid>> _nodes;

        public SiteMapProvider()
        {
            _nodes = new Dictionary<Guid, IPublishable<Guid>>();
        }

        public override void Initialize(string name, NameValueCollection attributes)
        {
            base.Initialize(name, attributes);
        }

        public override System.Web.SiteMapNode FindSiteMapNode(string rawUrl)
        {
            return null;
        }

        public override System.Web.SiteMapNode FindSiteMapNodeFromKey(string key)
        {
            try
            {
                Guid id = new Guid(key);

                if (_nodes.ContainsKey(id))
                    return new SiteMapNode(this, _nodes[id]);

                return null;
            }
            catch(Exception)
            {
                return null;
            }
        }

        public override System.Web.SiteMapNodeCollection GetChildNodes(System.Web.SiteMapNode node)
        {
            System.Web.SiteMapNodeCollection children = new System.Web.SiteMapNodeCollection();

            IPublishable<Guid> item = _nodes[new Guid(node.Key)];

            // We shouldn't show page's child pages, just section's child pages.
            if (item is Section)
            {
                Section s = (Section)item;

                foreach (Section child in s.Childs)
                {
                    if (!_nodes.ContainsKey(child.ID))
                        _nodes.Add(child.ID, child);

                    SiteMapNode nodewrapper = new SiteMapNode(this, child);

                    if (HttpContext.Current.User.IsInRole("Administrator") || (nodewrapper.IsAccessibleToUser(HttpContext.Current) && child.IsVisible)) 
                        children.Add(nodewrapper);
                }

                foreach (Page child in s.Pages)
                {
                    if (!_nodes.ContainsKey(child.ID))
                        _nodes.Add(child.ID, child);

                    SiteMapNode nodewrapper = new SiteMapNode(this, child);

                    if (HttpContext.Current.User.IsInRole("Administrator") || (nodewrapper.IsAccessibleToUser(HttpContext.Current) && child.IsVisible))
                        children.Add(nodewrapper);
                }
            }

            return children;
        }

        public override bool IsAccessibleToUser(System.Web.HttpContext context, System.Web.SiteMapNode node)
        {
            if (!this.SecurityTrimmingEnabled)
                return true;

            if (node.Roles != null)
            {
                if (node.Roles.Count == 0)
                    return true;

                foreach (string rolename in node.Roles)
                {
                    if (rolename == "*" || (context.User != null && context.User.IsInRole(rolename)))
                        return true;
                }
            }

            return false;
        }

        public override System.Web.SiteMapNode GetParentNode(System.Web.SiteMapNode node)
        {
            IPublishable<Guid> item = _nodes[new Guid(node.Key)];

            if (item is ISection)
            {
                ISection section = (ISection)item;

                ISection parent = section.Parent;

                return new SiteMapNode(this, parent != null ? parent : new Section(Guid.Empty));
            }
            else if (item is IPage)
            {
                IPage page = (IPage)item;

                ISection parent = page.Section;

                return new SiteMapNode(this, parent != null ? parent : new Section(Guid.Empty));
            }

            return new SiteMapNode(this, new Section(Guid.Empty));
        }

        protected override System.Web.SiteMapNode GetRootNodeCore()
        {
            ISection root = new Section(Guid.Empty);

            root.Name = ContentManagementService.Settings.SiteMapRootName;

            if (!_nodes.ContainsKey(root.ID))
                _nodes.Add(root.ID, root);

            return new SiteMapNode(this, root);
        }
    }
}
