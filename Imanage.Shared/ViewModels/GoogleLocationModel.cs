using System;
using System.Collections.Generic;
using System.Text;

namespace Imanage.Shared.ViewModels
{
    public class GoogleLocationModel
    {
        public Results[] Results { get; set; }
        public string Status { get; set; }
        public string Error_Message { get; set; }
    }

    public class Results
    {
        public AddressComponent[] Address_Components { get; set; }
        public string Formatted_Address { get; set; }
        public Geometry Geometry { get; set; }
        public string Partial_Match { get; set; }
        public string Place_Id { get; set; }
        public PlusCode Plus_Code { get; set; }
        public string[] Types { get; set; }


    }

    public class AddressComponent
    {
        public string Long_Name { get; set; }
        public string Short_Name { get; set; }
        public string[] Types { get; set; }
    }

    public class Geometry
    {
        public Location Location { get; set; }
        public string Locatipn_Type { get; set; }
        public ViewPort ViewPort { get; set; }
    }

    public class Location
    {
        public string Lat { get; set; }
        public string Lng { get; set; }
    }

    public class ViewPort 
    {
        public Location Northeast { get; set; }
        public Location Southwest { get; set; }
    }

    public class PlusCode 
    {
        public string Compound_Code { get; set; }
        public string Global_Code { get; set; }
    }

}
