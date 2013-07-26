using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;

namespace CodeFactory.ContentManager.WebControls
{
    public class SectionDataSourceView : HierarchicalDataSourceView
    {
        private string viewPath;

        public SectionDataSourceView(string viewPath)
        {
            this.viewPath = viewPath;
        }

        public override IHierarchicalEnumerable Select()
        {
            HttpRequest currentRequest = HttpContext.Current.Request;
 
            //if (!currentRequest.IsAuthenticated)
            //    throw new NotSupportedException("The SectionDataSourceView only presents data in an authenticated context.");

            SectionCollection sections = new SectionCollection();

            if (this.viewPath == Section.Root)
            {
                Section root = new Section(Guid.Empty);

                foreach (Section c in root.Childs)
                    sections.Add(new SectionHierarchyData(c));

                return sections;
            }

            ISection section = ContentManagementService.GetSection(this.viewPath);

            if (section != null)
            {
                foreach (ISection child in section.Childs)
                {
                    sections.Add(new SectionHierarchyData(child));
                }
            }

            return sections;
        }
    }
}
