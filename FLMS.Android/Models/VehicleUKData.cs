using System;
using System.Collections.Generic;

public class DataKeys
{
    public string Vrm { get; set; }
}

public class Request
{
    public string RequestGuid { get; set; }
    public string PackageId { get; set; }
    public int PackageVersion { get; set; }
    public int ResponseVersion { get; set; }
    public DataKeys DataKeys { get; set; }
}

public class Lookup
{
    public string StatusCode { get; set; }
    public string StatusMessage { get; set; }
}

public class StatusInformation
{
    public Lookup Lookup { get; set; }
}

public class ImageDetailsList
{
    public string ImageUrl { get; set; }
    public DateTime ExpiryDate { get; set; }
    public string ViewPoint { get; set; }
}

public class VehicleImages
{
    public List<ImageDetailsList> ImageDetailsList { get; set; }
    public int ImageDetailsCount { get; set; }
}

public class DataItems
{
    public VehicleImages VehicleImages { get; set; }
}

public class Response
{
    public string StatusCode { get; set; }
    public string StatusMessage { get; set; }
    public StatusInformation StatusInformation { get; set; }
    public DataItems DataItems { get; set; }
}

public class VehicleUKData
{
    public Request Request { get; set; }
    public Response Response { get; set; }
}