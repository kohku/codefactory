using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeFactory.ContentManager.WebControls
{
    public abstract class PageLayout : System.Web.UI.UserControl
    {
        public abstract string Name { get; }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is PageLayout))
                return false;

            PageLayout layout = (PageLayout)obj;

            return (this.Name == layout.Name);
        }

        public override int GetHashCode()
        {
            return this.Name.GetHashCode();
        }
    }
}
