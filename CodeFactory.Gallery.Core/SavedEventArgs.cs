using System;
using System.Collections.Generic;
using System.Text;

namespace CodeFactory.Gallery.Core
{
    /// <summary>
    /// 
    /// </summary>
    public class SavedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SavedEventArgs"/> class.
        /// </summary>
        /// <param name="action">The action.</param>
        public SavedEventArgs(SaveAction action)
        {
            Action = action;
        }

        private SaveAction _Action;
        /// <summary>
        /// Gets or sets the action that occured when the object was saved.
        /// </summary>
        public SaveAction Action
        {
            get { return _Action; }
            set { _Action = value; }
        }
    }

    /// <summary>
    /// The action performed by the save event.
    /// </summary>
    public enum SaveAction
    {
        /// <summary>
        /// Default. Nothing happened.
        /// </summary>
        None,
        /// <summary>
        /// It's a new object that has been inserted.
        /// </summary>
        Insert,
        /// <summary>
        /// It's an old object that has been updated.
        /// </summary>
        Update,
        /// <summary>
        /// The object was deleted.
        /// </summary>
        Delete
    }
}
