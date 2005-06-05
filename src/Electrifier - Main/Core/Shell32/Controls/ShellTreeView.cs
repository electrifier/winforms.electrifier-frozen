//	<file>
//		<copyright see="www.electrifier.org"/>
//		<license   see="www.electrifier.org"/>
//		<owner     name="Thorsten Jung" email="taj@electrifier.org"/>
//		<version   value="$Id: ShellTreeView.cs,v 1.19 2004/09/10 20:30:34 taj bender Exp $"/>
//	</file>

using System;
using System.Windows.Forms;

using Electrifier.Core.Controls;
using Electrifier.Core.Services;
using Electrifier.Core.Shell32.Services;
using Electrifier.Win32API;

namespace Electrifier.Core.Shell32.Controls {
	/// <summary>
	/// Zusammenfassung f�r ShellTreeView.
	/// </summary>
	public class ShellTreeView : ExtTreeView {
		protected static IconManager                 iconManager = (IconManager)ServiceManager.Services.GetService(typeof(IconManager));
		protected        ShellTreeViewNode           rootNode    = null;
		protected new    ShellTreeViewNodeCollection nodes       = null;
		public    new    ShellTreeViewNodeCollection Nodes       { get { return nodes; } }

		public ShellTreeView(ShellAPI.CSIDL shellObjectCSIDL)
			: this(PIDLManager.CreateFromCSIDL(shellObjectCSIDL), true) {}

		public ShellTreeView(string shellObjectFullPath)
			: this(PIDLManager.CreateFromPathW(shellObjectFullPath), true) {}

		public ShellTreeView(IntPtr shellObjectPIDL)
			: this(shellObjectPIDL, false) {}

		public ShellTreeView(IntPtr pidl, bool pidlSelfCreated) : base() {
			// Initialize underlying ExtTreeView-component
			this.rootNode        = new ShellTreeViewNode(pidl, pidlSelfCreated);
			this.nodes           = new ShellTreeViewNodeCollection(base.Nodes);
			this.SystemImageList = iconManager.SmallImageList;
			this.HideSelection   = false;
			this.AllowDrop       = true;
			this.ShowRootLines   = false;

			this.Nodes.Add(rootNode);
			this.rootNode.Expand();
			if(this.rootNode.FirstNode != null) {
				this.rootNode.FirstNode.Expand();
				this.SelectedNode = rootNode.FirstNode;
			}

			// Create a file info thread to gather visual info for root item
			IconManager.FileInfoThread fileInfoThread = new IconManager.FileInfoThread(rootNode);
		}

	}
}
