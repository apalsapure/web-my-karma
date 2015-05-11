using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarmaRewards.Model
{
    /// <summary>
    /// Authentication Provider
    /// </summary>
    public enum AuthenticationProvider
    {
        /// <summary>
        /// Authentication Provider is Windows
        /// </summary>
        Windows,
        /// <summary>
        /// Authentication Provider is Google
        /// </summary>
        Google,
        /// <summary>
        /// Authentication Provider is Live
        /// </summary>
        Live,
        /// <summary>
        /// Authentication Provider is Yahoo
        /// </summary>
        Yahoo
    }
}
