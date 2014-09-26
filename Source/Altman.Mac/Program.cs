﻿using System;
using System.IO;
using System.Reflection;
using Altman.Desktop;
using Eto;
using Eto.Mac.Forms.Controls;
using Eto.Mac.Forms;
using Eto.Mac;
using System.Diagnostics;

using MonoMac.AppKit;

namespace Altman.Mac
{
	static class Program
	{
		static void Main(string[] args)
		{
			var path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
			if (AppDomain.CurrentDomain.IsDefaultAppDomain())
			{
				var setup = new AppDomainSetup();
				setup.PrivateBinPath = "Bin";
				setup.ShadowCopyFiles = "true";
				setup.CachePath = Path.Combine(Path.GetTempPath(), "__cache__");
				setup.ShadowCopyDirectories = Path.Combine(path, "Plugins") + ";" + Path.Combine(path, "Bin");

				var appDomain = AppDomain.CreateDomain("Host_AppDomain", AppDomain.CurrentDomain.Evidence, setup);
				appDomain.ExecuteAssembly(Assembly.GetExecutingAssembly().CodeBase);
			}
			else
			{
				Start();
			}
		}

		static void Start()
		{
			#if DEBUG && !XAMMAC2
			Debug.Listeners.Add(new ConsoleTraceListener());
			#endif
			AddStyles();

			var generator = new Eto.Mac.Platform();
			var app = new AltmanApplication(generator);
			app.Run();
		}

		static void AddStyles()
		{
			// support full screen mode!
			Style.Add<FormHandler>("main", handler =>
				{
					handler.Control.CollectionBehavior |= NSWindowCollectionBehavior.FullScreenPrimary;
				});

			Style.Add<ApplicationHandler>("application", handler =>
				{
					handler.EnableFullScreen();
				});

			// other styles
			Style.Add<TreeGridViewHandler>("sectionList", handler =>
				{
					handler.ScrollView.BorderType = NSBorderType.NoBorder;
					handler.Control.SelectionHighlightStyle = NSTableViewSelectionHighlightStyle.SourceList;
				});

			Style.Add<ButtonToolItemHandler>(null, handler =>
				{
					// use standard textured/round buttons, and make the image grayscale
					handler.UseStandardButton(grayscale: true);
				});

			Style.Add<ToolBarHandler>(null, handler =>
				{
					// change display mode or other options
					//handler.Control.DisplayMode = NSToolbarDisplayMode.Icon;
				});
		}
	}
}	



