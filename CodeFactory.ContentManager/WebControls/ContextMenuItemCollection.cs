using System;
using System.Collections;
using System.ComponentModel;
using System.Web.UI;
using System.ComponentModel.Design;
using System.Reflection;

namespace CodeFactory.ContentManager.WebControls
{
	#region ContextMenuItem Class

	[TypeConverter(typeof(ExpandableObjectConverter))]
	public class ContextMenuItem
	{
		#region Constructor(s)

		public ContextMenuItem()
		{
		}
		public ContextMenuItem(string text, string commandName)
		{
			_text = text;
			_commandName = commandName;
		}

		#endregion

		#region Private members

		private string _text;
		private string _commandName;
		private string _tooltip;

		#endregion

		#region Properties
		[Category("Behavior"), DefaultValue(""), Description("Text of the menu item"), NotifyParentProperty(true)]
		public string Text
		{
			get {return _text;}
			set {_text = value;}
		}

		[Category("Behavior"), DefaultValue(""), Description("Command name associated with the menu item"), NotifyParentProperty(true)]
		public string CommandName
		{
			get {return _commandName;}
			set {_commandName = value;}
		}

		[Category("Behavior"), DefaultValue(""), Description("The tooltip for the menu item"), NotifyParentProperty(true)]
		public string Tooltip
		{
			get {return _tooltip;}
			set {_tooltip = value;}
		}

		#endregion

        public override string ToString()
        {
            return !string.IsNullOrEmpty(this.Text) ? this.Text : "Separator";
        }
	}

	#endregion 

	#region ContextMenuItemCollection

	public class ContextMenuItemCollection : CollectionBase
	{
        /// Default constructor
        /// </summary>
		public ContextMenuItemCollection()
		{
		}
	
        /// Gets and sets the element at the specified position.
        /// </summary>
        /// <param name="index">Position index.</param>
        /// <returns>Element</returns>
		public ContextMenuItem this[int index]
		{
			get { return (ContextMenuItem) InnerList[index]; }
			set { InnerList[index] = value; }
		}

        /// <summary>
        /// Adds an object to the end of the collection
        /// </summary>
        /// <param name="item"></param>
		public void Add(ContextMenuItem item)
		{
			InnerList.Add(item);
		}

        /// <summary>
        /// Adds an object at the specified position in the collection
        /// </summary>
        /// <param name="index">Position</param>
        /// <param name="item">Item to add</param>
		public void AddAt(int index, ContextMenuItem item)
		{
			InnerList.Insert(index, item);
		}
	}

	#endregion

	#region ContextMenuItemCollectionEditor Class

    public class ContextMenuItemCollectionEditor : CollectionEditor
	{
		public ContextMenuItemCollectionEditor(Type type) : base(type)
		{
		}

		protected override bool CanSelectMultipleInstances()
		{
			return false;
		}

		protected override Type CreateCollectionItemType()
		{
			return typeof(ContextMenuItem);
		}
	}

	#endregion
}
