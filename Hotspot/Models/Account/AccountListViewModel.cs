using System.Collections.Generic;

namespace Hotspot.Models.Account
{
    public class AccountListViewModel
    {
        public IEnumerable<AccountListItemViewModel> AccountList { get; set; }
    }
}
