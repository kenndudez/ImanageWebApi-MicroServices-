
using Imanage.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Imanage.Shared.Dtos
{
    public class WorkflowInstanceHistoryDto : BaseDto
    {
        public Guid AppId { get; set; }
        public int Order { get; set; }
        public string ValidationLvl { get; set; }
        public string Comment { get; set; }
        public Guid WorkflowId { get; set; }
        public string WorkflowStage { get; set; }
        public Guid ApproverId { get; set; }
        public string ApproverName { get; set; }
        public string RejectReason { get; internal set; }
        public Guid? DocumentId { get; set; }
        public Guid? CreatedBy { get; set; }
        public VerificationState ActionTaken { get; set; }
    }
}
