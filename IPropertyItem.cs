/* *********************************************************************
 * Date: 20 Feb 2008
 * Created by: Zoltan Juhasz
 * E-Mail: forge@jzo.hu
***********************************************************************/

using System.Collections.Generic;

namespace Forge.Configuration
{

    /// <summary>
    /// Property data interface
    /// </summary>
    public interface IPropertyItem
    {

        /// <summary>
        /// Gets the id.
        /// </summary>
        /// <value>
        /// The id.
        /// </value>
        string Id { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        string Value { get; set; }

        /// <summary>
        /// Gets the property items.
        /// </summary>
        /// <value>
        /// The property items.
        /// </value>
        Dictionary<string, IPropertyItem> Items { get; }

    }

}
