// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace iOSVeryfiTestApp
{
	[Register ("ViewController")]
	partial class ViewController
	{
        [Outlet]
        UIKit.UITextView Logs { get; set; }

        [Action ("OpenLens:")]
		partial void OpenLens (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
            if (Logs != null)
            {
                Logs.Dispose();
                Logs = null;
            }
        }
	}
}
