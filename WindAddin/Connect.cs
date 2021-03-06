using System;
using Extensibility;
using EnvDTE;
using EnvDTE80;
using System.IO;
namespace WindAddin
{

	/// <summary>The object for implementing an Add-in.</summary>
	/// <seealso class='IDTExtensibility2' />
	public class Connect : IDTExtensibility2
	{

		private Events2 events;
		private DocumentEvents eventsOnDocs;
		private DTE2 _applicationObject;
		private AddIn _addInInstance;
		private AddIn instance;
		private DTE2 App;
        private ScriptConvertor windc;
        private MinCompiler     minc ;


		/// <summary>Implements the constructor for the Add-in object. Place your initialization code within this method.</summary>
		public Connect()
		{
		}

		/// <summary>Implements the OnConnection method of the IDTExtensibility2 interface. Receives notification that the Add-in is being loaded.</summary>
		/// <param term='application'>Root object of the host application.</param>
		/// <param term='connectMode'>Describes how the Add-in is being loaded.</param>
		/// <param term='addInInst'>Object representing this Add-in.</param>
		/// <seealso class='IDTExtensibility2' />
		public void OnConnection(object application, ext_ConnectMode connectMode, object addInInst, ref Array custom)
		{
			_applicationObject = (DTE2)application;
			_addInInstance = (AddIn)addInInst;

			this.instance = addInInst as AddIn;
			this.App = application as DTE2;
			this.events = this.App.Events as Events2;
			
		}

		/// <summary>Implements the OnDisconnection method of the IDTExtensibility2 interface. Receives notification that the Add-in is being unloaded.</summary>
		/// <param term='disconnectMode'>Describes how the Add-in is being unloaded.</param>
		/// <param term='custom'>Array of parameters that are host application specific.</param>
		/// <seealso class='IDTExtensibility2' />
		public void OnDisconnection(ext_DisconnectMode disconnectMode, ref Array custom)
		{
		}

		/// <summary>Implements the OnAddInsUpdate method of the IDTExtensibility2 interface. Receives notification when the collection of Add-ins has changed.</summary>
		/// <param term='custom'>Array of parameters that are host application specific.</param>
		/// <seealso class='IDTExtensibility2' />		
		public void OnAddInsUpdate(ref Array custom)
		{
		}

		/// <summary>Implements the OnStartupComplete method of the IDTExtensibility2 interface. Receives notification that the host application has completed loading.</summary>
		/// <param term='custom'>Array of parameters that are host application specific.</param>
		/// <seealso class='IDTExtensibility2' />
		public void OnStartupComplete(ref Array custom)
		{
			this.eventsOnDocs = this.events.get_DocumentEvents();
			this.eventsOnDocs.DocumentSaved += DocumentEvents_DocumentSaved;
            windc = new ScriptConvertor();
            minc = new MinCompiler();
			
		}

		/// <summary>Implements the OnBeginShutdown method of the IDTExtensibility2 interface. Receives notification that the host application is being unloaded.</summary>
		/// <param term='custom'>Array of parameters that are host application specific.</param>
		/// <seealso class='IDTExtensibility2' />
		public void OnBeginShutdown(ref Array custom)
		{
		}

        private string CompiledFileExt
        {
            get
            {
                return ".runtime.js";
            }
        }
        private string MinFileExt
        {
            get
            {
                return ".min.js";
            }
        }
        private string MatchFileExt
        {
            get
            {
                return ".w.js";
            }
        }

        private bool ShouldConvert(string path)
        {
            if (!path.EndsWith(".w.js"))
                return false;
            if (path.EndsWith(CompiledFileExt))
                return false;
            return true;
        }

        private string GetCompliedFilePath(string path,bool min=false)
        {
            var newPath = path.Remove(path.Length - MatchFileExt.Length) + (min ? MinFileExt : CompiledFileExt);
            return newPath;
        }


		private void DocumentEvents_DocumentSaved(Document document)
		{
			try
			{
				var doc = document;
				ProjectItem projectItem = this.App.SelectedItems.Item(1).ProjectItem;
				string path = projectItem.FileName();
                if (ShouldConvert(path) == false)
                    return;
                var newPath = GetCompliedFilePath(path);
                var newMinPath = GetCompliedFilePath(path, true);
				var success= windc.Convert(path, newPath);
				if(success) 
                    AddItem(projectItem, newPath);
                success = minc.Convert(newPath, newMinPath);
                if (success)
                    AddItem(projectItem, newMinPath);
			}
			catch (Exception e)
			{
				//this.OutputWindowWriteText(e.ToString());
			}
		}

		internal static void AddItem(ProjectItem projectItem, string fullFileNameToAdd)
		{
			if (projectItem == null)
			{
				throw new ArgumentNullException("projectItem");
			}

			if (string.IsNullOrEmpty(fullFileNameToAdd))
			{
				throw new Exception("fullFileNameToAdd needs to contain a valid filename.");
			}

			if (projectItem.ProjectItems != null)
			{
				string curringItemDir = projectItem.get_FileNames(0);
				curringItemDir = new FileInfo(curringItemDir).DirectoryName;
				if (curringItemDir == new FileInfo(fullFileNameToAdd).DirectoryName)
				{
					projectItem.ProjectItems.AddFromFile(fullFileNameToAdd);
				}
			}
		}
		
	}
}