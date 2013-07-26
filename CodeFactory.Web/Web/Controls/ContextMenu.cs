using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.IO;
using System.Reflection;
using System.Collections;


namespace CodeFactory.Web.Controls
{
	[ParseChildren(true, "ContextMenuItems")]
	[Designer(typeof(CodeFactory.Web.Controls.Design.ContextMenuDesign))]
	[DefaultEvent("ItemCommand")]
	[DefaultProperty("ContextMenuItems")]
	public class ContextMenu : WebControl, INamingContainer
	{
		#region Private Members

		// ******************************************************************************
		// Private members
		ContextMenuItemCollection _contextMenuItems;
		ArrayList _boundControls;

		#endregion

		#region Constants
		// ******************************************************************************
		// Constants
		private const string HrSeparator = "<hr style=\"height:1px;border:solid 1px #c6c6c6;\" />";
		private const string AttachContextMenu = "return __showContextMenu({0});";
		private const string TrapEscKey = "__trapESC({0})";
		private const string HideOnClick = "{0}.style.display = 'none';";
		private const string OnMouseOver = "this.style.background = '{0}';";
		private const string OnMouseOut = "this.style.background = '{0}';";

		#endregion

		#region Events

        /// <summary>
        /// Public event
        /// </summary>
		public event CommandEventHandler ItemCommand;

		#endregion

		#region Methods

        /// <summary>
        /// Returns the Javascript code to attach the context menu to a HTML element
        /// </summary>
        /// <returns>Javascript reference</returns>
		public string GetMenuReference()
		{
			return String.Format(AttachContextMenu, Controls[0].ClientID);
		}

        /// <summary>
        /// Returns the Javascript code to dismiss the context menu when the user hits ESC
        /// </summary>
        /// <returns>Javascript reference</returns>
		public string GetEscReference()
		{
			return String.Format(TrapEscKey, Controls[0].ClientID);
		}

        /// <summary>
        /// Returns the Javascript code to dismiss the context menu when the user clicks outside the menu
        /// </summary>
        /// <returns></returns>
		public string GetOnClickReference()
		{
			return String.Format(HideOnClick, Controls[0].ClientID);
        }

        #endregion

        #region Properties: ContextMenuItems, ...

        /// <summary>
        /// Gets the collection of the menu items
        /// </summary>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		//[Editor(typeof(ContextMenuItemCollectionEditor), typeof(UITypeEditor))]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[Description("Gets the collection of the menu items")]
		public ContextMenuItemCollection ContextMenuItems
		{
			get 
			{
				if (_contextMenuItems == null)
					_contextMenuItems = new ContextMenuItemCollection();
				return _contextMenuItems;
			}
		}

        /// <summary>
        /// Gets the collection of controls for which the context menu should be displayed 
        /// </summary>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		[Description("Gets the collection of controls for which the context menu should be displayed")]
		public ArrayList BoundControls
		{
			get 
			{
				if (_boundControls == null)
					_boundControls = new ArrayList();
				return _boundControls;
			}
		}

        /// <summary>
        /// Gets and sets the background color when the mouse hovers over the menu item
        /// </summary>
		[Description("Gets and sets the background color when the mouse hovers over the menu item")]
		public Color RolloverColor
		{
			get 
			{
				object o = ViewState["RolloverColor"];
				if (o == null)
					return Color.Gold;
				return (Color) o;
			}
			set {ViewState["RolloverColor"] = value;}
		}

        /// <summary>
        /// Determines the pixels around each menu item 
        /// </summary>
		[Description("The space in pixels around each menu item")]
		public int CellPadding
		{
			get 
			{
				object o = ViewState["CellPadding"];
				if (o == null)
					return 2;
				return (int) o;
			}
			set 
			{
				ViewState["CellPadding"] = value;
			}
		}

        /// <summary>
        /// Indicates whether the context menu should be dismissed as the user moves out
        /// </summary>
		[Description("Indicates whether the context menu should be dismissed as the user moves out")]
		public bool AutoHide
		{
			get 
			{
				object o = ViewState["AutoHide"];
				if (o == null)
					return true;
				return (bool) o;
			}
			set 
			{
				ViewState["AutoHide"] = value;
			}
		}

		#endregion

		#region Rendering

        /// <summary>
        /// Determines the standard style of the control
        /// </summary>
        /// <returns></returns>
		protected override Style CreateControlStyle()
		{
			Style style = base.CreateControlStyle();
			style.BorderStyle = BorderStyle.Outset;
			style.BorderColor = Color.Snow;
			style.BorderWidth = Unit.Pixel(2);
			style.BackColor = Color.FromName("#efefef");
			style.Font.Name = "verdana";
			style.Font.Size = FontUnit.Point(8);
			return style;
		}
		
        /// <summary>
        /// Builds the UI of the control
        /// </summary>
		protected override void CreateChildControls()
		{
			Controls.Clear();

			// A context menu is an invisible DIV that is moved around via scripting when the user
			// right-clicks on a bound HTML tag
			HtmlGenericControl div = new HtmlGenericControl("div");
			div.ID = "Root";
			div.Style["display"] = "none";
			div.Style["position"] = "absolute";
            div.Style["text-align"] = "left";
			if (AutoHide)
				div.Attributes["onmouseleave"] = "this.style.display='none'";

			Table menu = new Table();
			menu.ApplyStyle(CreateControlStyle());
			menu.CellSpacing = 1;
			menu.CellPadding = CellPadding;
			div.Controls.Add(menu);

			// Loop on ContextMenuItems and add rows to the table
			foreach(ContextMenuItem item in ContextMenuItems)
			{
				// Create and add the menu item
				TableRow menuItem = new TableRow();
				menu.Rows.Add(menuItem);

				// Configure the menu item
				TableCell container = new TableCell();
				menuItem.Cells.Add(container);

				// Define the menu item's contents
				if( item.Text == String.Empty || item.Text == null)
				{
					// Empty item is a separator
					container.Controls.Add(new LiteralControl(ContextMenu.HrSeparator));
				}
				else
				{
					// Add roll-over capabilities
					container.Attributes["onmouseover"] = String.Format(ContextMenu.OnMouseOver, ColorTranslator.ToHtml(RolloverColor));
					container.Attributes["onmouseout"] = String.Format(ContextMenu.OnMouseOut, ColorTranslator.ToHtml(BackColor));

					// Add the link for post back
					LinkButton button = new LinkButton();
					container.Controls.Add(button);
					button.Click += new EventHandler(ButtonClicked);
					button.Width = Unit.Percentage(100);
					button.ToolTip = item.Tooltip;
					button.Text = item.Text;
					button.CommandName = item.CommandName;
                    button.CommandArgument = this.UniqueID;
				}
			}

			// Add the button to the control's hierarchy for display
			Controls.Add(div);

			// Inject any needed script code into the page
			EmbedScriptCode();

			// Inject the script code for all bound controls
			foreach(Control c in BoundControls)
			{
				WebControl ctl1 = (c as WebControl);
				HtmlControl ctl2 = (c as HtmlControl);

                if (ctl1 != null)
                    ctl1.Attributes["oncontextmenu"] = GetMenuReference();

                if (ctl2 != null)
                    ctl2.Attributes["oncontextmenu"] = GetMenuReference();
			}
		}

        /// <summary>
        /// Renders the UI of the control
        /// </summary>
        /// <param name="writer"></param>
		protected override void Render(HtmlTextWriter writer)
		{
			// Ensures the control behaves well at design-time 
			// (You don't need this, if the control supports data-binding because this gets 
			// implicitly called in DataBind()) 
			EnsureChildControls();

			// Style controls before rendering
			PrepareControlForRendering();

			// Avoid a surrounding <span> tag
			RenderContents(writer);
		}

        /// <summary>
        /// Apply styles to the control components immediately before rendering
        /// </summary>
		protected virtual void PrepareControlForRendering()
		{
			// Make sure there are controls to work with
			if (Controls.Count != 1)
				return;

			// Apply the table style
			HtmlGenericControl div = (HtmlGenericControl) Controls[0];

			Table menu = (Table) div.Controls[0];
			menu.CopyBaseAttributes(this);

			if (ControlStyleCreated)
				menu.ApplyStyle(ControlStyle);
			
			// Style each menu item individually
			for(int i = 0; i < menu.Rows.Count; i++)
			{
				TableRow menuItem = menu.Rows[i];
				TableCell cell = menuItem.Cells[0];

				// Style the link button
				LinkButton button = (cell.Controls[0] as LinkButton);
				if (button != null)
				{
					button.ForeColor = ForeColor;
					button.Style["text-decoration"] = "none";
				}
			}
		}

        /// <summary>
        /// Insert the script code needed to refresh the UI
        /// </summary>
		private void EmbedScriptCode()
		{
			// Add the script to declare the function
			string js = ReadResourceString("CodeFactory.Web.Controls.ContextMenu.js");

			// Expand a {0} placeholder
			// Can't use String.Format because of the curly brackets in a JS source
			
			// Modify the script to expand placeholders
			//js = js.Replace("{0}", Controls[0].ClientID);

			if (!Page.ClientScript.IsStartupScriptRegistered("ContextMenuHelper"))
				Page.ClientScript.RegisterStartupScript(this.GetType(), "ContextMenuHelper", js);
		}

        /// <summary>
        /// Read the specified string resource from the current assembly
        /// </summary>
        /// <param name="resourceName">Script resource name.</param>
        /// <returns>Script stream.</returns>
		private string ReadResourceString(string resourceName)
		{
            string js = null;
            StreamReader reader = new StreamReader(
                Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName));

            try
            {
                js = reader.ReadToEnd();
            }
            finally
            {
                reader.Close();
            }

			return js;
		}

		#endregion

		#region Event-related Members

        /// <summary>
        /// Fires the ItemCommand event to the host page
        /// </summary>
        /// <param name="sender">Option clicked.</param>
        /// <param name="e">Empty.</param>
		private void ButtonClicked(object sender, EventArgs e)
		{
			LinkButton button = sender as LinkButton;

			if (button != null)
			{
				CommandEventArgs args = new CommandEventArgs(button.CommandName, button.CommandArgument);
				OnItemCommand(args);
			}
		}
		
        /// <summary>
        /// Fires the ItemCommand event to the host page
        /// </summary>
        /// <param name="e">Command arguments</param>
		protected virtual void OnItemCommand(CommandEventArgs e)
		{
			if (ItemCommand != null)
				ItemCommand(this, e);
		}
		
		#endregion
	}
}
