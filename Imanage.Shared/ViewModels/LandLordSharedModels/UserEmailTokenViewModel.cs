
using Imanage.Shared.Enums;
using Imanage.Shared.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Imanage.Shared.ViewModels.MarketerSharedModels
{
    public class UserEmailTokenViewModel
    {
        public EmailTypeEnum EmailType { get; set; }
        public ImanageUser User { get; set; }
        public string Code { get; set; }
    }
}
