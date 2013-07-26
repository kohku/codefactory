using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.Design;
using System.ComponentModel.Design;

namespace Recaptcha.Design
{
    public class RecaptchaControlDesigner : ControlDesigner
    {
        // Methods
        public override string GetDesignTimeHtml()
        {
            return base.CreatePlaceHolderDesignTimeHtml("reCAPTCHA Validator");
        }

        // Properties
        public override DesignerActionListCollection ActionLists
        {
            get
            {
                DesignerActionListCollection _actionLists = new DesignerActionListCollection();
                _actionLists.AddRange(base.ActionLists);
                _actionLists.Add(new ActionList(this));
                return _actionLists;
            }
        }

        public override bool AllowResize
        {
            get
            {
                return false;
            }
        }

        // Nested Types
        public class ActionList : DesignerActionList
        {
            // Fields
            private RecaptchaControlDesigner _parent;

            // Methods
            public ActionList(RecaptchaControlDesigner parent)
                : base(parent.Component)
            {
                this._parent = parent;
            }

            public override DesignerActionItemCollection GetSortedActionItems()
            {
                DesignerActionItemCollection items = new DesignerActionItemCollection();
                items.Add(new DesignerActionHeaderItem("API Key"));
                items.Add(new DesignerActionTextItem("To use reCAPTCHA, you need an API key from http://admin.recaptcha.net/", string.Empty));
                return items;
            }
        }
    }
}
