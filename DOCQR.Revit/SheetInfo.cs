﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;

namespace DOCQR.Revit
{
    public class SheetInfo
    {
        public ElementId sheetId;
        public List<ViewPortInfo> ViewPorts;

        public SheetInfo(Document doc, ViewSheet TempSheet)
        {
            this.ViewPorts = new List<ViewPortInfo>();


            this.sheetId = TempSheet.Id;                     // extract sheet ID


            // for each sheet extract each view port
            foreach (ElementId vid in TempSheet.GetAllViewports())
            {
                Viewport vport = (Viewport)doc.GetElement(vid);
                View v = (View)doc.GetElement(vport.ViewId);

                if (v.ViewType == ViewType.AreaPlan || v.ViewType == ViewType.Elevation || v.ViewType == ViewType.FloorPlan || v.ViewType == ViewType.Section || v.ViewType == ViewType.ThreeD)
                {
                    // TODO: REAL GUID!
                    Guid guid = Guid.NewGuid();

                    ViewPorts.Add(new ViewPortInfo(v.Id, guid, vport.GetBoxCenter()));
                }
            }

        }
    }

    public class ViewPortInfo
    {
        public ElementId id;
        public Guid guid;
        public XYZ location;

        public ViewPortInfo(ElementId id, Guid guid, XYZ location)
        {
            this.id = id;
            this.guid = guid;
            this.location = location;
        }
    }
}