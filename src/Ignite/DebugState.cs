using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Configuration;

namespace Ignite
{
    public class DebugState : IDebugState
    {
        private bool forceDebug;
        private bool forceRelease;

        public bool IsDebugging()
        {
            if (this.forceDebug) { return true; }
            if (this.forceRelease) { return false; }
            return HttpContext.Current != null && HttpContext.Current.IsDebuggingEnabled;
        }

        public void Disable()
        {
            this.forceDebug = false;
            this.forceRelease = true;
        }

        public void Enable()
        {
            this.forceDebug = true;
            this.forceRelease = false;
        }
    }
}
