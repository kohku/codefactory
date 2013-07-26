using System;
using System.Collections.Generic;
using System.Text;

namespace CodeFactory.Web.Core
{
    /// <summary>
    /// An interface implemented by the classed that can be published.
    /// <remarks>
    /// To implemnet this interface means that the class can be searched
    /// from the search page and that it can be syndicated in RSS and ATOM.
    /// </remarks>
    /// </summary>
    public interface IPublishable<T> : CodeFactory.Web.Core.IIdentifiable<T>
    {
        /// <summary>
        /// Gets the title of the object
        /// </summary>
        String Title { get; set; }

        /// <summary>
        /// Gets the content.
        /// </summary>
        /// <value>The content.</value>
        String Content { get; set; }

        String Slug { get; set; }

        String Keywords { get; set; }

        /// <summary>
        /// Gets the date created.
        /// </summary>
        /// <value>The date created.</value>
        DateTime DateCreated { get; set; }

        /// <summary>
        /// Gets the date modified.
        /// </summary>
        /// <value>The date modified.</value>
        DateTime? LastUpdated { get; set; }

        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <value>The description.</value>
        String Description { get; set; }

        /// <summary>
        /// Gets the author.
        /// </summary>
        /// <value>The author.</value>
        String Author { get; set; }

        /// <summary>
        /// Gets whether or not this item should be shown
        /// </summary>
        bool IsVisible { get; set; }

        List<string> Roles { get; }

        /// <summary>
        /// Gets the last user that modified the document.
        /// </summary>
        string LastUpdatedBy { get; set; }

        /// <summary>
        /// Gets the relative link.
        /// </summary>
        /// <value>The relative link.</value>
        String RelativeLink { get; }

        /// <summary>
        /// Gets the absolute link.
        /// </summary>
        /// <value>The absolute link.</value>
        Uri AbsoluteLink { get; }

    }
}
