﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;

namespace DOCQR.Revit
{
    [Transaction(TransactionMode.Manual)]
    public class Upload : IExternalCommand
    {
        private UIApplication _uiApp;

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            _uiApp = commandData.Application;
            Document doc = uiApp.ActiveUIDocument.Document;
            UIDocument uidoc = uiApp.ActiveUIDocument;
            
            // take care of AppDomain load issues
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;


            try
            {
                Transaction trans = new Transaction(doc, "QR");
                trans.Start();

                //if (doc.ActiveView.ViewType == ViewType.DrawingSheet) createQRCode(doc, (ViewSheet)doc.ActiveView);

                trans.Commit();
                trans.Dispose();
            }
            catch (System.Exception ex)
            {
                return Result.Failed;
            }

            return Result.Succeeded;

        }

        System.Reflection.Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            // if the request is coming from us
            if ((args.RequestingAssembly != null) && (args.RequestingAssembly == this.GetType().Assembly))
            {
                if ((args.Name != null) && (args.Name.Contains(",")))  // ignore resources and such
                {
                    string asmName = args.Name.Split(',')[0];

                    string targetFilename = Path.Combine( System.Reflection.Assembly.GetExecutingAssembly().Location, asmName + ".dll" );

                    _uiApp.Application.WriteJournalComment("Assembly Resolve issue. Looking for: "  +args.Name, false );
                    _uiApp.Application.WriteJournalComment("Looking for " + targetFilename, false);
                    
                    if (File.Exists( targetFilename ))
                    {
                        _uiApp.Application.WriteJournalComment("Found, and loading...", false);
                        return System.Reflection.Assembly.LoadFrom( targetFilename );
                    }
                }
            }

            return null;
        }

    }

}