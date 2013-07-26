using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace CodeFactory.Gallery.Core.Web.HttpHandlers
{
    public partial class featuredProjects
    {
        private List<featuredProjectsProj> items;

        public featuredProjects()
        {
            items = new List<featuredProjectsProj>();
        }

        public void Add(featuredProjectsProj proj)
        {
            items.Add(proj);
            this.projField = items.ToArray();
        }
    }
}
