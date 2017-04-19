using System.Collections.Generic;

namespace ArcadiaWebForm.Models
{
    public class IdModel
    {
        public IList<InnerId> results { get; set; }
    }

    public class InnerId
    {
        public IList<string> ids { get; set; }
    }
}
