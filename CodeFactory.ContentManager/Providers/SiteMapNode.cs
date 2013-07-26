using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Collections;

namespace CodeFactory.ContentManager.Providers
{
    public class SiteMapNode : System.Web.SiteMapNode
    {
        private CodeFactory.Web.Core.IPublishable<Guid> _node;

        public SiteMapNode(System.Web.SiteMapProvider provider, CodeFactory.Web.Core.IPublishable<Guid> node)
            : base(provider, node.ID.ToString(), node.RelativeLink, node.Title, node.Description)
        {
            _node = node;

            this.Roles = !_node.ID.Equals(Guid.Empty) && _node.Roles.Count > 0 ? _node.Roles : new List<string>(new string[] { "*" });
        }

        public SiteMapNode(System.Web.SiteMapProvider provider, Section node)
            : base(provider, node.ID.ToString(), node.RelativeLink, node.Name, "Section")
        {
            _node = node;

            this.Roles = !_node.ID.Equals(Guid.Empty) && _node.Roles.Count > 0 ? _node.Roles : new List<string>(new string[] { "*" });
        }

        public SiteMapNode(System.Web.SiteMapProvider provider, Page node)
            : base(provider, node.ID.ToString(), node.RelativeLink, node.Title, node.Description)
        {
            _node = node;

            this.Roles = !_node.ID.Equals(Guid.Empty) && _node.Roles.Count > 0 ? _node.Roles : new List<string>(new string[] { "*" });
        }
    }
}
