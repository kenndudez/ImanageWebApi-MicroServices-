using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Imanage.Shared.Enums
{
    public enum WorkFlowDocumentType
    {
        PassPort = 1,
        CAC,
        DPRReceipt,
        DPRLicense,
        BulkPurchaseAgreement,
        ATC,
        ATO,
        DeedOfAssignment,
        TransferOfTitle,
        Identification,
        VehicleRegistration,
        Insurance,
        TruckCalibration,
        RoadWorthiness,
        NIN,
        NARTO,
        Logo,
        DriversLicence,
        VotersCard,
        ProofOfOwnership
    }


    public enum TruckDriverDocuments
    {
       DriverImage = 1,

    }
    public enum VerificationState
    {
        UNKNOWN,
        [Description("Pending")]
        Pending,
        [Description("Passed")]
        Approved,
        [Description("Failed")]
        Rejected
    }

    //public enum BatchStatus
    //{
    //    UNKNOWN,
    //    [Display(Name = "Pending")]
    //    PENDING,
    //    [Display(Name = "Open")]
    //    OPEN,
    //    [Display(Name = "Closed")]
    //    CLOSED
    //}
}
